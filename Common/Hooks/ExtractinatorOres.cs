using AltLibrary.Common.AltOres;
using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common.Hooks
{
	public static class ExtractinatorOres
	{
		public static List<int> Gems;
		public static List<int> Ores;
		public static List<int> HardmodeOres;

		internal static void Load()
		{
			Gems = new()
			{
				ItemID.Amethyst,
				ItemID.Topaz,
				ItemID.Sapphire,
				ItemID.Emerald,
				ItemID.Ruby,
				ItemID.Diamond,
			};

			Ores = new()
			{
				ItemID.CopperOre,
				ItemID.TinOre,
				ItemID.IronOre,
				ItemID.LeadOre,
				ItemID.SilverOre,
				ItemID.TungstenOre,
				ItemID.GoldOre,
				ItemID.PlatinumOre
			};

			foreach (AltOre o in AltLibrary.Ores.Where(x => x.OreType >= OreType.Copper && x.OreType <= OreType.Gold || x.IncludeInExtractinator))
				Ores.Add(o.ore);

			HardmodeOres = new();
			HardmodeOres.AddRange(Ores);
			HardmodeOres.AddRange(new List<int>()
			{
				ItemID.CobaltOre,
				ItemID.PalladiumOre,
				ItemID.MythrilOre,
				ItemID.OrichalcumOre,
				ItemID.AdamantiteOre,
				ItemID.TitaniumOre,
			});

			foreach (AltOre o in AltLibrary.Ores.Where(x => x.OreType >= OreType.Cobalt && x.OreType <= OreType.Adamantite || x.IncludeInChloroExtractinator))
				HardmodeOres.Add(o.ore);

			EditsHelper.IL<Player>(nameof(Player.ExtractinatorUse), Player_ExtractinatorUse);
		}

		internal static void Unload()
		{
			Gems = null;
			HardmodeOres = null;
			Ores = null;
		}

		private static void Player_ExtractinatorUse(ILContext il)
		{
			ILCursor c = new(il);

			try
			{
				c.GotoNext(MoveType.After, i => i.MatchLdcI4(ItemID.Diamond),
					i => i.MatchStloc(7),
					i => i.MatchBr(out _));

				c.Emit(OpCodes.Nop);
				c.EmitDelegate(() => Main.rand.Next(Gems));
				c.Emit(OpCodes.Stloc, 7);

				c.GotoNext(MoveType.After, i => i.MatchLdcI4(1106),
					i => i.MatchStloc(7),
					i => i.MatchBr(out _));

				c.Emit(OpCodes.Nop);
				c.EmitDelegate(() => Main.rand.Next(HardmodeOres));
				c.Emit(OpCodes.Stloc, 7);

				c.GotoNext(MoveType.After, i => i.MatchLdcI4(702),
					i => i.MatchStloc(7),
					i => i.MatchBr(out _));

				c.Emit(OpCodes.Nop);
				c.EmitDelegate(() => Main.rand.Next(Ores));
				c.Emit(OpCodes.Stloc, 7);
			}
			catch
			{
			}
		}
	}
}
