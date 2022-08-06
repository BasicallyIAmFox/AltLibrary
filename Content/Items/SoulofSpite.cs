using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Content.Items
{
	public class SoulofSpite : ModItem
	{
		//public override bool IsLoadingEnabled(Mod mod) => AltLibrary._steamId == 76561198831015363;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul of Spite");
			Tooltip.SetDefault("'The essence of bloody creatures'");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
			ItemID.Sets.ItemIconPulse[Item.type] = true;
			ItemID.Sets.ItemNoGravity[Item.type] = true;

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
		}

		public override void PostUpdate() => Lighting.AddLight(Item.Center, Color.Crimson.ToVector3() * 0.55f * Main.essScale);

		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override void SetDefaults()
		{
			Item refItem = new();
			refItem.SetDefaults(ItemID.SoulofNight);
			Item.width = refItem.width;
			Item.height = refItem.height;
			Item.value = 1000;
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 999;
		}

		public override void Load()
		{
			On.Terraria.Item.SetDefaults_int_bool += Item_SetDefaults_int_bool;
		}

		private void Item_SetDefaults_int_bool(On.Terraria.Item.orig_SetDefaults_int_bool orig, Item self, int Type, bool noMatCheck)
		{
			orig(self, Type, noMatCheck);
			if (self.type == this.Type || self.type == ModContent.ItemType<DeathsRaze>() || self.type == ModContent.ItemType<TrueDeathsRaze>())
			{
				int type = ItemID.SoulofNight;
				if (self.type == ModContent.ItemType<DeathsRaze>())
					type = ItemID.NightsEdge;
				else if (self.type == ModContent.ItemType<TrueDeathsRaze>())
					type = ItemID.TrueNightsEdge;

				self.TurnToAir();
				self.netID = self.type = type;
				self.CloneDefaults(type);
			}
		}
	}
	public class SoulofSpiteDrop : GlobalNPC
	{
		public override bool IsLoadingEnabled(Mod mod) => AltLibrary._steamId == 76561198831015363;

		public override void ModifyGlobalLoot(GlobalLoot globalLoot)
		{
			globalLoot.RemoveWhere(
				rule => rule is ItemDropWithConditionRule drop
					&& drop.itemId == ItemID.SoulofNight
					&& drop.condition is Conditions.SoulOfNight
			);
			globalLoot.Add(ItemDropRule.ByCondition(new SoulOfNight(), ItemID.SoulofNight));
			globalLoot.Add(ItemDropRule.ByCondition(new SoulOfSpite(), ModContent.ItemType<SoulofSpite>()));
		}

		private class SoulOfNight : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info) => Conditions.SoulOfWhateverConditionCanDrop(info) && info.player.ZoneCorrupt;

			public bool CanShowItemDropInUI() => false;

			public string GetConditionDescription() => null;
		}
		private class SoulOfSpite : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info) => Conditions.SoulOfWhateverConditionCanDrop(info) && info.player.ZoneCrimson;

			public bool CanShowItemDropInUI() => false;

			public string GetConditionDescription() => null;
		}
	}
}