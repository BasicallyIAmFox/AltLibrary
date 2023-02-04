using AltLibrary.Common.AltOres;
using AltLibrary.Common.TierOres;
using Terraria.ID;

namespace AltLibrary.Content.Ores;

public sealed class SilverTierOre : TierOre {
	public sealed override void SetupContent() {
		Order = 2f;
		base.SetupContent();
	}
}

public sealed class SilverOre : AltOre<SilverTierOre> {
	public override void SetStaticDefaults() {
		OreTile = TileID.Silver;
		OreItem = ItemID.SilverOre;
		OreBar = ItemID.SilverBar;
	}
}
public sealed class TungstenOre : AltOre<SilverTierOre> {
	public override void SetStaticDefaults() {
		OreTile = TileID.Tungsten;
		OreItem = ItemID.TungstenOre;
		OreBar = ItemID.TungstenBar;
	}
}
