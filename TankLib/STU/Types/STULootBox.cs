// Instance generated by TankLibHelper.InstanceBuilder
using TankLib.STU.Types.Enums;

// ReSharper disable All
namespace TankLib.STU.Types {
    [STUAttribute(0x56B6D12E, "STULootBox")]
    public class STULootBox : STUInstance {
        [STUFieldAttribute(0xCBE2DADD, "m_chestEntity")]
        public teStructuredDataAssetRef<STUEntityDefinition> m_chestEntity;

        [STUFieldAttribute(0xB2F9D222, "m_baseEntity")]
        public teStructuredDataAssetRef<STUEntityDefinition> m_baseEntity;

        [STUFieldAttribute(0x3970E137, "m_idleEffect")]
        public teStructuredDataAssetRef<STUEffect> m_idleEffect;

        [STUFieldAttribute(0xFFE7768F)]
        public teStructuredDataAssetRef<STUEffect> m_FFE7768F;

        [STUFieldAttribute(0xFEC3ED62)]
        public teStructuredDataAssetRef<STUEffect> m_FEC3ED62;

        [STUFieldAttribute(0x041CE51F, "m_modelLook")]
        public teStructuredDataAssetRef<STUModelLook> m_modelLook;

        [STUFieldAttribute(0x9B180535, "m_baseModelLook")]
        public teStructuredDataAssetRef<STUModelLook> m_baseModelLook;

        [STUFieldAttribute(0xB48F1D22, "m_name")]
        public teStructuredDataAssetRef<STUUXDisplayText> m_name;

        [STUFieldAttribute(0xE02BEE24, "m_celebration")]
        public teStructuredDataAssetRef<STUCelebration> m_celebration;

        [STUFieldAttribute(0xD75586C0, "m_shopCards", ReaderType = typeof(InlineInstanceFieldReader))]
        public STULootBoxShopCard[] m_shopCards;

        [STUFieldAttribute(0x3DFAC8CA)]
        public teStructuredDataAssetRef<STUUXDisplayText>[] m_3DFAC8CA;

        [STUFieldAttribute(0x7AB4E3F8, "m_lootboxType")]
        public Enum_BABC4175 m_lootboxType;

        [STUFieldAttribute(0x45C33D76)]
        public byte m_45C33D76;

        [STUFieldAttribute(0xFA2D81E7, "m_hidePucks")]
        public byte m_hidePucks;
    }
}
