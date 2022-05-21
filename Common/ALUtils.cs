using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria.GameContent.UI.Elements;
using Terraria.IO;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace AltLibrary.Common
{
    internal static class ALUtils
    {
        internal static void ReplaceIDs<T>(ILContext il, T id, Func<T, T> replace, Func<T, bool> check, Func<ILCursor, bool> extraPred = null)
        {
            ILCursor c = new(il);
            switch (id)
            {
                case int num:
                    {
                        while (c.TryGotoNext(i => i.MatchLdcI4(num) && (extraPred == null || extraPred.Invoke(c)) && i.Offset != 0))
                        {
                            c.Index++;
                            c.EmitDelegate<Func<T, T>>(orig => check.Invoke(orig) ? replace.Invoke(orig) : orig);
                        }
                        break;
                    }
                case short num:
                    {
                        while (c.TryGotoNext(i => i.MatchLdcI4(num) && (extraPred == null || extraPred.Invoke(c)) && i.Offset != 0))
                        {
                            c.Index++;
                            c.EmitDelegate<Func<T, T>>(orig => check.Invoke(orig) ? replace.Invoke(orig) : orig);
                        }
                        break;
                    }
                case ushort num:
                    {
                        while (c.TryGotoNext(i => i.MatchLdcI4(num) && (extraPred == null || extraPred.Invoke(c)) && i.Offset != 0))
                        {
                            c.Index++;
                            c.EmitDelegate<Func<T, T>>(orig => check.Invoke(orig) ? replace.Invoke(orig) : orig);
                        }
                        break;
                    }
                default:
                    throw new ArgumentException("Invalid type: " + typeof(T).Name, nameof(id));
            }
        }

        public static void GetWorldData(UIWorldListItem self, out Dictionary<string, AltLibraryConfig.WorldDataValues> tempDict, out string path2)
        {
            WorldFileData _data = (WorldFileData)typeof(UIWorldListItem).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self);
            path2 = Path.ChangeExtension(_data.Path, ".twld");
            tempDict = AltLibraryConfig.Config.GetWorldData();
            if (!tempDict.ContainsKey(path2))
            {
                if (!FileUtilities.Exists(path2, _data.IsCloudSave))
                {
                    return;
                }
                byte[] buf = FileUtilities.ReadAllBytes(path2, _data.IsCloudSave);
                if (buf[0] != 31 || buf[1] != 139)
                {
                    return;
                }
                var stream = new MemoryStream(buf);
                var tag = TagIO.FromStream(stream);
                bool containsMod = false;
                if (tag.ContainsKey("modData"))
                {
                    foreach (TagCompound modDataTag in tag.GetList<TagCompound>("modData").Skip(2))
                    {
                        if (modDataTag.Get<string>("mod") == AltLibrary.Instance.Name)
                        {
                            TagCompound dataTag = modDataTag.Get<TagCompound>("data");
                            AltLibraryConfig.WorldDataValues worldData;
                            worldData.worldEvil = dataTag.Get<string>("AltLibrary:WorldEvil");
                            worldData.worldHallow = dataTag.Get<string>("AltLibrary:WorldHallow");
                            worldData.worldHell = dataTag.Get<string>("AltLibrary:WorldHell");
                            worldData.worldJungle = dataTag.Get<string>("AltLibrary:WorldJungle");
                            worldData.drunkEvil = dataTag.Get<string>("AltLibrary:DrunkEvil");
                            tempDict[path2] = worldData;
                            containsMod = true;
                            break;
                        }
                    }
                    if (!containsMod)
                    {
                        AltLibraryConfig.WorldDataValues worldData;
                        worldData.worldHallow = "";
                        worldData.worldEvil = "";
                        worldData.worldHell = "";
                        worldData.worldJungle = "";
                        worldData.drunkEvil = "";
                        tempDict[path2] = worldData;
                    }
                    AltLibraryConfig.Config.SetWorldData(tempDict);
                    AltLibraryConfig.Save(AltLibraryConfig.Config);
                }
            }
        }
    }
}
