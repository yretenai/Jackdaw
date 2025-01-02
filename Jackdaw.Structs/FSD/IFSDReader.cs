namespace Jackdaw.Structs.FSD;

public interface IFSDReader {
	public int Offset { get; set; }
	public T Read<T>() where T : struct;
	public T[] ReadArray<T>() where T : struct;
	public string ReadString();
	public string[] ReadStringArray();
	public T ReadClass<T>() where T : IFSDValue<T>;
	public T[] ReadClassArray<T>() where T : IFSDValue<T>;
	public Dictionary<object, T> ReadClassDict<T>() where T : IFSDValue<T>, IFSDDict;
	void Align();
}

public interface IFSDValue<out T> {
	public static abstract T Read(IFSDReader reader);
}

public interface IFSDDict {
	public object Key { get; }
}
