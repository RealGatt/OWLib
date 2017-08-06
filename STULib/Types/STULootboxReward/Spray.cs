﻿using OWLib.Types;
using static STULib.Types.Generic;

namespace STULib.Types.STULootboxReward {
    [System.Diagnostics.DebuggerDisplay(STU.DEBUG_STR)]
    [STU(Checksum = 0x15720E8A)]
    public class Spray : Cosmetic {
        public ulong Unknown1;
        public OWRecord EffectResource;
        public OWRecord DecalResource;
        public ulong Unknown2;
    }

    [System.Diagnostics.DebuggerDisplay(STU.DEBUG_STR)]
    [STU(Checksum = 0x6DA011A1)]
    public class SprayMipmap  : STUInstance {
        public OWRecord EffectResource;
        public OWRecord DecalResource;
    }
}
