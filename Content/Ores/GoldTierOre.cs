using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Data;
using AltLibrary.Content.Groups;
using Terraria.ID;

namespace AltLibrary.Content.Ores;

public sealed class GoldOre : AltOre<GoldOreGroup> {
	public override string Texture => $"Terraria/Images/Item_{ItemID.GoldOre}";

	public override void SetStaticDefaults() {
		DataHandler.Add(new OreData {
			Bar = ItemID.GoldBar,
			Ore = ItemID.GoldOre,
			Tile = TileID.Gold
		});
	}
}
public sealed class PlatinumOre : AltOre<GoldOreGroup> {
	public override string Texture => $"Terraria/Images/Item_{ItemID.PlatinumOre}";

	public override void SetStaticDefaults() {
		DataHandler.Add(new OreData {
			Bar = ItemID.PlatinumBar,
			Ore = ItemID.PlatinumOre,
			Tile = TileID.Platinum
		});
	}
}
