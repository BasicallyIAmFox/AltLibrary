using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AltLibrary.Common.Systems
{
    internal class RecipeGroups : ModSystem
    {
        internal static RecipeGroup EvilOres;
        internal static RecipeGroup EvilBars;
        internal static RecipeGroup HallowBars;
        internal static RecipeGroup HellBars;
        internal static RecipeGroup JungleBars;
        internal static RecipeGroup MushroomBars;
        internal static RecipeGroup EvilSwords;
        internal static RecipeGroup HallowSwords;
        internal static RecipeGroup HellSwords;
        internal static RecipeGroup JungleSwords;
        internal static RecipeGroup ComboSwords;
        internal static RecipeGroup TrueComboSwords;
        internal static RecipeGroup TrueHallowSwords;
        internal static RecipeGroup RottenChunks;
        internal static RecipeGroup PixieDusts;
        internal static RecipeGroup UnicornHorns;
        internal static RecipeGroup CrystalShards;
        internal static RecipeGroup CursedFlames;
        internal static RecipeGroup ShadowScales;
        internal static RecipeGroup JungleSpores;
        internal static RecipeGroup Deathweed;
        internal static RecipeGroup Fireblossom;
        internal static RecipeGroup Moonglow;
        internal static RecipeGroup Hellforges;
        internal static RecipeGroup CopperBars;
        internal static RecipeGroup IronBars;
        internal static RecipeGroup SilverBars;
        internal static RecipeGroup GoldBars;
        internal static RecipeGroup CobaltBars;
        internal static RecipeGroup MythrilBars;
        internal static RecipeGroup AdamantiteBars;
        internal static RecipeGroup GoldCandles;
        internal static RecipeGroup CopperWatches;
        internal static RecipeGroup SilverWatches;
        internal static RecipeGroup GoldWatches;

        public override void Unload()
        {
            EvilOres = null;
            EvilBars = null;
            HallowBars = null;
            HellBars = null;
            JungleBars = null;
            MushroomBars = null;
            EvilSwords = null;
            HallowSwords = null;
            HellSwords = null;
            JungleSwords = null;
            ComboSwords = null;
            TrueComboSwords = null;
            TrueHallowSwords = null;
            RottenChunks = null;
            PixieDusts = null;
            UnicornHorns = null;
            CrystalShards = null;
            CursedFlames = null;
            ShadowScales = null;
            JungleSpores = null;
            Deathweed = null;
            Fireblossom = null;
            Moonglow = null;
            Hellforges = null;
            CopperBars = null;
            IronBars = null;
            SilverBars = null;
            GoldBars = null;
            CobaltBars = null;
            MythrilBars = null;
            AdamantiteBars = null;
            GoldCandles = null;
            CopperWatches = null;
            SilverWatches = null;
            GoldWatches = null;
        }

        public override void AddRecipeGroups()
        {
            int[] array = new int[] { ItemID.DemoniteOre, ItemID.CrimtaneOre };
            EvilOres = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.EvilOres")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:EvilOres", EvilOres);

            array = new int[] { ItemID.DemoniteBar, ItemID.CrimtaneBar };
            EvilBars = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.EvilBars")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:EvilBars", EvilBars);

            array = new int[] { ItemID.HallowedBar };
            HallowBars = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.HallowBars")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:HallowBars", HallowBars);

            array = new int[] { ItemID.HellstoneBar };
            HellBars = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.HellBars")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:HellBars", HellBars);

            array = new int[] { ItemID.ChlorophyteBar };
            JungleBars = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.JungleBars")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:JungleBars", JungleBars);

            array = new int[] { ItemID.ShroomiteBar };
            MushroomBars = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.MushroomBars")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:MushroomBars", MushroomBars);

            array = new int[] { ItemID.LightsBane, ItemID.BloodButcherer };
            EvilSwords = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.EvilSwords")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:EvilSwords", EvilSwords);

            array = new int[] { ItemID.Excalibur };
            HallowSwords = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.HallowSwords")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:HallowSwords", HallowSwords);

            array = new int[] { ItemID.FieryGreatsword };
            HellSwords = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.HellSwords")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:HellSwords", HellSwords);

            array = new int[] { ItemID.BladeofGrass };
            JungleSwords = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.JungleSwords")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:JungleSwords", JungleSwords);

            array = new int[] { ItemID.NightsEdge };
            ComboSwords = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.ComboSwords")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:ComboSwords", ComboSwords);

            array = new int[] { ItemID.TrueNightsEdge };
            TrueComboSwords = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.TrueComboSwords")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:TrueComboSwords", TrueComboSwords);

            array = new int[] { ItemID.TrueExcalibur };
            TrueHallowSwords = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.TrueHallowSwords")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:TrueHallowSwords", TrueHallowSwords);

            array = new int[] { ItemID.RottenChunk, ItemID.Vertebrae };
            RottenChunks = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.RottenChunks")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:RottenChunks", RottenChunks);

            array = new int[] { ItemID.PixieDust };
            PixieDusts = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.PixieDusts")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:PixieDusts", PixieDusts);

            array = new int[] { ItemID.UnicornHorn };
            UnicornHorns = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.UnicornHorns")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:UnicornHorns", UnicornHorns);

            array = new int[] { ItemID.CrystalShard };
            CrystalShards = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.CrystalShards")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:CrystalShards", CrystalShards);

            array = new int[] { ItemID.CursedFlame, ItemID.Ichor };
            CursedFlames = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.CursedFlames")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:CursedFlames", CursedFlames);

            array = new int[] { ItemID.ShadowScale, ItemID.TissueSample };
            ShadowScales = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.ShadowScales")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:ShadowScales", ShadowScales);

            array = new int[] { ItemID.JungleSpores };
            JungleSpores = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.JungleSpores")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:JungleSpores", JungleSpores);

            array = new int[] { ItemID.Deathweed };
            Deathweed = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.Deathweed")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:Deathweed", Deathweed);

            array = new int[] { ItemID.Fireblossom };
            Fireblossom = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.Fireblossom")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:Fireblossom", Fireblossom);

            array = new int[] { ItemID.Moonglow };
            Moonglow = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.Moonglow")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:Moonglow", Moonglow);

            array = new int[] { ItemID.Hellforge };
            Hellforges = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.Hellforges")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:Hellforges", Hellforges);

            array = new int[] { ItemID.CopperBar, ItemID.TinBar };
            CopperBars = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.CopperBars")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:CopperBars", CopperBars);

            array = new int[] { ItemID.IronBar, ItemID.LeadBar };
            IronBars = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.IronBars")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:IronBars", IronBars);

            array = new int[] { ItemID.SilverBar, ItemID.TungstenBar };
            SilverBars = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.SilverBars")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:SilverBars", SilverBars);

            array = new int[] { ItemID.GoldBar, ItemID.PlatinumBar };
            GoldBars = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.GoldBars")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:GoldBars", GoldBars);

            array = new int[] { ItemID.CobaltBar, ItemID.PalladiumBar };
            CobaltBars = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.CobaltBars")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:CobaltBars", CobaltBars);

            array = new int[] { ItemID.MythrilBar, ItemID.OrichalcumBar };
            MythrilBars = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.MythrilBars")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:MythrilBars", MythrilBars);

            array = new int[] { ItemID.AdamantiteBar, ItemID.TitaniumBar };
            AdamantiteBars = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.AdamantiteBars")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:AdamantiteBars", AdamantiteBars);

            array = new int[] { ItemID.Candle, ItemID.PlatinumCandle };
            GoldCandles = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.GoldCandles")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:GoldCandles", GoldCandles);

            array = new int[] { ItemID.CopperWatch, ItemID.TinWatch };
            CopperWatches = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.CopperWatches")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:CopperWatches", CopperWatches);

            array = new int[] { ItemID.SilverWatch, ItemID.TungstenWatch };
            SilverWatches = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.SilverWatches")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:SilverWatches", SilverWatches);

            array = new int[] { ItemID.GoldWatch, ItemID.PlatinumWatch };
            GoldWatches = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.GoldWatches")}", array);
            RecipeGroup.RegisterGroup("AltLibrary:GoldWatches", GoldWatches);
        }
    }
}
