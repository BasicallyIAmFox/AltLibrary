using AltLibrary.Common.Conversion;
using AltLibrary.Common.Solutions;
using Terraria;
using Terraria.ID;

namespace AltLibrary.Content.Solutions;

// Last update: 1.4.4.9
public sealed class GreenSolution : ModSolution {
	public override void SetStaticDefaults() {
		Conversion
			.From(492)
			.To(477)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.JungleGrass)
			.From(TileID.Sets.Conversion.MushroomGrass)
			.To(60)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Grass)
			.To(2)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Stone)
			.To(1)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Sand)
			.To(53)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.HardenedSand)
			.To(397)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Sandstone)
			.To(396)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Ice)
			.To(161)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(32)
			.From(352)
			.To(ConversionHandler.Break)
			.RegisterTile()

			.From(69)
			.From(70)
			.From(81)
			.BeforeConversion((Tile tile, int i, int j) => {
				tile.WallType = j >= Main.worldSurface ? (ushort)64 : (!WorldGen.genRand.NextBool(10) ? (ushort)63 : (ushort)65);
				return ConversionRunCodeValues.DontRun;
			})
			.RegisterWall()

			.From(WallID.Sets.Conversion.Stone)
			.To(1)
			.RegisterWall()

			.From(262)
			.To(61)
			.RegisterWall()

			.From(274)
			.To(185)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall1)
			.To(212)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall2)
			.To(213)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall3)
			.To(214)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall4)
			.To(215)
			.RegisterWall()

			.From(80)
			.BeforeConversion((Tile tile, int i, int j) => {
				if (j < Main.worldSurface + 4.0 + WorldGen.genRand.Next(3) || j > (Main.maxTilesY + Main.rockLayer) / 2.0 - 3.0 + WorldGen.genRand.Next(3)) {
					tile.WallType = 15;
				}
				else {
					tile.WallType = 64;
				}
				return ConversionRunCodeValues.DontRun;
			})
			.RegisterWall()

			.From(WallID.Sets.Conversion.HardenedSand)
			.To(216)
			.RegisterWall()

			.From(WallID.Sets.Conversion.Sandstone)
			.To(187)
			.RegisterWall();
	}
}
