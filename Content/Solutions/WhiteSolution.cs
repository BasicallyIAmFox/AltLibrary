using AltLibrary.Common.Conversion;
using AltLibrary.Common.Solutions;
using Terraria;
using Terraria.ID;

namespace AltLibrary.Content.Solutions;

// Last update: 1.4.4.9
public sealed class WhiteSolution : ModSolution {
	public override void SetStaticDefaults() {
		Conversion
			.From(TileID.Sets.Conversion.Grass)
			.From(TileID.Sets.Conversion.Sand)
			.From(TileID.Sets.Conversion.HardenedSand)
			.From(TileID.Sets.Conversion.Snow)
			.From(TileID.Sets.Conversion.Dirt)
			.To(147)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(Main.tileMoss)
			.From(TileID.Sets.Conversion.Stone)
			.From(TileID.Sets.Conversion.Ice)
			.From(TileID.Sets.Conversion.Sandstone)
			.To(161)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Thorn)
			.To(ConversionHandler.Break)
			.RegisterTile()

			.From(WallID.Sets.Conversion.Stone)
			.From(WallID.Sets.Conversion.NewWall1)
			.From(WallID.Sets.Conversion.NewWall2)
			.From(WallID.Sets.Conversion.NewWall3)
			.From(WallID.Sets.Conversion.NewWall4)
			.From(WallID.Sets.Conversion.Ice)
			.From(WallID.Sets.Conversion.Sandstone)
			.To(71)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterWall()

			.From(WallID.Sets.Conversion.HardenedSand)
			.From(WallID.Sets.Conversion.Dirt)
			.From(WallID.Sets.Conversion.Snow)
			.To(40)
			.RegisterWall();
	}
}
