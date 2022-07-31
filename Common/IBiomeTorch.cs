using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.ID;

namespace AltLibrary.Common
{
	public interface IBiomeTorch // interface for ModItem
	{
		bool InWhatBiome(Player player);
	}

	internal static class BiomeTorchIL
	{
		internal static void Init()
		{
			IL.Terraria.Player.PlaceThing_Tiles_PlaceIt += Player_PlaceThing_Tiles_PlaceIt;
			IL.Terraria.Player.ItemCheck_EmitHeldItemLight += Player_ItemCheck_EmitHeldItemLight;
		}

		internal static void Uninit()
		{
			IL.Terraria.Player.PlaceThing_Tiles_PlaceIt -= Player_PlaceThing_Tiles_PlaceIt;
			IL.Terraria.Player.ItemCheck_EmitHeldItemLight -= Player_ItemCheck_EmitHeldItemLight;
		}

		// tile id, item id, style
		public static (int, int, int) BiomeTorchPlacing(Player player, Item selected)
		{
			int tile = selected.createTile;
			int item = selected.type;
			int style = selected.placeStyle;

			if (!player.UsingBiomeTorches || selected.placeStyle != 8)
				goto result;

			if (player.ZoneDungeon)
			{
				tile = TileID.Torches;
				item = ItemID.BoneTorch;
				style = 13;
			}
			else if (player.position.Y > Main.UnderworldLayer * 16)
			{
				tile = TileID.Torches;
				item = ItemID.DemonTorch;
				style = 7;
			}
			else if (player.ZoneHallow)
			{
				tile = TileID.Torches;
				item = ItemID.HallowedTorch;
				style = 20;
			}
			else if (player.ZoneCorrupt)
			{
				tile = TileID.Torches;
				item = ItemID.CorruptTorch;
				style = 18;
			}
			else if (player.ZoneCrimson)
			{
				tile = TileID.Torches;
				item = ItemID.CrimsonTorch;
				style = 19;
			}
			else if (player.ZoneSnow)
			{
				tile = TileID.Torches;
				item = ItemID.IceTorch;
				style = 9;
			}
			else if (player.ZoneJungle)
			{
				tile = TileID.Torches;
				item = ItemID.JungleTorch;
				style = 21;
			}
			else if ((player.ZoneDesert && player.position.Y < Main.worldSurface * 16.0) || player.ZoneUndergroundDesert)
			{
				tile = TileID.Torches;
				item = ItemID.DesertTorch;
				style = 16;
			}

			if (selected.ModItem is not null && selected.ModItem is IBiomeTorch torch && torch.InWhatBiome(player))
			{
				tile = selected.createTile;
				item = selected.type;
				style = selected.placeStyle;
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
				c.Emit(OpCodes.Ldloc, 7);
				c.EmitDelegate<Func<Player, Item, int, int>>((Player, item, heldType) => BiomeTorchPlacing(Player, item).Item2);
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
