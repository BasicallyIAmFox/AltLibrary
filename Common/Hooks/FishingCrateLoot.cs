using AltLibrary.Common.AltOres;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common.Hooks
{
	internal class FishingCrateLoot : GlobalItem
	{
		public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
		{
			if (!ItemID.Sets.IsFishingCrate[item.type])
				return;

			var loot = itemLoot.Get(false);
			foreach (var l in loot)
			{
				if (l is AlwaysAtleastOneSuccessDropRule rule)
				{
					foreach (var r in rule.rules)
					{
						if (r is SequentialRulesNotScalingWithLuckRule seq)
						{
							foreach (var b in seq.rules)
							{
								if (b is OneFromRulesRule h)
								{
									if (h.options.Any(x => x is CommonDropNotScalingWithLuck g && g.itemId == ItemID.CopperOre))
									{
										List<IItemDropRule> list = h.options.ToList();
										foreach (AltOre o in AltLibrary.Ores.Where(x => x.OreType == OreType.Copper))
											list.Add(ItemDropRule.NotScalingWithLuck(o.ore, 1, (h.options[0] as CommonDropNotScalingWithLuck).amountDroppedMinimum, (h.options[0] as CommonDropNotScalingWithLuck).amountDroppedMaximum));
										h.options = list.ToArray();
									}
									if (h.options.Any(x => x is CommonDropNotScalingWithLuck g && g.itemId == ItemID.IronOre))
									{
										List<IItemDropRule> list = h.options.ToList();
										foreach (AltOre o in AltLibrary.Ores.Where(x => x.OreType == OreType.Iron))
											list.Add(ItemDropRule.NotScalingWithLuck(o.ore, 1, (h.options[0] as CommonDropNotScalingWithLuck).amountDroppedMinimum, (h.options[0] as CommonDropNotScalingWithLuck).amountDroppedMaximum));
										h.options = list.ToArray();
									}
									if (h.options.Any(x => x is CommonDropNotScalingWithLuck g && g.itemId == ItemID.SilverOre))
									{
										List<IItemDropRule> list = h.options.ToList();
										foreach (AltOre o in AltLibrary.Ores.Where(x => x.OreType == OreType.Silver))
											list.Add(ItemDropRule.NotScalingWithLuck(o.ore, 1, (h.options[0] as CommonDropNotScalingWithLuck).amountDroppedMinimum, (h.options[0] as CommonDropNotScalingWithLuck).amountDroppedMaximum));
										h.options = list.ToArray();
									}
									if (h.options.Any(x => x is CommonDropNotScalingWithLuck g && g.itemId == ItemID.GoldOre))
									{
										List<IItemDropRule> list = h.options.ToList();
										foreach (AltOre o in AltLibrary.Ores.Where(x => x.OreType == OreType.Gold))
											list.Add(ItemDropRule.NotScalingWithLuck(o.ore, 1, (h.options[0] as CommonDropNotScalingWithLuck).amountDroppedMinimum, (h.options[0] as CommonDropNotScalingWithLuck).amountDroppedMaximum));
										h.options = list.ToArray();
									}
									if (h.options.Any(x => x is CommonDropNotScalingWithLuck g && g.itemId == ItemID.CobaltOre))
									{
										List<IItemDropRule> list = h.options.ToList();
										foreach (AltOre o in AltLibrary.Ores.Where(x => x.OreType == OreType.Cobalt))
											list.Add(ItemDropRule.NotScalingWithLuck(o.ore, 1, (h.options[^1] as CommonDropNotScalingWithLuck).amountDroppedMinimum, (h.options[^1] as CommonDropNotScalingWithLuck).amountDroppedMaximum));
										h.options = list.ToArray();
									}
									if (h.options.Any(x => x is CommonDropNotScalingWithLuck g && g.itemId == ItemID.MythrilOre))
									{
										List<IItemDropRule> list = h.options.ToList();
										foreach (AltOre o in AltLibrary.Ores.Where(x => x.OreType == OreType.Mythril))
											list.Add(ItemDropRule.NotScalingWithLuck(o.ore, 1, (h.options[^1] as CommonDropNotScalingWithLuck).amountDroppedMinimum, (h.options[^1] as CommonDropNotScalingWithLuck).amountDroppedMaximum));
										h.options = list.ToArray();
									}
									if (h.options.Any(x => x is CommonDropNotScalingWithLuck g && g.itemId == ItemID.AdamantiteOre))
									{
										List<IItemDropRule> list = h.options.ToList();
										foreach (AltOre o in AltLibrary.Ores.Where(x => x.OreType == OreType.Adamantite))
											list.Add(ItemDropRule.NotScalingWithLuck(o.ore, 1, (h.options[^1] as CommonDropNotScalingWithLuck).amountDroppedMinimum, (h.options[^1] as CommonDropNotScalingWithLuck).amountDroppedMaximum));
										h.options = list.ToArray();
									}

									if (h.options.Any(x => x is CommonDropNotScalingWithLuck g && g.itemId == ItemID.CopperBar))
									{
										List<IItemDropRule> list = h.options.ToList();
										foreach (AltOre o in AltLibrary.Ores.Where(x => x.OreType == OreType.Copper))
											list.Add(ItemDropRule.NotScalingWithLuck(o.bar, 1, (h.options[0] as CommonDropNotScalingWithLuck).amountDroppedMinimum, (h.options[0] as CommonDropNotScalingWithLuck).amountDroppedMaximum));
										h.options = list.ToArray();
									}
									if (h.options.Any(x => x is CommonDropNotScalingWithLuck g && g.itemId == ItemID.IronBar))
									{
										List<IItemDropRule> list = h.options.ToList();
										foreach (AltOre o in AltLibrary.Ores.Where(x => x.OreType == OreType.Iron))
											list.Add(ItemDropRule.NotScalingWithLuck(o.bar, 1, (h.options[0] as CommonDropNotScalingWithLuck).amountDroppedMinimum, (h.options[0] as CommonDropNotScalingWithLuck).amountDroppedMaximum));
										h.options = list.ToArray();
									}
									if (h.options.Any(x => x is CommonDropNotScalingWithLuck g && g.itemId == ItemID.SilverBar))
									{
										List<IItemDropRule> list = h.options.ToList();
										foreach (AltOre o in AltLibrary.Ores.Where(x => x.OreType == OreType.Silver))
											list.Add(ItemDropRule.NotScalingWithLuck(o.bar, 1, (h.options[0] as CommonDropNotScalingWithLuck).amountDroppedMinimum, (h.options[0] as CommonDropNotScalingWithLuck).amountDroppedMaximum));
										h.options = list.ToArray();
									}
									if (h.options.Any(x => x is CommonDropNotScalingWithLuck g && g.itemId == ItemID.GoldBar))
									{
										List<IItemDropRule> list = h.options.ToList();
										foreach (AltOre o in AltLibrary.Ores.Where(x => x.OreType == OreType.Gold))
											list.Add(ItemDropRule.NotScalingWithLuck(o.bar, 1, (h.options[0] as CommonDropNotScalingWithLuck).amountDroppedMinimum, (h.options[0] as CommonDropNotScalingWithLuck).amountDroppedMaximum));
										h.options = list.ToArray();
									}
									if (h.options.Any(x => x is CommonDropNotScalingWithLuck g && g.itemId == ItemID.CobaltBar))
									{
										List<IItemDropRule> list = h.options.ToList();
										foreach (AltOre o in AltLibrary.Ores.Where(x => x.OreType == OreType.Cobalt))
											list.Add(ItemDropRule.NotScalingWithLuck(o.bar, 1, (h.options[^1] as CommonDropNotScalingWithLuck).amountDroppedMinimum, (h.options[^1] as CommonDropNotScalingWithLuck).amountDroppedMaximum));
										h.options = list.ToArray();
									}
									if (h.options.Any(x => x is CommonDropNotScalingWithLuck g && g.itemId == ItemID.MythrilBar))
									{
										List<IItemDropRule> list = h.options.ToList();
										foreach (AltOre o in AltLibrary.Ores.Where(x => x.OreType == OreType.Mythril))
											list.Add(ItemDropRule.NotScalingWithLuck(o.bar, 1, (h.options[^1] as CommonDropNotScalingWithLuck).amountDroppedMinimum, (h.options[^1] as CommonDropNotScalingWithLuck).amountDroppedMaximum));
										h.options = list.ToArray();
									}
									if (h.options.Any(x => x is CommonDropNotScalingWithLuck g && g.itemId == ItemID.AdamantiteBar))
									{
										List<IItemDropRule> list = h.options.ToList();
										foreach (AltOre o in AltLibrary.Ores.Where(x => x.OreType == OreType.Adamantite))
											list.Add(ItemDropRule.NotScalingWithLuck(o.bar, 1, (h.options[^1] as CommonDropNotScalingWithLuck).amountDroppedMinimum, (h.options[^1] as CommonDropNotScalingWithLuck).amountDroppedMaximum));
										h.options = list.ToArray();
									}
								}
							}
						}
					}
				}
			}
		}
	}
}