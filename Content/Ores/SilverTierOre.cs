using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Data;
using AltLibrary.Content.Groups;
using Terraria.ID;

namespace AltLibrary.Content.Ores;

public sealed class SilverOre : AltOre<SilverOreGroup> {
	public override string Texture => $"Terraria/Images/Item_{ItemID.SilverOre}";

	public override void SetStaticDefaults() {
		DataHandler.Add(new OreData {
			Bar = ItemID.SilverBar,
			Ore = ItemID.SilverOre,
			Tile = TileID.Silver
		});
	}
}
public sealed class TungstenOre : AltOre<SilverOreGroup> {
	public override string Texture => $"Terraria/Images/Item_{ItemID.TungstenOre}";

	public override void SetStaticDefaults() {
		DataHandler.Add(new OreData {
			Bar = ItemID.TungstenBar,
			Ore = ItemID.TungstenOre,
			Tile = TileID.Tungsten
		});
	}
}
