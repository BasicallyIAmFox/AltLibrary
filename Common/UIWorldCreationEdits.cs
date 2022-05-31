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
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace AltLibrary.Common
{
    [Autoload(Side = ModSide.Client)]
    internal class UIWorldCreationEdits
    {
        internal static ALGroupOptionButton<CurrentAltOption>[] chosingOption;
        internal static CurrentAltOption chosenOption;
        internal static int AltEvilBiomeChosenType;
        internal static int AltHallowBiomeChosenType;
        internal static int AltJungleBiomeChosenType;
        internal static int AltHellBiomeChosenType;
        internal static UIList _biomeList;
        internal static List<ALUIBiomeListItem> _biomeElements = new();
        internal static UIList _oreList;
        internal static List<ALUIOreListItem> _oreElements = new();
        internal static UIList _oreHmList;
        internal static List<ALUIOreListItem> _oreHmElements = new();
        internal static int Copper;
        internal static int Iron;
        internal static int Silver;
        internal static int Gold;
        internal static int Cobalt;
        internal static int Mythril;
        internal static int Adamantite;
        internal static bool isCrimson;
        internal static string seed;
        internal enum CurrentAltOption
        {
            Biome,
            Ore,
            HMOre
        }

        public static void Init()
        {
            IL.Terraria.GameContent.UI.States.UIWorldCreation.MakeInfoMenu += ILMakeInfoMenu;
            On.Terraria.GameContent.UI.States.UIWorldCreation.AddWorldEvilOptions += OnAddWorldEvilOptions;
            On.Terraria.GameContent.UI.States.UIWorldCreation.SetDefaultOptions += UIWorldCreation_SetDefaultOptions;
            On.Terraria.GameContent.UI.States.UIWorldCreation.BuildPage += UIWorldCreation_BuildPage;
            IL.Terraria.GameContent.UI.States.UIWorldCreation.Draw += UIWorldCreation_Draw;
            IL.Terraria.GameContent.UI.States.UIWorldCreation.FinishCreatingWorld += UIWorldCreation_FinishCreatingWorld;
            IL.Terraria.GameContent.UI.Elements.UIWorldCreationPreview.DrawSelf += UIWorldCreationPreview_DrawSelf1;
            On.Terraria.GameContent.UI.Elements.UIWorldListItem.PlayGame += UIWorldListItem_PlayGame;
        }

        private static void UIWorldCreationPreview_DrawSelf1(ILContext il)
        {
            ILCursor c = new(il);
            ILLabel label = il.DefineLabel();

            if (!c.TryGotoNext(i => i.MatchLdarg(0),
                i => i.MatchLdfld<UIWorldCreationPreview>("_size"),
                i => i.MatchStloc(3),
                i => i.MatchLdloc(3),
                i => i.MatchSwitch(out _),
                i => i.MatchBr(out _)))
            {
                AltLibrary.Instance.Logger.Info("z $ 1");
                return;
            }

            c.Index++;
            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Br, label);

            if (!c.TryGotoNext(i => i.MatchLdarg(0),
                i => i.MatchLdfld<UIWorldCreationPreview>("_difficulty"),
                i => i.MatchStloc(3),
                i => i.MatchLdloc(3),
                i => i.MatchSwitch(out _),
                i => i.MatchBr(out _)))
            {
                AltLibrary.Instance.Logger.Info("z $ 2");
                return;
            }

            c.MarkLabel(label);
            c.Emit(OpCodes.Ldarg, 0);
            c.Emit(OpCodes.Ldarg, 1);
            c.Emit(OpCodes.Ldloc, 1);
            c.Emit(OpCodes.Ldloc, 2);
            c.Emit(OpCodes.Ldarg, 0);
            c.Emit(OpCodes.Ldfld, typeof(UIWorldCreationPreview).GetField("_size", BindingFlags.NonPublic | BindingFlags.Instance));
            c.Emit(OpCodes.Ldarg, 0);
            c.Emit(OpCodes.Ldfld, typeof(UIWorldCreationPreview).GetField("_EvilCorruptionTexture", BindingFlags.NonPublic | BindingFlags.Instance));
            c.Emit(OpCodes.Ldarg, 0);
            c.Emit(OpCodes.Ldfld, typeof(UIWorldCreationPreview).GetField("_EvilCrimsonTexture", BindingFlags.NonPublic | BindingFlags.Instance));
            c.Emit(OpCodes.Ldarg, 0);
            c.Emit(OpCodes.Ldfld, typeof(UIWorldCreationPreview).GetField("_EvilRandomTexture", BindingFlags.NonPublic | BindingFlags.Instance));
            c.EmitDelegate<Action<UIWorldCreationPreview, SpriteBatch, Vector2, Color, byte, Asset<Texture2D>, Asset<Texture2D>, Asset<Texture2D>>>((self, spriteBatch, position, color, size, _EvilCorruptionTexture, _EvilCrimsonTexture, _EvilRandomTexture) =>
            {
                string folder = (seed != null ? seed.ToLower() : "") switch
                {
                    "05162020" or "5162020" => "Drunk",
                    "not the bees" or "not the bees!" => "NotTheBees",
                    "for the worthy" => "ForTheWorthy",
                    "celebrationmk10" or "05162011" or "5162011" or "05162021" or "5162021" => "Anniversary",
                    "constant" or "theconstant" or "​the constant" or "eye4aneye" or "eye4aneye" => "Constant",
                    _ => "",
                };
                bool broken = false;
                if (AltLibrary.PreviewWorldIcons.Count > 0)
                {
                    foreach (AltLibrary.CustomPreviews preview in AltLibrary.PreviewWorldIcons)
                    {
                        if ((seed != null ? seed.ToLower() : "").ToLower() == preview.seed.ToLower())
                        {
                            switch (size)
                            {
                                case 0:
                                default:
                                    spriteBatch.Draw(ModContent.Request<Texture2D>(preview.pathSmall, AssetRequestMode.ImmediateLoad).Value, position, color);
                                    break;
                                case 1:
                                    spriteBatch.Draw(ModContent.Request<Texture2D>(preview.pathMedium, AssetRequestMode.ImmediateLoad).Value, position, color);
                                    break;
                                case 2:
                                    spriteBatch.Draw(ModContent.Request<Texture2D>(preview.pathLarge, AssetRequestMode.ImmediateLoad).Value, position, color);
                                    break;
                            }
                            broken = true;
                        }
                    }
                }
                if (!broken)
                {
                    int style = AltLibraryConfig.Config.SpecialSeedWorldPreview ? (folder switch
                    {
                        "Drunk" => 1,
                        "NotTheBees" => 2,
                        "ForTheWorthy" => 3,
                        "Anniversary" => 4,
                        "Constant" => 5,
                        _ => 0,
                    }) : 0;
                    spriteBatch.Draw(ALTextureAssets.PreviewSpecialSizes[style, size].Value, position, color);
                }
                Asset<Texture2D> asset = AltEvilBiomeChosenType switch
                {
                    -333 => _EvilCorruptionTexture,
                    -666 => _EvilCrimsonTexture,
                    _ => _EvilRandomTexture,
                };
                if (AltEvilBiomeChosenType > -1)
                {
                    asset = ALTextureAssets.BiomeIconLarge[AltEvilBiomeChosenType] ?? ALTextureAssets.NullPreview;
                }
                spriteBatch.Draw(asset.Value, position, color);
                WorldCreationUIIcons(self, spriteBatch);
            });
        }

        public static void Unload()
        {
            IL.Terraria.GameContent.UI.States.UIWorldCreation.MakeInfoMenu -= ILMakeInfoMenu;
            On.Terraria.GameContent.UI.States.UIWorldCreation.AddWorldEvilOptions -= OnAddWorldEvilOptions;
            On.Terraria.GameContent.UI.States.UIWorldCreation.SetDefaultOptions -= UIWorldCreation_SetDefaultOptions;
            On.Terraria.GameContent.UI.States.UIWorldCreation.BuildPage -= UIWorldCreation_BuildPage;
            IL.Terraria.GameContent.UI.States.UIWorldCreation.Draw -= UIWorldCreation_Draw;
            IL.Terraria.GameContent.UI.States.UIWorldCreation.FinishCreatingWorld -= UIWorldCreation_FinishCreatingWorld;
            IL.Terraria.GameContent.UI.Elements.UIWorldCreationPreview.DrawSelf -= UIWorldCreationPreview_DrawSelf1;
            On.Terraria.GameContent.UI.Elements.UIWorldListItem.PlayGame -= UIWorldListItem_PlayGame;
            chosingOption = null;
            chosenOption = CurrentAltOption.Biome;
            AltEvilBiomeChosenType = 0;
            AltHallowBiomeChosenType = 0;
            AltJungleBiomeChosenType = 0;
            AltHellBiomeChosenType = 0;
            _biomeList = null;
            _biomeElements = null;
            _oreList = null;
            _oreElements = null;
            _oreHmList = null;
            _oreHmElements = null;
            Copper = 0;
            Iron = 0;
            Silver = 0;
            Gold = 0;
            Cobalt = 0;
            Mythril = 0;
            Adamantite = 0;
            seed = null;
        }

        private static void UIWorldCreation_Draw(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchRet() && i.Offset != 0))
            {
                AltLibrary.Instance.Logger.Info("3 $ 1");
                return;
            }

            c.Emit(OpCodes.Ldarg, 0);
            c.EmitDelegate<Action<UIWorldCreation>>((self) =>
            {
                seed = (string)typeof(UIWorldCreation).GetField("_optionSeed", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(self);
            });
        }

        private static void UIWorldListItem_PlayGame(On.Terraria.GameContent.UI.Elements.UIWorldListItem.orig_PlayGame orig, UIWorldListItem self, UIMouseEvent evt, UIElement listeningElement)
        {
            if ((WorldFileData)typeof(UIWorldListItem).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self) == null)
                return;
            if (ALUtils.IsWorldValid(self))
            {
                orig(self, evt, listeningElement);
            }
        }

        public static void UIWorldCreation_BuildPage(On.Terraria.GameContent.UI.States.UIWorldCreation.orig_BuildPage orig, UIWorldCreation self)
        {
            chosenOption = (CurrentAltOption)(-1);

            List<int> evilBiomeTypes = new() { -333, -666 };
            AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Evil && x.Selectable).ToList().ForEach(x => evilBiomeTypes.Add(x.Type - 1));
            AltEvilBiomeChosenType = Main.rand.Next(evilBiomeTypes);
            isCrimson = AltEvilBiomeChosenType == -666;
            List<int> hallowBiomeTypes = new() { -3 };
            AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Hallow && x.Selectable).ToList().ForEach(x => hallowBiomeTypes.Add(x.Type - 1));
            AltHallowBiomeChosenType = Main.rand.Next(hallowBiomeTypes);
            List<int> hellBiomeTypes = new() { -5 };
            AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Hell && x.Selectable).ToList().ForEach(x => hellBiomeTypes.Add(x.Type - 1));
            AltHellBiomeChosenType = Main.rand.Next(hellBiomeTypes);
            List<int> jungleBiomeTypes = new() { -4 };
            AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Jungle && x.Selectable).ToList().ForEach(x => jungleBiomeTypes.Add(x.Type - 1));
            AltJungleBiomeChosenType = Main.rand.Next(jungleBiomeTypes);
            List<int> ores = new() { -1, -2 };
            AltLibrary.Ores.Where(x => x.OreType == OreType.Copper && x.Selectable).ToList().ForEach(x => ores.Add(x.Type));
            Copper = Main.rand.Next(ores);
            ores = new() { -3, -4 };
            AltLibrary.Ores.Where(x => x.OreType == OreType.Iron && x.Selectable).ToList().ForEach(x => ores.Add(x.Type));
            Iron = Main.rand.Next(ores);
            ores = new() { -5, -6 };
            AltLibrary.Ores.Where(x => x.OreType == OreType.Silver && x.Selectable).ToList().ForEach(x => ores.Add(x.Type));
            Silver = Main.rand.Next(ores);
            ores = new() { -7, -8 };
            AltLibrary.Ores.Where(x => x.OreType == OreType.Gold && x.Selectable).ToList().ForEach(x => ores.Add(x.Type));
            Gold = Main.rand.Next(ores);
            ores = new() { -9, -10 };
            AltLibrary.Ores.Where(x => x.OreType == OreType.Cobalt && x.Selectable).ToList().ForEach(x => ores.Add(x.Type));
            Cobalt = Main.rand.Next(ores);
            ores = new() { -11, -12 };
            AltLibrary.Ores.Where(x => x.OreType == OreType.Mythril && x.Selectable).ToList().ForEach(x => ores.Add(x.Type));
            Mythril = Main.rand.Next(ores);
            ores = new() { -13, -14 };
            AltLibrary.Ores.Where(x => x.OreType == OreType.Adamantite && x.Selectable).ToList().ForEach(x => ores.Add(x.Type));
            Adamantite = Main.rand.Next(ores);

            orig(self);

            _oreElements.Clear();
            _oreList = null;

            #region Ore UI List
            {
                UIElement uIElement3 = new()
                {
                    Left = StyleDimension.FromPixels(Main.screenWidth - (Main.screenWidth - 100f))
                };
                uIElement3.Width.Set(0f, 0.8f);
                uIElement3.MaxWidth.Set(450, 0f);
                uIElement3.MinWidth.Set(350, 0f);
                uIElement3.Top.Set(150f, 0f);
                uIElement3.Height.Set(-150f, 1f);
                uIElement3.HAlign = 1f;
                uIElement3.OnUpdate += RUIElement3_OnUpdate;
                self.Append(uIElement3);
                UIPanel uIPanel = new();
                uIPanel.Width.Set(0f, 1f);
                uIPanel.Height.Set(-110f, 1f);
                uIPanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
                uIPanel.PaddingTop = 0f;
                uIPanel.OnUpdate += JUIPanel_OnUpdate;
                uIElement3.Append(uIPanel);
                _oreList = new UIList();
                _oreList.Width.Set(25f, 1f);
                _oreList.Height.Set(-50f, 1f);
                _oreList.Top.Set(25f, 0f);
                _oreList.ListPadding = 5f;
                _oreList.HAlign = 1f;
                _oreList.OnUpdate += M2oreList_OnUpdate;
                uIPanel.Append(_oreList);

                UIScrollbar uIScrollbar = new();
                uIScrollbar.SetView(100f, 100f);
                uIScrollbar.Left = StyleDimension.FromPixels(Main.screenWidth - (Main.screenWidth - 75f));
                uIScrollbar.Height.Set(-250f, 1f);
                uIScrollbar.Top.Set(150f, 0f);
                uIScrollbar.HAlign = 1f;
                uIScrollbar.OnUpdate += GUIScrollbar_OnUpdate;
                self.Append(uIScrollbar);
                _oreList.SetScrollbar(uIScrollbar);

                UIImageButton closeIcon = new(ALTextureAssets.ButtonClose);
                closeIcon.Width.Set(22, 0f);
                closeIcon.Height.Set(22, 0f);
                closeIcon.Top.Set(5, 0);
                closeIcon.Left.Set(5, 0);
                closeIcon.SetVisibility(1f, 1f);
                closeIcon.OnClick += CloseIcon_OnClick;
                uIElement3.Append(closeIcon);

                List<AltOre> prehmList = new();
                prehmList.Clear();
                prehmList.Add(new RandomOptionOre("RandomCopper"));
                prehmList.Add(new RandomOptionOre("RandomIron"));
                prehmList.Add(new RandomOptionOre("RandomSilver"));
                prehmList.Add(new RandomOptionOre("RandomGold"));
                prehmList.Add(new VanillaOre("Copper", "Copper", -1, TileID.Copper, ItemID.CopperBar, OreType.Copper));
                prehmList.Add(new VanillaOre("Tin", "Tin", -2, TileID.Tin, ItemID.TinBar, OreType.Copper));
                prehmList.AddRange(AltLibrary.Ores.Where(x => x.OreType == OreType.Copper && x.Selectable));
                prehmList.Add(new VanillaOre("Iron", "Iron", -3, TileID.Iron, ItemID.IronBar, OreType.Iron));
                prehmList.Add(new VanillaOre("Lead", "Lead", -4, TileID.Lead, ItemID.LeadBar, OreType.Iron));
                prehmList.AddRange(AltLibrary.Ores.Where(x => x.OreType == OreType.Iron && x.Selectable));
                prehmList.Add(new VanillaOre("Silver", "Silver", -5, TileID.Silver, ItemID.SilverBar, OreType.Silver));
                prehmList.Add(new VanillaOre("Tungsten", "Tungsten", -6, TileID.Tungsten, ItemID.TungstenBar, OreType.Silver));
                prehmList.AddRange(AltLibrary.Ores.Where(x => x.OreType == OreType.Silver && x.Selectable));
                prehmList.Add(new VanillaOre("Gold", "Gold", -7, TileID.Gold, ItemID.GoldBar, OreType.Gold));
                prehmList.Add(new VanillaOre("Platinum", "Platinum", -8, TileID.Platinum, ItemID.PlatinumBar, OreType.Gold));
                prehmList.AddRange(AltLibrary.Ores.Where(x => x.OreType == OreType.Gold && x.Selectable));
                foreach (AltOre ore in prehmList)
                {
                    ALUIOreListItem item = new(ore, false);
                    _oreList.Add(item);
                    _oreElements.Add(item);
                }
            }
            #endregion

            _oreHmElements.Clear();
            _oreHmList = null;

            #region Hardmore Ores
            {
                UIElement uIElement3 = new()
                {
                    Left = StyleDimension.FromPixels(Main.screenWidth - (Main.screenWidth - 100f))
                };
                uIElement3.Width.Set(0f, 0.8f);
                uIElement3.MaxWidth.Set(450, 0f);
                uIElement3.MinWidth.Set(350, 0f);
                uIElement3.Top.Set(150f, 0f);
                uIElement3.Height.Set(-150f, 1f);
                uIElement3.HAlign = 1f;
                uIElement3.OnUpdate += UIElement3_OnUpdate1;
                self.Append(uIElement3);
                UIPanel uIPanel = new();
                uIPanel.Width.Set(0f, 1f);
                uIPanel.Height.Set(-110f, 1f);
                uIPanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
                uIPanel.PaddingTop = 0f;
                uIPanel.OnUpdate += UIPanel_OnUpdate1;
                uIElement3.Append(uIPanel);
                _oreHmList = new UIList();
                _oreHmList.Width.Set(25f, 1f);
                _oreHmList.Height.Set(-50f, 1f);
                _oreHmList.Top.Set(25f, 0f);
                _oreHmList.ListPadding = 5f;
                _oreHmList.HAlign = 1f;
                _oreHmList.OnUpdate += MoreHmList_OnUpdate;
                uIPanel.Append(_oreHmList);

                UIScrollbar uIScrollbar = new();
                uIScrollbar.SetView(100f, 100f);
                uIScrollbar.Left = StyleDimension.FromPixels(Main.screenWidth - (Main.screenWidth - 75f));
                uIScrollbar.Height.Set(-250f, 1f);
                uIScrollbar.Top.Set(150f, 0f);
                uIScrollbar.HAlign = 1f;
                uIScrollbar.OnUpdate += UIScrollbar_OnUpdate1;
                self.Append(uIScrollbar);
                _oreHmList.SetScrollbar(uIScrollbar);

                UIImageButton closeIcon = new(ALTextureAssets.ButtonClose);
                closeIcon.Width.Set(22, 0f);
                closeIcon.Height.Set(22, 0f);
                closeIcon.Top.Set(5, 0);
                closeIcon.Left.Set(5, 0);
                closeIcon.SetVisibility(1f, 1f);
                closeIcon.OnClick += CloseIcon_OnClick;
                uIElement3.Append(closeIcon);

                List<AltOre> hardmodeListing = new();
                hardmodeListing.Clear();
                hardmodeListing.Add(new RandomOptionOre("RandomCobalt"));
                hardmodeListing.Add(new RandomOptionOre("RandomMythril"));
                hardmodeListing.Add(new RandomOptionOre("RandomAdamantite"));
                hardmodeListing.Add(new VanillaOre("Cobalt", "Cobalt", -9, TileID.Cobalt, ItemID.CobaltBar, OreType.Cobalt));
                hardmodeListing.Add(new VanillaOre("Palladium", "Palladium", -10, TileID.Palladium, ItemID.PalladiumBar, OreType.Cobalt));
                hardmodeListing.AddRange(AltLibrary.Ores.Where(x => x.OreType == OreType.Cobalt && x.Selectable));
                hardmodeListing.Add(new VanillaOre("Mythril", "Mythril", -11, TileID.Mythril, ItemID.MythrilBar, OreType.Mythril));
                hardmodeListing.Add(new VanillaOre("Orichalcum", "Orichalcum", -12, TileID.Orichalcum, ItemID.OrichalcumBar, OreType.Mythril));
                hardmodeListing.AddRange(AltLibrary.Ores.Where(x => x.OreType == OreType.Mythril && x.Selectable));
                hardmodeListing.Add(new VanillaOre("Adamantite", "Adamantite", -13, TileID.Adamantite, ItemID.AdamantiteBar, OreType.Adamantite));
                hardmodeListing.Add(new VanillaOre("Titanium", "Titanium", -14, TileID.Titanium, ItemID.TitaniumBar, OreType.Adamantite));
                hardmodeListing.AddRange(AltLibrary.Ores.Where(x => x.OreType == OreType.Adamantite && x.Selectable));
                foreach (AltOre ore in hardmodeListing)
                {
                    ALUIOreListItem item = new(ore, false);
                    _oreHmList.Add(item);
                    _oreHmElements.Add(item);
                }
            }
            #endregion

            _biomeElements.Clear();
            _biomeList = null;

            #region Biome UI List
            {
                UIElement uIElement3 = new()
                {
                    Left = StyleDimension.FromPixels(Main.screenWidth - (Main.screenWidth + 100f))
                };
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
                _biomeList.OnUpdate += ZbiomeList_OnUpdate;
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

                UIImageButton closeIcon = new(ALTextureAssets.ButtonClose);
                closeIcon.Width.Set(22, 0f);
                closeIcon.Height.Set(22, 0f);
                closeIcon.Top.Set(5, 0);
                closeIcon.Left.Set(5, 0);
                closeIcon.SetVisibility(1f, 1f);
                closeIcon.OnClick += CloseIcon_OnClick;
                uIElement3.Append(closeIcon);

                List<AltBiome> list = new();
                list.Clear();
                list.Add(new RandomOptionBiome("RandomEvilBiome"));
                if (AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Hallow && x.Selectable).Any())
                {
                    list.Add(new RandomOptionBiome("RandomHallowBiome"));
                }
                if (AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Jungle && x.Selectable).Any())
                {
                    list.Add(new RandomOptionBiome("RandomJungleBiome"));
                }
                if (AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Hell && x.Selectable).Any())
                {
                    list.Add(new RandomOptionBiome("RandomUnderworldBiome"));
                }
                list.Add(new VanillaBiome("CorruptBiome", BiomeType.Evil, -333, Color.MediumPurple, false));
                list.Add(new VanillaBiome("CrimsonBiome", BiomeType.Evil, -666, Color.IndianRed, true));
                list.AddRange(AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Evil && x.Selectable));
                bool bl = AltLibraryConfig.Config.VanillaShowUpIfOnlyAltVarExist == true && AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Hallow && x.Selectable).Any();
                if (AltLibraryConfig.Config.VanillaShowUpIfOnlyAltVarExist == false)
                {
                    bl = true;
                }
                if (bl)
                {
                    list.Add(new VanillaBiome("HallowBiome", BiomeType.Hallow, -3, Color.HotPink));
                }
                list.AddRange(AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Hallow && x.Selectable));
                bl = AltLibraryConfig.Config.VanillaShowUpIfOnlyAltVarExist == true && AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Jungle && x.Selectable).Any();
                if (AltLibraryConfig.Config.VanillaShowUpIfOnlyAltVarExist == false)
                {
                    bl = true;
                }
                if (bl)
                {
                    list.Add(new VanillaBiome("JungleBiome", BiomeType.Jungle, -4, Color.SpringGreen));
                }
                list.AddRange(AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Jungle && x.Selectable));
                bl = AltLibraryConfig.Config.VanillaShowUpIfOnlyAltVarExist == true && AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Hell && x.Selectable).Any();
                if (AltLibraryConfig.Config.VanillaShowUpIfOnlyAltVarExist == false)
                {
                    bl = true;
                }
                if (bl)
                {
                    list.Add(new VanillaBiome("UnderworldBiome", BiomeType.Hell, -5, Color.OrangeRed));
                }
                list.AddRange(AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Hell && x.Selectable));
                foreach (AltBiome biome in list)
                {
                    ALUIBiomeListItem item = new(biome, false);
                    _biomeList.Add(item);
                    _biomeElements.Add(item);
                }
            }
            #endregion
        }

        private static void UIScrollbar_OnUpdate1(UIElement affectedElement)
        {
            UIScrollbar scrollbar = affectedElement as UIScrollbar;
            if (chosenOption == CurrentAltOption.HMOre)
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

        private static void MoreHmList_OnUpdate(UIElement affectedElement)
        {
            UIList element = affectedElement as UIList;
            if (chosenOption == CurrentAltOption.HMOre)
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

        private static void UIPanel_OnUpdate1(UIElement affectedElement)
        {
            UIPanel element = affectedElement as UIPanel;
            if (chosenOption == CurrentAltOption.HMOre)
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

        private static void UIElement3_OnUpdate1(UIElement affectedElement)
        {
            UIElement element = affectedElement;
            if (chosenOption == CurrentAltOption.HMOre)
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

        private static void CloseIcon_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            chosenOption = (CurrentAltOption)(-1);
            ALGroupOptionButton<CurrentAltOption>[] evilButtons = chosingOption;
            for (int i = 0; i < evilButtons.Length; i++)
            {
                evilButtons[i].SetCurrentOption((CurrentAltOption)(-1));
            }
        }

        private static void M2oreList_OnUpdate(UIElement affectedElement)
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

        private static void GUIScrollbar_OnUpdate(UIElement affectedElement)
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

        private static void JUIPanel_OnUpdate(UIElement affectedElement)
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

        private static void RUIElement3_OnUpdate(UIElement affectedElement)
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
            {
                AltLibrary.Instance.Logger.Info("0 $ 1");
                return;
            }
            if (!c.TryGotoPrev(i => i.MatchLdnull()))
            {
                AltLibrary.Instance.Logger.Info("0 $ 2");
                return;
            }
            c.EmitDelegate(() =>
            {
                if (AltHallowBiomeChosenType <= -1)
                {
                    WorldBiomeManager.WorldHallow = "";
                }
                else
                {
                    WorldBiomeManager.WorldHallow = AltLibrary.Biomes[AltHallowBiomeChosenType].FullName;
                }
                if (AltEvilBiomeChosenType <= -1)
                {
                    WorldBiomeManager.WorldEvil = "";
                    WorldGen.WorldGenParam_Evil = isCrimson ? 1 : 0;
                    WorldGen.crimson = isCrimson;
                }
                else
                {
                    WorldBiomeManager.WorldEvil = AltLibrary.Biomes[AltEvilBiomeChosenType].FullName;
                    WorldGen.WorldGenParam_Evil = 0;
                    WorldGen.crimson = false;
                }
                if (AltJungleBiomeChosenType <= -1)
                {
                    WorldBiomeManager.WorldJungle = "";
                }
                else
                {
                    WorldBiomeManager.WorldJungle = AltLibrary.Biomes[AltJungleBiomeChosenType].FullName;
                }
                if (AltHellBiomeChosenType <= -1)
                {
                    WorldBiomeManager.WorldHell = "";
                }
                else
                {
                    WorldBiomeManager.WorldHell = AltLibrary.Biomes[AltHellBiomeChosenType].FullName;
                }
                WorldBiomeManager.Copper = Copper;
                WorldBiomeManager.Iron = Iron;
                WorldBiomeManager.Silver = Silver;
                WorldBiomeManager.Gold = Gold;
                WorldBiomeManager.Cobalt = Cobalt;
                WorldBiomeManager.Mythril = Mythril;
                WorldBiomeManager.Adamantite = Adamantite;
                //WorldBiomeManager.cobaIndex = Cobalt < 0 ? (Cobalt == -10 ? 1 : 0) : AltLibrary.OrderedCobalt.FindIndex(x => x == AltLibrary.Ores.Find(x => x.Type == Cobalt).ore);
                //WorldBiomeManager.mythIndex = Mythril < 0 ? (Mythril == -12 ? 1 : 0) : AltLibrary.OrderedMythril.FindIndex(x => x == AltLibrary.Ores.Find(x => x.Type == Mythril).ore);
                //WorldBiomeManager.adamIndex = Adamantite < 0 ? (Adamantite == -14 ? 1 : 0) : AltLibrary.OrderedAdamant.FindIndex(x => x == AltLibrary.Ores.Find(x => x.Type == Adamantite).ore);

                AltLibrary.Instance.Logger.Info($"On creating world - Hallow: {AltHallowBiomeChosenType} Corrupt: {AltEvilBiomeChosenType} Jungle: {AltJungleBiomeChosenType} Underworld: {AltHellBiomeChosenType}");
            });
        }

        private static void ZbiomeList_OnUpdate(UIElement affectedElement)
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
            {
                AltLibrary.Instance.Logger.Info("1 $ 1");
                return;
            }
            if (!c.TryGotoNext(i => i.MatchLdloc(1)))
            {
                AltLibrary.Instance.Logger.Info("1 $ 2");
                return;
            }
            RemoveUntilInstruction(c, i => i.MatchLdarg(0));
        }

        internal static void WorldCreationUIIcons(UIWorldCreationPreview self, SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = self.GetDimensions();
            Vector2 position = new(dimensions.X + 4f, dimensions.Y + 4f);
            Color color = Color.White;
            Rectangle mouseRectangle = Utils.CenteredRectangle(Main.MouseScreen, Vector2.One * 2f);
            int x = 0;

            void DrawIcon(Func<bool> cond, Action<SpriteBatch> action)
            {
                if (cond.Invoke())
                {
                    Asset<Texture2D> asset = ALTextureAssets.BestiaryIcons;
                    action.Invoke(spriteBatch);
                    x++;
                }
            }

            DrawIcon(() =>
            {
                bool isAlt = AltLibraryConfig.Config.VanillaShowUpIfOnlyAltVarExist == true;
                if (isAlt) isAlt &= AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Hallow && x.Selectable).Any();
                if (AltLibraryConfig.Config.VanillaShowUpIfOnlyAltVarExist == false)
                {
                    isAlt = true;
                }
                return (chosenOption == CurrentAltOption.Biome || AltLibraryConfig.Config.BiomeIconsVisibleOutsideBiomeUI) && isAlt;
            },
                new Action<SpriteBatch>((spriteBatch) =>
                {
                    Asset<Texture2D> asset = ALTextureAssets.BestiaryIcons;
                    if (AltHallowBiomeChosenType >= 0) asset = ModContent.Request<Texture2D>(AltLibrary.Biomes[AltHallowBiomeChosenType].IconSmall ?? "AltLibrary/Assets/Menu/ButtonHallow", AssetRequestMode.ImmediateLoad);
                    Rectangle? rectangle = null;
                    if (AltHallowBiomeChosenType < 0) rectangle = new(30, 30, 30, 30);
                    ValueTuple<Asset<Texture2D>, Rectangle?> valueTuple = new(asset, rectangle);
                    spriteBatch.Draw(ALTextureAssets.Button.Value, new Vector2(position.X + 96f, position.Y + 26f * x), color * 0.8f);
                    spriteBatch.Draw(valueTuple.Item1.Value, new Vector2(position.X + 99f, position.Y + 26f * x + 3f), valueTuple.Item2, color, 0f, new Vector2(0f, 0f), 0.5f, SpriteEffects.None, 0f);
                    Vector2 vector2 = new(position.X + 96f, position.Y + 26f * x);
                    if (mouseRectangle.Intersects(Utils.CenteredRectangle(vector2 + new Vector2(7.5f, 7.5f), Utils.Size(new Rectangle(0, 0, 30, 30)))))
                    {
                        string line1 = "";
                        if (AltHallowBiomeChosenType < 0) line1 = Language.GetTextValue("Mods.AltLibrary.AltBiomeName.HallowBiome");
                        if (AltHallowBiomeChosenType >= 0) line1 = AltLibrary.Biomes[AltHallowBiomeChosenType].Name;
                        string line2 = Language.GetTextValue("Mods.AltLibrary.AddedBy") + " ";
                        if (AltHallowBiomeChosenType < 0) line2 += "Terraria";
                        if (AltHallowBiomeChosenType >= 0) line2 += AltLibrary.Biomes[AltHallowBiomeChosenType].Mod.Name;
                        string line = line1 + '\n' + line2;
                        Main.instance.MouseText(line);
                    }
                }));
            DrawIcon(() => chosenOption == CurrentAltOption.Biome || AltLibraryConfig.Config.BiomeIconsVisibleOutsideBiomeUI,
                new Action<SpriteBatch>((spriteBatch) =>
                {
                    Asset<Texture2D> asset = ALTextureAssets.BestiaryIcons;
                    if (AltEvilBiomeChosenType > -1) asset = ModContent.Request<Texture2D>(AltLibrary.Biomes[AltEvilBiomeChosenType].IconSmall ?? "AltLibrary/Assets/Menu/ButtonCorrupt", AssetRequestMode.ImmediateLoad);
                    Rectangle? rectangle = null;
                    if (AltEvilBiomeChosenType > -666 && AltEvilBiomeChosenType <= -333) rectangle = new(210, 0, 30, 30);
                    if (AltEvilBiomeChosenType <= -666) rectangle = new(360, 0, 30, 30);
                    ValueTuple<Asset<Texture2D>, Rectangle?> valueTuple = new(asset, rectangle);
                    spriteBatch.Draw(ALTextureAssets.Button.Value, new Vector2(position.X + 96f, position.Y + 26f * x), color * 0.8f);
                    spriteBatch.Draw(valueTuple.Item1.Value, new Vector2(position.X + 99f, position.Y + 26f * x + 3f), valueTuple.Item2, color, 0f, new Vector2(0f, 0f), 0.5f, SpriteEffects.None, 0f);
                    Vector2 vector2 = new(position.X + 96f, position.Y + 26f * x);
                    if (mouseRectangle.Intersects(Utils.CenteredRectangle(vector2 + new Vector2(7.5f, 7.5f), Utils.Size(new Rectangle(0, 0, 30, 30)))))
                    {
                        string line1 = "";
                        if (AltEvilBiomeChosenType == -333) line1 = Language.GetTextValue("Mods.AltLibrary.AltBiomeName.CorruptBiome");
                        if (AltEvilBiomeChosenType == -666) line1 = Language.GetTextValue("Mods.AltLibrary.AltBiomeName.CrimsonBiome");
                        if (AltEvilBiomeChosenType >= 0) line1 = AltLibrary.Biomes[AltEvilBiomeChosenType].Name;
                        string line2 = Language.GetTextValue("Mods.AltLibrary.AddedBy") + " ";
                        if (AltEvilBiomeChosenType < 0) line2 += "Terraria";
                        if (AltEvilBiomeChosenType >= 0) line2 += AltLibrary.Biomes[AltEvilBiomeChosenType].Mod.Name;
                        string line = line1 + '\n' + line2;
                        Main.instance.MouseText(line);
                    }
                }));
            DrawIcon(() =>
            {
                bool isAlt = AltLibraryConfig.Config.VanillaShowUpIfOnlyAltVarExist == true;
                if (isAlt) isAlt &= AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Hell && x.Selectable).Any();
                if (AltLibraryConfig.Config.VanillaShowUpIfOnlyAltVarExist == false)
                {
                    isAlt = true;
                }
                return (chosenOption == CurrentAltOption.Biome || AltLibraryConfig.Config.BiomeIconsVisibleOutsideBiomeUI) && isAlt;
            },
                new Action<SpriteBatch>((spriteBatch) =>
                {
                    Asset<Texture2D> asset = ALTextureAssets.BestiaryIcons;
                    if (AltHellBiomeChosenType >= 0) asset = ModContent.Request<Texture2D>(AltLibrary.Biomes[AltHellBiomeChosenType].IconSmall ?? "AltLibrary/Assets/Menu/ButtonHell", AssetRequestMode.ImmediateLoad);
                    Rectangle? rectangle = null;
                    if (AltHellBiomeChosenType < 0) rectangle = new(30, 60, 30, 30);
                    ValueTuple<Asset<Texture2D>, Rectangle?> valueTuple = new(asset, rectangle);
                    spriteBatch.Draw(ALTextureAssets.Button.Value, new Vector2(position.X + 96f, position.Y + 26f * x), color * 0.8f);
                    spriteBatch.Draw(valueTuple.Item1.Value, new Vector2(position.X + 99f, position.Y + 26f * x + 3f), valueTuple.Item2, color, 0f, new Vector2(0f, 0f), 0.5f, SpriteEffects.None, 0f);
                    Vector2 vector2 = new(position.X + 96f, position.Y + 26f * x);
                    if (mouseRectangle.Intersects(Utils.CenteredRectangle(vector2 + new Vector2(7.5f, 7.5f), Utils.Size(new Rectangle(0, 0, 30, 30)))))
                    {
                        string line1 = "";
                        if (AltHellBiomeChosenType < 0) line1 = Language.GetTextValue("Mods.AltLibrary.AltBiomeName.UnderworldBiome");
                        if (AltHellBiomeChosenType >= 0) line1 = AltLibrary.Biomes[AltHellBiomeChosenType].Name;
                        string line2 = Language.GetTextValue("Mods.AltLibrary.AddedBy") + " ";
                        if (AltHallowBiomeChosenType < 0) line2 += "Terraria";
                        if (AltHallowBiomeChosenType >= 0) line2 += AltLibrary.Biomes[AltHallowBiomeChosenType].Mod.Name;
                        string line = line1 + '\n' + line2;
                        Main.instance.MouseText(line);
                    }
                }));
            DrawIcon(() =>
            {
                bool isAlt = AltLibraryConfig.Config.VanillaShowUpIfOnlyAltVarExist == true;
                if (isAlt) isAlt &= AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Jungle && x.Selectable).Any();
                if (AltLibraryConfig.Config.VanillaShowUpIfOnlyAltVarExist == false)
                {
                    isAlt = true;
                }
                return (chosenOption == CurrentAltOption.Biome || AltLibraryConfig.Config.BiomeIconsVisibleOutsideBiomeUI) && isAlt;
            },
                new Action<SpriteBatch>((spriteBatch) =>
                {
                    Asset<Texture2D> asset = ALTextureAssets.BestiaryIcons;
                    if (AltJungleBiomeChosenType >= 0) asset = ModContent.Request<Texture2D>(AltLibrary.Biomes[AltJungleBiomeChosenType].IconSmall ?? "AltLibrary/Assets/Menu/ButtonJungle", AssetRequestMode.ImmediateLoad);
                    Rectangle? rectangle = null;
                    if (AltHellBiomeChosenType < 0) rectangle = new(180, 30, 30, 30);
                    ValueTuple<Asset<Texture2D>, Rectangle?> valueTuple = new(asset, rectangle);
                    spriteBatch.Draw(ALTextureAssets.Button.Value, new Vector2(position.X + 96f, position.Y + 26f * x), color * 0.8f);
                    spriteBatch.Draw(valueTuple.Item1.Value, new Vector2(position.X + 99f, position.Y + 26f * x + 3f), valueTuple.Item2, color, 0f, new Vector2(0f, 0f), 0.5f, SpriteEffects.None, 0f);
                    Vector2 vector2 = new(position.X + 96f, position.Y + 26f * x);
                    if (mouseRectangle.Intersects(Utils.CenteredRectangle(vector2 + new Vector2(7.5f, 7.5f), Utils.Size(new Rectangle(0, 0, 30, 30)))))
                    {
                        string line1 = "";
                        if (AltJungleBiomeChosenType < 0) line1 = Language.GetTextValue("Mods.AltLibrary.AltBiomeName.JungleBiome");
                        if (AltJungleBiomeChosenType >= 0) line1 = AltLibrary.Biomes[AltJungleBiomeChosenType].Name;
                        string line2 = Language.GetTextValue("Mods.AltLibrary.AddedBy") + " ";
                        if (AltJungleBiomeChosenType < 0) line2 += "Terraria";
                        if (AltJungleBiomeChosenType >= 0) line2 += AltLibrary.Biomes[AltJungleBiomeChosenType].Mod.Name;
                        string line = line1 + '\n' + line2;
                        Main.instance.MouseText(line);
                    }
                }));
            if (chosenOption == CurrentAltOption.Ore || AltLibraryConfig.Config.OreIconsVisibleOutsideOreUI)
            {
                for (int i = 0; i < 4; i++)
                {
                    Asset<Texture2D> asset = ALTextureAssets.OreIcons;
                    if (i == 0 && Copper >= 0) asset = ModContent.Request<Texture2D>(AltLibrary.Ores[Copper - 1].Texture);
                    if (i == 1 && Iron >= 0) asset = ModContent.Request<Texture2D>(AltLibrary.Ores[Iron - 1].Texture);
                    if (i == 2 && Silver >= 0) asset = ModContent.Request<Texture2D>(AltLibrary.Ores[Silver - 1].Texture);
                    if (i == 3 && Gold >= 0) asset = ModContent.Request<Texture2D>(AltLibrary.Ores[Gold - 1].Texture);
                    Rectangle? rectangle = null;
                    if (i == 0 && Copper == -1) rectangle = new(0, 0, 30, 30);
                    if (i == 0 && Copper == -2) rectangle = new(30, 0, 30, 30);
                    if (i == 1 && Iron == -3) rectangle = new(60, 0, 30, 30);
                    if (i == 1 && Iron == -4) rectangle = new(90, 0, 30, 30);
                    if (i == 2 && Silver == -5) rectangle = new(120, 0, 30, 30);
                    if (i == 2 && Silver == -6) rectangle = new(150, 0, 30, 30);
                    if (i == 3 && Gold == -7) rectangle = new(180, 0, 30, 30);
                    if (i == 3 && Gold == -8) rectangle = new(210, 0, 30, 30);
                    ValueTuple<Asset<Texture2D>, Rectangle?> valueTuple = new(asset, rectangle);
                    spriteBatch.Draw(ALTextureAssets.Button.Value, new Vector2(position.X + 96f, position.Y + 26f * (i + x)), color * 0.8f);
                    spriteBatch.Draw(valueTuple.Item1.Value, new Vector2(position.X + 99f, position.Y + 26f * (i + x) + 3f), valueTuple.Item2, color, 0f, new Vector2(1f, 1f), 0.5f, SpriteEffects.None, 0f);
                    Vector2 vector2 = new(position.X + 96f, position.Y + 26f * (i + x));
                    if (mouseRectangle.Intersects(Utils.CenteredRectangle(vector2 + new Vector2(7.5f, 7.5f), Utils.Size(new Rectangle(0, 0, 30, 30)))))
                    {
                        string line1 = "";
                        if (i == 0 && Copper == -1) line1 = Language.GetTextValue("Mods.AltLibrary.AltOreName.Copper");
                        if (i == 0 && Copper == -2) line1 = Language.GetTextValue("Mods.AltLibrary.AltOreName.Tin");
                        if (i == 0 && Copper >= 0) line1 = AltLibrary.Ores[Copper - 1].Name;
                        if (i == 1 && Iron == -3) line1 = Language.GetTextValue("Mods.AltLibrary.AltOreName.Iron");
                        if (i == 1 && Iron == -4) line1 = Language.GetTextValue("Mods.AltLibrary.AltOreName.Lead");
                        if (i == 1 && Iron >= 0) line1 = AltLibrary.Ores[Iron - 1].Name;
                        if (i == 2 && Silver == -5) line1 = Language.GetTextValue("Mods.AltLibrary.AltOreName.Silver");
                        if (i == 2 && Silver == -6) line1 = Language.GetTextValue("Mods.AltLibrary.AltOreName.Tungsten");
                        if (i == 2 && Silver >= 0) line1 = AltLibrary.Ores[Silver - 1].Name;
                        if (i == 3 && Gold == -7) line1 = Language.GetTextValue("Mods.AltLibrary.AltOreName.Gold");
                        if (i == 3 && Gold == -8) line1 = Language.GetTextValue("Mods.AltLibrary.AltOreName.Platinum");
                        if (i == 3 && Gold >= 0) line1 = AltLibrary.Ores[Gold - 1].Name;
                        string line2 = Language.GetTextValue("Mods.AltLibrary.AddedBy") + " ";
                        if (i == 0 && Copper < 0) line2 += "Terraria";
                        if (i == 0 && Copper >= 0) line2 += AltLibrary.Ores[Copper - 1].Mod.Name;
                        if (i == 1 && Iron < 0) line2 += "Terraria";
                        if (i == 1 && Iron >= 0) line2 += AltLibrary.Ores[Iron - 1].Mod.Name;
                        if (i == 2 && Silver < 0) line2 += "Terraria";
                        if (i == 2 && Silver >= 0) line2 += AltLibrary.Ores[Silver - 1].Mod.Name;
                        if (i == 3 && Gold < 0) line2 += "Terraria";
                        if (i == 3 && Gold >= 0) line2 += AltLibrary.Ores[Gold - 1].Mod.Name;
                        string line = line1 + '\n' + line2;
                        Main.instance.MouseText(line);
                    }
                }
                x += 4;
            }
            if (chosenOption == CurrentAltOption.HMOre || AltLibraryConfig.Config.OreIconsVisibleOutsideOreUI)
            {
                for (int i = 0; i < 3; i++)
                {
                    Asset<Texture2D> asset = ALTextureAssets.OreIcons;
                    if (i == 0 && Cobalt >= 0) asset = ModContent.Request<Texture2D>(AltLibrary.Ores[Cobalt - 1].Texture);
                    if (i == 1 && Mythril >= 0) asset = ModContent.Request<Texture2D>(AltLibrary.Ores[Mythril - 1].Texture);
                    if (i == 2 && Adamantite >= 0) asset = ModContent.Request<Texture2D>(AltLibrary.Ores[Adamantite - 1].Texture);
                    Rectangle? rectangle = null;
                    if (i == 0 && Cobalt == -9) rectangle = new(0, 30, 30, 30);
                    if (i == 0 && Cobalt == -10) rectangle = new(30, 30, 30, 30);
                    if (i == 1 && Mythril == -11) rectangle = new(60, 30, 30, 30);
                    if (i == 1 && Mythril == -12) rectangle = new(90, 30, 30, 30);
                    if (i == 2 && Adamantite == -13) rectangle = new(120, 30, 30, 30);
                    if (i == 2 && Adamantite == -14) rectangle = new(150, 30, 30, 30);
                    ValueTuple<Asset<Texture2D>, Rectangle?> valueTuple = new(asset, rectangle);
                    spriteBatch.Draw(ALTextureAssets.Button.Value, new Vector2(position.X + 96f, position.Y + 26f * (i + x)), color * 0.8f);
                    spriteBatch.Draw(valueTuple.Item1.Value, new Vector2(position.X + 99f, position.Y + 26f * (i + x) + 3f), valueTuple.Item2, color, 0f, new Vector2(1f, 1f), 0.5f, SpriteEffects.None, 0f);
                    Vector2 vector2 = new(position.X + 96f, position.Y + 26f * (i + x));
                    if (mouseRectangle.Intersects(Utils.CenteredRectangle(vector2 + new Vector2(7.5f, 7.5f), Utils.Size(new Rectangle(0, 0, 30, 30)))))
                    {
                        string line1 = "";
                        if (i == 0 && Cobalt == -9) line1 = Language.GetTextValue("Mods.AltLibrary.AltOreName.Cobalt");
                        if (i == 0 && Cobalt == -10) line1 = Language.GetTextValue("Mods.AltLibrary.AltOreName.Palladium");
                        if (i == 0 && Cobalt >= 0) line1 = AltLibrary.Ores[Cobalt - 1].Name;
                        if (i == 1 && Mythril == -11) line1 = Language.GetTextValue("Mods.AltLibrary.AltOreName.Mythril");
                        if (i == 1 && Mythril == -12) line1 = Language.GetTextValue("Mods.AltLibrary.AltOreName.Orichalcum");
                        if (i == 1 && Mythril >= 0) line1 = AltLibrary.Ores[Mythril - 1].Name;
                        if (i == 2 && Adamantite == -13) line1 = Language.GetTextValue("Mods.AltLibrary.AltOreName.Adamantite");
                        if (i == 2 && Adamantite == -14) line1 = Language.GetTextValue("Mods.AltLibrary.AltOreName.Titanium");
                        if (i == 2 && Adamantite >= 0) line1 = AltLibrary.Ores[Adamantite - 1].Name;
                        string line2 = Language.GetTextValue("Mods.AltLibrary.AddedBy") + " ";
                        if (i == 0 && Cobalt < 0) line2 += "Terraria";
                        if (i == 0 && Cobalt >= 0) line2 += AltLibrary.Ores[Cobalt - 1].Mod.Name;
                        if (i == 1 && Mythril < 0) line2 += "Terraria";
                        if (i == 1 && Mythril >= 0) line2 += AltLibrary.Ores[Mythril - 1].Mod.Name;
                        if (i == 2 && Adamantite < 0) line2 += "Terraria";
                        if (i == 2 && Adamantite >= 0) line2 += AltLibrary.Ores[Adamantite - 1].Mod.Name;
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
            CurrentAltOption[] array11 = new CurrentAltOption[3]
            {
                CurrentAltOption.Biome,
                CurrentAltOption.Ore,
                CurrentAltOption.HMOre
            };
            LocalizedText[] array10 = new LocalizedText[3]
            {
                Language.GetText("Mods.AltLibrary.ChooseBiome"),
                Language.GetText("Mods.AltLibrary.ChooseOre"),
                Language.GetText("Mods.AltLibrary.ChooseHardmodeOre")
            };
            Color[] array8 = new Color[3]
            {
                new Color(130, 183, 108),
                new Color(143, 183, 183),
                new Color(192, 183, 203)
            };
            string[] array7 = new string[3]
            {
                "Terraria/Images/UI/Bestiary/Icon_Tags_Shadow",
                "Terraria/Images/UI/Bestiary/Icon_Tags_Shadow",
                "Terraria/Images/UI/Bestiary/Icon_Tags_Shadow"
            };
            Rectangle[] array9 = new Rectangle[3]
            {
                new Rectangle(0, 0, 30, 30),
                new Rectangle(60, 0, 30, 30),
                new Rectangle(60, 30, 30, 30)
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
                    Width = StyleDimension.FromPixelsAndPercent(-4 * (array6.Length - 3), 1f / array6.Length * usableWidthPercent),
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
            while (!predicate.Invoke(c.Next))
            {
                c.Remove();
            }
        }
    }
}
