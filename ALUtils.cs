using AltLibrary.Common.AltBiomes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace AltLibrary
{
    internal static class ALUtils
    {
        internal static int AdvancedGetSizeOfCategory(string key, out LocalizedText[] texts)
        {
            int num = 0;
            List<LocalizedText> localizedTexts = new();
            for (int i = 0; i < 420; i++)
            {
                if (Language.Exists(key + "." + i.ToString()))
                {
                    localizedTexts.Add(Language.GetText(key + "." + i.ToString()));
                    num++;
                }
                else
                {
                    break;
                }
            }
            texts = localizedTexts.ToArray();
            return num;
        }

        internal static void ReplaceIDs<T>(ILContext il, T id, Func<T, T> replace, Func<T, bool> check, Func<T, T> replaceCheck = null) where T : IConvertible
        {
            ILCursor c = new(il);
            switch (id)
            {
                case int num:
                    {
                        while (c.TryGotoNext(i => i.MatchLdcI4(num) && i.Offset != 0))
                        {
                            c.Index++;
                            c.EmitDelegate<Func<T, T>>(orig => check.Invoke(orig) ? replace.Invoke(orig) : (replaceCheck == null ? orig : replaceCheck.Invoke(orig)));
                        }
                        break;
                    }
                case short num:
                    {
                        while (c.TryGotoNext(i => i.MatchLdcI4(num) && i.Offset != 0))
                        {
                            c.Index++;
                            c.EmitDelegate<Func<T, T>>(orig => check.Invoke(orig) ? replace.Invoke(orig) : (replaceCheck == null ? orig : replaceCheck.Invoke(orig)));
                        }
                        break;
                    }
                case ushort num:
                    {
                        while (c.TryGotoNext(i => i.MatchLdcI4(num) && i.Offset != 0))
                        {
                            c.Index++;
                            c.EmitDelegate<Func<T, T>>(orig => check.Invoke(orig) ? replace.Invoke(orig) : (replaceCheck == null ? orig : replaceCheck.Invoke(orig)));
                        }
                        break;
                    }
                default:
                    throw new ArgumentException("Invalid type: " + typeof(T).Name, nameof(id));
            }
        }

        public static bool IsWorldValid(UIWorldListItem self)
        {
            GetWorldData(self, out Dictionary<string, AltLibraryConfig.WorldDataValues> tempDict, out string path2);
            bool valid = true;
            if (tempDict.ContainsKey(path2))
            {
                if (tempDict[path2].worldHallow != "" && !ModContent.TryFind<AltBiome>(tempDict[path2].worldHallow, out _))
                {
                    valid = false;
                }
                if (tempDict[path2].worldEvil != "" && !ModContent.TryFind<AltBiome>(tempDict[path2].worldEvil, out _))
                {
                    valid = false;
                }
                if (tempDict[path2].worldHell != "" && !ModContent.TryFind<AltBiome>(tempDict[path2].worldHell, out _))
                {
                    valid = false;
                }
                if (tempDict[path2].worldJungle != "" && !ModContent.TryFind<AltBiome>(tempDict[path2].worldJungle, out _))
                {
                    valid = false;
                }
                if (tempDict[path2].drunkEvil != "Terraria/Corruption" && tempDict[path2].drunkEvil != "Terraria/Crimson" && tempDict[path2].drunkEvil != "" && !ModContent.TryFind<AltBiome>(tempDict[path2].drunkEvil, out _))
                {
                    valid = false;
                }
            }
            return valid;
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

        internal static void RemoveUntilInstruction(ILCursor c, Func<Instruction, bool> predicate)
        {
            while (!predicate.Invoke(c.Next))
            {
                c.Remove();
            }
        }

        internal static void DrawBoxedCursorTooltip(SpriteBatch spriteBatch, string text)
        {
            string[] array = Utils.WordwrapString(text, FontAssets.MouseText.Value, 460,
                10, out int lineAmount);
            lineAmount++;
            float num7 = 0f;
            for (int l = 0; l < lineAmount; l++)
            {
                float x = FontAssets.MouseText.Value.MeasureString(array[l]).X;
                if (num7 < x)
                {
                    num7 = x;
                }
            }

            if (num7 > 460f)
            {
                num7 = 460f;
            }

            Vector2 vector = new Vector2(Main.mouseX, Main.mouseY) + new Vector2(16f);
            vector += new Vector2(8f, 2f);
            if (vector.Y > Main.screenHeight - 30 * lineAmount)
            {
                vector.Y = Main.screenHeight - 30 * lineAmount;
            }

            if (vector.X > Main.screenWidth - num7)
            {
                vector.X = Main.screenWidth - num7;
            }

            var color = new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor,
                Main.mouseTextColor);
            color = Color.Lerp(color, Color.White, 1f);
            const int num8 = 10;
            const int num9 = 5;
            Utils.DrawInvBG(
                spriteBatch,
                new Rectangle((int)vector.X - num8, (int)vector.Y - num9, (int)num7 + num8 * 2,
                    30 * lineAmount + num9 + num9 / 2), new Color(23, 25, 81, 255) * 0.925f * 0.85f);
            for (int m = 0; m < lineAmount; m++)
            {
                Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, array[m], vector.X,
                    vector.Y + m * 30, color, Color.Black, Vector2.Zero);
            }
        }

        internal static Vector2 ToNearestPixel(this Vector2 vector) => new((int)vector.X, (int)vector.Y);
    }
}
