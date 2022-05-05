using AltLibrary.Common.AltBiomes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Terraria.Utilities;

namespace AltLibrary.Common.Hooks
{
    internal static class WorldIcons
    {
        internal static int WarnUpdate = 0;

        public static void Init()
        {
            IL.Terraria.GameContent.UI.Elements.UIWorldListItem.ctor += UIWorldListItem_ctor;
            IL.Terraria.GameContent.UI.Elements.UIWorldListItem.DrawSelf += UIWorldListItem_DrawSelf1;
            On.Terraria.GameContent.UI.Elements.UIWorldListItem.DrawSelf += UIWorldListItem_DrawSelf;
            IL.Terraria.GameContent.UI.Elements.UIWorldListItem.DrawPanel += UIWorldListItem_DrawPanel;
        }

        private static void UIWorldListItem_DrawPanel(ILContext il)
        {
            ILCursor c = new(il);
            for (int j = 0; j < 3; j++)
            {
                if (!c.TryGotoNext(i => i.MatchCall<Color>("get_White")))
                    return;
                c.Index++;
                c.Emit(OpCodes.Ldarg, 0);
                c.EmitDelegate<Func<Color, UIWorldListItem, Color>>((value, self) =>
                {
                    if ((WorldFileData)typeof(UIWorldListItem).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self) == null)
                        return value;
                    WorldFileData _data = (WorldFileData)typeof(UIWorldListItem).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self);
                    UIImage _worldIcon = (UIImage)typeof(UIWorldListItem).GetField("_worldIcon", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self);
                    string path2 = Path.ChangeExtension(_data.Path, ".twld");
                    Dictionary<string, AltLibraryConfig.WorldDataValues> tempDict = AltLibraryConfig.Config.GetWorldData();
                    if (!tempDict.ContainsKey(path2))
                    {
                        if (!FileUtilities.Exists(path2, _data.IsCloudSave))
                        {
                            return value;
                        }
                        byte[] buf = FileUtilities.ReadAllBytes(path2, _data.IsCloudSave);
                        if (buf[0] != 31 || buf[1] != 139)
                        {
                            return value;
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
                        if (tempDict[path2].drunkEvil != "" && !ModContent.TryFind<AltBiome>(tempDict[path2].drunkEvil, out _))
                        {
                            valid = false;
                        }
                    }

                    if (!valid)
                        return Color.Gray;
                    return value;
                });
            }
        }

