﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using TACTLib;
using TankLib;
using TankLib.STU;
using TankLib.STU.Types;
using TankLib.STU.Types.Enums;
using static DataTool.Helper.STUHelper;
using static DataTool.Helper.IO;

namespace DataTool.DataModels {
    public class Unlock {
        public teResourceGUID GUID;

        /// <summary>
        /// Name of this unlock
        /// </summary>
        public string Name;

        /// <summary>
        /// DataTool enum for the type of Unlock
        /// </summary>
        public UnlockType Type;

        /// <summary>
        /// Unlock rarity
        /// </summary>
        public STUUnlockRarity Rarity;

        /// <summary>
        /// Description of this unlock
        /// </summary>
        public string Description;

        /// <summary>
        /// Where this unlock can be obtained from
        /// </summary>
        /// <example>"Available in Shop"</example>
        public string AvailableIn;

        /// <summary>
        /// Battle.net Product Id
        /// </summary>
        public long ProductId;

        /// <summary>
        /// If the Unlock is a Skin, the GUID of the SkinTheme
        /// </summary>
        public teResourceGUID SkinThemeGUID;

        /// <summary>
        /// If the Unlock is a Hero, the GUID of the Hero
        /// </summary>
        public teResourceGUID HeroGUID;

        /// <summary>
        /// If this unlock belongs to an ESports Team
        /// </summary>
        public bool IsEsportsUnlock;

        /// <summary>
        /// If this unlock belongs to an ESports Team, the name of the team
        /// </summary>
        public string EsportsTeam;

        /// <summary>
        /// Array of categories the Unlock belongs to that the Hero Gallery & Career Profile filtering options use
        /// </summary>
        public string[] Categories;

        /// <summary>
        ///  If the Unlock is a form of Currency or XP, the amount granted
        /// </summary>
        public int? Amount;

        public Enum_BABC4175 LootBoxType;

        /// <summary>
        /// Whether this is a "normal" Unlock like a skin, emote, voiceline, pose, icon, etc and not something like a Lootbox or Currency.
        /// </summary>
        [IgnoreDataMember]
        public bool IsTraditionalUnlock;

        [IgnoreDataMember]
        public STU_3021DDED STU;

        // These types are specific to certain unlocks so don't show them unless we're on that unlock
        public bool ShouldSerializeLootBoxType() => Type == UnlockType.Lootbox;
        public bool ShouldSerializeSkinThemeGUID() => Type == UnlockType.Skin;
        public bool ShouldSerializeHeroGUID() => Type == UnlockType.Hero;
        public bool ShouldSerializeIsEsportsUnlock() => IsEsportsUnlock || Type == UnlockType.Skin;
        public bool ShouldSerializeEsportsTeam() => IsEsportsUnlock;
        public bool ShouldSerializeAmount() => Amount != null;

        // These only really apply to "normal" unlocks and can be removed from others
        public bool ShouldSerializeAvailableIn() => IsTraditionalUnlock;
        public bool ShouldSerializeCategories() => IsTraditionalUnlock;

        public Unlock(STU_3021DDED unlock, ulong guid) {
            Init(unlock, guid);
        }

        public Unlock(ulong guid) {
            var unlock = GetInstance<STU_3021DDED>(guid);
            if (unlock == null) return;
            Init(unlock, guid);
        }

        private void Init(STU_3021DDED unlock, ulong guid) {
            GUID = (teResourceGUID) guid;
            STU = unlock;

            Name = GetCleanString(unlock.m_name);
            AvailableIn = GetString(unlock.m_53145FAF);
            Rarity = unlock.m_rarity;
            Description = GetDescriptionString(unlock.m_3446F580);
            Type = GetTypeForUnlock(unlock);
            ProductId = unlock.m_00B16A0B;

            IsTraditionalUnlock =
                Type == UnlockType.Icon || Type == UnlockType.Spray ||
                Type == UnlockType.Skin || Type == UnlockType.HighlightIntro ||
                Type == UnlockType.VictoryPose || Type == UnlockType.VoiceLine ||
                Type == UnlockType.Emote || Type == UnlockType.Souvenir ||
                Type == UnlockType.NameCard || Type == UnlockType.PlayerTitle ||
                Type == UnlockType.WeaponCharm || Type == UnlockType.WeaponSkin;

            if (unlock.m_BEE9BCDA != null) {
                Categories = unlock.m_BEE9BCDA
                    .Where(x => x.GUID != 0)
                    .Select(x => GetGUIDName(x.GUID)).ToArray();
            }

            // Lootbox and currency unlocks have some additional relevant data
            switch (unlock) {
                case STUUnlock_CompetitiveCurrency stu:
                    Amount = stu.m_760BF18E;
                    break;
                case STUUnlock_Currency stu:
                    Amount = stu.m_currency;
                    break;
                case STUUnlock_OWLToken stu:
                    Amount = stu.m_63A026AF;
                    break;
                case STUUnlock_LootBox stu:
                    Rarity = stu.m_2F922165;
                    LootBoxType = stu.m_lootboxType;
                    break;
                case STUUnlock_SkinTheme stu:
                    SkinThemeGUID = stu.m_skinTheme;
                    break;
                case STU_C3C6FD9E stu:
                    HeroGUID = stu.m_hero;
                    break;
                case STU_514C0F6B stu:
                    Amount = (int) stu.m_amount;
                    break;
                case STU_7A1A4764 stu:
                    Amount = stu.m_E0A45C1B;
                    break;
            }

            if (unlock.m_0B1BA7C1 != null) {
                var teamDefinition = new TeamDefinition(unlock.m_0B1BA7C1);
                if (teamDefinition.Id != 0) {
                    IsEsportsUnlock = true;
                    EsportsTeam = teamDefinition.FullName;
                }
            }
        }

