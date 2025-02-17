﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using TankLib;
using TankLib.STU.Types;
using static DataTool.Helper.IO;
using static DataTool.Helper.STUHelper;

namespace DataTool.DataModels.GameModes {
    [DataContract]
    public class Brawl {
        [DataMember]
        public teResourceGUID GUID;

        [DataMember]
        public string Name;

        [DataMember]
        public List<GameRuleset> Rulesets;

        [DataMember]
        public List<MapHeaderLite> Maps;

        [DataMember]
        public Achievement[] Achievements;

        [DataMember]
        public Unlock[] Unlocks;

        public Brawl(ulong key) {
            STU_2B8093CD stu = GetInstance<STU_2B8093CD>(key);
            if (stu == null) return;
            Init(stu, key);
        }

        public Brawl(STU_2B8093CD stu) {
            Init(stu);
        }

        private void Init(STU_2B8093CD brawl, ulong key = default) {
            GUID = (teResourceGUID) key;

            var brawlName = GetInstance<STU_4B259FE1>(brawl.m_A848F2C7);
            if (brawlName != null)
                Name = GetString(brawlName.m_name);

            // TODO: BROKEN BY OW2
            /*if (brawl.m_catalog != null) {
                var mapCatalog = GetInstance<STUMapCatalog>(brawl.m_catalog);
                if (mapCatalog?.m_headerGUIDs != null)
                    Maps = mapCatalog.m_headerGUIDs.Select(x => new MapHeader(x).ToLite()).ToList();
            }*/

            Achievements = brawl.m_ECCC6D23?.Select(x => new Achievement(x)).ToArray();
            Unlocks = brawl.m_B1449DF7?.Select(x => new Unlock(x)).ToArray();

            if (brawl.m_rulesets != null) {
                Rulesets = new List<GameRuleset>();

                foreach (var br in brawl.m_rulesets) {
                    var ruleset = new GameRuleset(br);
                    Rulesets.Add(ruleset);
                }
            }
        }
    }
}