        private static void UIWorldListItem_DrawSelf1(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchCall<Color>("get_White")))
                return;

            c.Index++;
            c.Emit(OpCodes.Ldarg, 0);
            c.EmitDelegate<Func<Color, UIWorldListItem, Color>>((value, self) =>
            {
                if ((WorldFileData)typeof(UIWorldListItem).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self) == null)
                    return value;
                WorldFileData _data = (WorldFileData)typeof(UIWorldListItem).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self);
                UIImage _worldIcon = (UIImage)typeof(UIWorldListItem).GetField("_worldIcon", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self);
                string path2 = Path.ChangeExtension(_data.Path, ".twld");
                Dictionary<string, AltLibraryConfig.WorldDataValues> tempDict = AltLibraryConfig.Config.GetWorldData();
                if (!tempDict.ContainsKey(path2))
                {
                    if (!FileUtilities.Exists(path2, _data.IsCloudSave))
                    {
                        return value;
                    }
                    byte[] buf = FileUtilities.ReadAllBytes(path2, _data.IsCloudSave);
                    if (buf[0] != 31 || buf[1] != 139)
                    {
                        return value;
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
                    if (tempDict[path2].drunkEvil != "" && !ModContent.TryFind<AltBiome>(tempDict[path2].drunkEvil, out _))
                    {
                        valid = false;
                    }
                }

                if (!valid)
                    return Color.Red;
                return value;
            });
        }

        private static void UIWorldListItem_ctor(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchLdftn(out _)))
                return;
            FieldReference fieldReference = null;
            if (!c.TryGotoNext(i => i.MatchLdfld(out fieldReference)))
                return;
            if (!c.TryGotoNext(i => i.MatchCall(out _)))
                return;
            c.Index++;
            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Ldfld, fieldReference);
            c.EmitDelegate<Action<UIWorldListItem, UIImage>>((uiWorldListItem, _worldIcon) =>
            {
                WorldFileData data = (WorldFileData)typeof(UIWorldListItem).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(uiWorldListItem);
                string path2 = Path.ChangeExtension(data.Path, ".twld");
                Dictionary<string, AltLibraryConfig.WorldDataValues> tempDict = AltLibraryConfig.Config.GetWorldData();
                if (!tempDict.ContainsKey(path2))
                {
                    if (!FileUtilities.Exists(path2, data.IsCloudSave))
                    {
                        return;
                    }
                    byte[] buf = FileUtilities.ReadAllBytes(path2, data.IsCloudSave);
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
                ReplaceIcons(data, ref _worldIcon);
                LayeredIcons("Normal", data, ref _worldIcon, tempDict, path2);
                LayeredIcons(data, ref _worldIcon, tempDict, path2);
                LayeredIcons("ForTheWorthy", data, ref _worldIcon, tempDict, path2);
                LayeredIcons("NotTheBees", data, ref _worldIcon, tempDict, path2);
                LayeredIcons("Anniversary", data, ref _worldIcon, tempDict, path2);
                LayeredIcons("DontStarve", data, ref _worldIcon, tempDict, path2);
            });
        }

        /// <summary>
        /// For drunk icons
        /// </summary>
        /// <param name="data"></param>
        /// <param name="image"></param>
        /// <param name="tempDict"></param>
        /// <param name="path2"></param>
        private static void LayeredIcons(WorldFileData data, ref UIImage image, Dictionary<string, AltLibraryConfig.WorldDataValues> tempDict, string path2)
        {
            Dictionary<string, Func<WorldFileData, bool>> assets = new();
            bool bl2 = data.DrunkWorld && tempDict.ContainsKey(path2);
            if (data.HasCrimson && bl2 && tempDict[path2].worldEvil == "" && ModContent.RequestIfExists("AltLibrary/Assets/WorldIcons/DrunkBase/Crimson", out Asset<Texture2D> newAsset2))
            {
                UIImage worldIcon = new(newAsset2);
                worldIcon.Height.Set(0, 1);
                worldIcon.Width.Set(0, 1);
                image.Append(worldIcon);
            }
            if (data.HasCorruption && bl2 && tempDict[path2].worldEvil == "" && ModContent.RequestIfExists("AltLibrary/Assets/WorldIcons/DrunkBase/Corruption", out Asset<Texture2D> newAsset3))
            {
                UIImage worldIcon = new(newAsset3);
                worldIcon.Height.Set(0, 1);
                worldIcon.Width.Set(0, 1);
                image.Append(worldIcon);
            }
            if (data.HasCorruption && bl2 && tempDict[path2].worldEvil != "" && ModContent.RequestIfExists(ModContent.Find<AltBiome>(tempDict[path2].worldEvil).IconLarge + "DrunkBase", out Asset<Texture2D> newAsset1))
            {
                UIImage worldIcon = new(newAsset1);
                worldIcon.Height.Set(0, 1);
                worldIcon.Width.Set(0, 1);
                image.Append(worldIcon);
            }

            assets.Add("Corrupt", (ourData) =>
            {
                return ourData.DrunkWorld && tempDict.ContainsKey(path2) && tempDict[path2].drunkEvil == "Terraria/Corruption";
            });
            assets.Add("Crimson", (ourData) =>
            {
                return ourData.DrunkWorld && tempDict.ContainsKey(path2) && tempDict[path2].drunkEvil == "Terraria/Crimson";
            });
            foreach (AltBiome biome in AltLibrary.biomes)
            {
                if (biome.BiomeType == BiomeType.Evil)
                {
                    assets.Add(biome.FullName, (ourData) =>
                    {
                        return ourData.DrunkWorld && tempDict.ContainsKey(path2) && tempDict[path2].drunkEvil == biome.FullName;
                    });
                }
            }
            assets.Add("Hallow", (ourData) =>
            {
                return ourData.DrunkWorld && ourData.IsHardMode && tempDict.ContainsKey(path2) && tempDict[path2].worldHallow == "";
            });
            foreach (AltBiome biomes in AltLibrary.biomes)
            {
                if (!biomes.FullName.StartsWith("Terraria/") && biomes.BiomeType != BiomeType.Evil)
                {
                    AltBiome biome = ModContent.Find<AltBiome>(biomes.FullName);
                    if (biome is not null && tempDict.ContainsKey(path2))
                    {
                        assets.Add(biome.FullName, new Func<WorldFileData, bool>((ourData) =>
                        {
                            string equals = biome.BiomeType switch
                            {
                                BiomeType.Hallow => tempDict[path2].worldHallow,
                                BiomeType.Jungle => tempDict[path2].worldJungle,
                                BiomeType.Hell => tempDict[path2].worldHell,
                                _ => tempDict[path2].worldHallow,
                            };
                            bool bl = ourData.DrunkWorld && equals == biome.FullName;
                            if (biome.BiomeType == BiomeType.Hallow) bl &= ourData.IsHardMode;
                            return bl;
                        }));
                    }
                }
            }

            bool bl = data.DrunkWorld;
            foreach (KeyValuePair<string, Func<WorldFileData, bool>> entry in assets)
            {
                string path = $"AltLibrary/Assets/WorldIcons/Drunk/{entry.Key.Replace("Terraria/", "")}";
                if (entry.Key != "Corrupt" && entry.Key != "Crimson" && entry.Key != "Hallow")
                {
                    path = ModContent.Find<AltBiome>(entry.Key).IconLarge + "Drunk";
                }
                if (bl && entry.Value.Invoke(data) && ModContent.RequestIfExists(path, out Asset<Texture2D> newAsset))
                {
                    UIImage worldIcon = new(newAsset);
                    worldIcon.Height.Set(0, 1);
                    worldIcon.Width.Set(0, 1);
                    image.Append(worldIcon);
                }
            }
        }

        private static void UIWorldListItem_DrawSelf(On.Terraria.GameContent.UI.Elements.UIWorldListItem.orig_DrawSelf orig, UIWorldListItem self, SpriteBatch spriteBatch)
        {
            orig(self, spriteBatch);
            if (++WarnUpdate >= 120)
            {
                WarnUpdate = 0;
            }
            if ((WorldFileData)typeof(UIWorldListItem).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self) == null)
                return;
            WorldFileData _data = (WorldFileData)typeof(UIWorldListItem).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self);
            UIImage _worldIcon = (UIImage)typeof(UIWorldListItem).GetField("_worldIcon", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self);
            string path2 = Path.ChangeExtension(_data.Path, ".twld");
            Dictionary<string, AltLibraryConfig.WorldDataValues> tempDict = AltLibraryConfig.Config.GetWorldData();
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
            CalculatedStyle innerDimensions = self.GetInnerDimensions();
            CalculatedStyle dimensions = _worldIcon.GetDimensions();
            float num7 = innerDimensions.X + innerDimensions.Width;
            for (int i = 0; i < 4; i++)
            {
                Asset<Texture2D> asset = ModContent.Request<Texture2D>("Terraria/Images/UI/Bestiary/Icon_Tags_Shadow");
                if (i == 0 && tempDict[path2].worldHallow != "") asset = ModContent.Request<Texture2D>(ModContent.Find<AltBiome>(tempDict[path2].worldHallow) == null ? ModContent.Find<AltBiome>(tempDict[path2].worldHallow).IconSmall ?? "AltLibrary/Assets/WorldIcons/ButtonHallow" : "AltLibrary/Assets/WorldIcons/ButtonHallow");
                if (i == 1 && tempDict[path2].worldEvil != "") asset = ModContent.Request<Texture2D>(ModContent.Find<AltBiome>(tempDict[path2].worldEvil) == null ? ModContent.Find<AltBiome>(tempDict[path2].worldEvil).IconSmall ?? "AltLibrary/Assets/WorldIcons/ButtonCorrupt" : "AltLibrary/Assets/WorldIcons/ButtonCorrupt");
                if (i == 2 && tempDict[path2].worldHell != "") asset = ModContent.Request<Texture2D>(ModContent.Find<AltBiome>(tempDict[path2].worldHell) == null ? ModContent.Find<AltBiome>(tempDict[path2].worldHell).IconSmall ?? "AltLibrary/Assets/WorldIcons/ButtonHell" : "AltLibrary/Assets/WorldIcons/ButtonHell");
                if (i == 3 && tempDict[path2].worldJungle != "") asset = ModContent.Request<Texture2D>(ModContent.Find<AltBiome>(tempDict[path2].worldJungle) == null ? ModContent.Find<AltBiome>(tempDict[path2].worldJungle).IconSmall ?? "AltLibrary/Assets/WorldIcons/ButtonJungle" : "AltLibrary/Assets/WorldIcons/ButtonJungle");
                Rectangle? rectangle = null;
                if (i == 0 && tempDict[path2].worldHallow == "") rectangle = new(30, 30, 30, 30);
                if (i == 1 && tempDict[path2].worldEvil == "") rectangle = new(_data.HasCorruption ? 210 : 360, 0, 30, 30);
                if (i == 2 && tempDict[path2].worldHell == "") rectangle = new(30, 60, 30, 30);
                if (i == 3 && tempDict[path2].worldJungle == "") rectangle = new(180, 30, 30, 30);
                ValueTuple<Asset<Texture2D>, Rectangle?> valueTuple = new(asset, rectangle);
                spriteBatch.Draw(ModContent.Request<Texture2D>("AltLibrary/Assets/WorldIcons/Button").Value, new Vector2(num7 - 26f * (i + 1), dimensions.Y - 2f), Color.White);
                spriteBatch.Draw(valueTuple.Item1.Value, new Vector2(num7 - 26f * (i + 1) + 3f, dimensions.Y + 1f), valueTuple.Item2, Color.White, 0f, new Vector2(0f, 0f), 0.5f, SpriteEffects.None, 0f);
            }

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
                if (tempDict[path2].drunkEvil != "" && !ModContent.TryFind<AltBiome>(tempDict[path2].drunkEvil, out _))
                {
                    valid = false;
                }
            }

            if (!valid)
            {
                Rectangle mouseRectangle = Utils.CenteredRectangle(Main.MouseScreen, Vector2.One * 2f);
                Asset<Texture2D> asset = ModContent.Request<Texture2D>("AltLibrary/Assets/WorldIcons/ButtonWarn");
                int num = WarnUpdate % 120;
                int num2 = num < 60 ? 0 : 1;
                Rectangle rectangle = new(num2 * 22, 0, 22, 22);
                spriteBatch.Draw(asset.Value, new Vector2(num7 - 26f * 5, dimensions.Y - 2f), rectangle, Color.White);
                Vector2 vector2 = new(num7 - 26f * 5, dimensions.Y - 2f);
                if (mouseRectangle.Intersects(Utils.CenteredRectangle(vector2 + new Vector2(11f, 11f), Utils.Size(new Rectangle(0, 0, 22, 22)))))
                {
                    string line = Language.GetTextValue("Mods.AltLibrary.WorldBreak");
                    Main.instance.MouseText(line);
                }
            }
        }

        private static void LayeredIcons(string forWhich, WorldFileData data, ref UIImage image, Dictionary<string, AltLibraryConfig.WorldDataValues> tempDict, string path2)
        {
            Dictionary<string, Func<WorldFileData, bool>> assets = new();
            assets.Add("Corrupt", new Func<WorldFileData, bool>((ourData) => ourData.HasCorruption && tempDict.ContainsKey(path2) && tempDict[path2].worldEvil == ""));
            assets.Add("Crimson", new Func<WorldFileData, bool>((ourData) => ourData.HasCrimson && tempDict.ContainsKey(path2) && tempDict[path2].worldEvil == ""));
            assets.Add("Hallow", new Func<WorldFileData, bool>((ourData) => ourData.IsHardMode && tempDict.ContainsKey(path2) && tempDict[path2].worldHallow == ""));
            foreach (AltBiome biomes in AltLibrary.biomes)
            {
                if (!biomes.FullName.StartsWith("Terraria/"))
                {
                    AltBiome biome = ModContent.Find<AltBiome>(biomes.FullName);
                    if (biome is not null && tempDict.ContainsKey(path2))
                    {
                        assets.Add(biome.FullName, new Func<WorldFileData, bool>((ourData) =>
                        {
                            string equals = biome.BiomeType switch
                            {
                                BiomeType.Hallow => tempDict[path2].worldHallow,
                                BiomeType.Hell => tempDict[path2].worldHell,
                                BiomeType.Jungle => tempDict[path2].worldJungle,
                                BiomeType.Evil => tempDict[path2].worldEvil,
                                _ => tempDict[path2].worldEvil,
                            };
                            bool bl = equals == biome.FullName;
                            if (biome.BiomeType == BiomeType.Hallow) bl &= ourData.IsHardMode;
                            return bl;
                        }));
                    }
                }
            }
            bool bl;
            if (forWhich != "Normal")
            {
                bl = (bool)typeof(WorldFileData).GetField($"{(forWhich == "Drunk" ? "DrunkWorld" : forWhich)}").GetValue(data);
            }
            else
            {
                bl = !data.Anniversary && !data.DontStarve && !data.NotTheBees && !data.ForTheWorthy && !data.DrunkWorld;
            }
            foreach (KeyValuePair<string, Func<WorldFileData, bool>> entry in assets)
            {
                string path = $"AltLibrary/Assets/WorldIcons/{forWhich}/{entry.Key}";
                if (entry.Key != "Corrupt" && entry.Key != "Crimson" && entry.Key != "Hallow")
                {
                    path = ModContent.Find<AltBiome>(entry.Key).IconLarge + forWhich;
                }
                if (bl && entry.Value.Invoke(data) && ModContent.RequestIfExists(path, out Asset<Texture2D> newAsset))
                {
                    UIImage worldIcon = new(newAsset);
                    worldIcon.Height.Set(0, 1);
                    worldIcon.Width.Set(0, 1);
                    image.Append(worldIcon);
                }
            }
        }

        private static void ReplaceIcons(WorldFileData data, ref UIImage image)
        {
            Asset<Texture2D> asset = ModContent.Request<Texture2D>("AltLibrary/Assets/WorldIcons/IconNormal");
            if (data.DrunkWorld)
            {
                asset = ModContent.Request<Texture2D>("AltLibrary/Assets/WorldIcons/IconDrunk");
                if (data.HasCrimson)
                {
                    asset = ModContent.Request<Texture2D>("AltLibrary/Assets/WorldIcons/DrunkBase/Crimson");
                }
                else if (data.HasCorruption)
                {
                    asset = ModContent.Request<Texture2D>("AltLibrary/Assets/WorldIcons/DrunkBase/Corruption");
                }
            }
            if (data.ForTheWorthy)
            {
                asset = ModContent.Request<Texture2D>("AltLibrary/Assets/WorldIcons/IconForTheWorthy");
            }
            if (data.NotTheBees)
            {
                asset = ModContent.Request<Texture2D>("AltLibrary/Assets/WorldIcons/IconNotTheBees");
            }
            if (data.Anniversary)
            {
                asset = ModContent.Request<Texture2D>("AltLibrary/Assets/WorldIcons/IconAnniversary");
            }
            if (data.DontStarve)
            {
                asset = ModContent.Request<Texture2D>("AltLibrary/Assets/WorldIcons/IconDontStarve");
            }
            UIImage worldIcon = new(asset);
            worldIcon.Height.Set(0, 1);
            worldIcon.Width.Set(0, 1);
            image.Append(worldIcon);
        }
    }
}
