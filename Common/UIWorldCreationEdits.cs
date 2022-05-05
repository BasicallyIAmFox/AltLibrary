using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.AltOres;
using AltLibrary.Common.Systems;
using AltLibrary.Core.UIs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Terraria.Utilities;

namespace AltLibrary.Common
{
    internal class UIWorldCreationEdits
    {
        public static ALGroupOptionButton<CurrentAltOption>[] chosingOption;
        public static CurrentAltOption chosenOption;
        public static int AltEvilBiomeChosenType;
        public static int AltHallowBiomeChosenType;
        public static int AltJungleBiomeChosenType;
        public static int AltHellBiomeChosenType;
        public static UIList _biomeList;
        public static List<ALUIBiomeListItem> _biomeElements = new();
        public static UIList _oreList;
        public static List<ALUIOreListItem> _oreElements = new();
        public static int Copper;
        public static int Iron;
        public static int Silver;
        public static int Gold;
        public static int Cobalt;
        public static int Mythril;
        public static int Adamantite;
        public static bool isCrimson;
        public enum CurrentAltOption
        {
            Biome,
            Ore
        }

        public static void Init()
        {
            IL.Terraria.GameContent.UI.States.UIWorldCreation.MakeInfoMenu += ILMakeInfoMenu;
            On.Terraria.GameContent.UI.States.UIWorldCreation.AddWorldEvilOptions += OnAddWorldEvilOptions;
            On.Terraria.GameContent.UI.States.UIWorldCreation.SetDefaultOptions += UIWorldCreation_SetDefaultOptions;
            On.Terraria.GameContent.UI.States.UIWorldCreation.BuildPage += UIWorldCreation_BuildPage;
            IL.Terraria.GameContent.UI.States.UIWorldCreation.FinishCreatingWorld += UIWorldCreation_FinishCreatingWorld;
            On.Terraria.GameContent.UI.Elements.UIWorldCreationPreview.DrawSelf += UIWorldCreationPreview_DrawSelf;
            On.Terraria.GameContent.UI.Elements.UIWorldListItem.PlayGame += UIWorldListItem_PlayGame;
        }

        private static void UIWorldListItem_PlayGame(On.Terraria.GameContent.UI.Elements.UIWorldListItem.orig_PlayGame orig, UIWorldListItem self, UIMouseEvent evt, UIElement listeningElement)
        {
            if ((WorldFileData)typeof(UIWorldListItem).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self) == null)
                return;
            WorldFileData data = (WorldFileData)typeof(UIWorldListItem).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self);
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

