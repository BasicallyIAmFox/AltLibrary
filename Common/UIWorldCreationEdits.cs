using AltLibrary.Common.AltBiomes;
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
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

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
        }

        public static void UIWorldCreation_BuildPage(On.Terraria.GameContent.UI.States.UIWorldCreation.orig_BuildPage orig, UIWorldCreation self)
        {
            chosenOption = (CurrentAltOption)(-1);
            List<int> evilBiomeTypes = new() { -1, -2 };
            AltLibrary.biomes.Where(x => x.BiomeType == BiomeType.Evil).ToList().ForEach(x => evilBiomeTypes.Add(x.Type - 1));
            AltEvilBiomeChosenType = evilBiomeTypes[Main.rand.Next(evilBiomeTypes.Count)];
            List<int> hallowBiomeTypes = new() { -3 };
            AltLibrary.biomes.Where(x => x.BiomeType == BiomeType.Hallow).ToList().ForEach(x => hallowBiomeTypes.Add(x.Type - 1));
            AltHallowBiomeChosenType = hallowBiomeTypes[Main.rand.Next(hallowBiomeTypes.Count)];
            List<int> hellBiomeTypes = new() { -5 };
            AltLibrary.biomes.Where(x => x.BiomeType == BiomeType.Hell).ToList().ForEach(x => hellBiomeTypes.Add(x.Type - 1));
            AltHellBiomeChosenType = hellBiomeTypes[Main.rand.Next(hellBiomeTypes.Count)];
            List<int> jungleBiomeTypes = new() { -4 };
            AltLibrary.biomes.Where(x => x.BiomeType == BiomeType.Jungle).ToList().ForEach(x => jungleBiomeTypes.Add(x.Type - 1));
            AltJungleBiomeChosenType = jungleBiomeTypes[Main.rand.Next(jungleBiomeTypes.Count)];
            Copper = Main.rand.Next(2);
            Iron = Main.rand.Next(2);
            Silver = Main.rand.Next(2);
            Gold = Main.rand.Next(2);
            Cobalt = Main.rand.Next(2);
            Mythril = Main.rand.Next(2);
            Adamantite = Main.rand.Next(2);
            _oreElements.Clear();
            _oreList = null;
            orig(self);

            #region Ore UI List
            {
                UIElement uIElement3 = new();
                uIElement3.Left = StyleDimension.FromPixels(100f);
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
                uIScrollbar.Left = StyleDimension.FromPixels(75f);
                uIScrollbar.Height.Set(-250f, 1f);
                uIScrollbar.Top.Set(150f, 0f);
                uIScrollbar.HAlign = 1f;
                uIScrollbar.OnUpdate += _UIScrollbar_OnUpdate;
                self.Append(uIScrollbar);
                _oreList.SetScrollbar(uIScrollbar);

                List<int> list = new();
                list.Clear();
                list.Add(0);
                list.Add(1);
                list.Add(2);
                list.Add(3);
                list.Add(4);
                list.Add(5);
                list.Add(6);
                list.Add(7);
                list.Add(8);
                list.Add(9);
                list.Add(10);
                list.Add(11);
                list.Add(12);
                list.Add(13);
                list.Add(14);
                list.Add(15);
                list.Add(16);
                list.Add(17);
                list.Add(18);
                list.Add(19);
                list.Add(20);
                list.Sort((n, t) => n > t ? 1 : -1);
                for (int i = 0; i < list.Count; i++)
                {
                    int biome = list[i];
                    ALUIOreListItem item = new(biome, true);
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
                uIElement3.Left = StyleDimension.FromPixels(-1100f);
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
                uIScrollbar.Left = StyleDimension.FromPixels(-1075f);
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
                list.Add(new VanillaBiome("CorruptBiome", BiomeType.Evil, -1, Color.MediumPurple, Language.GetText("Mods.AltLibrary.Biomes.CorruptName"), Language.GetText("Mods.AltLibrary.Biomes.CorruptDesc")));
                list.Add(new VanillaBiome("CrimsonBiome", BiomeType.Evil, -2, Color.IndianRed, Language.GetText("Mods.AltLibrary.Biomes.CrimsonName"), Language.GetText("Mods.AltLibrary.Biomes.CrimsonDesc")));
                foreach (AltBiome biome in AltLibrary.biomes.Where(x => x.BiomeType == BiomeType.Evil))
                {
                    list.Add(biome);
                }
                list.Add(new VanillaBiome("HallowBiome", BiomeType.Hallow, -3, Color.HotPink, Language.GetText("Mods.AltLibrary.Biomes.HallowName"), Language.GetText("Mods.AltLibrary.Biomes.HallowDesc")));
                foreach (AltBiome biome in AltLibrary.biomes.Where(x => x.BiomeType == BiomeType.Hallow))
                {
                    list.Add(biome);
                }
                list.Add(new VanillaBiome("JungleBiome", BiomeType.Jungle, -4, Color.SpringGreen, Language.GetText("Mods.AltLibrary.Biomes.JungleName"), Language.GetText("Mods.AltLibrary.Biomes.JungleDesc")));
                foreach (AltBiome biome in AltLibrary.biomes.Where(x => x.BiomeType == BiomeType.Jungle))
                {
                    list.Add(biome);
                }
                list.Add(new VanillaBiome("UnderworldBiome", BiomeType.Hell, -5, Color.OrangeRed, Language.GetText("Mods.AltLibrary.Biomes.UnderworldName"), Language.GetText("Mods.AltLibrary.Biomes.UnderworldDesc")));
                foreach (AltBiome biome in AltLibrary.biomes.Where(x => x.BiomeType == BiomeType.Hell))
                {
                    list.Add(biome);
                }
                foreach (AltBiome biome in list)
                {
                    ALUIBiomeListItem item = new(biome, true);
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
                scrollbar.Left = StyleDimension.FromPixels(-100000f);
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
                element.Left = StyleDimension.FromPixels(-1100f);
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
                    AltLibrary.Instance.Logger.Info(AltEvilBiomeChosenType.ToString());
                    WorldGen.crimson = AltEvilBiomeChosenType == -2;
                }
                else
                {
                    WorldBiomeManager.worldEvil = AltLibrary.biomes[AltEvilBiomeChosenType].FullName;
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
            if (chosenOption == CurrentAltOption.Biome || AltLibraryConfig.Config.BiomeIconsVisibleOutsideBiomeUI)
            {
                for (int i = 0; i < 4; i++)
                {
                    Asset<Texture2D> asset = ModContent.Request<Texture2D>("Terraria/Images/UI/Bestiary/Icon_Tags_Shadow");
                    if (i == 0 && AltHallowBiomeChosenType >= 0) asset = ModContent.Request<Texture2D>(AltLibrary.biomes[AltHallowBiomeChosenType].IconSmall ?? "AltLibrary/Assets/WorldIcons/ButtonHallow");
                    if (i == 1 && AltEvilBiomeChosenType > -1) asset = ModContent.Request<Texture2D>(AltLibrary.biomes[AltEvilBiomeChosenType].IconSmall ?? "AltLibrary/Assets/WorldIcons/ButtonCorrupt");
                    if (i == 2 && AltHellBiomeChosenType >= 0) asset = ModContent.Request<Texture2D>(AltLibrary.biomes[AltHellBiomeChosenType].IconSmall ?? "AltLibrary/Assets/WorldIcons/ButtonHell");
                    if (i == 3 && AltJungleBiomeChosenType >= 0) asset = ModContent.Request<Texture2D>(AltLibrary.biomes[AltJungleBiomeChosenType].IconSmall ?? "AltLibrary/Assets/WorldIcons/ButtonJungle");
                    Rectangle? rectangle = null;
                    if (i == 0 && AltHallowBiomeChosenType < 0) rectangle = new(30, 30, 30, 30);
                    if (i == 1 && AltEvilBiomeChosenType == -1) rectangle = new(210, 0, 30, 30);
                    if (i == 1 && AltEvilBiomeChosenType <= -2) rectangle = new(360, 0, 30, 30);
                    if (i == 2 && AltHellBiomeChosenType < 0) rectangle = new(30, 60, 30, 30);
                    if (i == 3 && AltJungleBiomeChosenType < 0) rectangle = new(180, 30, 30, 30);
                    ValueTuple<Asset<Texture2D>, Rectangle?> valueTuple = new(asset, rectangle);
                    spriteBatch.Draw(ModContent.Request<Texture2D>("AltLibrary/Assets/WorldIcons/Button").Value, new Vector2(position.X + 96f, position.Y + 26f * i), color * 0.8f);
                    spriteBatch.Draw(valueTuple.Item1.Value, new Vector2(position.X + 99f, position.Y + 26f * i + 3f), valueTuple.Item2, color, 0f, new Vector2(0f, 0f), 0.5f, SpriteEffects.None, 0f);
                }
                x = 4;
            }
            if (chosenOption == CurrentAltOption.Ore || AltLibraryConfig.Config.OreIconsVisibleOutsideOreUI)
            {
                for (int i = 0; i < 7; i++)
                {
                    Asset<Texture2D> asset = ModContent.Request<Texture2D>("AltLibrary/Assets/WorldIcons/OreIcons");
                    Rectangle? rectangle = null;
                    if (i == 0) rectangle = new(Copper * 30, 0, 30, 30);
                    if (i == 1) rectangle = new((Iron + 2) * 30, 0, 30, 30);
                    if (i == 2) rectangle = new((Silver + 4) * 30, 0, 30, 30);
                    if (i == 3) rectangle = new((Gold + 6) * 30, 0, 30, 30);
                    if (i == 4) rectangle = new(Cobalt * 30, 30, 30, 30);
                    if (i == 5) rectangle = new((Mythril + 2) * 30, 30, 30, 30);
                    if (i == 6) rectangle = new((Adamantite + 4) * 30, 30, 30, 30);
                    ValueTuple<Asset<Texture2D>, Rectangle?> valueTuple = new(asset, rectangle);
                    spriteBatch.Draw(ModContent.Request<Texture2D>("AltLibrary/Assets/WorldIcons/Button").Value, new Vector2(position.X + 96f, position.Y + 26f * (i + x)), color * 0.8f);
                    spriteBatch.Draw(valueTuple.Item1.Value, new Vector2(position.X + 99f, position.Y + 26f * (i + x) + 3f), valueTuple.Item2, color, 0f, new Vector2(0f, 0f), 0.5f, SpriteEffects.None, 0f);
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
