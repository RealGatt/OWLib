﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using DataTool.FindLogic;
using DataTool.Flag;
using TankLib;
using TACTLib.Container;
using TACTLib.Core.Product.Tank;
using static DataTool.Program;

namespace DataTool.ToolLogic.Extract.Debug {
    [Tool("extract-debug-newents", Description = "Extract new entities (debug)", TrackTypes = new ushort[] {}, CustomFlags = typeof(ExtractFlags), IsSensitive = true)]
    public class ExtractDebugNewEntities : ITool {
        public void IntegrateView(object sender) {
            throw new NotImplementedException();
        }

        public void Parse(ICLIFlags toolFlags) {
            ExtractNewEntities(toolFlags);
        }
        
        public void AddNewByGUID(Combo.ComboInfo info, List<ulong> lastVerGuids, ushort type) {
            foreach (ulong key in TrackedFiles[type]) {
                if (lastVerGuids.Contains(key)) continue;
                Combo.Find(info, key);
            }
        }

        public void AddNewByContentHash(Combo.ComboInfo info, HashSet<CKey> contentHashes, params ushort[] types) {
            foreach (KeyValuePair<ulong,ProductHandler_Tank.Asset> asset in TankHandler.Assets) {
                TankHandler.UnpackAsset(asset.Value, out var manifest, out var package, out var record);
                
                if (!manifest.ContentManifest.TryGet(record.GUID, out var cmfData)) {
                    throw new FileNotFoundException();
                }
                
                ushort fileType = teResourceGUID.Type(asset.Key);
                if (fileType == 0x9C) continue;  // bundle
                if (fileType == 0x77) continue;  // package
                
                if (!types.Contains(fileType)) continue;
                if (contentHashes.Contains(cmfData.ContentKey)) continue;

                if (fileType == 0x4) {
                    var locale = teResourceGUID.Locale(asset.Key);
                    if (locale == 0xF) continue; // ? 
                    if (locale == 0x1F) continue; // ? 
                    if (locale == 0x2F) continue; // ? 
                    if (locale == 0x3F) continue; // ? 
                    if (locale == 0x4F) continue; // ? 
                    if (locale == 0x5F) continue; // ? 
                    if (teResourceGUID.Platform(asset.Key) == 0x8) continue; // effect images
                }
                
                Combo.Find(info, asset.Key);
            }
        }

        public static HashSet<CKey> GetContentHashes(string path) {
            using (StreamReader reader = new StreamReader(path)) {
                return new HashSet<CKey>(reader.ReadToEnd().Split('\n').Select(x => x.TrimEnd('\r'))
                                               .Where(x => !string.IsNullOrWhiteSpace(x))
                                               .Select(CKey.FromString), CASCKeyComparer.Instance);
            }
        }
        
        public static HashSet<ulong> GetGUIDs(string path) {
            using (StreamReader reader = new StreamReader(path)) {
                return new HashSet<ulong>(reader.ReadToEnd().Split('\n').Select(x => x.TrimEnd('\r'))
                                                .Where(x => !string.IsNullOrWhiteSpace(x))
                                                .Select(x => ulong.Parse(x, NumberStyles.HexNumber)));
            }
        }

        public void ExtractNewEntities(ICLIFlags toolFlags) {
            string basePath;
            
            if (toolFlags is ExtractFlags flags) {
                basePath = flags.OutputPath;
            } else {
                throw new Exception("no output path");
            }
            
            var contentHashes = GetContentHashes(@"D:\ow\resources\verdata\50951.cmfhashes");

            const string container = "DebugNewEntities3";
            
            Combo.ComboInfo info = new Combo.ComboInfo();
            AddNewByContentHash(info, contentHashes, 0x7C);
            
            SaveLogic.Combo.Save(flags, Path.Combine(basePath, container), info);
            SaveLogic.Combo.SaveAllSoundFiles(flags, Path.Combine(basePath, container, "Sounds"), info);
            SaveLogic.Combo.SaveAllVoiceSoundFiles(flags, Path.Combine(basePath, container, "VoiceSounds"), info);
            SaveLogic.Combo.SaveLooseTextures(flags, Path.Combine(basePath, container, "LooseTex"), info);
            SaveLogic.Combo.SaveAllStrings(flags, Path.Combine(basePath, container, "Strings"), info);
        }
    }
}