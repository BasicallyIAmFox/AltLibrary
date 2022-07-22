using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
#if TML_2022_07
using Terraria.ModLoader;
#endif
using Terraria.Utilities;

namespace AltLibrary.Common.Hooks
{
#if TML_2022_07
	internal class FishingCrateLoot : GlobalItem
#else
	internal class FishingCrateLoot
#endif
	{
#if TML_2022_07
		public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
		{
			if (!ItemID.Sets.IsFishingCrate[item.type])
				return;

			AltLibrary.Instance.Logger.Info(item.type.ToString());
		}
#else
        internal static void Load()
        {
            IL.Terraria.Player.OpenFishingCrate += Player_OpenFishingCrate;
        }

        internal static void Unload()
        {
            IL.Terraria.Player.OpenFishingCrate -= Player_OpenFishingCrate;
        }

        private delegate int delegate_6301(int rng);
		private static void Player_OpenFishingCrate(ILContext il)
		{
			ILCursor c = new(il);

			void method_8252(int number, int rand, int stloc, delegate_6301 rngDel)
			{
				if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand"),
				i => i.MatchLdcI4(rand),
				i => i.MatchCallvirt<UnifiedRandom>(nameof(UnifiedRandom.Next)),
				i => i.MatchStloc(stloc)))
				{
					AltLibrary.Instance.Logger.Info("9 $ " + number + " 1");
					return;
				}
				if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand")))
				{
					AltLibrary.Instance.Logger.Info("9 $ " + number + " 2");
					return;
				}

				c.Emit(OpCodes.Ldloc, stloc);
				c.EmitDelegate(rngDel);
				c.Emit(OpCodes.Stloc, stloc);
			}

#region Wooden Crate's tier 1-2 ores
			method_8252(1, 4, 23, delegate (int rng)
			{
				List<int> handler = new()
				{
					ItemID.CopperOre,
					ItemID.TinOre,
					ItemID.IronOre,
					ItemID.LeadOre,
				};
				AltLibrary.Ores.Where(x => (x.OreType == OreType.Copper || x.OreType == OreType.Iron) && x.Selectable)
								.ToList().ForEach(x => handler.Add(x.ore));
				return Main.rand.Next(handler);
			});
#endregion

#region Pearlwood Crate's cobalt/palladium
			method_8252(2, 2, 23, delegate (int rng)
			{
				List<int> handler = new()
				{
					ItemID.CobaltOre,
					ItemID.PalladiumOre,
				};
				AltLibrary.Ores.Where(x => x.OreType == OreType.Cobalt && x.Selectable)
								.ToList().ForEach(x => handler.Add(x.ore));
				return Main.rand.Next(handler);
			});
#endregion

#region Wooden Crate's tier 1-2 bars
			method_8252(3, 4, 26, delegate (int rng)
			{
				List<int> handler = new()
				{
					ItemID.CopperBar,
					ItemID.TinBar,
					ItemID.IronBar,
					ItemID.LeadBar,
				};
				AltLibrary.Ores.Where(x => (x.OreType == OreType.Copper || x.OreType == OreType.Iron) && x.Selectable)
								.ToList().ForEach(x => handler.Add(x.bar));
				return Main.rand.Next(handler);
			});
#endregion

#region Pearlwood Crate's hardmode tier 1 bars
			method_8252(4, 2, 26, delegate (int rng)
			{
				List<int> handler = new()
				{
					ItemID.CobaltBar,
					ItemID.PalladiumBar,
				};
				AltLibrary.Ores.Where(x => x.OreType == OreType.Cobalt && x.Selectable)
								.ToList().ForEach(x => handler.Add(x.bar));
				return Main.rand.Next(handler);
			});
#endregion

