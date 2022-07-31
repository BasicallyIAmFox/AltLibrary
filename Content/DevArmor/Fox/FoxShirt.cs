using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Content.DevArmor.Fox
{
	[AutoloadEquip(EquipType.Body)]
	internal class FoxShirt : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fox's Sweater");
			Tooltip.SetDefault("Great for impersonating mod devs!");
			SacrificeTotal = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 14;
			Item.value = Item.sellPrice(gold: 5);
			Item.rare = ItemRarityID.Cyan;
			Item.vanity = true;
		}
	}
}
