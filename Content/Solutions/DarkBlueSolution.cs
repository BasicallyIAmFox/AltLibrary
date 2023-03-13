using AltLibrary.Common.Conversion;
using AltLibrary.Common.Solutions;
using Terraria.ID;

namespace AltLibrary.Content.Solutions;

// Last update: 1.4.4.9
public sealed class DarkBlueSolution : ModSolution {
	public override void SetStaticDefaults() {
		Conversion
			.From(TileID.JungleGrass)
			.To(TileID.MushroomGrass)
			.OnConversion(TryKillingTreesAboveIfTheyWouldBecomeInvalid)
			.RegisterTile()

			.From(TileID.Sets.Conversion.Thorn)
			.To(ConversionHandler.Break)
			.RegisterTile()

			.From(WallID.Sets.CanBeConvertedToGlowingMushroom)
			.To(WallID.MushroomUnsafe)
			.RegisterWall();
	}
}
