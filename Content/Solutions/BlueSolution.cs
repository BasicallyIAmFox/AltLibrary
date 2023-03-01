using AltLibrary.Common.Conversion;
using AltLibrary.Common.Solutions;
using Terraria;
using Terraria.ID;

namespace AltLibrary.Content.Solutions;

// Last update: 1.4.4.9
public sealed class BlueSolution : ModSolution {
	public override void SetStaticDefaults() {
		Conversion
			.From(Main.tileMoss)
			.From(TileID.Sets.Conversion.Stone)
			.To(117)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.GolfGrass)
			.To(492)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Grass)
			.To(109)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Ice)
			.To(164)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Sand)
			.To(116)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.HardenedSand)
			.To(402)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Sandstone)
			.To(403)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Thorn)
			.To(ConversionHandler.Break)
			.RegisterTile()

			.From(59)
			.To(0)
			.BeforeConversion((Tile tile, int i, int j) => {
				if (Main.tile[i - 1, j].TileType == 109 || Main.tile[i + 1, j].TileType == 109 || Main.tile[i, j - 1].TileType == 109 || Main.tile[i, j + 1].TileType == 109) {
					return ConversionRunCodeValues.Run;
				}
				return ConversionRunCodeValues.DontRun;
			})
			.RegisterTile()

			.From(WallID.Sets.Conversion.Grass)
			.To(70)
			.RegisterWall()

			.From(WallID.Sets.Conversion.Stone)
			.To(28)
			.RegisterWall()

			.From(WallID.Sets.Conversion.HardenedSand)
			.To(219)
			.RegisterWall()

			.From(WallID.Sets.Conversion.Sandstone)
			.To(222)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall1)
			.To(200)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall2)
			.To(201)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall3)
			.To(202)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall4)
			.To(203)
			.RegisterWall();
	}
}
