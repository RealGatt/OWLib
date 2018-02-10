// File auto generated by STUHashTool

using STULib.Types.Generic;

namespace STULib.Types {
    [STU(0x6FE91269)]
    public class STULevelPortrait : STUUnlock {
        [STUField(0x2C01908B, "m_level")]
        public ushort Level;

        [STUField(0x8F736177, "m_rank")]
        public Enums.STUEnumPortraitTier Tier;

        [STUField(0x949D9C2A)]
        public Common.STUGUID BorderImage;

        [STUField(0x78A2AC5C, "m_stars")]
        public ushort Star;

        [STUField(0xA4A66AB6)]
        public Common.STUGUID StarImage;

        // todo: ImageResource => BorderResource, PortraitResource
    }
}
