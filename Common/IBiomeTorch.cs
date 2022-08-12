using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common
{
	[Obsolete]
	public interface IBiomeTorch // interface for ModItem
	{
		bool InWhatBiome(Player player);

		BiomeTorchPriority Priority { get; }
	}

	[Obsolete]
	public enum BiomeTorchPriority : int
	{
		Low,

		Dungeon,
		Demon,
		Hallowed,
		Corrupt,
		Crimson,
		Ice,
		Jungle,
		Desert,

		High,
		VeryHigh,
	}

	[Obsolete]
	public struct BiomeTorchTile
	{
		public BiomeTorchPriority Priority = BiomeTorchPriority.High;

		public ushort tile = TileID.Torches;
		public int item = ItemID.Torch;
		public int style = 0;
		public Func<Player, bool> check = (p) => true;

		public BiomeTorchTile()
		{
		}
	}

	internal static class BiomeTorchIL
	{
		internal static void Init()
		{
			//IL.Terraria.Player.PlaceThing_Tiles_PlaceIt += Player_PlaceThing_Tiles_PlaceIt;
			//IL.Terraria.Player.ItemCheck_EmitHeldItemLight += Player_ItemCheck_EmitHeldItemLight;
		}

		internal static void Uninit()
		{
			//IL.Terraria.Player.PlaceThing_Tiles_PlaceIt -= Player_PlaceThing_Tiles_PlaceIt;
			//IL.Terraria.Player.ItemCheck_EmitHeldItemLight -= Player_ItemCheck_EmitHeldItemLight;
		}

		// tile id, item id, style
		public static (int, int, int) BiomeTorchPlacing(Player player, Item selected)
		{
			int tile = selected.createTile;
			int item = selected.type;
			int style = selected.placeStyle;

			if (!player.UsingBiomeTorches)
				goto result;

			foreach (BiomeTorchTile t in AltLibrary.BiomeTorchModItems)
			{
				if (t.check(player))
				{
					tile = t.tile;
					item = t.item;
					style = t.style;
				}
			}

			result:
			return (tile, item, style);
		}

		private static void Player_ItemCheck_EmitHeldItemLight(ILContext il)
		{
			ILCursor c = new(il);
			try
			{
				c.GotoNext(MoveType.After,
					i => i.MatchLdfld<Item>(nameof(Item.type)),
					i => i.MatchCall<Player>(nameof(Player.BiomeTorchHoldStyle)),
					i => i.MatchStloc(7));

				c.Emit(OpCodes.Ldarg, 0);
				c.Emit(OpCodes.Ldarg, 1);
				c.EmitDelegate<Func<Player, Item, int>>((Player, item) => BiomeTorchPlacing(Player, item).Item2);
				c.Emit(OpCodes.Stloc, 7);
			}
			catch (Exception e)
			{
				AltLibrary.Instance.Logger.Error($"[Biome Torches Held]\n{e.Message}\n{e.StackTrace}");
			}
		}

		private static void Player_PlaceThing_Tiles_PlaceIt(ILContext il)
		{
			ILCursor c = new(il);
			try
			{
				ILLabel label = c.DefineLabel();

				// go right before:
				// if (UsingBiomeTorches && inventory[selectedItem].createTile == 4 && num == 0)
				// and replace all further behavior with our own
				c.GotoNext(MoveType.Before,
					i => i.MatchLdarg(0),
					i => i.MatchCall<Player>("get_UsingBiomeTorches"),
					i => i.MatchBrfalse(out _));

				c.Emit(OpCodes.Ldarg, 0);
				c.Emit(OpCodes.Ldloc, 0);
				c.EmitDelegate<Func<Player, int, int>>((Player, style) =>
				{
					if (Player.UsingBiomeTorches && style == 0)
						return BiomeTorchPlacing(Player, Player.inventory[Player.selectedItem]).Item3;

					return style;
				});
				c.Emit(OpCodes.Stloc, 0);

				c.Emit(OpCodes.Ldarg, 0);
				c.Emit(OpCodes.Ldloc, 0);
				c.Emit(OpCodes.Ldloc, 2);
				c.EmitDelegate<Func<Player, int, bool, bool>>((Player, style, forced) =>
				{
					int tile = Player.inventory[Player.selectedItem].createTile;

					if (Player.UsingBiomeTorches)
						tile = BiomeTorchPlacing(Player, Player.inventory[Player.selectedItem]).Item1;

					return WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY, tile, mute: false, forced, Player.whoAmI, style);
				});
				c.Emit(OpCodes.Stloc, 3);

				c.Emit(OpCodes.Br, label);

				c.GotoNext(MoveType.After,
					i => i.MatchCall<WorldGen>(nameof(WorldGen.PlaceTile)),
					i => i.MatchStloc(3));

				c.MarkLabel(label);
			}
			catch (Exception e)
			{
				AltLibrary.Instance.Logger.Error($"[Biome Torches Placing]\n{e.Message}\n{e.StackTrace}");
			}
		}
	}
}
