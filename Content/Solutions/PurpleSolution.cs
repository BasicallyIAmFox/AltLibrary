using AltLibrary.Common.Solutions;
using Terraria;
using Terraria.ID;

namespace AltLibrary.Content.Solutions;

// Last update: 1.4.4.9
public sealed class PurpleSolution : ModSolution {
	public override void SetStaticDefaults() {
		Conversion
			.From(TileID.Sets.Conversion.JungleGrass)
			.To(TileID.CorruptJungleGrass)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(Main.tileMoss)
			.From(TileID.Sets.Conversion.Stone)
			.To(TileID.Ebonstone)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Grass)
			.To(TileID.CorruptGrass)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Ice)
			.To(TileID.CorruptIce)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Sand)
			.To(TileID.Ebonsand)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.HardenedSand)
			.To(TileID.CorruptHardenedSand)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Sandstone)
			.To(TileID.CorruptSandstone)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Thorn)
			.To(TileID.CorruptThorns)
			.RegisterTile()

			.From(WallID.Sets.Conversion.Grass)
			.To(WallID.CorruptGrassUnsafe)
			.RegisterWall()

			.From(WallID.Sets.Conversion.Stone)
			.To(WallID.EbonstoneUnsafe)
			.RegisterWall()

			.From(WallID.Sets.Conversion.HardenedSand)
			.To(WallID.CorruptHardenedSand)
			.RegisterWall()

			.From(WallID.Sets.Conversion.Sandstone)
			.To(WallID.CorruptSandstone)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall1)
			.To(WallID.CorruptionUnsafe1)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall2)
			.To(WallID.CorruptionUnsafe2)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall3)
			.To(WallID.CorruptionUnsafe3)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall4)
			.To(WallID.CorruptionUnsafe4)
			.RegisterWall();
	}
}
