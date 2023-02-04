using AltLibrary.Common.AltOres;
using AltLibrary.Common.TierOres;
using Terraria.ID;

namespace AltLibrary.Content.Ores;

public sealed class CobaltTierOre : TierOre {
	public sealed override void SetupContent() {
		Order = 4f;
		base.SetupContent();
	}
}

public sealed class CobaltOre : AltOre<CobaltTierOre> {
	public override void SetStaticDefaults() {
		OreTile = TileID.Cobalt;
		OreItem = ItemID.CobaltOre;
		OreBar = ItemID.CobaltBar;
	}
}
public sealed class PalladiumOre : AltOre<CobaltTierOre> {
	public override void SetStaticDefaults() {
		OreTile = TileID.Palladium;
		OreItem = ItemID.PalladiumOre;
		OreBar = ItemID.PalladiumBar;
	}
}
