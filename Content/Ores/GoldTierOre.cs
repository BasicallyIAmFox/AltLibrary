using AltLibrary.Common.AltTypes;
using AltLibrary.Content.Groups;
using Terraria.ID;

namespace AltLibrary.Content.Ores;

public sealed class GoldOre : AltOre<GoldOreGroup> {
	public override string Texture => $"Terraria/Images/Item_{ItemID.GoldOre}";

	public override void SetStaticDefaults() {
	}
}
public sealed class PlatinumOre : AltOre<GoldOreGroup> {
	public override string Texture => $"Terraria/Images/Item_{ItemID.PlatinumOre}";

	public override void SetStaticDefaults() {
	}
}
