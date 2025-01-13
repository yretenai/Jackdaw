from binaryninja.highlevelil import HighLevelILOperation
from binaryninja.plugin import BackgroundTaskThread
from binaryninja.log import log_error
from binaryninja.plugin import PluginCommand
import binaryninja.interaction as interaction
import json

# finding BlackSetInterface:
# find "GetIListIID"
# first method that calls `BlackSetInterface(&data_1815b79d8, rdx_5, &var_c8)`
# check if the sub has 1000+ calls
# that sub is BlackSetInterface, any function that wraps it is an overload.

# finding BlackRegisterType find:
# serach for the hex bytes: 49bd6766666666666666
# or something like if (data_1816b0cf8 == rdx_5) BlackRegisterType(&data_1816b0ce8, rdx_5, &data_1815cc6d0) (after BlackSetInterface)
# check if the sub has 1000+ calls
# that sub is BlackRegisterType, any function that wraps it is an overload.

# types:
# 0x1 - uint
# 0x2 - float
# 0x3 - ?
# 0x4 - boolean
# 0x5 - array
# 0x6 - object
# 0x7..0xe - ?
# 0xf - float array
# 0x10..0x16 - ?
# 0x17 - string
# 0x18 - ushort
# 0x19..0x1e - ?
# 0x1f - byte
# 0x20..0x21 - ?
# 0x22 - path

# struct PyTypeHolder
# {
#     void* pyClass;
#     void* thisClass;
#     char** description;
#     void* classList;
#     void* propList;
#     void* funcList;
#     struct PyTypeHolder* root;
#     int64_t field_38;
#     int64_t field_40;
#     void* manager;
#     int32_t field_50;
#     int32_t field_54;
#     void** vtable;
# };

# struct PyTypeProp
# {
#     char** name;
#     int64_t binaryType;
#     int64_t offset;
#     int64_t size;
#     void* type;
#     char** description;
#     int32_t typeId;
#     int32_t field_34;
#     int64_t isObject;
#     int64_t unwrapObject;
#     int64_t castObject;
# };


def Tr2String(bv, address):
    val = bv.get_ascii_string_at(address, 0)
    if val is None:
        return ""
    return val.value


def Tr2FindIIDs(bv, data, fns):
    for fn in fns:
        for ref in bv.get_code_refs(fn):
            hlil = ref.hlil
            if hlil.operation is not HighLevelILOperation.HLIL_CALL:
                continue
            args = hlil.operands[1]
            addr = args[0].value.value
            if addr < 0:
                continue
            name = Tr2String(bv, args[1].value.value)
            data["id"][addr] = name


def Tr2FindCLSIDs(bv, data, fns):
    for fn in fns:
        for ref in bv.get_code_refs(fn):
            hlil = ref.hlil
            if hlil.operation is not HighLevelILOperation.HLIL_CALL:
                continue
            args = hlil.operands[1]
            addr = args[0].value.value
            if addr < 0:
                continue
            namespace =  Tr2String(bv, args[1].value.value)
            name =  Tr2String(bv, args[2].value.value)
            data["clsid"][addr] = {"namespace": namespace, "name": name}


