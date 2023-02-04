using AltLibrary.Common.AltOres;
using AltLibrary.Common.TierOres;
using Terraria.ID;

namespace AltLibrary.Content.Ores;

public sealed class IronTierOre : TierOre {
	public sealed override void SetupContent() {
		Order = 1f;
		base.SetupContent();
	}
}

public sealed class IronOre : AltOre<IronTierOre> {
	public override void SetStaticDefaults() {
		OreTile = TileID.Iron;
		OreItem = ItemID.IronOre;
		OreBar = ItemID.IronBar;
	}
}
public sealed class LeadOre : AltOre<IronTierOre> {
	public override void SetStaticDefaults() {
		OreTile = TileID.Lead;
		OreItem = ItemID.LeadOre;
		OreBar = ItemID.LeadBar;
	}
}