			if (!c.TryGotoNext(i => i.MatchLdarg(1),
				i => i.MatchLdcI4(ItemID.IronCrate)))
			{
				return;
			}

#region Iron Crate's tier 1-3 ores
			method_8252(5, 6, 56, delegate (int rng)
			{
				List<int> handler = new()
				{
					ItemID.CopperOre,
					ItemID.TinOre,
					ItemID.IronOre,
					ItemID.LeadOre,
					ItemID.SilverOre,
					ItemID.TungstenOre
				};
				AltLibrary.Ores.Where(x => (x.OreType == OreType.Copper || x.OreType == OreType.Iron || x.OreType == OreType.Silver) && x.Selectable)
								.ToList().ForEach(x => handler.Add(x.ore));
				return Main.rand.Next(handler);
			});
#endregion

#region Mythril Crate's hardmode tier 1-2 ores
			method_8252(6, 4, 56, delegate (int rng)
			{
				List<int> handler = new()
				{
					ItemID.CobaltOre,
					ItemID.PalladiumOre,
					ItemID.MythrilOre,
					ItemID.OrichalcumOre
				};
				AltLibrary.Ores.Where(x => (x.OreType == OreType.Cobalt || x.OreType == OreType.Mythril) && x.Selectable)
								.ToList().ForEach(x => handler.Add(x.ore));
				return Main.rand.Next(handler);
			});
#endregion

#region Iron Crate's tier 1-3 bars
			method_8252(7, 6, 59, delegate (int rng)
			{
				List<int> handler = new()
				{
					ItemID.CopperBar,
					ItemID.TinBar,
					ItemID.IronBar,
					ItemID.LeadBar,
					ItemID.SilverBar,
					ItemID.TungstenBar
				};
				AltLibrary.Ores.Where(x => (x.OreType == OreType.Copper || x.OreType == OreType.Iron || x.OreType == OreType.Silver) && x.Selectable)
								.ToList().ForEach(x => handler.Add(x.bar));
				return Main.rand.Next(handler);
			});
#endregion

#region Mythril Crate's hardmode tier 1-2 bars
			method_8252(8, 4, 59, delegate (int rng)
			{
				List<int> handler = new()
				{
					ItemID.CobaltBar,
					ItemID.PalladiumBar,
					ItemID.MythrilBar,
					ItemID.OrichalcumBar
				};
				AltLibrary.Ores.Where(x => (x.OreType == OreType.Cobalt || x.OreType == OreType.Mythril) && x.Selectable)
								.ToList().ForEach(x => handler.Add(x.bar));
				return Main.rand.Next(handler);
			});
#endregion

			if (!c.TryGotoNext(i => i.MatchLdarg(1),
				i => i.MatchLdcI4(ItemID.GoldenCrate)))
			{
				return;
			}

#region Golden Crate's tier 3-4 ores
			method_8252(9, 4, 85, delegate (int rng)
			{
				List<int> handler = new()
				{
					ItemID.SilverOre,
					ItemID.TungstenOre,
					ItemID.GoldOre,
					ItemID.PlatinumOre,
				};
				AltLibrary.Ores.Where(x => (x.OreType == OreType.Silver || x.OreType == OreType.Gold) && x.Selectable)
								.ToList().ForEach(x => handler.Add(x.ore));
				return Main.rand.Next(handler);
			});
#endregion

#region Titanium Crate's hardmode tier 2-3 ores
			method_8252(10, 4, 85, delegate (int rng)
			{
				List<int> handler = new()
				{
					ItemID.MythrilOre,
					ItemID.OrichalcumOre,
					ItemID.AdamantiteOre,
					ItemID.TitaniumOre,
				};
				AltLibrary.Ores.Where(x => (x.OreType == OreType.Mythril || x.OreType == OreType.Adamantite) && x.Selectable)
								.ToList().ForEach(x => handler.Add(x.ore));
				return Main.rand.Next(handler);
			});
#endregion

#region Golden Crate's tier 3-4 bars
			method_8252(11, 4, 85, delegate (int rng)
			{
				List<int> handler = new()
				{
					ItemID.SilverBar,
					ItemID.TungstenBar,
					ItemID.GoldBar,
					ItemID.PlatinumBar,
				};
				AltLibrary.Ores.Where(x => (x.OreType == OreType.Silver || x.OreType == OreType.Gold) && x.Selectable)
								.ToList().ForEach(x => handler.Add(x.bar));
				return Main.rand.Next(handler);
			});
#endregion

#region Titanium Crate's hardmode tier 2-3 bars
			method_8252(12, 4, 85, delegate (int rng)
			{
				List<int> handler = new()
				{
					ItemID.MythrilBar,
					ItemID.OrichalcumBar,
					ItemID.AdamantiteBar,
					ItemID.TitaniumBar,
				};
				AltLibrary.Ores.Where(x => (x.OreType == OreType.Mythril || x.OreType == OreType.Adamantite) && x.Selectable)
								.ToList().ForEach(x => handler.Add(x.bar));
				return Main.rand.Next(handler);
			});
#endregion

