﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataTool.DataModels.Hero;
using DataTool.Flag;
using DataTool.Helper;
using TankLib;
using TankLib.STU.Types;
using TankLib.STU.Types.Enums;
using static DataTool.Helper.Logger;
using static DataTool.Helper.STUHelper;

namespace DataTool.SaveLogic.Unlock {
    public static class SkinTheme {
        public static void Save(ICLIFlags flags, string directory, DataModels.Unlock unlock, STUHero hero) {
            if (!(unlock.STU is STUUnlock_SkinTheme unlockSkinTheme)) return;

            var skinBase = GetInstance<STUSkinBase>(unlockSkinTheme.m_skinTheme);
            if (skinBase == null) return;

            if (hero == null) {
                LoudLog($"\tSkipping skin {unlock.Name}");
                LoudLog("\t\tCan not extract skin without a hero (thanks blizz)");
                return;
            }

            if (skinBase is STUSkinTheme skinTheme) {
                LoudLog($"\tExtracting skin {unlock.Name}");
                Save(flags, directory, skinTheme, hero);
            } else if (skinBase is STU_EF85B312 mythicSkin) {
                LoudLog($"\tExtracting mythic skin {unlock.Name}");
                MythicSkin.SaveMythicSkin(flags, directory, unlockSkinTheme.m_skinTheme, mythicSkin, hero);
            } else {
                throw new Exception($"wtf is a {skinBase.GetType()} when its at home");
            }
        }

        public static void Save(ICLIFlags flags, string directory, STU_63172E83 skin, STUHero hero) {
            STUSkinTheme skinTheme = GetInstance<STUSkinTheme>(skin.m_5E9665E3);
            if (skinTheme == null) return;
            LoudLog($"\tExtracting npc variant {IO.GetFileName(skin.m_5E9665E3)}");
            Save(flags, directory, skinTheme, hero);
        }

        public static Dictionary<ulong, ulong> FindEntities(FindLogic.Combo.ComboInfo info, STUSkinBase skin, STUHero hero) {
            Dictionary<ulong, ulong> replacements = GetReplacements(skin);

            FindLogic.Combo.Find(info, hero.m_gameplayEntity, replacements);
            info.SetEntityName(hero.m_gameplayEntity, "Gameplay3P");

            FindLogic.Combo.Find(info, hero.m_previewEmoteEntity, replacements);
            info.SetEntityName(hero.m_previewEmoteEntity, "PreviewEmote");

            FindLogic.Combo.Find(info, hero.m_322C521A, replacements);
            info.SetEntityName(hero.m_322C521A, "Showcase");

            FindLogic.Combo.Find(info, hero.m_26D71549, replacements);
            info.SetEntityName(hero.m_26D71549, "HeroGallery");

            FindLogic.Combo.Find(info, hero.m_8125713E, replacements);
            info.SetEntityName(hero.m_8125713E, "HighlightIntro");

            return replacements;
        }

        public static void Save(ICLIFlags flags, string directory, STUSkinBase skin, STUHero hero) {
            LoudLog("\t\tFinding");

            FindLogic.Combo.ComboInfo info = new FindLogic.Combo.ComboInfo();
            var replacements = FindEntities(info, skin, hero);
            FindSkinStuff(skin, hero, info, replacements);

            SaveCore(flags, directory, skin, info);
        }

        public static void SaveCore(ICLIFlags flags, string directory, STUSkinBase skin, FindLogic.Combo.ComboInfo info) {
            Dictionary<ulong, ulong> replacements = GetReplacements(skin);

            FindSoundFiles(flags, directory, replacements);

            LoudLog("\t\tSaving");
            var saveContext = new Combo.SaveContext(info);
            Combo.SaveLooseTextures(flags, Path.Combine(directory, "GUI"), saveContext);
            Combo.Save(flags, directory, saveContext);
            LoudLog("\t\tDone");
        }

        public static void FindSoundFiles(ICLIFlags flags, string directory, Dictionary<ulong, ulong> replacements) {
            string soundDirectory = Path.Combine(directory, "Sound");

            FindLogic.Combo.ComboInfo diffInfoBefore = new FindLogic.Combo.ComboInfo();
            FindLogic.Combo.ComboInfo diffInfoAfter = new FindLogic.Combo.ComboInfo();
            var diffInfoAfterContext = new Combo.SaveContext(diffInfoAfter); // todo: remove

            foreach (KeyValuePair<ulong, ulong> replacement in replacements) {
                uint diffReplacementType = teResourceGUID.Type(replacement.Value);
                if (diffReplacementType != 0x2C && diffReplacementType != 0x3F &&
                    diffReplacementType != 0xB2) continue; // no voice sets, use extract-hero-voice
                FindLogic.Combo.Find(diffInfoAfter, replacement.Value);
                FindLogic.Combo.Find(diffInfoBefore, replacement.Key);
            }

            foreach (KeyValuePair<ulong, FindLogic.Combo.VoiceSetAsset> voiceSet in diffInfoAfter.m_voiceSets) {
                if (diffInfoBefore.m_voiceSets.ContainsKey(voiceSet.Key)) continue;
                Combo.SaveVoiceSet(flags, soundDirectory, diffInfoAfterContext, voiceSet.Key);
            }

            foreach (KeyValuePair<ulong, FindLogic.Combo.SoundFileAsset> soundFile in diffInfoAfter.m_soundFiles) {
                if (diffInfoBefore.m_soundFiles.ContainsKey(soundFile.Key)) continue;
                Combo.SaveSoundFile(flags, soundDirectory, diffInfoAfterContext, soundFile.Key, false);
            }

            foreach (KeyValuePair<ulong, FindLogic.Combo.SoundFileAsset> soundFile in diffInfoAfter.m_voiceSoundFiles) {
                if (diffInfoBefore.m_voiceSoundFiles.ContainsKey(soundFile.Key)) continue;
                Combo.SaveSoundFile(flags, soundDirectory, diffInfoAfterContext, soundFile.Key, true);
            }
        }

