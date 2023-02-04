using AltLibrary.Common.AltTypes;
using AltLibrary.Content.Groups;
using Terraria.ID;

namespace AltLibrary.Content.Ores;

public sealed class CobaltOre : AltOre<CobaltOreGroup> {
	public override void SetStaticDefaults() {
		OreTile = TileID.Cobalt;
		OreItem = ItemID.CobaltOre;
		OreBar = ItemID.CobaltBar;
	}
}
public sealed class PalladiumOre : AltOre<CobaltOreGroup> {
	public override void SetStaticDefaults() {
		OreTile = TileID.Palladium;
		OreItem = ItemID.PalladiumOre;
		OreBar = ItemID.PalladiumBar;
	}
}
