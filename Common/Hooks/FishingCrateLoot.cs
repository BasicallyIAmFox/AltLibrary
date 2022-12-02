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

			static void m(IItemDropRule[] options, int f, OreType t) {
				if (options.Any(x => x is CommonDropNotScalingWithLuck g && g.itemId == f)) {
					List<IItemDropRule> list = options.ToList();
					foreach (AltOre o in AltLibrary.Ores.Where(x => x.OreType == t))
						list.Add(ItemDropRule.NotScalingWithLuck(o.ore, 1, (options[0] as CommonDropNotScalingWithLuck).amountDroppedMinimum, (options[0] as CommonDropNotScalingWithLuck).amountDroppedMaximum));
					options = list.ToArray();
				}
			}
			static void f(IItemDropRule[] options, int f, OreType t) {
				if (options.Any(x => x is CommonDropNotScalingWithLuck g && g.itemId == f)) {
					List<IItemDropRule> list = options.ToList();
					foreach (AltOre o in AltLibrary.Ores.Where(x => x.OreType == t))
						list.Add(ItemDropRule.NotScalingWithLuck(o.bar, 1, (options[0] as CommonDropNotScalingWithLuck).amountDroppedMinimum, (options[0] as CommonDropNotScalingWithLuck).amountDroppedMaximum));
					options = list.ToArray();
				}
			}

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
									m(h.options, ItemID.CopperOre, OreType.Copper);
									m(h.options, ItemID.IronOre, OreType.Iron);
									m(h.options, ItemID.SilverOre, OreType.Silver);
									m(h.options, ItemID.GoldOre, OreType.Gold);
									m(h.options, ItemID.CobaltOre, OreType.Cobalt);
									m(h.options, ItemID.MythrilOre, OreType.Mythril);
									m(h.options, ItemID.AdamantiteOre, OreType.Adamantite);

									f(h.options, ItemID.CopperBar, OreType.Copper);
									f(h.options, ItemID.IronBar, OreType.Iron);
									f(h.options, ItemID.SilverBar, OreType.Silver);
									f(h.options, ItemID.GoldBar, OreType.Gold);
									f(h.options, ItemID.CobaltBar, OreType.Cobalt);
									f(h.options, ItemID.MythrilBar, OreType.Mythril);
									f(h.options, ItemID.AdamantiteBar, OreType.Adamantite);
								}
							}
						}
					}
				}
			}
		}
	}
}