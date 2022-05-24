using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AltLibrary.Content.Items
{
    internal class HallowBunnyCage : ModItem
    {
        public override string Texture => "AltLibrary/Assets/HallowBunnyCageItem";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hallow Bunny Cage");
            Tooltip.SetDefault("'I swear you'll pay!'");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.BunnyCage);
            Item.rare = ModContent.RarityType<HallowBunny.CustomRarity>();
            Item.createTile = ModContent.TileType<Tiles.HallowBunnyCage>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Terrarium)
                .AddIngredient(ModContent.ItemType<HallowBunny>())
                .Register();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            string line = Language.GetTextValue("Mods.AltLibrary.ItemTooltip.HallowBunnyCage");
            int index = tooltips.FindIndex(i => i.Name == "Tooltip0" && i.Mod == "Terraria");
            if (index != -1)
            {
                if (Main.GameUpdateCount % Main.rand.Next(60, 90) == 0)
                {
                    string str = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                    int size = Main.rand.Next(Math.Max(0, line.Length - 5), Math.Max(5, line.Length));
                    string ran = "";
                    for (int i = 0; i < size; i++)
                    {
                        int x = Main.rand.Next(str.Length);
                        ran += str[x];
                    }
                    tooltips[index].Text = ran;
                    tooltips[index].OverrideColor = new Color(MathHelper.Lerp(0f, 1f, Main.rand.NextFloat()), 1f, 1f);
                }
                else
                {
                    tooltips[index].Text = line;
                    tooltips[index].OverrideColor = Colors.RarityRed;
                }
            }
        }

        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset) => ModContent.GetInstance<HallowBunny>().PreDrawTooltipLine(line, ref yOffset);
    }
}
