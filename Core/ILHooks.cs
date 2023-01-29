using AltLibrary.Common;
using AltLibrary.Common.Hooks;
using AltLibrary.Common.Systems;
using AltLibrary.Content.NPCs;
using AltLibrary.Core.Baking;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AltLibrary.Core {
	internal class ILHooks {
		public static void OnInitialize() {
			EditsHelper.On<Main>(nameof(Main.EraseWorld), Main_EraseWorld);
			EditsHelper.On<Main>(nameof(Main.GUIChatDrawInner), Main_GUIChatDrawInner);

			WorldIcons.Init();
			OuterVisual.Init();
			EvenMoreWorldGen.Init();
			UnderworldVisual.Init();
			UIWorldCreationEdits.Init();
			HardmodeWorldGen.Init();
			TwinsRules.Init();
			DungeonChests.Init();
			SmashAltarInfection.Init();
			MowingGrassTile.Init();
			AltOreInsideBodies.Load();
			MimicSummon.Init();
			SimpleReplacements.Load();
			DryadText.Init();
			JungleHuts.Init(); // TODO: redo?
			TenthAnniversaryFix.Init();
			DrunkCrimsonFix.Load();
			BackgroundsAlternating.Inject();
		}

		public static void Unload()
		{
			WorldIcons.Unload();
			OuterVisual.Unload();
			EvenMoreWorldGen.Unload();
			UnderworldVisual.Unload();
			UIWorldCreationEdits.Unload();
			HardmodeWorldGen.Unload();
			TwinsRules.Unload();
			DungeonChests.Unload();
			SmashAltarInfection.Unload();
			MowingGrassTile.Unload();
			AltOreInsideBodies.Unload();
			MimicSummon.Unload();
			SimpleReplacements.Unload();
			DryadText.Unload();
			JungleHuts.Unload();
			TenthAnniversaryFix.Unload();
			GenPasses.Unload();
			DrunkCrimsonFix.Unload();
			BackgroundsAlternating.Uninit();
		}

		internal static void Main_GUIChatDrawInner(On_Main.orig_GUIChatDrawInner orig, Main self)
		{
			orig(self);
			if (Main.LocalPlayer.talkNPC != -1 && Main.npc[Main.LocalPlayer.talkNPC].type == ModContent.NPCType<PieChartTownNPC>())
			{
				if (AnalystShopLoader.MaxShopCount() >= 1)
				{
					Main.npcChatCornerItem = ItemID.DirtBlock;
				}
				if (Main.npcChatText == Language.GetTextValue("Mods.AltLibrary.AnalysisDone", Main.LocalPlayer.name, Main.worldName) + WorldBiomeManager.AnalysisDoneSpaces && Main.LocalPlayer.GetModPlayer<ALPlayer>().IsAnalysingClick)
				{
					AltLibrary.userInterface.Update(Main._drawInterfaceGameTime);
					AltLibrary.userInterface.Draw(Main.spriteBatch, Main._drawInterfaceGameTime);
				}
			}
		}

		internal static void Main_EraseWorld(On_Main.orig_EraseWorld orig, int i)
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
	}
}
