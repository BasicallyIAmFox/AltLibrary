using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Data;
using AltLibrary.Content.Groups;
using Terraria.ID;

namespace AltLibrary.Content.Biomes;

public sealed class JungleBiome : AltBiome<TropicsBiomeGroup> {
	public override void SetStaticDefaults() {
		DataHandler.Add(new ConversionData {
			Stone = TileID.Ebonstone,
			Sandstone = TileID.CorruptSandstone,
			HardSand = TileID.CorruptHardenedSand,

			ThornBush = TileID.CorruptThorns,

			Grass = TileID.CorruptGrass,
			JungleGrass = TileID.CorruptJungleGrass,

			Sand = TileID.Ebonsand,
			Ice = TileID.CorruptIce
		});
	}
}
