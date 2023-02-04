using AltLibrary.Common.AltOres;
using AltLibrary.Common.TierOres;
using Terraria.ID;

namespace AltLibrary.Content.Ores;

public sealed class GoldTierOre : TierOre {
	public sealed override void SetupContent() {
		Order = 3f;
		base.SetupContent();
	}
}

public sealed class GoldOre : AltOre<GoldTierOre> {
	public override void SetStaticDefaults() {
		OreTile = TileID.Gold;
		OreItem = ItemID.GoldOre;
		OreBar = ItemID.GoldBar;
	}
}
public sealed class PlatinumOre : AltOre<GoldTierOre> {
	public override void SetStaticDefaults() {
		OreTile = TileID.Platinum;
		OreItem = ItemID.PlatinumOre;
		OreBar = ItemID.PlatinumBar;
	}
}
