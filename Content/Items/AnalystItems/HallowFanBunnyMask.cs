using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Content.Items.AnalystItems
{
	[AutoloadEquip(EquipType.Head)]
	internal class HallowFanBunnyMask : ModItem
	{
		public override string Texture => "AltLibrary/Assets/HallowFanBunnyMask";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kawaii Bunny Perch");
			Tooltip.SetDefault("'UwU'");
			SacrificeTotal = 1;
			ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.vanity = true;
			Item.value = Item.buyPrice(gold: 5);
			Item.rare = ItemRarityID.Pink;
		}
	}
}
