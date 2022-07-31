using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using AltLibrary.Core.Baking;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace AltLibrary.Common.Hooks
{
	internal static class HardmodeWorldGen
	{
		public static void Init()
		{
			IL.Terraria.WorldGen.smCallBack += GenPasses.ILSMCallBack;
			IL.Terraria.WorldGen.GERunner += WorldGen_GERunner;
			On.Terraria.WorldGen.GERunner += WorldGen_GERunner1;
			GenPasses.HookGenPassHardmodeWalls += GenPasses_HookGenPassHardmodeWalls;
		}

		public static void Unload()
		{
			IL.Terraria.WorldGen.smCallBack -= GenPasses.ILSMCallBack;
			IL.Terraria.WorldGen.GERunner -= WorldGen_GERunner;
			On.Terraria.WorldGen.GERunner -= WorldGen_GERunner1;
			GenPasses.HookGenPassHardmodeWalls -= GenPasses_HookGenPassHardmodeWalls;
		}

		private static void WorldGen_GERunner1(On.Terraria.WorldGen.orig_GERunner orig, int i, int j, float speedX, float speedY, bool good)
		{
			if (Main.drunkWorld && WorldBiomeGeneration.WofKilledTimes > 1)
			{
				if (good)
				{
					List<int> possibles = new() { 0 };
					AltLibrary.Biomes.FindAll(x => x.BiomeType == BiomeType.Hallow).ForEach(x => possibles.Add(x.Type));
					if (AltLibraryServerConfig.Config.HardmodeGenRandom)
						AltLibrary.Biomes.FindAll(x => x.BiomeType == BiomeType.Jungle).ForEach(x => possibles.Add(x.Type));
					WorldBiomeManager.drunkGoodGen = Main.rand.Next(possibles);
					possibles = new()
					{
						0,
						-1
					};
					AltLibrary.Biomes.FindAll(x => x.BiomeType == BiomeType.Evil).ForEach(x => possibles.Add(x.Type));
					if (AltLibraryServerConfig.Config.HardmodeGenRandom)
						AltLibrary.Biomes.FindAll(x => x.BiomeType == BiomeType.Hell).ForEach(x => possibles.Add(x.Type));
					WorldBiomeManager.drunkEvilGen = Main.rand.Next(possibles);
				}

				int addX = WorldGen.genRand.Next(300, 400) * WorldBiomeGeneration.WofKilledTimes;
				if (!good) addX *= -1;
				i += addX;
				if (i < 0)
				{
					i *= -1;
				}
				if (i > Main.maxTilesX)
				{
					i %= Main.maxTilesX;
				}
			}
			orig(i, j, speedX, speedY, good);
		}

		private static void GenPasses_HookGenPassHardmodeWalls(ILContext il)
		{
			ILCursor c = new(il);
			if (!c.TryGotoNext(i => i.MatchStloc(8)))
			{
				AltLibrary.Instance.Logger.Info("h $ 1");
				return;
			}
			if (!c.TryGotoNext(i => i.MatchLdloc(4)))
			{
				AltLibrary.Instance.Logger.Info("h $ 2");
				return;
			}
			c.Index++;
			c.Emit(OpCodes.Pop);
			c.Emit(OpCodes.Ldloc, 7);
			c.Emit(OpCodes.Ldloc, 5);
			c.EmitDelegate<Func<int, Tile, int>>((orig, tile) =>
			{
				if (WorldBiomeGeneration.WofKilledTimes <= 1)
				{
					if (WorldBiomeManager.WorldHallow != "" &&
						Find<AltBiome>(WorldBiomeManager.WorldHallow).HardmodeWalls.Count > 0 &&
						((Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeGrass.HasValue &&
							tile.TileType == Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeGrass.Value) ||
						(Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeStone.HasValue &&
							tile.TileType == Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeStone.Value) ||
						(Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeSand.HasValue &&
							tile.TileType == Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeSand.Value) ||
						(Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeIce.HasValue &&
							tile.TileType == Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeIce.Value) ||
						(Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeHardenedSand.HasValue &&
							tile.TileType == Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeHardenedSand.Value) ||
						(Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeSandstone.HasValue &&
							tile.TileType == Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeSandstone.Value))
						// Vine here!
						)
					{
						orig = WorldGen.genRand.Next(Find<AltBiome>(WorldBiomeManager.WorldHallow).HardmodeWalls);
					}
					if (WorldBiomeManager.WorldEvil != "" &&
						Find<AltBiome>(WorldBiomeManager.WorldEvil).HardmodeWalls.Count > 0 &&
						((Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeGrass.HasValue &&
							tile.TileType == Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeGrass.Value) ||
						(Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeStone.HasValue &&
							tile.TileType == Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeStone.Value) ||
						(Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeSand.HasValue &&
							tile.TileType == Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeSand.Value) ||
						(Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeIce.HasValue &&
							tile.TileType == Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeIce.Value) ||
						(Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeHardenedSand.HasValue &&
							tile.TileType == Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeHardenedSand.Value) ||
						(Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeSandstone.HasValue &&
							tile.TileType == Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeSandstone.Value))
						// Vine here!
						)
					{
						orig = WorldGen.genRand.Next(Find<AltBiome>(WorldBiomeManager.WorldEvil).HardmodeWalls);
					}
				}
				else
				{
					if (Main.drunkWorld)
					{
						if (Evil?.HardmodeWalls.Count > 0 &&
							(tile.TileType == Evil?.BiomeGrass.GetValueOrDefault() ||
							tile.TileType == Evil?.BiomeStone.GetValueOrDefault() ||
							tile.TileType == Evil?.BiomeSand.GetValueOrDefault() ||
							tile.TileType == Evil?.BiomeIce.GetValueOrDefault() ||
							tile.TileType == Evil?.BiomeHardenedSand.GetValueOrDefault() ||
							tile.TileType == Evil?.BiomeSandstone.GetValueOrDefault()))
						{
							orig = WorldGen.genRand.Next(Evil?.HardmodeWalls);
						}
						if (Good?.HardmodeWalls.Count > 0 &&
							(tile.TileType == Good?.BiomeGrass.GetValueOrDefault() ||
							tile.TileType == Good?.BiomeStone.GetValueOrDefault() ||
							tile.TileType == Good?.BiomeSand.GetValueOrDefault() ||
							tile.TileType == Good?.BiomeIce.GetValueOrDefault() ||
							tile.TileType == Good?.BiomeHardenedSand.GetValueOrDefault() ||
							tile.TileType == Good?.BiomeSandstone.GetValueOrDefault()))
						{
							orig = WorldGen.genRand.Next(Good?.HardmodeWalls);
						}
					}
				}
				return orig;
			});
			c.Emit(OpCodes.Stloc, 7);
			c.Emit(OpCodes.Ldloc, 4);
		}

		private static int GetTileOnStateHallow(int tileID, int x, int y)
		{
			int rv = ALConvertInheritanceData.GetConvertedTile_Vanilla(tileID, 2, x, y);
			if (WorldBiomeManager.WorldHallow != "" && WorldBiomeGeneration.WofKilledTimes <= 1)
				rv = ALConvertInheritanceData.GetConvertedTile_Modded(tileID, Find<AltBiome>(WorldBiomeManager.WorldHallow), x, y);
			if (WorldBiomeManager.drunkGoodGen > 0)
				rv = ALConvertInheritanceData.GetConvertedTile_Modded(tileID, Good, x, y);
			if (rv == -1)
				return tileID;
			else if (rv == -2)
				return 0;
			return rv;
		}

		private static int GetTileOnStateEvil(int tileID, int x, int y)
		{
			int rv = ALConvertInheritanceData.GetConvertedTile_Vanilla(tileID, WorldBiomeGeneration.WofKilledTimes <= 1 ? (!WorldGen.crimson ? 1 : 4) : (WorldBiomeManager.drunkEvilGen == 0 ? 1 : 4), x, y);
			if (WorldBiomeManager.WorldEvil != "" && WorldBiomeGeneration.WofKilledTimes <= 1)
				rv = ALConvertInheritanceData.GetConvertedTile_Modded(tileID, Find<AltBiome>(WorldBiomeManager.WorldEvil), x, y);
			if (WorldBiomeManager.drunkEvilGen > 0)
				rv = ALConvertInheritanceData.GetConvertedTile_Modded(tileID, Evil, x, y);
			if (rv == -1)
				return tileID;
			else if (rv == -2)
				return 0;
			return rv;
		}

		private static int GetWallOnStateHallow(int wallID, int x, int y)
		{
			if (WorldBiomeManager.drunkGoodGen > 0)
				return ALConvertInheritanceData.GetConvertedWall_Modded(wallID, Good, x, y);
			return ALConvertInheritanceData.GetConvertedWall_Vanilla(wallID, 2, x, y);
		}

		private static int GetWallOnStateEvil(int wallID, int x, int y)
		{
			if (WorldBiomeManager.drunkEvilGen > 0)
				return ALConvertInheritanceData.GetConvertedWall_Modded(wallID, Evil, x, y);
			return ALConvertInheritanceData.GetConvertedWall_Vanilla(wallID, WorldBiomeManager.drunkEvilGen == 0 ? (!WorldGen.crimson ? 1 : 4) : 4, x, y);
		}

		private static AltBiome Good => AltLibrary.Biomes.Find(x => x.Type == WorldBiomeManager.drunkGoodGen);

		private static AltBiome Evil => AltLibrary.Biomes.Find(x => x.Type == WorldBiomeManager.drunkEvilGen);

		private static void WorldGen_GERunner(ILContext il)
		{
			ILCursor c = new(il);
			int good = 0;
			if (!c.TryGotoNext(i => i.MatchBrfalse(out _)))
			{
				AltLibrary.Instance.Logger.Info("i $ 1");
				return;
			}
			if (!c.TryGotoNext(i => i.MatchBrfalse(out _)))
			{
				AltLibrary.Instance.Logger.Info("i $ 2");
				return;
			}
			if (!c.TryGotoPrev(i => i.MatchLdarg(out good)))
			{
				AltLibrary.Instance.Logger.Info("i $ 3");
				return;
			}
			if (!c.TryGotoPrev(i => i.MatchBgeUn(out _)))
			{
				AltLibrary.Instance.Logger.Info("i $ 4");
				return;
			}

			c.Index++;
			c.Emit(OpCodes.Ldarg, good);
			c.Emit(OpCodes.Ldloc, 15);
			c.Emit(OpCodes.Ldloc, 16);
			c.EmitDelegate<Action<bool, int, int>>((good, m, l) =>
			{
				if (!good)
				{
					Tile tile = Main.tile[m, l];
					if (WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldEvil != "")
					{
						foreach (KeyValuePair<int, int> entry in Find<AltBiome>(WorldBiomeManager.WorldEvil).SpecialConversion)
						{
							if (tile.TileType == entry.Key)
							{
								tile = Main.tile[m, l];
								tile.TileType = (ushort)entry.Value;
								WorldGen.SquareTileFrame(m, l, true);
							}
						}
						foreach (KeyValuePair<ushort, ushort> entry in Find<AltBiome>(WorldBiomeManager.WorldEvil).WallContext.wallsReplacement)
						{
							if (tile.WallType == entry.Key)
							{
								tile.WallType = entry.Value;
							}
						}
					}
					if (Main.drunkWorld && WorldBiomeGeneration.WofKilledTimes > 1)
					{
						int type = tile.TileType;
						int wall = tile.WallType;
						if (WorldGen.InWorld(m, l) && type != -1 && GetTileOnStateEvil(type, m, l) != -1 && type != GetTileOnStateEvil(type, m, l))
						{
							tile.TileType = (ushort)GetTileOnStateEvil(type, m, l);
							WorldGen.SquareTileFrame(m, l, true);
						}
						if (WorldGen.InWorld(m, l) && wall != -1 && GetWallOnStateEvil(wall, m, l) != -1 && wall != GetWallOnStateEvil(wall, m, l))
						{
							tile.WallType = (ushort)GetWallOnStateEvil(wall, m, l);
						}
					}
				}
			});

			if (!c.TryGotoNext(i => i.MatchBrfalse(out _)))
			{
				AltLibrary.Instance.Logger.Info("i $ 5");
				return;
			}

			c.Index++;
			c.Emit(OpCodes.Ldloc, 15);
			c.Emit(OpCodes.Ldloc, 16);
			c.EmitDelegate<Action<int, int>>((m, l) =>
			{
				Tile tile = Main.tile[m, l];
				if (WorldBiomeGeneration.WofKilledTimes <= 1 && WorldBiomeManager.WorldHallow != "")
				{
					foreach (KeyValuePair<int, int> entry in Find<AltBiome>(WorldBiomeManager.WorldHallow).SpecialConversion)
					{
						if (tile.TileType == entry.Key)
						{
							tile.TileType = (ushort)entry.Value;
							WorldGen.SquareTileFrame(m, l, true);
						}
					}
					foreach (KeyValuePair<ushort, ushort> entry in Find<AltBiome>(WorldBiomeManager.WorldHallow).WallContext.wallsReplacement)
					{
						if (tile.WallType == entry.Key)
						{
							tile.WallType = entry.Value;
						}
					}
				}
				if (Main.drunkWorld && WorldBiomeGeneration.WofKilledTimes > 1)
				{
					int type = tile.TileType;
					int wall = tile.WallType;
					if (WorldGen.InWorld(m, l) && type != -1 && GetTileOnStateHallow(type, m, l) != -1 && type != GetTileOnStateHallow(type, m, l))
					{
						type = (ushort)GetTileOnStateHallow(type, m, l);
						WorldGen.SquareTileFrame(m, l, true);
					}
					if (WorldGen.InWorld(m, l) && wall != -1 && GetWallOnStateHallow(wall, m, l) != -1 && wall != GetWallOnStateHallow(wall, m, l))
					{
						wall = (ushort)GetWallOnStateHallow(wall, m, l);
					}
				}
			});

			void goodWall(int id)
			{
				ILCursor c = new(il);
				while (c.TryGotoNext(i => i.MatchLdcI4(id) && i.Offset != 0))
				{
					c.Index++;
					c.Emit(OpCodes.Ldloc, 15);
					c.Emit(OpCodes.Ldloc, 16);
					c.EmitDelegate(GetWallOnStateHallow);
				}
			}
			void goodTile(int id)
			{
				ILCursor c = new(il);
				while (c.TryGotoNext(i => i.MatchLdcI4(id) && i.Offset != 0))
				{
					c.Index++;
					c.Emit(OpCodes.Ldloc, 15);
					c.Emit(OpCodes.Ldloc, 16);
					c.EmitDelegate(GetTileOnStateHallow);
				}
			}
			void evilWall(int id)
			{
				ILCursor c = new(il);
				while (c.TryGotoNext(i => i.MatchLdcI4(id) && i.Offset != 0))
				{
					c.Index++;
					c.Emit(OpCodes.Ldloc, 15);
					c.Emit(OpCodes.Ldloc, 16);
					c.EmitDelegate(GetWallOnStateEvil);
				}
			}
			void evilTile(int id)
			{
				ILCursor c = new(il);
				if (id != TileID.FleshIce)
				{
					while (c.TryGotoNext(i => i.MatchLdcI4(id) && i.Offset != 0))
					{
						c.Index++;
						c.Emit(OpCodes.Ldloc, 15);
						c.Emit(OpCodes.Ldloc, 16);
						c.EmitDelegate(GetTileOnStateEvil);
					}
				}
				else
				{
					while (c.TryGotoNext(i => !i.MatchCall<WorldGen>("get_genRand"),
						i => i.MatchLdcI4(id) && i.Offset != 0))
					{
						c.Index += 2;
						c.Emit(OpCodes.Ldloc, 15);
						c.Emit(OpCodes.Ldloc, 16);
						c.EmitDelegate(GetTileOnStateEvil);
					}
				}
			}

			goodWall(WallID.HallowedGrassUnsafe);
			goodWall(WallID.HallowHardenedSand);
			goodWall(WallID.HallowSandstone);
			goodWall(WallID.PearlstoneBrickUnsafe);

			evilWall(WallID.CorruptGrassUnsafe);
			evilWall(WallID.CorruptHardenedSand);
			evilWall(WallID.CorruptSandstone);

			evilWall(WallID.CrimsonGrassUnsafe);
			evilWall(WallID.CrimsonHardenedSand);
			evilWall(WallID.CrimsonSandstone);

			goodTile(TileID.Pearlstone);
			goodTile(TileID.HallowHardenedSand);
			goodTile(TileID.HallowedGrass);
			goodTile(TileID.Pearlsand);
			goodTile(TileID.HallowedIce);
			goodTile(TileID.HallowSandstone);

			evilTile(TileID.Ebonstone);
			evilTile(TileID.CorruptHardenedSand);
			evilTile(TileID.CorruptGrass);
			evilTile(TileID.Ebonsand);
			evilTile(TileID.CorruptIce);
			evilTile(TileID.CorruptSandstone);

			evilTile(TileID.Crimstone);
			evilTile(TileID.CrimsonHardenedSand);
			evilTile(TileID.CrimsonGrass);
			evilTile(TileID.Crimsand);
			evilTile(TileID.FleshIce);
			evilTile(TileID.CrimsonSandstone);
		}
	}
}
