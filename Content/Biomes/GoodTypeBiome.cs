using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Data;
using AltLibrary.Content.Groups;
using Terraria.ID;

namespace AltLibrary.Content.Biomes;

public sealed class HallowBiome : AltBiome<GoodBiomeGroup> {
	public override void SetStaticDefaults() {
		DataHandler.Add(new WorldIconData {
			NormalWorldIcon = "AltLibrary/Assets/WorldIcons/Normal/Hallow",
			DrunkWorldIcon = "AltLibrary/Assets/WorldIcons/Drunk/Hallow",
			ForTheWorthyWorldIcon = "AltLibrary/Assets/WorldIcons/ForTheWorthy/Hallow",
			NotTheBeesWorldIcon = "AltLibrary/Assets/WorldIcons/NotTheBees/Hallow",
			Celebrationmk10WorldIcon = "AltLibrary/Assets/WorldIcons/Anniversary/Hallow",
			TheConstantWorldIcon = "AltLibrary/Assets/WorldIcons/DontStarve/Hallow",
			NoTrapsWorldIcon = "AltLibrary/Assets/WorldIcons/NoTraps/Hallow",
			DontDigUpWorldIcon = "AltLibrary/Assets/WorldIcons/Remix/Hallow",

			GetFixedBoiLeftWorldIcon = "AltLibrary/Assets/WorldIcons/Zenith/Hallow",
			GetFixedBoiFullWorldIcon = "AltLibrary/Assets/WorldIcons/Zenith/Hallow",
			GetFixedBoiRightWorldIcon = "AltLibrary/Assets/WorldIcons/Normal/Hallow",
		});
		DataHandler.Add(new ConversionData {
			Stone = TileID.Pearlstone,
			Sandstone = TileID.HallowSandstone,
			HardSand = TileID.HallowHardenedSand,

			Grass = TileID.HallowedGrass,
			MowedGrass = TileID.GolfGrassHallowed,

			Sand = TileID.Pearlsand,
			Ice = TileID.HallowedIce
		});
	}
}
