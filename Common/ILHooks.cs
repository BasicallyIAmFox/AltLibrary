using AltLibrary.Common;
using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
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
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics.Effects;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Terraria.Utilities;

namespace AltLibrary.Core
{
    internal class ILHooks
    {
        public static void OnInitialize()
        {
            IL.Terraria.WorldGen.GenerateWorld += GenPasses.ILGenerateWorld;
            GenPasses.HookGenPassReset += GenPasses_HookGenPassReset;
            GenPasses.HookGenPassShinies += GenPasses_HookGenPassShinies;
            GenPasses.HookGenPassAltars += ILGenPassAltars;
            IL.Terraria.GameContent.UI.Elements.UIWorldListItem.ctor += UIWorldListItem_ctor;
            On.Terraria.GameContent.UI.Elements.UIWorldListItem.DrawSelf += UIWorldListItem_DrawSelf;
            IL.Terraria.GameContent.UI.Elements.UIGenProgressBar.DrawSelf += UIGenProgressBar_DrawSelf;
            On.Terraria.Main.EraseWorld += Main_EraseWorld;
            IL.Terraria.Main.DrawUnderworldBackgroudLayer += Main_DrawUnderworldBackgroudLayer;
            IL.Terraria.GameContent.UI.States.UIWorldCreation.MakeInfoMenu += UIWorldCreationEdits.ILMakeInfoMenu;
            On.Terraria.GameContent.UI.States.UIWorldCreation.AddWorldEvilOptions += UIWorldCreationEdits.OnAddWorldEvilOptions;
            On.Terraria.GameContent.UI.States.UIWorldCreation.SetDefaultOptions += UIWorldCreationEdits.UIWorldCreation_SetDefaultOptions;
            On.Terraria.GameContent.UI.States.UIWorldCreation.BuildPage += UIWorldCreationEdits.UIWorldCreation_BuildPage;
            IL.Terraria.GameContent.UI.States.UIWorldCreation.FinishCreatingWorld += UIWorldCreationEdits.UIWorldCreation_FinishCreatingWorld;
            On.Terraria.GameContent.UI.Elements.UIWorldCreationPreview.DrawSelf += UIWorldCreationEdits.UIWorldCreationPreview_DrawSelf;
        }

