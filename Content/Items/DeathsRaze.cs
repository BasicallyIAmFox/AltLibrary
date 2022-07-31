using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Content.Items
{
	public class DeathsRaze : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod) => AltLibrary._steamId == 76561198831015363;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Death's Raze");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 45;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 15;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = 54000;
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item1;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.BloodButcherer)
				.AddIngredient(ItemID.Muramasa)
				.AddIngredient(ItemID.BladeofGrass)
				.AddIngredient(ItemID.FieryGreatsword)
				.AddTile(TileID.DemonAltar)
				.Register();
		}
	}
}