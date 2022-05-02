using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace AltLibrary.Common.Systems
{
    internal class RecipeGroups : ModSystem
    {
        public static RecipeGroup group;
        public override void AddRecipeGroups()
        {
            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.EvilBars")}", new int[]
            {
                ItemID.DemoniteBar,
                ItemID.CrimtaneBar
            });
            RecipeGroup.RegisterGroup("AltLibrary:EvilBars", group);

            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.HallowBars")}", new int[]
            {
                ItemID.HallowedBar
            });
            RecipeGroup.RegisterGroup("AltLibrary:HallowBars", group);

            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.HellBars")}", new int[]
            {
                ItemID.HellstoneBar
            });
            RecipeGroup.RegisterGroup("AltLibrary:HellBars", group);

            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.JungleBars")}", new int[]
            {
                ItemID.ChlorophyteBar
            });
            RecipeGroup.RegisterGroup("AltLibrary:JungleBars", group);

            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.MushroomBars")}", new int[]
            {
                ItemID.ShroomiteBar
            });
            RecipeGroup.RegisterGroup("AltLibrary:MushroomBars", group);


            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.EvilSwords")}", new int[]
            {
                ItemID.LightsBane,
                ItemID.BloodButcherer
            });
            RecipeGroup.RegisterGroup("AltLibrary:EvilSwords", group);

            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.HallowSwords")}", new int[]
            {
                ItemID.Excalibur
            });
            RecipeGroup.RegisterGroup("AltLibrary:HallowSwords", group);

            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.HellSwords")}", new int[]
            {
                ItemID.FieryGreatsword
            });
            RecipeGroup.RegisterGroup("AltLibrary:HellSwords", group);

            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.JungleSwords")}", new int[]
            {
                ItemID.BladeofGrass
            });
            RecipeGroup.RegisterGroup("AltLibrary:JungleSwords", group);

            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.ComboSwords")}", new int[]
            {
                ItemID.NightsEdge
            });
            RecipeGroup.RegisterGroup("AltLibrary:ComboSwords", group);

            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.TrueComboSwords")}", new int[]
            {
                ItemID.TrueNightsEdge
            });
            RecipeGroup.RegisterGroup("AltLibrary:TrueComboSwords", group);

            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.TrueHallowSwords")}", new int[]
            {
                ItemID.TrueExcalibur
            });
            RecipeGroup.RegisterGroup("AltLibrary:TrueHallowSwords", group);


            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.RottenChunks")}", new int[]
            {
                ItemID.RottenChunk,
                ItemID.Vertebrae
            });
            RecipeGroup.RegisterGroup("AltLibrary:RottenChunks", group);

            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.PixieDusts")}", new int[]
            {
                ItemID.PixieDust
            });
            RecipeGroup.RegisterGroup("AltLibrary:PixieDusts", group);

            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.UnicornHorns")}", new int[]
            {
                ItemID.UnicornHorn
            });
            RecipeGroup.RegisterGroup("AltLibrary:UnicornHorns", group);

            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.CrystalShards")}", new int[]
            {
                ItemID.CrystalShard
            });
            RecipeGroup.RegisterGroup("AltLibrary:CrystalShards", group);

            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.CursedFlames")}", new int[]
            {
                ItemID.CursedFlame,
                ItemID.Ichor
            });
            RecipeGroup.RegisterGroup("AltLibrary:CursedFlames", group);

            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.ShadowScales")}", new int[]
            {
                ItemID.ShadowScale,
                ItemID.TissueSample
            });
            RecipeGroup.RegisterGroup("AltLibrary:ShadowScales", group);


            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.JungleSpores")}", new int[]
            {
                ItemID.JungleSpores
            });
            RecipeGroup.RegisterGroup("AltLibrary:JungleSpores", group);


            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.Deathweed")}", new int[]
            {
                ItemID.Deathweed
            });
            RecipeGroup.RegisterGroup("AltLibrary:Deathweed", group);

            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.Fireblossom")}", new int[]
            {
                ItemID.Fireblossom
            });
            RecipeGroup.RegisterGroup("AltLibrary:Fireblossom", group);

            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.Moonglow")}", new int[]
            {
                ItemID.Moonglow
            });
            RecipeGroup.RegisterGroup("AltLibrary:Moonglow", group);


            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Language.GetTextValue("Mods.AltLibrary.RecipeGroups.Hellforges")}", new int[]
            {
                ItemID.Hellforge
            });
            RecipeGroup.RegisterGroup("AltLibrary:Hellforges", group);
        }
    }
}