def Tr2ProcessMethod(bv, fn, initFn, setTypeFns, setInterfaceFns, data):
    obj = {"interfaces": [], "inherit": 0, "description": "", "address": 0, "props": []}
    seen = set()
    for call_site in fn.call_sites:
        if call_site.hlil is None:
            continue
        call = call_site.hlil.operands[0]
        if call_site.hlil.operation is not HighLevelILOperation.HLIL_CALL:
            continue
        call_address = call.value.value
        if call_address == initFn:
            obj["address"] = call_site.hlil.operands[1][1].value.value
            obj["description"] = Tr2String(bv, call_site.hlil.operands[1][2].value.value)
            assign = fn.hlil[call_site.hlil.instr_index + 1]
            if assign.operation is not HighLevelILOperation.HLIL_ASSIGN:
                continue
            parentAddress = assign.operands[0].value.value + 0x30
            if parentAddress == 0x30:
                parentAddress = assign.operands[0].src.value.value + 0x30
            parentAddress2 = assign.operands[0].value.value + 0x28
            if parentAddress2 == 0x28:
                parentAddress2 = assign.operands[0].src.value.value + 0x28
            refList = list(bv.get_code_refs(parentAddress)) + list(bv.get_code_refs(parentAddress2))
            for refEntry in refList:
                if refEntry.hlil is None:
                    continue
                refHlil = refEntry.hlil
                if refHlil.operation is not HighLevelILOperation.HLIL_ASSIGN:
                    continue
                assign = refHlil.operands[1]
                if assign.operation is HighLevelILOperation.HLIL_VAR:
                    refInherit = fn.hlil[refHlil.instr_index - 1]
                    if refInherit.operation is not HighLevelILOperation.HLIL_ASSIGN:
                        continue
                    inherit = refInherit.operands[1]
                    if inherit.operation is not HighLevelILOperation.HLIL_CALL:
                        continue
                    obj["inherit"] = inherit.operands[0].value.value
                    if obj["inherit"] != 0:
                        break
        elif call_address in setInterfaceFns:
            var = call_site.hlil.operands[1][-1]
            if var.operation is not HighLevelILOperation.HLIL_ADDRESS_OF:
                continue
            varObj = var.src.var
            for inst_index in range(var.instr_index, 0, -1):
                varInst = fn.hlil[inst_index]
                if varInst.operation is not HighLevelILOperation.HLIL_VAR_INIT:
                    continue
                if varInst.dest != varObj:
                    continue
                if varInst.src.operation is not HighLevelILOperation.HLIL_CONST_PTR:
                    continue
                obj["interfaces"].append(varInst.src.value.value)
        elif call_address in setTypeFns:
            prop = {"address": -1, "name": "", "description": "", "type": -1, "offset": -1, "size": -1, "iid": ""}
            prop["address"] = call_site.hlil.operands[1][-1].value.value
            if prop["address"] < 0:
                continue
            prop["type"] = bv.read_int(prop["address"] + 8, 8)
            prop["offset"] = bv.read_int(prop["address"] + 0x10, 8)
            prop["size"] = bv.read_int(prop["address"] + 0x18, 8)
            if prop["type"] == 0:
                refList = list(bv.get_code_refs(prop["address"] + 0x8))
                for refEntry in refList:
                    if refEntry.hlil is None:
                        continue
                    refHlil = refEntry.hlil
                    if refHlil.operation is not HighLevelILOperation.HLIL_ASSIGN:
                        continue
                    assign = refHlil.operands[1]
                    if assign.operation is HighLevelILOperation.HLIL_CONST:
                        prop["type"] = assign.value.value
                        break
            if prop["offset"] == 0:
                refList = list(bv.get_code_refs(prop["address"] + 0x10))
                for refEntry in refList:
                    if refEntry.hlil is None:
                        continue
                    refHlil = refEntry.hlil
                    if refHlil.operation is not HighLevelILOperation.HLIL_ASSIGN:
                        continue
                    assign = refHlil.operands[1]
                    if assign.operation is HighLevelILOperation.HLIL_CONST:
                        prop["offset"] = assign.value.value
                        break
            if prop["size"] == 0:
                refList = list(bv.get_code_refs(prop["address"] + 0x18))
                for refEntry in refList:
                    if refEntry.hlil is None:
                        continue
                    refHlil = refEntry.hlil
                    if refHlil.operation is not HighLevelILOperation.HLIL_ASSIGN:
                        continue
                    assign = refHlil.operands[1]
                    if assign.operation is HighLevelILOperation.HLIL_CONST:
                        prop["size"] = assign.value.value
                        break
            refList = list(bv.get_code_refs(prop["address"] + 0x20))
            for refEntry in refList:
                if refEntry.hlil is None:
                    continue
                refHlil = refEntry.hlil
                if refHlil.operation is not HighLevelILOperation.HLIL_ASSIGN:
                    continue
                assign = refHlil.operands[1]
                if assign.operation is HighLevelILOperation.HLIL_CALL:
                    prop["iid"] = str(assign.operands[0].tokens[0])
                elif assign.operation is HighLevelILOperation.HLIL_CONST_PTR:
                    prop["iid"] = str(assign.value.value)
            refList = list(bv.get_code_refs(prop["address"] + 0x28))
            for refEntry in refList:
                if refEntry.hlil is None:
                    continue
                refHlil = refEntry.hlil
                if refHlil.operation is not HighLevelILOperation.HLIL_ASSIGN:
                    continue
                assign = refHlil.operands[1]
                if assign.operation is HighLevelILOperation.HLIL_CONST_PTR:
                    prop["description"] = Tr2String(bv, assign.value.value)
            prop["name"] = Tr2String(bv, bv.read_pointer(prop["address"]))
            refList = list(bv.get_code_refs(prop["address"]))
            if prop["name"] == "":
                for refEntry in refList:
                    if refEntry.hlil is None:
                        continue
                    refHlil = refEntry.hlil
                    if refHlil.operation is not HighLevelILOperation.HLIL_ASSIGN:
                        continue
                    assign = refHlil.operands[1]
                    if assign.operation is HighLevelILOperation.HLIL_DEREF:
                        prop["name"] = Tr2String(bv, bv.read_pointer(assign.src.value.value))
            if prop["name"] in seen:
                continue
            seen.add(prop["name"])
            obj["props"].append(prop)
    data["types"][fn.address_ranges[0].start] = obj


class FindTask(BackgroundTaskThread):
    def __init__(self, bv, output_path):
        BackgroundTaskThread.__init__(self, "Finding Trinity Black Types", True)
        self.bv = bv
        self.path = output_path
        self.CLSID = [x.address for x in bv.get_symbols_by_name("Be::Clsid::Clsid")]
        self.IID = [x.address for x in bv.get_symbols_by_name("Be::IID::IID")]
        self.Init = [x.address for x in bv.get_symbols_by_name("BlueInitializePyType")]
        self.SetType = [x.address_ranges[0].start for x in bv.get_functions_by_name("BlackRegisterType")]
        self.SetInterface = [x.address_ranges[0].start for x in bv.get_functions_by_name("BlackSetInterface")]

    def run(self):
        data = {"id": {}, "clsid": {}, "types": {}}
        Tr2FindIIDs(self.bv, data, self.IID)
        Tr2FindCLSIDs(self.bv, data, self.CLSID)
        for fn in self.Init:
            for ref in self.bv.get_code_refs(fn):
                hlil = ref.hlil
                if hlil.operation is not HighLevelILOperation.HLIL_CALL:
                    continue
                try:
                    Tr2ProcessMethod(self.bv, ref.function, fn, self.SetType, self.SetInterface, data)
                except Exception as e:
                    log_error("Failed to process function at %s" % (ref))
                    log_error(e)
        with open(self.path, 'w') as jsonout:
             json.dump(data, jsonout)

def work(bv):
    typesDialog = interaction.SaveFileNameField("Save Target", "*.json", "types.json")
    if not interaction.get_form_input([typesDialog], "Save Target"):
        return
    task = FindTask(bv, typesDialog.result)
    task.start()

PluginCommand.register("Trinity", "DumpTypeTree", work)
