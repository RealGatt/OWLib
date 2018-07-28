// Instance generated by TankLibHelper.InstanceBuilder
using TankLib.Math;

// ReSharper disable All
namespace TankLib.STU.Types {
    [STUAttribute(0x3BDD30BE)]
    public class STU_3BDD30BE : STUInstance {
        [STUFieldAttribute(0xAA8E1BB0, "m_targetTags")]
        public teStructuredDataAssetRef<STUTargetTag>[] m_targetTags;

        [STUFieldAttribute(0x06B5A4F1, ReaderType = typeof(EmbeddedInstanceFieldReader))]
        public STU_FC28C044 m_06B5A4F1;

        [STUFieldAttribute(0x9352A840, "m_direction")]
        public teVec3 m_direction;
    }
}
