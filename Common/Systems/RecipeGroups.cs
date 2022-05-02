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
            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + "Evil Bar", new int[]
            {
                ItemID.DemoniteBar,
                ItemID.CrimtaneBar
            });
            RecipeGroup.RegisterGroup("AltLibrary:EvilBars", group);

            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + "Light Bar", new int[]
            {
                ItemID.HallowedBar
            });
            RecipeGroup.RegisterGroup("AltLibrary:HallowBars", group);

            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + "Underworld Bar", new int[]
            {
                ItemID.HellstoneBar
            });
            RecipeGroup.RegisterGroup("AltLibrary:HellBars", group);

            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + "Hardmode Tropical Bar", new int[]
            {
                ItemID.ChlorophyteBar
            });
            RecipeGroup.RegisterGroup("AltLibrary:JungleBars", group);

            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + "Mushroom Bar", new int[]
            {
                ItemID.ShroomiteBar
            });
            RecipeGroup.RegisterGroup("AltLibrary:JungleBars", group);


            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + "Evil Sword", new int[]
            {
                ItemID.LightsBane,
                ItemID.BloodButcherer
            });
            RecipeGroup.RegisterGroup("AltLibrary:EvilSwords", group);

            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + "Light Sword", new int[]
            {
                ItemID.Excalibur
            });
            RecipeGroup.RegisterGroup("AltLibrary:HallowSwords", group);

            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + "Underworld Sword", new int[]
            {
                ItemID.FieryGreatsword
            });
            RecipeGroup.RegisterGroup("AltLibrary:HellSwords", group);

            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + "Tropical Sword", new int[]
            {
                ItemID.BladeofGrass
            });
            RecipeGroup.RegisterGroup("AltLibrary:JungleSwords", group);

            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + "Combination Sword", new int[]
            {
                ItemID.NightsEdge
            });
            RecipeGroup.RegisterGroup("AltLibrary:ComboSwords", group);

            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + "True Combination Sword", new int[]
            {
                ItemID.TrueNightsEdge
            });
            RecipeGroup.RegisterGroup("AltLibrary:TrueComboSwords", group);

            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + "True Light Sword", new int[]
            {
                ItemID.TrueExcalibur
            });
            RecipeGroup.RegisterGroup("AltLibrary:TrueHallowSwords", group);


            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + "Vile Innard", new int[]
            {
                ItemID.RottenChunk,
                ItemID.Vertebrae
            });
            RecipeGroup.RegisterGroup("AltLibrary:RottenChunks", group);

            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + "Light Residue", new int[]
            {
                ItemID.PixieDust
            });
            RecipeGroup.RegisterGroup("AltLibrary:PixieDusts", group);

            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + "Light Component", new int[]
            {
                ItemID.CrystalShard
            });
            RecipeGroup.RegisterGroup("AltLibrary:CrystalShards", group);

            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + "Vile Component", new int[]
            {
                ItemID.CursedFlame,
                ItemID.Ichor
            });
            RecipeGroup.RegisterGroup("AltLibrary:CursedFlames", group);

            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + "Evil Boss Drop", new int[]
            {
                ItemID.ShadowScale,
                ItemID.TissueSample
            });
            RecipeGroup.RegisterGroup("AltLibrary:ShadowScales", group);


            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + "Tropical Foliage", new int[]
            {
                ItemID.JungleSpores
            });
            RecipeGroup.RegisterGroup("AltLibrary:JungleSpores", group);


            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + "Evil Herb", new int[]
            {
                ItemID.Deathweed
            });
            RecipeGroup.RegisterGroup("AltLibrary:Deathweed", group);

            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + "Underworld Herb", new int[]
            {
                ItemID.Fireblossom
            });
            RecipeGroup.RegisterGroup("AltLibrary:Fireblossom", group);

            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + "Tropical Herb", new int[]
            {
                ItemID.Moonglow
            });
            RecipeGroup.RegisterGroup("AltLibrary:Moonglow", group);


            group = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + "Underworld Forge", new int[]
            {
                ItemID.Hellforge
            });
            RecipeGroup.RegisterGroup("AltLibrary:Hellforges", group);
        }
    }
}
