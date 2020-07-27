﻿ using System.Collections.Generic;
 using System.Runtime.Serialization;
 using TankLib.STU.Types;

namespace DataTool.DataModels {
    [DataContract]
    public class PlayerProgression {
        [DataMember]
        public LootBoxUnlocks[] LootBoxesUnlocks;
        
        [DataMember]
        public AdditionalUnlocks[] AdditionalUnlocks;
        
        [DataMember]
        public Unlock[] OtherUnlocks;
        
        public PlayerProgression(STUGenericSettings_PlayerProgression progression) {
            if (progression.m_lootBoxesUnlocks != null) {
                LootBoxesUnlocks = new LootBoxUnlocks[progression.m_lootBoxesUnlocks.Length];

                for (int i = 0; i < progression.m_lootBoxesUnlocks.Length; i++) {
                    STULootBoxUnlocks lootBoxUnlocks = progression.m_lootBoxesUnlocks[i];
                    LootBoxesUnlocks[i] = new LootBoxUnlocks(lootBoxUnlocks);
                }
            }

            if (progression.m_additionalUnlocks != null) {
                AdditionalUnlocks = new AdditionalUnlocks[progression.m_additionalUnlocks.Length];
                for (int i = 0; i < progression.m_additionalUnlocks.Length; i++) {
                    AdditionalUnlocks[i] = new AdditionalUnlocks(progression.m_additionalUnlocks[i]);
                }
            }
            
            OtherUnlocks = Unlock.GetArray(progression.m_otherUnlocks);
        }
        
        public IEnumerable<Unlock> IterateUnlocks() {
            if (LootBoxesUnlocks != null) {
                foreach (LootBoxUnlocks lootBoxUnlocks in LootBoxesUnlocks) {
                    foreach (Unlock VARIABLE in lootBoxUnlocks.Unlocks) {
                        yield return VARIABLE;
                    }
                }
            }
            if (AdditionalUnlocks != null) {
                foreach (AdditionalUnlocks additionalUnlock in AdditionalUnlocks) {
                    foreach (Unlock unlock in additionalUnlock.Unlocks) {
                        yield return unlock;
                    }
                }
            }
            if (OtherUnlocks != null) {
                foreach (Unlock otherUnlock in OtherUnlocks) {
                    yield return otherUnlock;
                }
            }
        }
    }
    
    /// <summary>
    /// Additional Unlocks data model
    /// </summary>
    [DataContract]
    public class AdditionalUnlocks {
        /// <summary>
        /// Unlocks
        /// </summary>
        [DataMember]
        public Unlock[] Unlocks;
        
        /// <summary>
        /// Level unlocked at
        /// </summary>
        [DataMember]
        public uint Level;

        public AdditionalUnlocks(STUAdditionalUnlocks additionalUnlocks) {
            Level = additionalUnlocks.m_level;
            Unlocks = Unlock.GetArray(additionalUnlocks.m_unlocks);
        }
    }
}