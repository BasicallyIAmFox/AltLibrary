using AltLibrary.Common.Solutions;
using Terraria;
using Terraria.ID;

namespace AltLibrary.Content.Solutions;

// Last update: 1.4.4.9
public sealed class PurpleSolution : ModSolution {
	public override void SetStaticDefaults() {
		Conversion
			.From(TileID.Sets.Conversion.JungleGrass)
			.To(661)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(Main.tileMoss)
			.From(TileID.Sets.Conversion.Stone)
			.To(25)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Grass)
			.To(23)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Ice)
			.To(163)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Sand)
			.To(112)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.HardenedSand)
			.To(398)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Sandstone)
			.To(400)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Thorn)
			.To(32)
			.RegisterTile()

			.From(WallID.Sets.Conversion.Grass)
			.To(69)
			.RegisterWall()

			.From(WallID.Sets.Conversion.Stone)
			.To(3)
			.RegisterWall()

			.From(WallID.Sets.Conversion.HardenedSand)
			.To(217)
			.RegisterWall()

			.From(WallID.Sets.Conversion.Sandstone)
			.To(220)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall1)
			.To(188)
			.RegisterWall()
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall2)
			.To(189)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall3)
			.To(190)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall4)
			.To(191)
			.RegisterWall();
	}
}
