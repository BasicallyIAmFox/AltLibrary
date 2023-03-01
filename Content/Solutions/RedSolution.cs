using AltLibrary.Common.Solutions;
using Terraria;
using Terraria.ID;

namespace AltLibrary.Content.Solutions;

// Last update: 1.4.4.9
public sealed class RedSolution : ModSolution {
	public override void SetStaticDefaults() {
		Conversion
			.From(Main.tileMoss)
			.From(TileID.Sets.Conversion.Stone)
			.To(203)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.JungleGrass)
			.To(662)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Grass)
			.To(199)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Ice)
			.To(200)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Sand)
			.To(234)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.HardenedSand)
			.To(399)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Sandstone)
			.To(401)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Thorn)
			.To(352)
			.RegisterTile()

			.From(WallID.Sets.Conversion.Grass)
			.To(81)
			.RegisterWall()

			.From(WallID.Sets.Conversion.Stone)
			.To(83)
			.RegisterWall()

			.From(WallID.Sets.Conversion.HardenedSand)
			.To(218)
			.RegisterWall()

			.From(WallID.Sets.Conversion.Sandstone)
			.To(221)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall1)
			.To(192)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall2)
			.To(193)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall3)
			.To(194)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall4)
			.To(195)
			.RegisterWall();
	}
}
