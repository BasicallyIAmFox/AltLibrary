using AltLibrary.Common.Conversion;
using AltLibrary.Common.Solutions;
using Terraria;
using Terraria.ID;

namespace AltLibrary.Content.Solutions;

// Last update: 1.4.4.9
public sealed class BrownSolution : ModSolution {
	public override void SetStaticDefaults() {
		Conversion
			.From(TileID.Sets.Conversion.Stone)
			.From(TileID.Sets.Conversion.Ice)
			.From(TileID.Sets.Conversion.Sandstone)
			.To(TileID.Stone)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.GolfGrass)
			.To(TileID.GolfGrass)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Grass)
			.To(TileID.Grass)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Sand)
			.From(TileID.Sets.Conversion.HardenedSand)
			.From(TileID.Sets.Conversion.Snow)
			.From(TileID.Sets.Conversion.Dirt)
			.BeforeConversion((Tile tile, int i, int j) => {
				int newFloorType = TileID.Dirt;
				if (WorldGen.TileIsExposedToAir(i, j)) {
					newFloorType = TileID.Grass;
				}
				WorldGen.TryKillingTreesAboveIfTheyWouldBecomeInvalid(i, j, newFloorType);
				tile.TileType = (ushort)newFloorType;

				SquareFrameTile(i, j);
				return ConversionRunCodeValues.DontRun;
			})
			.RegisterTile()

			.From(TileID.Sets.Conversion.Thorn)
			.To(ConversionHandler.Break)
			.RegisterTile()

			.From(WallID.Sets.Conversion.Stone)
			.From(WallID.Sets.Conversion.Ice)
			.From(WallID.Sets.Conversion.Sandstone)
			.To(WallID.Stone)
			.RegisterWall()

			.From(WallID.Sets.Conversion.HardenedSand)
			.From(WallID.Sets.Conversion.Snow)
			.From(WallID.Sets.Conversion.Dirt)
			.To(WallID.DirtUnsafe)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall1)
			.To(WallID.DirtUnsafe1)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall2)
			.To(WallID.DirtUnsafe2)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall3)
			.To(WallID.DirtUnsafe3)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall4)
			.To(WallID.DirtUnsafe4)
			.RegisterWall();
	}
}
