using AltLibrary.Common.Groups;
using AltLibrary.Common.Ores;
using Terraria.ModLoader;

namespace AltLibrary.Content.Groups;

public abstract class OreGroup : Group<ModAltOre> {
	public static OreGroup Instance => ModContent.GetInstance<OreGroup>();
}
