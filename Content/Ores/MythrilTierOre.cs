using AltLibrary.Common.AltTypes;
using AltLibrary.Content.Groups;
using Terraria.ID;

namespace AltLibrary.Content.Ores;

public sealed class MythrilOre : AltOre<MythrilOreGroup> {
	public override string Texture => $"Terraria/Images/Item_{ItemID.MythrilOre}";

	public override void SetStaticDefaults() {
	}
}
public sealed class OrichalcumOre : AltOre<MythrilOreGroup> {
	public override string Texture => $"Terraria/Images/Item_{ItemID.OrichalcumOre}";

	public override void SetStaticDefaults() {
	}
}
