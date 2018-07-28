// Instance generated by TankLibHelper.InstanceBuilder

// ReSharper disable All
namespace TankLib.STU.Types {
    [STUAttribute(0x07142C70)]
    public class STU_07142C70 : STUStatescriptScreenVM {
        [STUFieldAttribute(0x67C1C70F)]
        public teString m_67C1C70F;

        [STUFieldAttribute(0xC08C4427, "m_name")]
        public teString m_name;

        [STUFieldAttribute(0x12716D08, "m_type")]
        public teString m_type;

        [STUFieldAttribute(0xBFB85D17)]
        public teString m_BFB85D17;

        [STUFieldAttribute(0xFD53ECB8, "m_rarity")]
        public teStructuredDataAssetRef<STUUXLink> m_rarity;

        [STUFieldAttribute(0xE75BEDA5, "m_heroGUID")]
        public ulong m_heroGUID;

        [STUFieldAttribute(0x32BC347F, "m_unlockGUID")]
        public ulong m_unlockGUID;

        [STUFieldAttribute(0x79DCD591, "m_hoverUnlock", ReaderType = typeof(EmbeddedInstanceFieldReader))]
        public STU_A0BDC3D3 m_hoverUnlock;

        [STUFieldAttribute(0x2A0F81E0, "m_previewUnlock", ReaderType = typeof(EmbeddedInstanceFieldReader))]
        public STU_A0BDC3D3 m_previewUnlock;

        [STUFieldAttribute(0xCC84D865, "m_index")]
        public int m_index;

        [STUFieldAttribute(0xF2B48985)]
        public int m_F2B48985;

        [STUFieldAttribute(0xF06C6608)]
        public float m_F06C6608;

        [STUFieldAttribute(0xCAED3A4C, "m_isDuplicate")]
        public byte m_isDuplicate;

        [STUFieldAttribute(0x1F72D39F)]
        public byte m_1F72D39F;
    }
}
