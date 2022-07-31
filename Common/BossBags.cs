using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Condition;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common
{
	internal class BossBags : GlobalItem
	{
		public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
		{
			List<AltBiome> HallowList = new();
			List<AltBiome> HellList = new();
			List<AltBiome> JungleList = new();
			List<AltBiome> EvilList = new();
			foreach (AltBiome biome in AltLibrary.Biomes)
			{
				switch (biome.BiomeType)
				{
					case BiomeType.Evil:
						EvilList.Add(biome);
						break;
					case BiomeType.Hallow:
						HallowList.Add(biome);
						break;
					case BiomeType.Hell:
						HellList.Add(biome);
						break;
					case BiomeType.Jungle:
						JungleList.Add(biome);
						break;
				}
			}

			var entries = itemLoot.Get(false);
			switch (item.type)
			{
				case ItemID.EyeOfCthulhuBossBag:
					{
						foreach (var entry in entries)
						{
							if (entry is ItemDropWithConditionRule conditionRule)
							{
								if (conditionRule.itemId == ItemID.DemoniteOre || conditionRule.itemId == ItemID.CrimtaneOre ||
									conditionRule.itemId == ItemID.CorruptSeeds || conditionRule.itemId == ItemID.CrimsonSeeds
									|| conditionRule.itemId == ItemID.UnholyArrow)
								{
									itemLoot.Remove(entry);
								}
							}
						}

						var corroCrimCondition = new LeadingConditionRule(new CorroCrimDropCondition());
						var corroCondition = new LeadingConditionRule(new Conditions.IsCorruptionAndNotExpert());
						var crimCondition = new LeadingConditionRule(new Conditions.IsCrimsonAndNotExpert());

						corroCrimCondition.OnSuccess(corroCondition);
						corroCrimCondition.OnSuccess(crimCondition);
						corroCondition.OnSuccess(ItemDropRule.Common(ItemID.DemoniteOre, 1, 30, 90));
						corroCondition.OnSuccess(ItemDropRule.Common(ItemID.CorruptSeeds, 1, 1, 3));
						corroCondition.OnSuccess(ItemDropRule.Common(ItemID.UnholyArrow, 1, 20, 50));

						crimCondition.OnSuccess(ItemDropRule.Common(ItemID.CrimtaneOre, 1, 30, 90));
						crimCondition.OnSuccess(ItemDropRule.Common(ItemID.CrimsonSeeds, 1, 1, 3));

						itemLoot.Add(corroCrimCondition);

						var expertCondition = new LeadingConditionRule(new Conditions.NotExpert());

						foreach (AltBiome biome in EvilList)
						{
							var biomeDropRule = new LeadingConditionRule(new EvilAltDropCondition(biome));
							if (biome.BiomeOreItem != null) biomeDropRule.OnSuccess(ItemDropRule.Common((int)biome.BiomeOreItem, 1, 30, 90));
							if (biome.SeedType != null) biomeDropRule.OnSuccess(ItemDropRule.Common((int)biome.SeedType, 1, 1, 3));
							if (biome.ArrowType != null) biomeDropRule.OnSuccess(ItemDropRule.Common((int)biome.ArrowType, 20, 50));
							expertCondition.OnSuccess(biomeDropRule);
						}
						itemLoot.Add(expertCondition);

						break;
					}
				case ItemID.WallOfFleshBossBag:
					{
						foreach (var entry in entries)
						{
							if (entry is ItemDropWithConditionRule rule && rule.itemId == ItemID.Pwnhammer)
							{
								itemLoot.Remove(rule);
							}
						}
						var expertCondition = new LeadingConditionRule(new Conditions.NotExpert());
						var hallowBarCondition = new LeadingConditionRule(new HallowDropCondition());
						expertCondition.OnSuccess(hallowBarCondition);
						hallowBarCondition.OnSuccess(ItemDropRule.Common(ItemID.Pwnhammer));
						foreach (AltBiome biome in HallowList)
						{
							var biomeDropRule = new LeadingConditionRule(new HallowAltDropCondition(biome));
							biomeDropRule.OnSuccess(ItemDropRule.Common(biome.HammerType));
							expertCondition.OnSuccess(biomeDropRule);
						}
						itemLoot.Add(expertCondition);
						break;
					}
				case ItemID.TwinsBossBag:
				case ItemID.DestroyerBossBag:
				case ItemID.SkeletronPrimeBossBag:
					{
						foreach (var entry in entries)
						{
							if (entry is ItemDropWithConditionRule conditionRule && conditionRule.itemId == ItemID.HallowedBar)
							{
								itemLoot.Remove(entry);
								break;
							}
						}
						var expertCondition = new LeadingConditionRule(new Conditions.NotExpert());
						var hallowBarCondition = new LeadingConditionRule(new HallowDropCondition());
						expertCondition.OnSuccess(hallowBarCondition);
						hallowBarCondition.OnSuccess(ItemDropRule.Common(ItemID.HallowedBar, 1, 15, 30));

						foreach (AltBiome biome in HallowList)
						{
							var biomeDropRule = new LeadingConditionRule(new HallowAltDropCondition(biome));
							if (biome.MechDropItemType != null) biomeDropRule.OnSuccess(ItemDropRule.Common((int)biome.MechDropItemType, 1, 15, 30));
							expertCondition.OnSuccess(biomeDropRule);
						}
						itemLoot.Add(expertCondition);
						break;
					}
			}
		}
	}
}
