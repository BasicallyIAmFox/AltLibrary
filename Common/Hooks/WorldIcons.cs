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
using Terraria.GameContent.UI.Elements;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Terraria.Utilities;

namespace AltLibrary.Common.Hooks
{
    internal static class WorldIcons
    {
        public static void Init()
        {
            IL.Terraria.GameContent.UI.Elements.UIWorldListItem.ctor += UIWorldListItem_ctor;
            On.Terraria.GameContent.UI.Elements.UIWorldListItem.DrawSelf += UIWorldListItem_DrawSelf;
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
                            tempDict[path2] = worldData;
                        }
                        AltLibraryConfig.Config.SetWorldData(tempDict);
                        AltLibraryConfig.Save(AltLibraryConfig.Config);
                    }
                }
                ReplaceIcons(data, ref _worldIcon);
                LayeredIcons("Normal", data, ref _worldIcon, tempDict, path2);
                LayeredIcons("Drunk", data, ref _worldIcon, tempDict, path2);
                LayeredIcons("ForTheWorthy", data, ref _worldIcon, tempDict, path2);
                LayeredIcons("NotTheBees", data, ref _worldIcon, tempDict, path2);
                LayeredIcons("Anniversary", data, ref _worldIcon, tempDict, path2);
                LayeredIcons("DontStarve", data, ref _worldIcon, tempDict, path2);
            });
        }

        private static void UIWorldListItem_DrawSelf(On.Terraria.GameContent.UI.Elements.UIWorldListItem.orig_DrawSelf orig, UIWorldListItem self, SpriteBatch spriteBatch)
        {
            orig(self, spriteBatch);
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
                if (i == 0 && tempDict[path2].worldHallow != "") asset = ModContent.Request<Texture2D>(ModContent.Find<AltBiome>(tempDict[path2].worldHallow).IconSmall ?? "AltLibrary/Assets/WorldIcons/ButtonHallow");
                if (i == 1 && tempDict[path2].worldEvil != "") asset = ModContent.Request<Texture2D>(ModContent.Find<AltBiome>(tempDict[path2].worldEvil).IconSmall ?? "AltLibrary/Assets/WorldIcons/ButtonCorrupt");
                if (i == 2 && tempDict[path2].worldHell != "") asset = ModContent.Request<Texture2D>(ModContent.Find<AltBiome>(tempDict[path2].worldHell).IconSmall ?? "AltLibrary/Assets/WorldIcons/ButtonHell");
                if (i == 3 && tempDict[path2].worldJungle != "") asset = ModContent.Request<Texture2D>(ModContent.Find<AltBiome>(tempDict[path2].worldJungle).IconSmall ?? "AltLibrary/Assets/WorldIcons/ButtonJungle");
                Rectangle? rectangle = null;
                if (i == 0 && tempDict[path2].worldHallow == "") rectangle = new(30, 30, 30, 30);
                if (i == 1 && tempDict[path2].worldEvil == "") rectangle = new(_data.HasCorruption ? 210 : 360, 0, 30, 30);
                if (i == 2 && tempDict[path2].worldHell == "") rectangle = new(30, 60, 30, 30);
                if (i == 3 && tempDict[path2].worldJungle == "") rectangle = new(180, 30, 30, 30);
                ValueTuple<Asset<Texture2D>, Rectangle?> valueTuple = new(asset, rectangle);
                spriteBatch.Draw(ModContent.Request<Texture2D>("AltLibrary/Assets/WorldIcons/Button").Value, new Vector2(num7 - 26f * (i + 1), dimensions.Y - 2f), Color.White);
                spriteBatch.Draw(valueTuple.Item1.Value, new Vector2(num7 - 26f * (i + 1) + 3f, dimensions.Y + 1f), valueTuple.Item2, Color.White, 0f, new Vector2(0f, 0f), 0.5f, SpriteEffects.None, 0f);
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
                asset = ModContent.Request<Texture2D>("AltLibrary/Assets/WorldIcons/IconDrunkWorld");
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
