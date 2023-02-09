using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Data;
using AltLibrary.Content.Groups;
using Terraria.ID;

namespace AltLibrary.Content.Biomes;

public sealed class HallowBiome : AltBiome<GoodBiomeGroup> {
	public override void SetStaticDefaults() {
		DataHandler.Add(new ConversionData {
			Stone = TileID.Pearlstone,
			Sandstone = TileID.HallowSandstone,
			HardSand = TileID.HallowHardenedSand,

			Grass = TileID.HallowedGrass,
			MowedGrass = TileID.GolfGrassHallowed,

			Sand = TileID.Pearlstone,
			Ice = TileID.HallowedIce
		});
	}
}
