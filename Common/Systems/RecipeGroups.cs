using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AltLibrary.Common.Systems
{
    internal class RecipeGroups : ModSystem
    {
        public static RecipeGroup group;
        public override void AddRecipeGroups()
        {
            int[] array = new int[] { ItemID.DemoniteOre, ItemID.CrimtaneOre };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.EvilOres")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:EvilOres", group);

            array = new int[] { ItemID.DemoniteBar, ItemID.CrimtaneBar };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.EvilBars")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:EvilBars", group);

            array = new int[] { ItemID.HallowedBar };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.HallowBars")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:HallowBars", group);

            array = new int[] { ItemID.HellstoneBar };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.HellBars")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:HellBars", group);

            array = new int[] { ItemID.ChlorophyteBar };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.JungleBars")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:JungleBars", group);

            array = new int[] { ItemID.ShroomiteBar };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.MushroomBars")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:MushroomBars", group);

            array = new int[] { ItemID.LightsBane, ItemID.BloodButcherer };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.EvilSwords")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:EvilSwords", group);

            array = new int[] { ItemID.Excalibur };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.HallowSwords")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:HallowSwords", group);

            array = new int[] { ItemID.FieryGreatsword };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.HellSwords")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:HellSwords", group);

            array = new int[] { ItemID.BladeofGrass };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.JungleSwords")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:JungleSwords", group);

            array = new int[] { ItemID.NightsEdge };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.ComboSwords")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:ComboSwords", group);

            array = new int[] { ItemID.TrueNightsEdge };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.TrueComboSwords")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:TrueComboSwords", group);

            array = new int[] { ItemID.TrueExcalibur };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.TrueHallowSwords")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:TrueHallowSwords", group);

            array = new int[] { ItemID.RottenChunk, ItemID.Vertebrae };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.RottenChunks")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:RottenChunks", group);

            array = new int[] { ItemID.PixieDust };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.PixieDusts")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:PixieDusts", group);

            array = new int[] { ItemID.UnicornHorn };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.UnicornHorns")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:UnicornHorns", group);

            array = new int[] { ItemID.CrystalShard };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.CrystalShards")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:CrystalShards", group);

            array = new int[] { ItemID.CursedFlame, ItemID.Ichor };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.CursedFlames")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:CursedFlames", group);

            array = new int[] { ItemID.ShadowScale, ItemID.TissueSample };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.ShadowScales")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:ShadowScales", group);

            array = new int[] { ItemID.JungleSpores };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.JungleSpores")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:JungleSpores", group);

            array = new int[] { ItemID.Deathweed };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.Deathweed")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:Deathweed", group);

            array = new int[] { ItemID.Fireblossom };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.Fireblossom")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:Fireblossom", group);

            array = new int[] { ItemID.Moonglow };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.Moonglow")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:Moonglow", group);

            array = new int[] { ItemID.Hellforge };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.Hellforges")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:Hellforges", group);

            array = new int[] { ItemID.CopperBar, ItemID.TinBar };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.CopperBars")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:CopperBars", group);

            array = new int[] { ItemID.SilverBar, ItemID.TungstenBar };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.SilverBars")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:SilverBars", group);

            array = new int[] { ItemID.GoldBar, ItemID.PlatinumBar };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.GoldBars")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:GoldBars", group);

            array = new int[] { ItemID.GoldenCandle, ItemID.PlatinumCandle };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.GoldCandles")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:GoldCandles", group);

            array = new int[] { ItemID.CobaltBar, ItemID.PalladiumBar };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.CobaltBars")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:CobaltBars", group);

            array = new int[] { ItemID.MythrilBar, ItemID.OrichalcumBar };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.MythrilBars")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:MythrilBars", group);

            array = new int[] { ItemID.AdamantiteBar, ItemID.TitaniumBar };
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.AdamantiteBars")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:AdamantiteBars", group);
        }
    }
}