			if (!c.TryGotoNext(i => i.MatchLdarg(1),
				i => i.MatchLdcI4(ItemID.OceanCrate)))
			{
				return;
			}

#region Other Crates tier 1-4 ores
			method_8252(13, 8, 141, delegate (int rng)
			{
				List<int> handler = new()
				{
					ItemID.CopperOre,
					ItemID.TinOre,
					ItemID.IronOre,
					ItemID.LeadOre,
					ItemID.SilverOre,
					ItemID.TungstenOre,
					ItemID.GoldOre,
					ItemID.PlatinumOre,
				};
				AltLibrary.Ores.Where(x => (x.OreType == OreType.Copper || x.OreType == OreType.Iron || x.OreType == OreType.Silver || x.OreType == OreType.Gold) && x.Selectable)
								.ToList().ForEach(x => handler.Add(x.ore));
				return Main.rand.Next(handler);
			});
#endregion

#region Other Crates hardmode tier 1-3 ores
			method_8252(14, 6, 141, delegate (int rng)
			{
				List<int> handler = new()
				{
					ItemID.CobaltOre,
					ItemID.PalladiumOre,
					ItemID.MythrilOre,
					ItemID.OrichalcumOre,
					ItemID.AdamantiteOre,
					ItemID.TitaniumOre,
				};
				AltLibrary.Ores.Where(x => (x.OreType == OreType.Cobalt || x.OreType == OreType.Mythril || x.OreType == OreType.Adamantite) && x.Selectable)
								.ToList().ForEach(x => handler.Add(x.ore));
				return Main.rand.Next(handler);
			});
#endregion

#region Other Crates tier 2-4 bars
			method_8252(15, 6, 144, delegate (int rng)
			{
				List<int> handler = new()
				{
					ItemID.IronBar,
					ItemID.LeadBar,
					ItemID.SilverBar,
					ItemID.TungstenBar,
					ItemID.GoldBar,
					ItemID.PlatinumBar,
				};
				AltLibrary.Ores.Where(x => (x.OreType == OreType.Iron || x.OreType == OreType.Silver || x.OreType == OreType.Gold) && x.Selectable)
								.ToList().ForEach(x => handler.Add(x.bar));
				return Main.rand.Next(handler);
			});
#endregion

#region Other Crates hardmode tier 2-3 bars
			method_8252(16, 6, 144, delegate (int rng)
			{
				List<int> handler = new()
				{
					ItemID.CobaltBar,
					ItemID.PalladiumBar,
					ItemID.MythrilBar,
					ItemID.OrichalcumBar,
					ItemID.AdamantiteBar,
					ItemID.TitaniumBar,
				};
				AltLibrary.Ores.Where(x => (x.OreType == OreType.Cobalt || x.OreType == OreType.Mythril || x.OreType == OreType.Adamantite) && x.Selectable)
								.ToList().ForEach(x => handler.Add(x.bar));
				return Main.rand.Next(handler);
			});
#endregion
		}
#endif
	}
}