        private static void FindSkinStuff(STUSkinBase skin, STUHero hero, FindLogic.Combo.ComboInfo info, Dictionary<ulong, ulong> skinReplacements) {
            Dictionary<ulong, ulong> replacements = GetReplacements(skin);

            if (skin is STUSkinTheme skinTheme) {
                info.m_processExistingEntities = true;
                List<Dictionary<ulong, ulong>> weaponReplacementStack = new List<Dictionary<ulong, ulong>>();

                var heroWeapons = new[] {
                    skinTheme.m_EBC69014
                };
                for (var index = 0; index < heroWeapons.Length; index++) {
                    var weaponOverrideGUID = heroWeapons[index];
                    STUHeroWeapon heroWeapon = GetInstance<STUHeroWeapon>(weaponOverrideGUID);
                    if (heroWeapon == null) continue;

                    Dictionary<ulong, ulong> weaponReplacements = GetReplacements(heroWeapon);

                    foreach (var (key, value) in skinReplacements) {
                        if (!weaponReplacements.ContainsKey(key)) {
                            weaponReplacements[key] = value;
                        }
                    }

                    SetPreviewWeaponNames(info, weaponReplacements, hero.m_previewWeaponEntities, index);
                    SetPreviewWeaponNames(info, weaponReplacements, hero.m_C2FE396F, index);

                    weaponReplacementStack.Add(weaponReplacements == null ? new Dictionary<ulong, ulong>() : weaponReplacements.Where(x => teResourceGUID.Type(x.Key) == 0x1A).ToDictionary(x => x.Key, x => x.Value));
                }

                for (var index = 0; index < weaponReplacementStack.Count; index++) {
                    foreach (var pair in weaponReplacementStack[index].Where(pair => pair.Key != pair.Value && info.m_modelLooks.ContainsKey(pair.Value) && info.m_modelLooks[pair.Value].m_name == null)) {
                        info.SetModelLookName(pair.Value, $"{(STUWeaponType)index:G}-{teResourceGUID.Index(pair.Value):X}");
                    }
                }

                info.m_processExistingEntities = false;

                FindLogic.Combo.Find(info, skinTheme.m_ECCC4A5D, replacements);
                info.SetTextureName(skinTheme.m_ECCC4A5D, "Portrait");
                info.SetTextureProcessIcon(skinTheme.m_ECCC4A5D);
                info.SetTextureFileType(skinTheme.m_ECCC4A5D, "png");
            }

            foreach (STU_1A496D3C tex in hero.m_8203BFE1) { // find GUI
                FindLogic.Combo.Find(info, tex.m_texture, replacements);
                info.SetTextureName(tex.m_texture, teResourceGUID.AsString(tex.m_id));
                info.SetTextureProcessIcon(tex.m_texture);
                info.SetTextureFileType(tex.m_texture, "png");
            }
        }

        private static void SetPreviewWeaponNames(FindLogic.Combo.ComboInfo info, Dictionary<ulong, ulong> weaponReplacements, STU_A0872511[] entities, int index) {
            if (entities == null) return;
            foreach (STU_A0872511 weaponEntity in entities) {
                FindLogic.Combo.Find(info, weaponEntity.m_entityDefinition, weaponReplacements);

                if (weaponEntity.m_loadout == 0) continue;
                Loadout loadout = Loadout.GetLoadout(weaponEntity.m_loadout);
                if (loadout == null) continue;
                info.SetEntityName(weaponEntity.m_entityDefinition, $"{loadout.Name}-{teResourceGUID.Index(weaponEntity.m_entityDefinition)}");

                if ((weaponReplacements == null || index == 0) && info.m_entities.TryGetValue(weaponEntity.m_entityDefinition, out var entity) && info.m_models.TryGetValue(entity.m_modelGUID, out var model)) {
                    foreach (var modellook in model.m_modelLooks) {
                        info.SetModelLookName(modellook, $"{(STUWeaponType) index:G}-{teResourceGUID.Index(modellook):X}");
                    }
                }
            }
        }

        /// <summary>
        /// Returns mapping of replacements for the skin theme, replacements can override whole voice sets or individual voice lines or any file really.
        /// Pass these replacements into Combo.Find to make sure you're getting the right files for a specific skin theme.
        /// </summary>
        public static Dictionary<ulong, ulong> GetReplacements(STUSkinBase skin) {
            if (skin == null) return null;

            var replacements = new Dictionary<ulong, ulong>();
            if (skin.m_runtimeOverrides != null) {
                foreach (var (key, value) in skin.m_runtimeOverrides) {
                    replacements[key] = value.m_3D884507;
                }
            }
            return replacements;
        }
    }
}