        public string GetName() {
            return Name ?? GetFileName(GUID);
        }

        /// <summary>
        /// Returns the UnlockType for an Unlock
        /// </summary>
        /// <param name="unlock">Source unlock</param>
        private static UnlockType GetTypeForUnlock(STUUnlock unlock) {
            return GetUnlockType(unlock.GetType());
        }

        /// <summary>
        /// Returns the UnlockType for a STUUnlock Type
        /// </summary>
        /// <param name="type">unlock stu type</param>
        public static UnlockType GetUnlockType(Type type) {
            if (type == typeof(STUUnlock_SkinTheme)) {
                return UnlockType.Skin;
            }

            if (type == typeof(STUUnlock_AvatarPortrait)) {
                return UnlockType.Icon;
            }

            if (type == typeof(STUUnlock_Emote)) {
                return UnlockType.Emote;
            }

            if (type == typeof(STUUnlock_Pose)) {
                return UnlockType.VictoryPose;
            }

            if (type == typeof(STUUnlock_VoiceLine)) {
                return UnlockType.VoiceLine;
            }

            if (type == typeof(STUUnlock_SprayPaint)) {
                return UnlockType.Spray;
            }

            if (type == typeof(STUUnlock_Currency)) {
                return UnlockType.Currency;
            }

            if (type == typeof(STUUnlock_PortraitFrame)) {
                return UnlockType.PortraitFrame;
            }

            if (type == typeof(STUUnlock_Weapon)) {
                return UnlockType.WeaponSkin;
            }

            if (type == typeof(STUUnlock_POTGAnimation)) {
                return UnlockType.HighlightIntro;
            }

            if (type == typeof(STUUnlock_CompetitiveCurrency)) {
                return UnlockType.CompetitiveCurrency;
            }

            if (type == typeof(STUUnlock_OWLToken)) {
                return UnlockType.OWLToken;
            }

            if (type == typeof(STUUnlock_LootBox)) {
                return UnlockType.Lootbox;
            }

            if (type == typeof(STU_6A808718)) {
                return UnlockType.WeaponCharm;
            }

            if (type == typeof(STU_C3C6FD9E)) {
                return UnlockType.Hero;
            }

            if (type == typeof(STU_7A1A4764)) {
                return UnlockType.BattlePassXP;
            }

            if (type == typeof(STU_A458D547)) {
                return UnlockType.Souvenir;
            }

            if (type == typeof(STU_DB1B05B5)) {
                return UnlockType.NameCard;
            }

            if (type == typeof(STU_514C0F6B)) {
                return UnlockType.OverwatchCoins;
            }

            if (type == typeof(STU_52AB57E9)) {
                return UnlockType.PlayerTitle;
            }

            if (type == typeof(STU_AD84E2AA)) {
                return UnlockType.BattlePass;
            }

            if (type == typeof(STU_184D5944)) {
                return UnlockType.SeasonXPBoost;
            }

            // some lootbox thing
            if (type == typeof(STU_1EB22BDB)) {
                return UnlockType.Unknown;
            }

            // battlepass tier skip thing
            if (type == typeof(STU_80C1169E)) {
                return UnlockType.Unknown;
            }

            Logger.Debug("Unlock", $"Unknown unlock type ${type}");
            return UnlockType.Unknown;
        }

        /// <summary>
        /// Get an array of <see cref="Unlock"/> from an array of GUIDs
        /// </summary>
        /// <param name="guids">GUID collection</param>
        /// <returns>Array of <see cref="Unlock"/></returns>
        public static Unlock[] GetArray(IEnumerable<ulong> guids) {
            if (guids == null) return new Unlock[0];
            List<Unlock> unlocks = new List<Unlock>();
            foreach (ulong guid in guids) {
                STU_3021DDED stu = GetInstance<STU_3021DDED>(guid);
                if (stu == null) continue;
                Unlock unlock = new Unlock(stu, guid);
                unlocks.Add(unlock);
            }

            return unlocks.ToArray();
        }

        /// <summary>Get an array of <see cref="Unlock"/> from STUUnlocks</summary>
        /// <inheritdoc cref="GetArray(System.Collections.Generic.IEnumerable{ulong})"/>
        public static Unlock[] GetArray(STUUnlocks unlocks) {
            return GetArray(unlocks?.m_unlocks);
        }

        public static Unlock[] GetArray(teStructuredDataAssetRef<STUUnlock>[] unlocks) {
            return GetArray(unlocks?.Select(x => (ulong) x));
        }

        public UnlockLite ToLiteUnlock() {
            return UnlockLite.FromUnlock(this);
        }
    }

    public class UnlockLite {
        public teResourceGUID GUID;
        public string Name;
        public UnlockType Type;
        public STUUnlockRarity Rarity;
        public int? Amount;

        public bool ShouldSerializeAmount() => Amount != null;

        public static UnlockLite FromUnlock(Unlock unlock) {
            return new UnlockLite {
                GUID = unlock.GUID,
                Name = unlock.Name,
                Type = unlock.Type,
                Rarity = unlock.Rarity,
                Amount = unlock.Amount
            };
        }
    }

    public enum UnlockType {
        Unknown, // :david:
        Skin,
        Icon,
        Spray,
        Emote,
        VictoryPose,
        HighlightIntro,
        VoiceLine,
        WeaponSkin,
        Lootbox,
        PortraitFrame, // borders
        Currency, // legacy credits
        CompetitiveCurrency, // competitive points
        OWLToken,
        OverwatchCoins,
        SeasonXPBoost,
        WeaponCharm,
        PlayerTitle,
        BattlePass,
        BattlePassXP,
        Souvenir,
        NameCard,
        Hero,
    }
}