        private static void Main_DrawUnderworldBackgroudLayer(ILContext il)
        {
            var c = new ILCursor(il);
            if (!c.TryGotoNext(i => i.MatchStloc(0)))
                return;
            c.Index++;
            c.EmitDelegate(() =>
            {
                foreach (AltBiome biome in AltLibrary.biomes)
                {
                    if (biome.Biome.IsBiomeActive(Main.LocalPlayer))
                    {
                        AltLibrary.hellAltTrans[biome.Type] += 0.05f;
                        if (AltLibrary.hellAltTrans[biome.Type] > 1f)
                        {
                            AltLibrary.hellAltTrans[biome.Type] = 1f;
                        }
                    }
                    else
                    {
                        AltLibrary.hellAltTrans[biome.Type] -= 0.05f;
                        if (AltLibrary.hellAltTrans[biome.Type] < 0f)
                        {
                            AltLibrary.hellAltTrans[biome.Type] = 0f;
                        }
                    }
                }
            });
            if (!c.TryGotoNext(i => i.MatchRet()))
                return;
            c.Remove();
            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Ldarg_1);
            c.Emit(OpCodes.Ldarg_2);
            c.Emit(OpCodes.Ldarg_3);
            c.EmitDelegate<Action<bool, Vector2, float, int>>((flat, screenOffset, pushUp, layerTextureIndex) =>
            {
                foreach (AltBiome biome in AltLibrary.biomes)
                {
                    int num27 = Main.underworldBG[layerTextureIndex];
                    Asset<Texture2D>[] assets = new Asset<Texture2D>[TextureAssets.Underworld.Length];
                    for (int i = 0; i < TextureAssets.Underworld.Length; i++)
                    {
                        assets[i] = biome.AltUnderworldBackgrounds[i] ?? TextureAssets.Underworld[i];
                    }
                    Asset<Texture2D> asset = assets[num27];
                    Texture2D value5 = asset.Value;
                    Vector2 vec3 = new Vector2(value5.Width, value5.Height) * 0.5f;
                    float num26 = flat ? 1f : (layerTextureIndex * 2 + 3f);
                    var value4 = new Vector2(1f / num26);
                    var value3 = new Rectangle(0, 0, value5.Width, value5.Height);
                    float num25 = 1.3f;
                    Vector2 zero = Vector2.Zero;
                    int num24 = 0;
                    switch (num27)
                    {
                        case 1:
                            {
                                int num19 = (int)(Main.GlobalTimeWrappedHourly * 8f) % 4;
                                value3 = new Rectangle((num19 >> 1) * (value5.Width >> 1), num19 % 2 * (value5.Height >> 1), value5.Width >> 1, value5.Height >> 1);
                                vec3 *= 0.5f;
                                zero.Y += 175f;
                                break;
                            }
                        case 2:
                            zero.Y += 100f;
                            break;
                        case 3:
                            zero.Y += 75f;
                            break;
                        case 4:
                            num25 = 0.5f;
                            zero.Y -= 0f;
                            break;
                        case 5:
                            zero.Y += num24;
                            break;
                        case 6:
                            {
                                int num20 = (int)(Main.GlobalTimeWrappedHourly * 8f) % 4;
                                value3 = new Rectangle(num20 % 2 * (value5.Width >> 1), (num20 >> 1) * (value5.Height >> 1), value5.Width >> 1, value5.Height >> 1);
                                vec3 *= 0.5f;
                                zero.Y += num24;
                                zero.Y += -60f;
                                break;
                            }
                        case 7:
                            {
                                int num21 = (int)(Main.GlobalTimeWrappedHourly * 8f) % 4;
                                value3 = new Rectangle(num21 % 2 * (value5.Width >> 1), (num21 >> 1) * (value5.Height >> 1), value5.Width >> 1, value5.Height >> 1);
                                vec3 *= 0.5f;
                                zero.Y += num24;
                                zero.X -= 400f;
                                zero.Y += 90f;
                                break;
                            }
                        case 8:
                            {
                                int num22 = (int)(Main.GlobalTimeWrappedHourly * 8f) % 4;
                                value3 = new Rectangle(num22 % 2 * (value5.Width >> 1), (num22 >> 1) * (value5.Height >> 1), value5.Width >> 1, value5.Height >> 1);
                                vec3 *= 0.5f;
                                zero.Y += num24;
                                zero.Y += 90f;
                                break;
                            }
                        case 9:
                            zero.Y += num24;
                            zero.Y -= 30f;
                            break;
                        case 10:
                            zero.Y += 250f * num26;
                            break;
                        case 11:
                            zero.Y += 100f * num26;
                            break;
                        case 12:
                            zero.Y += 20f * num26;
                            break;
                        case 13:
                            {
                                zero.Y += 20f * num26;
                                int num23 = (int)(Main.GlobalTimeWrappedHourly * 8f) % 4;
                                value3 = new Rectangle(num23 % 2 * (value5.Width >> 1), (num23 >> 1) * (value5.Height >> 1), value5.Width >> 1, value5.Height >> 1);
                                vec3 *= 0.5f;
                                break;
                            }
                    }
                    if (flat)
                    {
                        num25 *= 1.5f;
                    }
                    vec3 *= num25;
                    SkyManager.Instance.DrawToDepth(Main.spriteBatch, 1f / value4.X);
                    if (flat)
                    {
                        zero.Y += (TextureAssets.Underworld[0].Height() >> 1) * 1.3f - vec3.Y;
                    }
                    zero.Y -= pushUp;
                    float num18 = num25 * value3.Width;
                    int num17 = (int)((int)(screenOffset.X * value4.X - vec3.X + zero.X - (Main.screenWidth >> 1)) / num18);
                    vec3 = vec3.Floor();
                    int num16 = (int)Math.Ceiling((double)(Main.screenWidth / num18));
                    int num15 = (int)(num25 * ((value3.Width - 1) / value4.X));
                    Vector2 vector = (new Vector2((num17 - 2) * num15, Main.UnderworldLayer * 16f) + vec3 - screenOffset) * value4 + screenOffset - Main.screenPosition - vec3 + zero;
                    vector = vector.Floor();
                    while (vector.X + num18 < 0f)
                    {
                        num17++;
                        vector.X += num18;
                    }
                    for (int i = num17 - 2; i <= num17 + 4 + num16; i++)
                    {
                        Main.spriteBatch.Draw(value5, vector, value3, Color.White * AltLibrary.hellAltTrans[biome.Type], 0f, Vector2.Zero, num25, SpriteEffects.None, 0f);
                        if (layerTextureIndex == 0)
                        {
                            int num14 = (int)(vector.Y + value3.Height * num25);
                            Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle((int)vector.X, num14, (int)(value3.Width * num25), Math.Max(0, Main.screenHeight - num14)), biome.AltUnderworldColor * AltLibrary.hellAltTrans[biome.Type]);
                        }
                        vector.X += num18;
                    }
                }
            });
            c.Emit(OpCodes.Ret);
        }

        public static void ILGenPassAltars(ILContext il)
        {
            ILCursor c = new(il);
            ILLabel endNormalAltar = c.DefineLabel();
            ILLabel startNormalAltar = c.DefineLabel();
            if (!c.TryGotoNext(i => i.MatchLdsfld<WorldGen>(nameof(WorldGen.crimson))))
                return;
            c.EmitDelegate(() => WorldGen.crimson || WorldBiomeManager.worldEvil != "");
            c.Emit(OpCodes.Brfalse, startNormalAltar);
            c.Emit(OpCodes.Ldloc, 3);
            c.Emit(OpCodes.Ldloc, 4);
            c.EmitDelegate((int x, int y) =>
            {
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).AltarTile.HasValue)
                {
                    if (!WorldGen.IsTileNearby(x, y, ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).AltarTile.Value, 3))
                    {
                        WorldGen.Place3x2(x, y, (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).AltarTile.Value);
                    }
                }
            });
            c.Emit(OpCodes.Br, endNormalAltar);
            c.MarkLabel(startNormalAltar);
            if (!c.TryGotoNext(i => i.MatchLdloc(5)))
                return;
            if (!c.TryGotoNext(i => i.MatchLdsflda(out _)))
                return;
            c.MarkLabel(endNormalAltar);
        }

        private static void GenPasses_HookGenPassReset(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchStsfld<WorldGen>(nameof(WorldGen.crimson))))
                return;
            if (!c.TryGotoNext(i => i.MatchLdfld(out _)))
                return;
            c.EmitDelegate(() =>
            {
                if (WorldBiomeManager.worldEvil == "")
                {
                    WorldGen.crimson = true;
                }
                else
                {
                    WorldGen.crimson = false;
                }
            });
            if (!c.TryGotoNext(i => i.MatchRet()))
                return;
            if (!c.TryGotoPrev(i => i.MatchBneUn(out _)))
                return;
            if (!c.TryGotoPrev(i => i.MatchLdcI4(-1)))
                return;
            c.EmitDelegate<Func<int, int>>(dungeonSide =>
            {
                WorldBiomeGeneration.dungeonLocation = dungeonSide;
                return dungeonSide;
            });
            for (int i = 0; i < 2; i++)
            {
                if (!c.TryGotoNext(i => i.MatchRet()))
                    return;
                c.Index--;
                c.EmitDelegate<Func<int, int>>(dungeonLocation =>
                {
                    WorldBiomeGeneration.dungeonLocation = dungeonLocation;
                    return dungeonLocation;
                });
            }
        }

        private static void GenPasses_HookGenPassShinies(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchRet()))
                return;
            if (!c.TryGotoPrev(i => i.MatchLdsfld<WorldGen>(nameof(WorldGen.drunkWorldGen))))
                return;
            if (!c.TryGotoNext(i => i.MatchBrfalse(out _)))
                return;
            c.Index++;
            c.EmitDelegate(() =>
            {
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeOre.HasValue)
                {
                    for (int i = 0; i < (int)(Main.maxTilesX * Main.maxTilesY * 2.25E-05 / 2.0); i++)
                    {
                        WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX),
                            WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY), WorldGen.genRand.Next(3, 6),
                            WorldGen.genRand.Next(4, 8), ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeOre.Value);
                    }
                }
            });
            if (!c.TryGotoNext(i => i.MatchRet()))
                return;
            if (!c.TryGotoNext(i => i.MatchBr(out _)))
                return;
            ILLabel startCorruptionGen = c.DefineLabel();
            c.EmitDelegate(() => !WorldGen.crimson && WorldBiomeManager.worldEvil == "");
            c.Emit(OpCodes.Brfalse, startCorruptionGen);
            c.EmitDelegate(() =>
            {
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeOre.HasValue)
                {
                    for (int i = 0; i < (int)(Main.maxTilesX * Main.maxTilesY * 2.25E-05); i++)
                    {
                        WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX),
                            WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY), WorldGen.genRand.Next(3, 6),
                            WorldGen.genRand.Next(4, 8), ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeOre.Value);
                    }
                }
            });
            c.Emit(OpCodes.Ret);
            c.MarkLabel(startCorruptionGen);
        }

        private static void Main_EraseWorld(On.Terraria.Main.orig_EraseWorld orig, int i)
        {
            Dictionary<string, AltLibraryConfig.WorldDataValues> tempDict = AltLibraryConfig.Config.GetWorldData();
            var path = Path.ChangeExtension(Main.WorldList[i].Path, ".twld");
            if (tempDict.ContainsKey(path))
            {
                tempDict.Remove(path);
                AltLibraryConfig.Config.SetWorldData(tempDict);
                AltLibraryConfig.Save(AltLibraryConfig.Config);
            }
            orig(i);
        }

        public static Asset<Texture2D> EmptyAsset => ModContent.Request<Texture2D>("AltLibrary/Assets/WorldIcons/Outer Empty");
        private static void UIGenProgressBar_DrawSelf(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchLdcI4(-8131073)))
                return;
            if (!c.TryGotoNext(i => i.MatchCall(out _)))
                return;
            c.Index++;
            c.Emit(OpCodes.Ldloc, 5);
            c.EmitDelegate<Func<Color, Color>>((color) =>
            {
                int worldGenStep = 0;
                if (WorldGen.crimson) worldGenStep = 1;
                if (WorldBiomeManager.worldEvil != "") worldGenStep = ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).Type + 2;

                if (WorldGen.drunkWorldGen && Main.rand.NextBool(2)) worldGenStep = Main.rand.Next(AltLibrary.biomes.Count + 2);

                Color expected = new(95, 242, 86);
                if (worldGenStep == 1) expected = new Color(255, 237, 131);
                if (WorldBiomeManager.worldEvil != "") expected = ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).OuterColor;

                Color result = expected;
                return result;
            });
            c.Emit(OpCodes.Stloc, 5);
            if (!c.TryGotoNext(i => i.MatchLdfld<UIGenProgressBar>("_texOuterCorrupt")))
                return;
            c.Remove();
            c.EmitDelegate<Func<UIGenProgressBar, Asset<Texture2D>>>((unusedVariableLeftInForLoading) =>
            {
                return EmptyAsset;
            });
            if (!c.TryGotoNext(i => i.MatchLdfld<UIGenProgressBar>("_texOuterCrimson")))
                return;
            c.Remove();
            c.EmitDelegate<Func<UIGenProgressBar, Asset<Texture2D>>>((unusedVariableLeftInForLoading) =>
            {
                return EmptyAsset;
            });
            if (!c.TryGotoNext(i => i.MatchCallvirt(out _)))
                return;
            if (!c.TryGotoNext(i => i.MatchCallvirt(out _)))
                return;
            c.Index++;
            c.Emit(OpCodes.Ldarg, 1);
            c.Emit(OpCodes.Ldloc, 6);
            c.EmitDelegate<Action<SpriteBatch, Rectangle>>((spriteBatch, r) =>
            {
                int worldGenStep = 0;
                if (WorldGen.crimson) worldGenStep = 1;
                if (WorldBiomeManager.worldEvil != "") worldGenStep = ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).Type + 2;
                if (WorldGen.drunkWorldGen && Main.rand.NextBool(2)) worldGenStep = Main.rand.Next(AltLibrary.biomes.Count + 2);
                Asset<Texture2D> asset = EmptyAsset;
                if (worldGenStep == 0) asset = Main.Assets.Request<Texture2D>("Images/UI/WorldGen/Outer_Corrupt");
                if (worldGenStep == 1) asset = Main.Assets.Request<Texture2D>("Images/UI/WorldGen/Outer_Crimson");
                if (WorldBiomeManager.worldEvil != "") asset = ModContent.Request<Texture2D>(ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).OuterTexture ?? "AltLibrary/Assets/WorldIcons/Outer Empty");
                spriteBatch.Draw(asset.Value, r.TopLeft(), Color.White);
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
            assets.Add("Corrupt", new Func<WorldFileData, bool>((ourData) => ourData.HasCorruption));
            assets.Add("Crimson", new Func<WorldFileData, bool>((ourData) => ourData.HasCrimson));
            assets.Add("Hallow", new Func<WorldFileData, bool>((ourData) => ourData.IsHardMode));
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
                            return equals == biome.FullName;
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
