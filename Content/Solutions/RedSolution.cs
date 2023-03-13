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
			.To(TileID.Crimstone)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.JungleGrass)
			.To(TileID.CrimsonJungleGrass)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Grass)
			.To(TileID.CrimsonGrass)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Ice)
			.To(TileID.FleshIce)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Sand)
			.To(TileID.Crimsand)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.HardenedSand)
			.To(TileID.CrimsonHardenedSand)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Sandstone)
			.To(TileID.CrimsonSandstone)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Thorn)
			.To(TileID.CrimsonThorns)
			.RegisterTile()

			.From(WallID.Sets.Conversion.Grass)
			.To(WallID.CrimsonGrassUnsafe)
			.RegisterWall()

			.From(WallID.Sets.Conversion.Stone)
			.To(WallID.CrimstoneUnsafe)
			.RegisterWall()

			.From(WallID.Sets.Conversion.HardenedSand)
			.To(WallID.CrimsonHardenedSand)
			.RegisterWall()

			.From(WallID.Sets.Conversion.Sandstone)
			.To(WallID.CrimsonSandstone)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall1)
			.To(WallID.CrimsonUnsafe1)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall2)
			.To(WallID.CrimsonUnsafe2)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall3)
			.To(WallID.CrimsonUnsafe3)
			.RegisterWall()

			.From(WallID.Sets.Conversion.NewWall4)
			.To(WallID.CrimsonUnsafe4)
			.RegisterWall();
	}
}
