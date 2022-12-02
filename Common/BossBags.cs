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
						int oreCountDen = 0;
						int oreCountMin = 0;
						int oreCountMax = 0;
						int oreSeedsDen = 0;
						int oreSeedsMin = 0;
						int oreSeedsMax = 0;
						int arrowDen = 0;
						int arrowMin = 0;
						int arrowMax = 0;

						foreach (var entry in entries)
						{
							if (entry is ItemDropWithConditionRule conditionRule)
							{
								if (conditionRule.itemId == ItemID.DemoniteOre || conditionRule.itemId == ItemID.CrimtaneOre ||
									conditionRule.itemId == ItemID.CorruptSeeds || conditionRule.itemId == ItemID.CrimsonSeeds
									|| conditionRule.itemId == ItemID.UnholyArrow)
								{
									if (conditionRule.itemId == ItemID.DemoniteOre || conditionRule.itemId == ItemID.CrimtaneOre)
									{
										oreCountDen = conditionRule.chanceDenominator;
										oreCountMin = conditionRule.amountDroppedMinimum;
										oreCountMax = conditionRule.amountDroppedMaximum;
									}
									else if (conditionRule.itemId == ItemID.UnholyArrow)
									{
										arrowDen = conditionRule.chanceDenominator;
										arrowMin = conditionRule.amountDroppedMinimum;
										arrowMax = conditionRule.amountDroppedMaximum;
									}
									else
									{
										oreSeedsDen = conditionRule.chanceDenominator;
										oreSeedsMin = conditionRule.amountDroppedMinimum;
										oreSeedsMax = conditionRule.amountDroppedMaximum;
									}

									itemLoot.Remove(entry);
								}
							}
						}

						var corroCrimCondition = new LeadingConditionRule(new CorroCrimDropCondition());
						var corroCondition = new LeadingConditionRule(new Conditions.IsCorruptionAndNotExpert());
						var crimCondition = new LeadingConditionRule(new Conditions.IsCrimsonAndNotExpert());

						corroCrimCondition.OnSuccess(corroCondition);
						corroCrimCondition.OnSuccess(crimCondition);
						corroCondition.OnSuccess(ItemDropRule.Common(ItemID.DemoniteOre, oreCountDen, oreCountMin, oreCountMax));
						corroCondition.OnSuccess(ItemDropRule.Common(ItemID.CorruptSeeds, oreSeedsDen, oreSeedsMin, oreSeedsMax));
						corroCondition.OnSuccess(ItemDropRule.Common(ItemID.UnholyArrow, arrowDen, arrowMin, arrowMax));

						crimCondition.OnSuccess(ItemDropRule.Common(ItemID.CrimtaneOre, oreCountDen, oreCountMin, oreCountMax));
						crimCondition.OnSuccess(ItemDropRule.Common(ItemID.CrimsonSeeds, oreSeedsDen, oreSeedsMin, oreSeedsMax));

						itemLoot.Add(corroCrimCondition);

						var expertCondition = new LeadingConditionRule(new Conditions.NotExpert());

						foreach (AltBiome biome in EvilList)
						{
							var biomeDropRule = new LeadingConditionRule(new EvilAltDropCondition(biome));
							if (biome.BiomeOreItem != null) biomeDropRule.OnSuccess(ItemDropRule.Common((int)biome.BiomeOreItem, oreCountDen, oreCountMin, oreCountMax));
							if (biome.SeedType != null) biomeDropRule.OnSuccess(ItemDropRule.Common((int)biome.SeedType, oreSeedsDen, oreSeedsMin, oreSeedsMax));
							if (biome.ArrowType != null) biomeDropRule.OnSuccess(ItemDropRule.Common((int)biome.ArrowType, arrowDen, arrowMin, arrowMax));
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
