using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.AltOres;
using AltLibrary.Common.Systems;
using AltLibrary.Core;
using AltLibrary.Core.Baking;
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
		internal static List<AltOre> AddInFinishedCreation;
		internal static List<AltBiome> AddInFinishedCreation2;
		internal static ALGroupOptionButton<CurrentAltOption>[] chosingOption;
		internal static CurrentAltOption chosenOption;
		internal static int AltEvilBiomeChosenType;
		internal static int AltHallowBiomeChosenType;
		internal static int AltJungleBiomeChosenType;
		internal static int AltHellBiomeChosenType;
		internal static UIList _biomeList;
		internal static List<ALUIBiomeListItem> _biomeElements;
		internal static UIList _oreList;
		internal static List<ALUIOreListItem> _oreElements;
		internal static List<ALDrawingStruct<AltBiome>> QuenedDrawing2;
		internal static List<ALDrawingStruct<AltOre>> QuenedDrawing;
		internal static int Copper;
		internal static int Iron;
		internal static int Silver;
		internal static int Gold;
		internal static int Cobalt;
		internal static int Mythril;
		internal static int Adamantite;
		internal static bool isCrimson;
		internal static string seed;
		internal static bool initializedLists;
		internal enum CurrentAltOption
		{
			Biome,
			Ore
		}

		public static void Init()
		{
			if (Main.dedServ)
				return;

			_biomeElements = new();
			_oreElements = new();
			AddInFinishedCreation = new();
			AddInFinishedCreation2 = new();

			QuenedDrawing = new();
			QuenedDrawing2 = new();
			initializedLists = false;
			IL.Terraria.GameContent.UI.States.UIWorldCreation.MakeInfoMenu += ILMakeInfoMenu;
			On.Terraria.GameContent.UI.States.UIWorldCreation.AddWorldEvilOptions += OnAddWorldEvilOptions;
			On.Terraria.GameContent.UI.States.UIWorldCreation.SetDefaultOptions += UIWorldCreation_SetDefaultOptions;
			On.Terraria.GameContent.UI.States.UIWorldCreation.BuildPage += UIWorldCreation_BuildPage;
			IL.Terraria.GameContent.UI.States.UIWorldCreation.Draw += UIWorldCreation_Draw;
			IL.Terraria.GameContent.UI.States.UIWorldCreation.FinishCreatingWorld += UIWorldCreation_FinishCreatingWorld;
			IL.Terraria.GameContent.UI.Elements.UIWorldCreationPreview.DrawSelf += UIWorldCreationPreview_DrawSelf1;
			On.Terraria.GameContent.UI.Elements.UIWorldListItem.PlayGame += MakesWorldsUnplayable;
		}

		public static void Unload()
		{
			if (Main.netMode == NetmodeID.Server)
				return;

			IL.Terraria.GameContent.UI.States.UIWorldCreation.MakeInfoMenu -= ILMakeInfoMenu;
			On.Terraria.GameContent.UI.States.UIWorldCreation.AddWorldEvilOptions -= OnAddWorldEvilOptions;
			On.Terraria.GameContent.UI.States.UIWorldCreation.SetDefaultOptions -= UIWorldCreation_SetDefaultOptions;
			On.Terraria.GameContent.UI.States.UIWorldCreation.BuildPage -= UIWorldCreation_BuildPage;
			IL.Terraria.GameContent.UI.States.UIWorldCreation.Draw -= UIWorldCreation_Draw;
			IL.Terraria.GameContent.UI.States.UIWorldCreation.FinishCreatingWorld -= UIWorldCreation_FinishCreatingWorld;
			IL.Terraria.GameContent.UI.Elements.UIWorldCreationPreview.DrawSelf -= UIWorldCreationPreview_DrawSelf1;
			On.Terraria.GameContent.UI.Elements.UIWorldListItem.PlayGame -= MakesWorldsUnplayable;
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
			Copper = 0;
			Iron = 0;
			Silver = 0;
			Gold = 0;
			Cobalt = 0;
			Mythril = 0;
			Adamantite = 0;
			seed = null;
			initializedLists = false;
			QuenedDrawing = null;
			QuenedDrawing2 = null;
			AddInFinishedCreation = null;
			AddInFinishedCreation2 = null;
		}

		#region Useful stuff
		internal static void RandomizeValues()
		{
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

			foreach (AltBiome ore in AltLibrary.Biomes)
			{
				ore.OnInitialize();
			}
			foreach (AltOre ore in AltLibrary.Ores)
			{
				ore.OnInitialize();
			}
		}

		internal static List<AltOre> MakeOreList()
		{
			List<AltOre> prehmList = new();
			prehmList.Clear();
			prehmList.Add(new RandomOptionOre("RandomCopper"));
			prehmList.Add(new RandomOptionOre("RandomIron"));
			prehmList.Add(new RandomOptionOre("RandomSilver"));
			prehmList.Add(new RandomOptionOre("RandomGold"));
			prehmList.Add(new RandomOptionOre("RandomCobalt"));
			prehmList.Add(new RandomOptionOre("RandomMythril"));
			prehmList.Add(new RandomOptionOre("RandomAdamantite"));
			prehmList.AddRange(ALWorldCreationLists.prehmOreData.Types);
			return prehmList;
		}

		internal static List<ALDrawingStruct<AltOre>> MakeQuenedDrawingList()
		{
			QuenedDrawing.Clear();
			QuenedDrawing.AddRange(ALWorldCreationLists.prehmOreData.Quenes);
			return QuenedDrawing;
		}

		internal static List<ALDrawingStruct<AltBiome>> MakeQuenedDrawingList2()
		{
			QuenedDrawing2.Clear();
			QuenedDrawing2.AddRange(ALWorldCreationLists.biomeData.Quenes);
			return QuenedDrawing2;
		}

		internal static List<AltBiome> MakeBiomeList()
		{
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

			List<AltBiome> b = new();
			b.Clear();
			b.Add(new VanillaBiome("CorruptBiome", BiomeType.Evil, -333, Color.MediumPurple, false));
			b.Add(new VanillaBiome("CrimsonBiome", BiomeType.Evil, -666, Color.IndianRed, true));
			b.AddRange(AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Evil && x.Selectable));
			bool bl = AltLibraryConfig.Config.VanillaShowUpIfOnlyAltVarExist && AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Hallow && x.Selectable).Any();
			if (!AltLibraryConfig.Config.VanillaShowUpIfOnlyAltVarExist)
			{
				bl = true;
			}
			if (bl)
			{
				b.Add(new VanillaBiome("HallowBiome", BiomeType.Hallow, -3, Color.HotPink));
			}
			b.AddRange(AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Hallow && x.Selectable));
			bl = AltLibraryConfig.Config.VanillaShowUpIfOnlyAltVarExist && AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Jungle && x.Selectable).Any();
			if (!AltLibraryConfig.Config.VanillaShowUpIfOnlyAltVarExist)
			{
				bl = true;
			}
			if (bl)
			{
				b.Add(new VanillaBiome("JungleBiome", BiomeType.Jungle, -4, Color.SpringGreen));
			}
			b.AddRange(AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Jungle && x.Selectable));
			bl = AltLibraryConfig.Config.VanillaShowUpIfOnlyAltVarExist && AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Hell && x.Selectable).Any();
			if (!AltLibraryConfig.Config.VanillaShowUpIfOnlyAltVarExist)
			{
				bl = true;
			}
			if (bl)
			{
				b.Add(new VanillaBiome("UnderworldBiome", BiomeType.Hell, -5, Color.OrangeRed));
			}
			b.AddRange(AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Hell && x.Selectable));

			foreach (AltBiome ore in AltLibrary.Biomes)
			{
				if (ore.BiomeType == BiomeType.None)
				{
					ore.CustomSelection(b);
				}
			}

			list.AddRange(b);

			return list;
		}

		public static void UIWorldCreation_BuildPage(On.Terraria.GameContent.UI.States.UIWorldCreation.orig_BuildPage orig, UIWorldCreation self)
		{
			if (!initializedLists)
			{
				ALWorldCreationLists.FillData();
				initializedLists = true;
			}
			chosenOption = (CurrentAltOption)(-1);
			MakeQuenedDrawingList();
			MakeQuenedDrawingList2();

			RandomizeValues();

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

				List<AltOre> prehmList = MakeOreList();
				List<ALUIOreListItem> items = new();
				prehmList.ForEach(x => items.Add(new(x, false)));
				_oreList._items.AddRange(items);
				foreach (UIElement item in items)
				{
					((UIElement)ALReflection.UIList__innerList.GetValue(_oreList)).Append(item);
				}
				((UIElement)ALReflection.UIList__innerList.GetValue(_oreList)).Recalculate();
				_oreElements.AddRange(items);
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

				List<AltBiome> list = MakeBiomeList();
				List<ALUIBiomeListItem> items = new();
				list.ForEach(x => items.Add(new(x, false)));
				_biomeList._items.AddRange(items);
				foreach (UIElement item in items)
				{
					((UIElement)ALReflection.UIList__innerList.GetValue(_biomeList)).Append(item);
				}
				((UIElement)ALReflection.UIList__innerList.GetValue(_biomeList)).Recalculate();
				_biomeElements.AddRange(items);
			}
			#endregion
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
				foreach (AltBiome o in AddInFinishedCreation2)
					o.OnCreating();
				foreach (AltOre o in AddInFinishedCreation)
					o.OnCreating();

				AltLibrary.Instance.Logger.Info($"On creating world - Hallow: {AltHallowBiomeChosenType} Corrupt: {AltEvilBiomeChosenType} Jungle: {AltJungleBiomeChosenType} Underworld: {AltHellBiomeChosenType}");
			});
		}

		internal static void WorldCreationUIIcons(UIWorldCreationPreview self, SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = self.GetDimensions();
			Vector2 position = new(dimensions.X + 4f, dimensions.Y + 4f);
			Color color = Color.White;
			Rectangle mouseRectangle = Utils.CenteredRectangle(Main.MouseScreen, Vector2.One * 2f);
			int x = 0;

			static void DrawBiomeIcon(SpriteBatch spriteBatch, Vector2 position, Rectangle mouseRectangle, ref int x, Color color, Func<bool> cond, Func<Asset<Texture2D>, Asset<Texture2D>> func, Func<Rectangle?> rect, Func<string> onHoverName, Func<string, string> onHoverMod)
			{
				if (cond())
				{
					Asset<Texture2D> asset = func(ALTextureAssets.BestiaryIcons);
					Rectangle? rectangle = null;
					if (rect() != null) rectangle = rect();
					ValueTuple<Asset<Texture2D>, Rectangle?> valueTuple = new(asset, rectangle);
					spriteBatch.Draw(ALTextureAssets.Button.Value, new Vector2(position.X + 96f, position.Y + 26f * x), color * 0.8f);
					spriteBatch.Draw(valueTuple.Item1.Value, new Vector2(position.X + 99f, position.Y + 26f * x + 3f), valueTuple.Item2, color, 0f, new Vector2(0f, 0f), 0.5f, SpriteEffects.None, 0f);
					Vector2 vector2 = new(position.X + 96f, position.Y + 26f * x);
					if (mouseRectangle.Intersects(Utils.CenteredRectangle(vector2 + new Vector2(7.5f, 7.5f), Utils.Size(new Rectangle(0, 0, 30, 30)))))
					{
						string line1 = onHoverName();
						string line2 = $"{Language.GetTextValue("Mods.AltLibrary.AddedBy")} {onHoverMod("Terraria")}";
						string line = $"{line1}\n{line2}";
						Main.instance.MouseText(line);
					}
					x++;
				}
			}

			static void DrawOreIcon(SpriteBatch spriteBatch, Vector2 position, ref int x, Rectangle mouseRectangle, Color color, Func<bool> cond, Func<Asset<Texture2D>, Asset<Texture2D>> func, Func<Rectangle?> rect, Func<string> onHoverName, Func<string, string> onHoverMod)
			{
				if (cond())
				{
					Asset<Texture2D> asset = func(ALTextureAssets.OreIcons);
					Rectangle? rectangle = null;
					if (rect() != null) rectangle = rect();
					ValueTuple<Asset<Texture2D>, Rectangle?> valueTuple = new(asset, rectangle);
					spriteBatch.Draw(ALTextureAssets.Button.Value, new Vector2(position.X + 96f, position.Y + 26f * x), color * 0.8f);
					spriteBatch.Draw(valueTuple.Item1.Value, new Vector2(position.X + 99f, position.Y + 26f * x + 3f), valueTuple.Item2, color, 0f, new Vector2(1f, 1f), 0.5f, SpriteEffects.None, 0f);
					Vector2 vector2 = new(position.X + 96f, position.Y + 26f * x);
					if (mouseRectangle.Intersects(Utils.CenteredRectangle(vector2 + new Vector2(7.5f, 7.5f), Utils.Size(new Rectangle(0, 0, 30, 30)))))
					{
						string line1 = onHoverName();
						string line2 = $"{Language.GetTextValue("Mods.AltLibrary.AddedBy")} {onHoverMod("Terraria")}";
						string line = $"{line1}\n{line2}";
						Main.instance.MouseText(line);
					}
					x++;
				}
			}

			foreach (ALDrawingStruct<AltBiome> ore in QuenedDrawing2)
			{
				DrawBiomeIcon(spriteBatch, position, mouseRectangle, ref x, color, ore.cond, ore.func, ore.rect, ore.onHoverName, ore.onHoverMod);
			}

			foreach (ALDrawingStruct<AltOre> ore in QuenedDrawing)
			{
				DrawOreIcon(spriteBatch, position, ref x, mouseRectangle, color, ore.cond, ore.func, ore.rect, ore.onHoverName, ore.onHoverMod);
			}
		}
		#endregion

		#region Other stuff
		public static void ILMakeInfoMenu(ILContext il)
		{
			var c = new ILCursor(il);

			c.GotoNext(i => i.MatchLdstr("evil"))
				.GotoNext(i => i.MatchLdloc(1), i => i.MatchLdcR4(48f));

			ILLabel label = c.DefineLabel();
			c.Emit(OpCodes.Br, label)
				.GotoNext(i => i.MatchLdarg(0), i => i.MatchLdloc(0), i => i.MatchLdloc(1), i => i.MatchLdstr("desc"));

			c.MarkLabel(label);
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
					"constant" or "theconstant" or "the constant" or "eye4aneye" or "eye4aneye" => "Constant",
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

		private static void MakesWorldsUnplayable(On.Terraria.GameContent.UI.Elements.UIWorldListItem.orig_PlayGame orig, UIWorldListItem self, UIMouseEvent evt, UIElement listeningElement)
		{
			if ((WorldFileData)typeof(UIWorldListItem).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self) == null)
				return;
			if (ALUtils.IsWorldValid(self))
			{
				orig(self, evt, listeningElement);
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
				CurrentAltOption.Ore,
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
					Width = StyleDimension.FromPixelsAndPercent(4 * (array6.Length - 3), 1f / array6.Length * usableWidthPercent),
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
		#endregion

		#region useless shit that need to be refactored somehow
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
		#endregion
	}

	public readonly struct ALDrawingStruct<T> where T : ModType
	{
		public readonly string UniqueID;
		internal readonly Func<bool> cond;
		internal readonly Func<Asset<Texture2D>, Asset<Texture2D>> func;
		internal readonly Func<Rectangle?> rect;
		internal readonly Func<string> onHoverName;
		internal readonly Func<string, string> onHoverMod;

		private readonly Func<bool> WhichOne
		{
			get
			{
				if (ContentInstance<T>.Instance is AltOre)
					return () => AltLibraryConfig.Config.OreIconsVisibleOutsideOreUI;
				return () => AltLibraryConfig.Config.BiomeIconsVisibleOutsideBiomeUI;
			}
		}

		public ALDrawingStruct(string ID, Func<Asset<Texture2D>, Asset<Texture2D>> func, Func<Rectangle?> rect, Func<string> onHoverName, Func<string, string> onHoverMod, Func<bool> cond = null)
		{
			UniqueID = ID;
			this.cond = cond;
			this.func = func;
			this.rect = rect;
			this.onHoverName = onHoverName;
			this.onHoverMod = onHoverMod;

			this.cond ??= WhichOne;
		}

		public ALDrawingStruct(ModType type, Func<Asset<Texture2D>, Asset<Texture2D>> func, Func<Rectangle?> rect, Func<string> onHoverName, Func<string, string> onHoverMod, Func<bool> cond = null)
		{
			UniqueID = type.FullName;
			this.cond = cond;
			this.func = func;
			this.rect = rect;
			this.onHoverName = onHoverName;
			this.onHoverMod = onHoverMod;

			this.cond ??= WhichOne;
		}
	}
}
