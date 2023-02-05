using AltLibrary.Common.AltTypes;
using AltLibrary.Content.Groups;
using Terraria.ID;

namespace AltLibrary.Content.Ores;

public sealed class CopperOre : AltOre<CopperOreGroup> {
	public override string Texture => $"Terraria/Images/Item_{ItemID.CopperOre}";

	public override void SetStaticDefaults() {
	}
}
public sealed class TinOre : AltOre<CopperOreGroup> {
	public override string Texture => $"Terraria/Images/Item_{ItemID.TinOre}";

	public override void SetStaticDefaults() {
	}
}
