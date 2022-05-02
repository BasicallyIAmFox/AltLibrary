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
        }
    }
}
