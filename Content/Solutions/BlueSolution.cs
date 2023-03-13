using AltLibrary.Common.Conversion;
using AltLibrary.Common.Solutions;
using AltLibrary.Core;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace AltLibrary.Content.Solutions;

// Last update: 1.4.4.9
public sealed class BlueSolution : ModSolution {
	public override void SetStaticDefaults() {
		Conversion
			.From(Main.tileMoss)
			.From(TileID.Sets.Conversion.Stone)
			.To(TileID.Pearlstone)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.GolfGrass)
			.To(TileID.GolfGrassHallowed)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Grass)
			.To(TileID.HallowedGrass)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Ice)
			.To(TileID.HallowedIce)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Sand)
			.To(TileID.Pearlsand)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.HardenedSand)
			.To(TileID.HallowHardenedSand)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Sandstone)
			.To(TileID.HallowSandstone)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Thorn)
			.To(ConversionHandler.Break)
			.RegisterTile()

			.From(TileID.Mud)
			.To(TileID.Dirt)
			.BeforeConversion((Tile tile, int i, int j) => {
				const ushort hallowedGrassId = TileID.HallowedGrass;
				if (TileScanner.ScanCircleTile(new(i, j), 1, hallowedGrassId)) {
					return ConversionRunCodeValues.Run;
				}
				return ConversionRunCodeValues.DontRun;
			})
			.RegisterTile()

			.From(WallID.Sets.Conversion.Grass)
			.To(WallID.HallowedGrassUnsafe)
			.RegisterWall()

			.From(WallID.Sets.Conversion.Stone)
			.To(WallID.PearlstoneBrickUnsafe)
			.RegisterWall()

			.From(WallID.Sets.Conversion.HardenedSand)
			.To(WallID.HallowHardenedSand)
			.RegisterWall()

			.From(WallID.Sets.Conversion.Sandstone)
			.To(WallID.HallowSandstone)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall1)
			.To(WallID.HallowUnsafe1)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall2)
			.To(WallID.HallowUnsafe2)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall3)
			.To(WallID.HallowUnsafe3)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall4)
			.To(WallID.HallowUnsafe4)
			.RegisterWall();
	}
}