            if (valid)
            {
                orig(self, evt, listeningElement);
            }
        }

        public static void UIWorldCreation_BuildPage(On.Terraria.GameContent.UI.States.UIWorldCreation.orig_BuildPage orig, UIWorldCreation self)
        {
            chosenOption = (CurrentAltOption)(-1);
            List<int> evilBiomeTypes = new() { -333, -666 };
            AltLibrary.biomes.Where(x => x.BiomeType == BiomeType.Evil && x.Selectable).ToList().ForEach(x => evilBiomeTypes.Add(x.Type - 1));
            AltEvilBiomeChosenType = evilBiomeTypes[Main.rand.Next(evilBiomeTypes.Count)];
            isCrimson = AltEvilBiomeChosenType == -666;
            List<int> hallowBiomeTypes = new() { -3 };
            AltLibrary.biomes.Where(x => x.BiomeType == BiomeType.Hallow && x.Selectable).ToList().ForEach(x => hallowBiomeTypes.Add(x.Type - 1));
            AltHallowBiomeChosenType = hallowBiomeTypes[Main.rand.Next(hallowBiomeTypes.Count)];
            List<int> hellBiomeTypes = new() { -5 };
            AltLibrary.biomes.Where(x => x.BiomeType == BiomeType.Hell && x.Selectable).ToList().ForEach(x => hellBiomeTypes.Add(x.Type - 1));
            AltHellBiomeChosenType = hellBiomeTypes[Main.rand.Next(hellBiomeTypes.Count)];
            List<int> jungleBiomeTypes = new() { -4 };
            AltLibrary.biomes.Where(x => x.BiomeType == BiomeType.Jungle && x.Selectable).ToList().ForEach(x => jungleBiomeTypes.Add(x.Type - 1));
            AltJungleBiomeChosenType = jungleBiomeTypes[Main.rand.Next(jungleBiomeTypes.Count)];
            List<int> ores = new() { -1, -2 };
            AltLibrary.ores.Where(x => x.OreType == OreType.Copper && x.Selectable).ToList().ForEach(x => ores.Add(x.Type));
            Copper = ores[Main.rand.Next(ores.Count)];
            ores = new() { -3, -4 };
            AltLibrary.ores.Where(x => x.OreType == OreType.Iron && x.Selectable).ToList().ForEach(x => ores.Add(x.Type));
            Iron = ores[Main.rand.Next(ores.Count)];
            ores = new() { -5, -6 };
            AltLibrary.ores.Where(x => x.OreType == OreType.Silver && x.Selectable).ToList().ForEach(x => ores.Add(x.Type));
            Silver = ores[Main.rand.Next(ores.Count)];
            ores = new() { -7, -8 };
            AltLibrary.ores.Where(x => x.OreType == OreType.Gold && x.Selectable).ToList().ForEach(x => ores.Add(x.Type));
            Gold = ores[Main.rand.Next(ores.Count)];
            ores = new() { -9, -10 };
            AltLibrary.ores.Where(x => x.OreType == OreType.Cobalt && x.Selectable).ToList().ForEach(x => ores.Add(x.Type));
            Cobalt = ores[Main.rand.Next(ores.Count)];
            ores = new() { -11, -12 };
            AltLibrary.ores.Where(x => x.OreType == OreType.Mythril && x.Selectable).ToList().ForEach(x => ores.Add(x.Type));
            Mythril = ores[Main.rand.Next(ores.Count)];
            ores = new() { -13, -14 };
            AltLibrary.ores.Where(x => x.OreType == OreType.Adamantite && x.Selectable).ToList().ForEach(x => ores.Add(x.Type));
            Adamantite = ores[Main.rand.Next(ores.Count)];
            _oreElements.Clear();
            _oreList = null;
            orig(self);

            #region Ore UI List
            {
                UIElement uIElement3 = new();
                uIElement3.Left = StyleDimension.FromPixels(Main.screenWidth - (Main.screenWidth - 100f));
                uIElement3.Width.Set(0f, 0.8f);
                uIElement3.MaxWidth.Set(450, 0f);
                uIElement3.MinWidth.Set(350, 0f);
                uIElement3.Top.Set(150f, 0f);
                uIElement3.Height.Set(-150f, 1f);
                uIElement3.HAlign = 1f;
                uIElement3.OnUpdate += _UIElement3_OnUpdate;
                self.Append(uIElement3);
                UIPanel uIPanel = new();
                uIPanel.Width.Set(0f, 1f);
                uIPanel.Height.Set(-110f, 1f);
                uIPanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
                uIPanel.PaddingTop = 0f;
                uIPanel.OnUpdate += _UIPanel_OnUpdate;
                uIElement3.Append(uIPanel);
                _oreList = new UIList();
                _oreList.Width.Set(25f, 1f);
                _oreList.Height.Set(-50f, 1f);
                _oreList.Top.Set(25f, 0f);
                _oreList.ListPadding = 5f;
                _oreList.HAlign = 1f;
                _oreList.OnUpdate += _oreList_OnUpdate;
                uIPanel.Append(_oreList);

                UIScrollbar uIScrollbar = new();
                uIScrollbar.SetView(100f, 100f);
                uIScrollbar.Left = StyleDimension.FromPixels(Main.screenWidth - (Main.screenWidth - 75f));
                uIScrollbar.Height.Set(-250f, 1f);
                uIScrollbar.Top.Set(150f, 0f);
                uIScrollbar.HAlign = 1f;
                uIScrollbar.OnUpdate += _UIScrollbar_OnUpdate;
                self.Append(uIScrollbar);
                _oreList.SetScrollbar(uIScrollbar);

                List<AltOre> list = new();
                list.Clear();
                list.Add(new RandomOptionOre("RandomCopper"));
                list.Add(new RandomOptionOre("RandomIron"));
                list.Add(new RandomOptionOre("RandomSilver"));
                list.Add(new RandomOptionOre("RandomGold"));
                list.Add(new RandomOptionOre("RandomCobalt"));
                list.Add(new RandomOptionOre("RandomMythril"));
                list.Add(new RandomOptionOre("RandomAdamantite"));
                list.Add(new VanillaOre("Copper", -1, TileID.Copper, ItemID.CopperBar));
                list.Add(new VanillaOre("Tin", -2, TileID.Tin, ItemID.TinBar));
                list.AddRange(AltLibrary.ores.Where(x => x.OreType == OreType.Copper && x.Selectable));
                list.Add(new VanillaOre("Iron", -3, TileID.Iron, ItemID.IronBar));
                list.Add(new VanillaOre("Lead", -4, TileID.Lead, ItemID.LeadBar));
                list.AddRange(AltLibrary.ores.Where(x => x.OreType == OreType.Iron && x.Selectable));
                list.Add(new VanillaOre("Silver", -5, TileID.Silver, ItemID.SilverBar));
                list.Add(new VanillaOre("Tungsten", -6, TileID.Tungsten, ItemID.TungstenBar));
                list.AddRange(AltLibrary.ores.Where(x => x.OreType == OreType.Silver && x.Selectable));
                list.Add(new VanillaOre("Gold", -7, TileID.Gold, ItemID.GoldBar));
                list.Add(new VanillaOre("Platinum", -8, TileID.Platinum, ItemID.PlatinumBar));
                list.AddRange(AltLibrary.ores.Where(x => x.OreType == OreType.Gold && x.Selectable));
                list.Add(new VanillaOre("Cobalt", -9, TileID.Cobalt, ItemID.CobaltBar));
                list.Add(new VanillaOre("Palladium", -10, TileID.Palladium, ItemID.PalladiumBar));
                list.AddRange(AltLibrary.ores.Where(x => x.OreType == OreType.Cobalt && x.Selectable));
                list.Add(new VanillaOre("Mythril", -11, TileID.Mythril, ItemID.MythrilBar));
                list.Add(new VanillaOre("Orichalcum", -12, TileID.Orichalcum, ItemID.OrichalcumBar));
                list.AddRange(AltLibrary.ores.Where(x => x.OreType == OreType.Mythril && x.Selectable));
                list.Add(new VanillaOre("Adamantite", -13, TileID.Adamantite, ItemID.AdamantiteBar));
                list.Add(new VanillaOre("Titanium", -14, TileID.Titanium, ItemID.TitaniumBar));
                list.AddRange(AltLibrary.ores.Where(x => x.OreType == OreType.Adamantite && x.Selectable));
                foreach (AltOre biome in list)
                {
                    ALUIOreListItem item = new(biome, false);
                    _oreList.Add(item);
                    _oreElements.Add(item);
                }
            }
            #endregion

            _biomeElements.Clear();
            _biomeList = null;

            #region Biome UI List
            {
                UIElement uIElement3 = new();
                uIElement3.Left = StyleDimension.FromPixels(Main.screenWidth - (Main.screenWidth + 100f));
                uIElement3.Width.Set(0f, 0.8f);
                uIElement3.MaxWidth.Set(450, 0f);
                uIElement3.MinWidth.Set(350, 0f);
                uIElement3.Top.Set(150f, 0f);
                uIElement3.Height.Set(-150f, 1f);
                uIElement3.HAlign = 1f;
                uIElement3.OnUpdate += UIElement3_OnUpdate;
                self.Append(uIElement3);
                UIPanel uIPanel = new();
                uIPanel.Width.Set(0f, 1f);
                uIPanel.Height.Set(-110f, 1f);
                uIPanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
                uIPanel.PaddingTop = 0f;
                uIPanel.OnUpdate += UIPanel_OnUpdate;
                uIElement3.Append(uIPanel);
                _biomeList = new UIList();
                _biomeList.Width.Set(25f, 1f);
                _biomeList.Height.Set(-50f, 1f);
                _biomeList.Top.Set(25f, 0f);
                _biomeList.ListPadding = 5f;
                _biomeList.HAlign = 1f;
                _biomeList.OnUpdate += _biomeList_OnUpdate;
                uIPanel.Append(_biomeList);

                UIScrollbar uIScrollbar = new();
                uIScrollbar.SetView(100f, 100f);
                uIScrollbar.Left = StyleDimension.FromPixels(Main.screenWidth - (Main.screenWidth + 75f));
                uIScrollbar.Height.Set(-250f, 1f);
                uIScrollbar.Top.Set(150f, 0f);
                uIScrollbar.HAlign = 1f;
                uIScrollbar.OnUpdate += UIScrollbar_OnUpdate;
                self.Append(uIScrollbar);
                _biomeList.SetScrollbar(uIScrollbar);

                List<AltBiome> list = new();
                list.Clear();
                list.Add(new RandomOptionBiome("RandomEvilBiome"));
                if (AltLibrary.biomes.Where(x => x.BiomeType == BiomeType.Hallow).Any())
                {
                    list.Add(new RandomOptionBiome("RandomHallowBiome"));
                }
                if (AltLibrary.biomes.Where(x => x.BiomeType == BiomeType.Jungle).Any())
                {
                    list.Add(new RandomOptionBiome("RandomJungleBiome"));
                }
                if (AltLibrary.biomes.Where(x => x.BiomeType == BiomeType.Hell).Any())
                {
                    list.Add(new RandomOptionBiome("RandomUnderworldBiome"));
                }
                list.Add(new VanillaBiome("CorruptBiome", BiomeType.Evil, -333, Color.MediumPurple, Language.GetText("Mods.AltLibrary.Biomes.CorruptName"), Language.GetText("Mods.AltLibrary.Biomes.CorruptDesc"), false));
                list.Add(new VanillaBiome("CrimsonBiome", BiomeType.Evil, -666, Color.IndianRed, Language.GetText("Mods.AltLibrary.Biomes.CrimsonName"), Language.GetText("Mods.AltLibrary.Biomes.CrimsonDesc"), true));
                list.AddRange(AltLibrary.biomes.Where(x => x.BiomeType == BiomeType.Evil && x.Selectable));
                list.Add(new VanillaBiome("HallowBiome", BiomeType.Hallow, -3, Color.HotPink, Language.GetText("Mods.AltLibrary.Biomes.HallowName"), Language.GetText("Mods.AltLibrary.Biomes.HallowDesc")));
                list.AddRange(AltLibrary.biomes.Where(x => x.BiomeType == BiomeType.Hallow && x.Selectable));
                list.Add(new VanillaBiome("JungleBiome", BiomeType.Jungle, -4, Color.SpringGreen, Language.GetText("Mods.AltLibrary.Biomes.JungleName"), Language.GetText("Mods.AltLibrary.Biomes.JungleDesc")));
                list.AddRange(AltLibrary.biomes.Where(x => x.BiomeType == BiomeType.Jungle && x.Selectable));
                list.Add(new VanillaBiome("UnderworldBiome", BiomeType.Hell, -5, Color.OrangeRed, Language.GetText("Mods.AltLibrary.Biomes.UnderworldName"), Language.GetText("Mods.AltLibrary.Biomes.UnderworldDesc")));
                list.AddRange(AltLibrary.biomes.Where(x => x.BiomeType == BiomeType.Hell && x.Selectable));
                foreach (AltBiome biome in list)
                {
                    ALUIBiomeListItem item = new(biome, false);
                    _biomeList.Add(item);
                    _biomeElements.Add(item);
                }
            }
            #endregion
        }

        private static void _oreList_OnUpdate(UIElement affectedElement)
        {
            UIList element = affectedElement as UIList;
            if (chosenOption == CurrentAltOption.Ore)
            {
                element.Width.Set(25f, 1f);
                element.Height.Set(-50f, 1f);
                element.Top.Set(25f, 0f);
            }
            else
            {
                element.Width.Set(250000f, 1f);
                element.Height.Set(-500000f, 1f);
                element.Top.Set(25000000f, 0f);
            }
        }

        private static void _UIScrollbar_OnUpdate(UIElement affectedElement)
        {
            UIScrollbar scrollbar = affectedElement as UIScrollbar;
            if (chosenOption == CurrentAltOption.Ore)
            {
                scrollbar.Left = StyleDimension.FromPixels(-25f);
                scrollbar.Height.Set(-250f, 1f);
                scrollbar.Top.Set(150f, 0f);
            }
            else
            {
                scrollbar.Left = StyleDimension.FromPixels(75000000f);
                scrollbar.Height.Set(-250f, 1f);
                scrollbar.Top.Set(150000f, 0f);
            }
        }

        private static void _UIPanel_OnUpdate(UIElement affectedElement)
        {
            UIPanel element = affectedElement as UIPanel;
            if (chosenOption == CurrentAltOption.Ore)
            {
                element.Width.Set(0f, 1f);
                element.Height.Set(-110f, 1f);
            }
            else
            {
                element.Width.Set(0f, 1f);
                element.Height.Set(-110000000f, 1f);
            }
        }

        private static void _UIElement3_OnUpdate(UIElement affectedElement)
        {
            UIElement element = affectedElement;
            if (chosenOption == CurrentAltOption.Ore)
            {
                element.Left = StyleDimension.FromPixels(-50f);
                element.Width.Set(0f, 0.8f);
                element.MaxWidth.Set(450, 0f);
                element.MinWidth.Set(350, 0f);
                element.Top.Set(150f, 0f);
                element.Height.Set(-150f, 1f);
            }
            else
            {
                element.Left = StyleDimension.FromPixels(1000000f);
                element.Width.Set(0f, 0.8f);
                element.MaxWidth.Set(4500000, 0f);
                element.MinWidth.Set(3500000, 0f);
                element.Top.Set(150000000f, 0f);
                element.Height.Set(-150000000f, 1f);
            }
        }

        private static void UIScrollbar_OnUpdate(UIElement affectedElement)
        {
            UIScrollbar scrollbar = affectedElement as UIScrollbar;
            if (chosenOption != CurrentAltOption.Biome)
            {
                scrollbar.Left = StyleDimension.FromPixels(-1000000f);
                scrollbar.Height.Set(-250f, 1f);
                scrollbar.Top.Set(150000f, 0f);
            }
            else
            {
                scrollbar.Left = StyleDimension.FromPixels(-Main.screenWidth + 500f);
                scrollbar.Height.Set(-250f, 1f);
                scrollbar.Top.Set(150f, 0f);
            }
        }

        private static void UIPanel_OnUpdate(UIElement affectedElement)
        {
            UIPanel element = affectedElement as UIPanel;
            if (chosenOption != CurrentAltOption.Biome)
            {
                element.Width.Set(0f, 1f);
                element.Height.Set(-110000000f, 1f);
            }
            else
            {
                element.Width.Set(0f, 1f);
                element.Height.Set(-110f, 1f);
            }
        }

        private static void UIElement3_OnUpdate(UIElement affectedElement)
        {
            UIElement element = affectedElement;
            if (chosenOption != CurrentAltOption.Biome)
            {
                element.Left = StyleDimension.FromPixels(-11000000f);
                element.Width.Set(0f, 0.8f);
                element.MaxWidth.Set(4500000, 0f);
                element.MinWidth.Set(3500000, 0f);
                element.Top.Set(150000000f, 0f);
                element.Height.Set(-150000000f, 1f);
            }
            else
            {
                element.Left = StyleDimension.FromPixels(-Main.screenWidth + 475f);
                element.Width.Set(0f, 0.8f);
                element.MaxWidth.Set(450, 0f);
                element.MinWidth.Set(350, 0f);
                element.Top.Set(150f, 0f);
                element.Height.Set(-150f, 1f);
            }
        }

        public static void UIWorldCreation_FinishCreatingWorld(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchRet()))
                return;
            if (!c.TryGotoPrev(i => i.MatchLdnull()))
                return;
            c.EmitDelegate(() =>
            {
                if (AltHallowBiomeChosenType <= -1)
                {
                    WorldBiomeManager.worldHallow = "";
                }
                else
                {
                    WorldBiomeManager.worldHallow = AltLibrary.biomes[AltHallowBiomeChosenType].FullName;
                }
                if (AltEvilBiomeChosenType <= -1)
                {
                    WorldBiomeManager.worldEvil = "";
                    WorldGen.WorldGenParam_Evil = isCrimson ? 1 : 0;
                    WorldGen.crimson = isCrimson;
                }
                else
                {
                    WorldBiomeManager.worldEvil = AltLibrary.biomes[AltEvilBiomeChosenType].FullName;
                    WorldGen.WorldGenParam_Evil = 0;
                    WorldGen.crimson = false;
                }
                if (AltJungleBiomeChosenType <= -1)
                {
                    WorldBiomeManager.worldJungle = "";
                }
                else
                {
                    WorldBiomeManager.worldJungle = AltLibrary.biomes[AltJungleBiomeChosenType].FullName;
                }
                if (AltHellBiomeChosenType <= -1)
                {
                    WorldBiomeManager.worldHell = "";
                }
                else
                {
                    WorldBiomeManager.worldHell = AltLibrary.biomes[AltHellBiomeChosenType].FullName;
                }
                WorldBiomeManager.Copper = Copper;
                WorldBiomeManager.Iron = Iron;
                WorldBiomeManager.Silver = Silver;
                WorldBiomeManager.Gold = Gold;
                WorldBiomeManager.Cobalt = Cobalt;
                WorldBiomeManager.Mythril = Mythril;
                WorldBiomeManager.Adamantite = Adamantite;

                AltLibrary.Instance.Logger.Info($"On creating world - Hallow: {AltHallowBiomeChosenType} Corrupt: {AltEvilBiomeChosenType} Jungle: {AltJungleBiomeChosenType} Underworld: {AltHellBiomeChosenType}");
            });
        }

        private static void _biomeList_OnUpdate(UIElement affectedElement)
        {
            UIList element = affectedElement as UIList;
            if (chosenOption != CurrentAltOption.Biome)
            {
                element.Width.Set(250000f, 1f);
                element.Height.Set(-500000f, 1f);
                element.Top.Set(25000000f, 0f);
            }
            else
            {
                element.Width.Set(25f, 1f);
                element.Height.Set(-50f, 1f);
                element.Top.Set(25f, 0f);
            }
        }

        public static void ILMakeInfoMenu(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchLdstr("evil")))
                return;
            if (!c.TryGotoNext(i => i.MatchLdloc(1)))
                return;
            RemoveUntilInstruction(c, i => i.MatchLdarg(0));
        }

        public static void UIWorldCreationPreview_DrawSelf(On.Terraria.GameContent.UI.Elements.UIWorldCreationPreview.orig_DrawSelf orig, UIWorldCreationPreview self, SpriteBatch spriteBatch)
        {
            orig(self, spriteBatch);
            CalculatedStyle dimensions = self.GetDimensions();
            Vector2 position = new(dimensions.X + 4f, dimensions.Y + 4f);
            Color color = Color.White;
            int x = 0;
            Rectangle mouseRectangle = Utils.CenteredRectangle(Main.MouseScreen, Vector2.One * 2f);
            if (chosenOption == CurrentAltOption.Biome || AltLibraryConfig.Config.BiomeIconsVisibleOutsideBiomeUI)
            {
                for (int i = 0; i < 4; i++)
                {
                    Asset<Texture2D> asset = ModContent.Request<Texture2D>("Terraria/Images/UI/Bestiary/Icon_Tags_Shadow");
                    if (i == 0 && AltHallowBiomeChosenType >= 0) asset = ModContent.Request<Texture2D>(AltLibrary.biomes[AltHallowBiomeChosenType].IconSmall ?? "AltLibrary/Assets/Menu/ButtonHallow");
                    if (i == 1 && AltEvilBiomeChosenType > -1) asset = ModContent.Request<Texture2D>(AltLibrary.biomes[AltEvilBiomeChosenType].IconSmall ?? "AltLibrary/Assets/Menu/ButtonCorrupt");
                    if (i == 2 && AltHellBiomeChosenType >= 0) asset = ModContent.Request<Texture2D>(AltLibrary.biomes[AltHellBiomeChosenType].IconSmall ?? "AltLibrary/Assets/Menu/ButtonHell");
                    if (i == 3 && AltJungleBiomeChosenType >= 0) asset = ModContent.Request<Texture2D>(AltLibrary.biomes[AltJungleBiomeChosenType].IconSmall ?? "AltLibrary/Assets/Menu/ButtonJungle");
                    Rectangle? rectangle = null;
                    if (i == 0 && AltHallowBiomeChosenType < 0) rectangle = new(30, 30, 30, 30);
                    if (i == 1 && AltEvilBiomeChosenType > -666 && AltEvilBiomeChosenType <= -333) rectangle = new(210, 0, 30, 30);
                    if (i == 1 && AltEvilBiomeChosenType <= -666) rectangle = new(360, 0, 30, 30);
                    if (i == 2 && AltHellBiomeChosenType < 0) rectangle = new(30, 60, 30, 30);
                    if (i == 3 && AltJungleBiomeChosenType < 0) rectangle = new(180, 30, 30, 30);
                    ValueTuple<Asset<Texture2D>, Rectangle?> valueTuple = new(asset, rectangle);
                    spriteBatch.Draw(ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/Button").Value, new Vector2(position.X + 96f, position.Y + 26f * i), color * 0.8f);
                    spriteBatch.Draw(valueTuple.Item1.Value, new Vector2(position.X + 99f, position.Y + 26f * i + 3f), valueTuple.Item2, color, 0f, new Vector2(0f, 0f), 0.5f, SpriteEffects.None, 0f);
                    Vector2 vector2 = new(position.X + 96f, position.Y + 26f * i);
                    if (mouseRectangle.Intersects(Utils.CenteredRectangle(vector2 + new Vector2(16f, 16f), Utils.Size(new Rectangle(0, 0, 30, 30)))))
                    {
                        string line1 = "";
                        if (i == 0 && AltHallowBiomeChosenType < 0) line1 = Language.GetTextValue("Mods.AltLibrary.Biomes.HallowName");
                        if (i == 0 && AltHallowBiomeChosenType >= 0) line1 = AltLibrary.biomes[AltHallowBiomeChosenType].Name;
                        if (i == 1 && AltEvilBiomeChosenType == -333) line1 = Language.GetTextValue("Mods.AltLibrary.Biomes.CorruptName");
                        if (i == 1 && AltEvilBiomeChosenType == -666) line1 = Language.GetTextValue("Mods.AltLibrary.Biomes.CrimsonName");
                        if (i == 1 && AltEvilBiomeChosenType >= 0) line1 = AltLibrary.biomes[AltEvilBiomeChosenType].Name;
                        if (i == 2 && AltHellBiomeChosenType < 0) line1 = Language.GetTextValue("Mods.AltLibrary.Biomes.UnderworldName");
                        if (i == 2 && AltHellBiomeChosenType >= 0) line1 = AltLibrary.biomes[AltHellBiomeChosenType].Name;
                        if (i == 3 && AltJungleBiomeChosenType < 0) line1 = Language.GetTextValue("Mods.AltLibrary.Biomes.JungleName");
                        if (i == 3 && AltJungleBiomeChosenType >= 0) line1 = AltLibrary.biomes[AltJungleBiomeChosenType].Name;
                        string line2 = Language.GetTextValue("Mods.AltLibrary.AddedBy") + " ";
                        if (i == 0 && AltHallowBiomeChosenType < 0) line2 += "Terraria";
                        if (i == 0 && AltHallowBiomeChosenType >= 0) line2 += AltLibrary.biomes[AltHallowBiomeChosenType].Mod.Name;
                        if (i == 1 && AltEvilBiomeChosenType < 0) line2 += "Terraria";
                        if (i == 1 && AltEvilBiomeChosenType >= 0) line2 += AltLibrary.biomes[AltEvilBiomeChosenType].Mod.Name;
                        if (i == 2 && AltHellBiomeChosenType < 0) line2 += "Terraria";
                        if (i == 2 && AltHellBiomeChosenType >= 0) line2 += AltLibrary.biomes[AltHellBiomeChosenType].Mod.Name;
                        if (i == 3 && AltJungleBiomeChosenType < 0) line2 += "Terraria";
                        if (i == 3 && AltJungleBiomeChosenType >= 0) line2 += AltLibrary.biomes[AltJungleBiomeChosenType].Mod.Name;
                        string line = line1 + '\n' + line2;
                        Main.instance.MouseText(line);
                    }
                }
                x = 4;
            }
            if (chosenOption == CurrentAltOption.Ore || AltLibraryConfig.Config.OreIconsVisibleOutsideOreUI)
            {
                for (int i = 0; i < 7; i++)
                {
                    Asset<Texture2D> asset = ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/OreIcons");
                    if (i == 0 && Copper >= 0) asset = ModContent.Request<Texture2D>(AltLibrary.ores[Copper - 1].Texture);
                    if (i == 1 && Iron >= 0) asset = ModContent.Request<Texture2D>(AltLibrary.ores[Iron - 1].Texture);
                    if (i == 2 && Silver >= 0) asset = ModContent.Request<Texture2D>(AltLibrary.ores[Silver - 1].Texture);
                    if (i == 3 && Gold >= 0) asset = ModContent.Request<Texture2D>(AltLibrary.ores[Gold - 1].Texture);
                    if (i == 4 && Cobalt >= 0) asset = ModContent.Request<Texture2D>(AltLibrary.ores[Cobalt - 1].Texture);
                    if (i == 5 && Mythril >= 0) asset = ModContent.Request<Texture2D>(AltLibrary.ores[Mythril - 1].Texture);
                    if (i == 6 && Adamantite >= 0) asset = ModContent.Request<Texture2D>(AltLibrary.ores[Adamantite - 1].Texture);
                    Rectangle? rectangle = null;
                    if (i == 0 && Copper == -1) rectangle = new(0, 0, 30, 30);
                    if (i == 0 && Copper == -2) rectangle = new(30, 0, 30, 30);
                    if (i == 1 && Iron == -3) rectangle = new(60, 0, 30, 30);
                    if (i == 1 && Iron == -4) rectangle = new(90, 0, 30, 30);
                    if (i == 2 && Silver == -5) rectangle = new(120, 0, 30, 30);
                    if (i == 2 && Silver == -6) rectangle = new(150, 0, 30, 30);
                    if (i == 3 && Gold == -7) rectangle = new(180, 0, 30, 30);
                    if (i == 3 && Gold == -8) rectangle = new(210, 0, 30, 30);
                    if (i == 4 && Cobalt == -9) rectangle = new(0, 30, 30, 30);
                    if (i == 4 && Cobalt == -10) rectangle = new(30, 30, 30, 30);
                    if (i == 5 && Mythril == -11) rectangle = new(60, 30, 30, 30);
                    if (i == 5 && Mythril == -12) rectangle = new(90, 30, 30, 30);
                    if (i == 6 && Adamantite == -13) rectangle = new(120, 30, 30, 30);
                    if (i == 6 && Adamantite == -14) rectangle = new(150, 30, 30, 30);
                    ValueTuple<Asset<Texture2D>, Rectangle?> valueTuple = new(asset, rectangle);
                    spriteBatch.Draw(ModContent.Request<Texture2D>("AltLibrary/Assets/Menu/Button").Value, new Vector2(position.X + 96f, position.Y + 26f * (i + x)), color * 0.8f);
                    spriteBatch.Draw(valueTuple.Item1.Value, new Vector2(position.X + 99f, position.Y + 26f * (i + x) + 3f), valueTuple.Item2, color, 0f, new Vector2(0f, 0f), 0.5f, SpriteEffects.None, 0f);
                    Vector2 vector2 = new(position.X + 96f, position.Y + 26f * (i + x));
                    if (mouseRectangle.Intersects(Utils.CenteredRectangle(vector2 + new Vector2(16f, 16f), Utils.Size(new Rectangle(0, 0, 30, 30)))))
                    {
                        string line1 = "";
                        if (i == 0 && Copper == -1) line1 = Language.GetTextValue("Mods.AltLibrary.Ores.CopperName");
                        if (i == 0 && Copper == -2) line1 = Language.GetTextValue("Mods.AltLibrary.Ores.TinName");
                        if (i == 0 && Copper >= 0) line1 = AltLibrary.ores[Copper - 1].Name;
                        if (i == 1 && Iron == -3) line1 = Language.GetTextValue("Mods.AltLibrary.Ores.IronName");
                        if (i == 1 && Iron == -4) line1 = Language.GetTextValue("Mods.AltLibrary.Ores.LeadName");
                        if (i == 1 && Iron >= 0) line1 = AltLibrary.ores[Iron - 1].Name;
                        if (i == 2 && Silver == -5) line1 = Language.GetTextValue("Mods.AltLibrary.Ores.SilverName");
                        if (i == 2 && Silver == -6) line1 = Language.GetTextValue("Mods.AltLibrary.Ores.TungstenName");
                        if (i == 2 && Silver >= 0) line1 = AltLibrary.ores[Silver - 1].Name;
                        if (i == 3 && Gold == -7) line1 = Language.GetTextValue("Mods.AltLibrary.Ores.GoldName");
                        if (i == 3 && Gold == -8) line1 = Language.GetTextValue("Mods.AltLibrary.Ores.PlatinumName");
                        if (i == 3 && Gold >= 0) line1 = AltLibrary.ores[Gold - 1].Name;
                        if (i == 4 && Cobalt == -9) line1 = Language.GetTextValue("Mods.AltLibrary.Ores.CobaltName");
                        if (i == 4 && Cobalt == -10) line1 = Language.GetTextValue("Mods.AltLibrary.Ores.PalladiumName");
                        if (i == 4 && Cobalt >= 0) line1 = AltLibrary.ores[Cobalt - 1].Name;
                        if (i == 5 && Mythril == -11) line1 = Language.GetTextValue("Mods.AltLibrary.Ores.MythrilName");
                        if (i == 5 && Mythril == -12) line1 = Language.GetTextValue("Mods.AltLibrary.Ores.OrichalcumName");
                        if (i == 5 && Mythril >= 0) line1 = AltLibrary.ores[Mythril - 1].Name;
                        if (i == 6 && Adamantite == -13) line1 = Language.GetTextValue("Mods.AltLibrary.Ores.AdamantiteName");
                        if (i == 6 && Adamantite == -14) line1 = Language.GetTextValue("Mods.AltLibrary.Ores.TitaniumName");
                        if (i == 6 && Adamantite >= 0) line1 = AltLibrary.ores[Adamantite - 1].Name;
                        string line2 = Language.GetTextValue("Mods.AltLibrary.AddedBy") + " ";
                        if (i == 0 && Copper < 0) line2 += "Terraria";
                        if (i == 0 && Copper >= 0) line2 += AltLibrary.ores[Copper - 1].Mod.Name;
                        if (i == 1 && Iron < 0) line2 += "Terraria";
                        if (i == 1 && Iron >= 0) line2 += AltLibrary.ores[Iron - 1].Mod.Name;
                        if (i == 2 && Silver < 0) line2 += "Terraria";
                        if (i == 2 && Silver >= 0) line2 += AltLibrary.ores[Silver - 1].Mod.Name;
                        if (i == 3 && Gold < 0) line2 += "Terraria";
                        if (i == 3 && Gold >= 0) line2 += AltLibrary.ores[Gold - 1].Mod.Name;
                        if (i == 4 && Cobalt < 0) line2 += "Terraria";
                        if (i == 4 && Cobalt >= 0) line2 += AltLibrary.ores[Cobalt - 1].Mod.Name;
                        if (i == 5 && Mythril < 0) line2 += "Terraria";
                        if (i == 5 && Mythril >= 0) line2 += AltLibrary.ores[Mythril - 1].Mod.Name;
                        if (i == 6 && Adamantite < 0) line2 += "Terraria";
                        if (i == 7 && Adamantite >= 0) line2 += AltLibrary.ores[Adamantite - 1].Mod.Name;
                        string line = line1 + '\n' + line2;
                        Main.instance.MouseText(line);
                    }
                }
            }
        }

        public static void OnAddWorldEvilOptions(
            On.Terraria.GameContent.UI.States.UIWorldCreation.orig_AddWorldEvilOptions orig,
            UIWorldCreation self, UIElement container,
            float accumualtedHeight,
            UIElement.MouseEvent clickEvent,
            string tagGroup, float usableWidthPercent)
        {
            orig(self, container, accumualtedHeight, clickEvent, tagGroup, usableWidthPercent);
            CurrentAltOption[] array11 = new CurrentAltOption[2]
            {
                CurrentAltOption.Biome,
                CurrentAltOption.Ore
            };
            LocalizedText[] array10 = new LocalizedText[2]
            {
                Language.GetText("Mods.AltLibrary.ChooseBiome"),
                Language.GetText("Mods.AltLibrary.ChooseOre")
            };
            Color[] array8 = new Color[2]
            {
                new Color(130, 183, 108),
                new Color(143, 183, 183)
            };
            string[] array7 = new string[2]
            {
                "Terraria/Images/UI/Bestiary/Icon_Tags_Shadow",
                "Terraria/Images/UI/Bestiary/Icon_Tags_Shadow"
            };
            Rectangle[] array9 = new Rectangle[2]
            {
                new Rectangle(0, 0, 30, 30),
                new Rectangle(60, 0, 30, 30)
            };
            UIElement[] tempArray = container.Children.ToArray();
            for (int i = tempArray.Length - 1; i > tempArray.Length - 4; i--)
            {
                tempArray[i].Remove();
            }
            ALGroupOptionButton<CurrentAltOption>[] array6 = new ALGroupOptionButton<CurrentAltOption>[array11.Length];
            for (int i = 0; i < array6.Length; i++)
            {
                ALGroupOptionButton<CurrentAltOption> groupOptionButton = new(array11[i], array10[i], null, array8[i], array7[i], array9[i], 1f, 1f, 16f)
                {
                    Width = StyleDimension.FromPixelsAndPercent(-4 * (array6.Length - 1), 1f / array6.Length * usableWidthPercent),
                    Left = StyleDimension.FromPercent(1f - usableWidthPercent),
                    HAlign = (float)i / (array6.Length - 1)
                };
                groupOptionButton.Top.Set(accumualtedHeight, 0f);
                groupOptionButton.OnMouseDown += ClickEvilOption;
                groupOptionButton.OnMouseOver += self.ShowOptionDescription;
                groupOptionButton.OnMouseOut += self.ClearOptionDescription;
                groupOptionButton.SetSnapPoint(tagGroup, i, null, null);
                container.Append(groupOptionButton);
                array6[i] = groupOptionButton;
            }
            chosingOption = array6;
        }

        private static void ClickEvilOption(UIMouseEvent evt, UIElement listeningElement)
        {
            ALGroupOptionButton<CurrentAltOption> groupOptionButton = (ALGroupOptionButton<CurrentAltOption>)listeningElement;
            chosenOption = groupOptionButton.OptionValue;
            ALGroupOptionButton<CurrentAltOption>[] evilButtons = chosingOption;
            for (int i = 0; i < evilButtons.Length; i++)
            {
                evilButtons[i].SetCurrentOption(groupOptionButton.OptionValue);
            }
        }

        public static void UIWorldCreation_SetDefaultOptions(On.Terraria.GameContent.UI.States.UIWorldCreation.orig_SetDefaultOptions orig, UIWorldCreation self)
        {
            orig(self);
            ALGroupOptionButton<CurrentAltOption>[] evilButtons = chosingOption;
            for (int i = 0; i < evilButtons.Length; i++)
            {
                evilButtons[i].SetCurrentOption((CurrentAltOption)(-1));
            }
        }

        private static void RemoveUntilInstruction(ILCursor c, Func<Instruction, bool> predicate)
        {
            List<Instruction> instructions = new();
            while (!predicate.Invoke(c.Next))
            {
                c.Remove();
            }
        }
    }
}
