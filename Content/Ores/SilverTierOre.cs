using AltLibrary.Common.AltTypes;
using AltLibrary.Content.Groups;
using Terraria.ID;

namespace AltLibrary.Content.Ores;

public sealed class SilverOre : AltOre<SilverOreGroup> {
	public override string Texture => $"Terraria/Images/Item_{ItemID.SilverOre}";

	public override void SetStaticDefaults() {
	}
}
public sealed class TungstenOre : AltOre<SilverOreGroup> {
	public override string Texture => $"Terraria/Images/Item_{ItemID.TungstenOre}";

	public override void SetStaticDefaults() {
	}
}
