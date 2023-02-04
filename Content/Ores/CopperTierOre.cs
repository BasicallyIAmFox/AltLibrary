using AltLibrary.Common.AltOres;
using AltLibrary.Common.TierOres;
using Terraria.ID;

namespace AltLibrary.Content.Ores;

public sealed class CopperTierOre : TierOre {
	public sealed override void SetupContent() {
		Order = 0f;
		base.SetupContent();
	}
}

public sealed class CopperOre : AltOre<CopperTierOre> {
	public override void SetStaticDefaults() {
		OreTile = TileID.Copper;
		OreItem = ItemID.CopperOre;
		OreBar = ItemID.CopperBar;
	}
}
public sealed class TinOre : AltOre<CopperTierOre> {
	public override void SetStaticDefaults() {
		OreTile = TileID.Tin;
		OreItem = ItemID.TinOre;
		OreBar = ItemID.TinBar;
	}
}
