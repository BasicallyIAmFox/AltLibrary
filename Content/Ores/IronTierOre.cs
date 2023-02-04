using AltLibrary.Common.AltTypes;
using AltLibrary.Content.Groups;
using Terraria.ID;

namespace AltLibrary.Content.Ores;

public sealed class IronOre : AltOre<IronOreGroup> {
	public override void SetStaticDefaults() {
		OreTile = TileID.Iron;
		OreItem = ItemID.IronOre;
		OreBar = ItemID.IronBar;
	}
}
public sealed class LeadOre : AltOre<IronOreGroup> {
	public override void SetStaticDefaults() {
		OreTile = TileID.Lead;
		OreItem = ItemID.LeadOre;
		OreBar = ItemID.LeadBar;
	}
}
