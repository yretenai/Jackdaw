struct FSDHEADER {
    u128 schemaChecksum;
    u64 dataChecksum; // checksum = 37; checksum = ((checksum * 54059) ^ (data[i + 0x18] * 76963)) for each byte
};

struct FSDSTRING {
    u64 offset;
    u64 length @ offset + 0x18;
    char text[length] @ offset + 0x20;
};

struct FSDARRAY<T> {
    u64 offset;
    u64 count @ offset + 0x18;
    T data[count] @ offset + 0x20;
};

struct FSDENTRY {
    // fill this out
};

struct FSDSLICEDATA {
    u64 count;

    FSDENTRY entries[count];
};

struct FSDSLICE {
    u64 offset;

    FSDSLICEDATA data @ offset + 0x18;
};

struct FSDFOOTER {
    u64 length;
    u64 headerSize;
    u64 numEntries;
    u64 numSlices;

    FSDSLICE slices[numSlices];
};

FSDHEADER Header @ 0;
FSDFOOTER Footer @ $;
