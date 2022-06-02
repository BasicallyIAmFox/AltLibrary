using AltLibrary.Common;
using AltLibrary.Common.AltLiquidStyles.Hooks;
using AltLibrary.Common.Hooks;
using AltLibrary.Content.NPCs;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AltLibrary.Core
{
    internal class ILHooks
    {
        public static void OnInitialize()
        {
            On.Terraria.Main.EraseWorld += Main_EraseWorld;
            On.Terraria.Main.GUIChatDrawInner += Main_GUIChatDrawInner;
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
            MimicSummon.Init();
            BloodMoonCritterTransformations.Init();
            DryadText.Init();
            JungleHuts.Init(); // TODO: redo?
            NearbyAltChloro.Init();
            EvilWofBox.Init();
            TenthAnniversaryFix.Init();
            ShadowKeyReplacement.Init();
            LiquidILHooks.Init();
            DrunkCrimsonFix.Load();
        }

        public static void Unload()
        {
            On.Terraria.Main.EraseWorld -= Main_EraseWorld;
            On.Terraria.Main.GUIChatDrawInner -= Main_GUIChatDrawInner;
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
            MimicSummon.Unload();
            BloodMoonCritterTransformations.Unload();
            DryadText.Unload();
            JungleHuts.Unload();
            NearbyAltChloro.Unload();
            EvilWofBox.Unload();
            TenthAnniversaryFix.Unload();
            ShadowKeyReplacement.Unload();
            LiquidILHooks.Unload();
            GenPasses.Unload();
            DrunkCrimsonFix.Unload();
        }

        private static void Main_GUIChatDrawInner(On.Terraria.Main.orig_GUIChatDrawInner orig, Main self)
        {
            orig(self);
            if (Main.npcChatText.StartsWith(Language.GetTextValue("Mods.AltLibrary.AnalysisDone")) && Main.LocalPlayer.GetModPlayer<ALPlayer>().IsAnalysingClick)
            {
                AltLibrary.userInterface.Update(Main._drawInterfaceGameTime);
                AltLibrary.userInterface.Draw(Main.spriteBatch, Main._drawInterfaceGameTime);
            }
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
    }
}
