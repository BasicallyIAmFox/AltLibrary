using AltLibrary.Common.Conversion;
using AltLibrary.Common.Solutions;
using Terraria;
using Terraria.ID;

namespace AltLibrary.Content.Solutions;

// Last update: 1.4.4.9
public sealed class GreenSolution : ModSolution {
	public override void SetStaticDefaults() {
		Conversion
			.From(TileID.Sets.Conversion.GolfGrass) // TileID.GolfGrassHallowed
			.To(TileID.GolfGrass)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.JungleGrass)
			.From(TileID.Sets.Conversion.MushroomGrass)
			.To(TileID.JungleGrass)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Grass)
			.To(TileID.Grass)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Stone)
			.To(TileID.Stone)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Sand)
			.To(TileID.Sand)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.HardenedSand)
			.To(TileID.HardenedSand)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Sandstone)
			.To(TileID.Sandstone)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Ice)
			.To(TileID.IceBlock)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.CorruptThorns)
			.From(TileID.CrimsonThorns)
			.To(ConversionHandler.Break)
			.RegisterTile()

			.From(WallID.CorruptGrassUnsafe)
			.From(WallID.HallowedGrassUnsafe)
			.From(WallID.CrimsonGrassUnsafe)
			.BeforeConversion((Tile tile, int i, int j) => {
				if (j >= Main.worldSurface) {
					tile.WallType = WallID.JungleUnsafe;
				}
				else if (WorldGen.genRand.NextBool(10)) {
					tile.WallType = WallID.FlowerUnsafe;
				}
				else {
					tile.WallType = WallID.GrassUnsafe;
				}
				SquareFrameTile(i, j);
				return ConversionRunCodeValues.DontRun;
			})
			.RegisterWall()

			.From(WallID.Sets.Conversion.Stone)
			.To(WallID.Stone)
			.RegisterWall()

			.From(WallID.Cave7Echo)
			.To(WallID.Cave7Unsafe)
			.RegisterWall()

			.From(WallID.Cave8Echo)
			.To(WallID.Cave8Unsafe)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall1)
			.To(WallID.RocksUnsafe1)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall2)
			.To(WallID.RocksUnsafe2)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall3)
			.To(WallID.RocksUnsafe3)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall4)
			.To(WallID.RocksUnsafe4)
			.RegisterWall()

			.From(WallID.Sets.CanBeConvertedToGlowingMushroom)
			.BeforeConversion((Tile tile, int i, int j) => {
				if (j < Main.worldSurface + 4.0 + WorldGen.genRand.Next(3) || j > (Main.maxTilesY + Main.rockLayer) / 2.0 - 3.0 + WorldGen.genRand.Next(3)) {
					tile.WallType = WallID.MudUnsafe;
				}
				else {
					tile.WallType = WallID.JungleUnsafe;
				}

				SquareFrameTile(i, j);
				return ConversionRunCodeValues.DontRun;
			})
			.RegisterWall()

			.From(WallID.Sets.Conversion.HardenedSand)
			.To(WallID.HardenedSand)
			.RegisterWall()

			.From(WallID.Sets.Conversion.Sandstone)
			.To(WallID.Sandstone)
			.RegisterWall();
	}
}
