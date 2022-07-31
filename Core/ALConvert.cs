using AltLibrary.Common.AltBiomes;
using AltLibrary.Core.Baking;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Core
{
	public static class ALConvert
	{
		internal static void Load()
		{
			On.Terraria.WorldGen.Convert += WorldGen_Convert;
		}

		internal static void Unload()
		{
			On.Terraria.WorldGen.Convert -= WorldGen_Convert;
		}

		private static void WorldGen_Convert(On.Terraria.WorldGen.orig_Convert orig, int i, int j, int conversionType, int size)
		{
			for (int k = i - size; k <= i + size; k++)
			{
				for (int l = j - size; l <= j + size; l++)
				{
					if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < 6)
					{
						Tile tile = Main.tile[k, l];
						int newTile = ALConvertInheritanceData.GetConvertedTile_Vanilla(tile.TileType, conversionType, k, l);
						if (newTile == -2)
						{
							WorldGen.KillTile(k, l, false, false, false);
							if (Main.netMode == NetmodeID.MultiplayerClient)
							{
								NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, k, l, 0f, 0, 0, 0);
							}
						}
						else if (newTile != -1 && newTile != tile.TileType)
						{
							tile.TileType = (ushort)newTile;

							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
						}

						int newWall = ALConvertInheritanceData.GetConvertedWall_Vanilla(tile.WallType, conversionType, k, l);

						if (newWall == -2)
						{
							WorldGen.KillWall(k, l, false);
							if (Main.netMode == NetmodeID.MultiplayerClient)
							{
								NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, k, l, 0f, 0, 0, 0);
							}
						}
						else if (newWall != -1 && newWall != tile.WallType)
						{
							tile.WallType = (ushort)newWall;

							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
						}

						//Hardcoded. Cannot eradicate.
						int corruptGrass;
						switch (conversionType)
						{
							case 1:
								corruptGrass = TileID.CorruptGrass;
								break;
							case 2:
								corruptGrass = TileID.HallowedGrass;
								break;
							case 4:
								corruptGrass = TileID.CrimsonGrass;
								break;
							default:
								corruptGrass = 0;
								break;
						}
						if (corruptGrass != 0)
							if (tile.TileType == 59 && (Main.tile[k - 1, l].TileType == corruptGrass || Main.tile[k + 1, l].TileType == corruptGrass || Main.tile[k, l - 1].TileType == corruptGrass || Main.tile[k, l + 1].TileType == corruptGrass))
							{
								Main.tile[k, l].TileType = 0;
								WorldGen.SquareTileFrame(k, l, true);
								NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
							}
					}
				}
			}
			return;
		}

		/// <summary>
		/// Makes throwing water converting effect.
		/// </summary>
		/// <param name="projectile"></param>
		/// <param name="fullName"></param>
		public static void SimulateThrownWater(Projectile projectile, string fullName)
		{
			int i = (int)(projectile.position.X + projectile.width / 2) / 16;
			int j = (int)(projectile.position.Y + projectile.height / 2) / 16;
			Convert(fullName, i, j, 4);
		}

		/// <summary>
		/// Makes throwing water converting effect.
		/// </summary>
		/// <param name="projectile"></param>
		/// <param name="mod"></param>
		/// <param name="name"></param>
		public static void SimulateThrownWater(Projectile projectile, Mod mod, string name)
		{
			int i = (int)(projectile.position.X + projectile.width / 2) / 16;
			int j = (int)(projectile.position.Y + projectile.height / 2) / 16;
			Convert(mod, name, i, j, 4);
		}

		/// <summary>
		/// Makes throwing water converting effect.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="projectile"></param>
		public static void SimulateThrownWater<T>(Projectile projectile) where T : AltBiome
		{
			int i = (int)(projectile.position.X + projectile.width / 2) / 16;
			int j = (int)(projectile.position.Y + projectile.height / 2) / 16;
			Convert<T>(i, j, 4);
		}

		/// <summary>
		/// Makes throwing water converting effect.
		/// </summary>
		/// <param name="projectile"></param>
		/// <param name="biome"></param>
		public static void SimulateThrownWater(Projectile projectile, AltBiome biome)
		{
			int i = (int)(projectile.position.X + projectile.width / 2) / 16;
			int j = (int)(projectile.position.Y + projectile.height / 2) / 16;
			Convert(biome, i, j, 4);
		}

		/// <summary>
		/// Makes solution converting effect.
		/// </summary>
		/// <param name="projectile"></param>
		/// <param name="mod"></param>
		/// <param name="name"></param>
		public static void SimulateSolution(Projectile projectile, Mod mod, string name) => SimulateSolution(projectile, AltLibrary.Biomes.Find(x => x.Mod == mod && x.Name == name));

		/// <summary>
		/// Makes solution converting effect.
		/// </summary>
		/// <param name="projectile"></param>
		/// <param name="fullname"></param>
		public static void SimulateSolution(Projectile projectile, string fullname) => SimulateSolution(projectile, AltLibrary.Biomes.Find(x => x.FullName == fullname));

		/// <summary>
		/// Makes solution converting effect.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="projectile"></param>
		public static void SimulateSolution<T>(Projectile projectile) where T : AltBiome => SimulateSolution(projectile, ContentInstance<T>.Instance);

		/// <summary>
		/// Makes solution converting effect.
		/// </summary>
		/// <param name="projectile"></param>
		/// <param name="biome"></param>
		public static void SimulateSolution(Projectile projectile, AltBiome biome)
		{
			Convert(biome, (int)(projectile.position.X + projectile.width / 2) / 16, (int)(projectile.position.Y + projectile.height / 2) / 16, 2);
		}

		public static void Convert<T>(int i, int j, int size = 4) where T : AltBiome => Convert(ContentInstance<T>.Instance, i, j, size);

		public static void Convert(string fullName, int i, int j, int size = 4) => Convert(AltLibrary.Biomes.Find(x => x.FullName == fullName), i, j, size);

		public static void Convert(Mod mod, string name, int i, int j, int size = 4) => Convert(AltLibrary.Biomes.Find(x => x.Mod == mod && x.Name == name), i, j, size);

		public static void Convert(AltBiome biome, int i, int j, int size = 4)
		{
			if (biome is null)
				throw new ArgumentNullException(nameof(biome), "Can't be null!");
			for (int k = i - size; k <= i + size; k++)
			{
				for (int l = j - size; l <= j + size; l++)
				{
					if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < 6)
					{
						Tile tile = Main.tile[k, l];
						int newTile = ALConvertInheritanceData.GetConvertedTile_Modded(tile.TileType, biome, k, l);
						if (newTile == -2)
						{
							WorldGen.KillTile(k, l, false, false, false);
							if (Main.netMode == NetmodeID.MultiplayerClient)
							{
								NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, k, l, 0f, 0, 0, 0);
							}
						}
						else if (newTile != -1 && newTile != tile.TileType)
						{
							tile.TileType = (ushort)newTile;

							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
						}

						int newWall = ALConvertInheritanceData.GetConvertedWall_Modded(tile.WallType, biome, k, l);
						if (newWall == -2)
						{
							WorldGen.KillWall(k, l, false);
							if (Main.netMode == NetmodeID.MultiplayerClient)
							{
								NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, k, l, 0f, 0, 0, 0);
							}
						}
						else if (newWall != -1 && newWall != tile.WallType)
						{
							tile.WallType = (ushort)newWall;

							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
						}

						//Hardcoded. Cannot do anything about this.
						if (biome.MudToDirt && tile.TileType == TileID.Mud && (Main.tile[k - 1, l].TileType == biome.BiomeGrass || Main.tile[k + 1, l].TileType == biome.BiomeGrass || Main.tile[k, l - 1].TileType == biome.BiomeGrass || Main.tile[k, l + 1].TileType == biome.BiomeGrass))
						{
							Main.tile[k, l].TileType = TileID.Dirt;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
						}
					}
				}
			}
			return;
		}
	}
}
