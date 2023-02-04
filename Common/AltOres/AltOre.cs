using AltLibrary.Common.TierOres;
using Terraria.ModLoader;
using static AltLibrary.Common.AltOres.IAltOre;

namespace AltLibrary.Common.AltOres;

[Autoload(false)]
public abstract class AltOre<T> : ModType, IAltOre where T : TierOre {
	public int Type { get; private set; }

	public int OreTile { get; set; } = 0;
	public int OreBar { get; set; } = 0;
	public int OreItem { get; set; } = 0;

	public sealed override void SetupContent() {
		SetStaticDefaults();
	}

	protected sealed override void Register() {
		ModTypeLookup<IAltOre>.Register(this);
		ModTypeLookup<AltOre<T>>.Register(this);

		Type = altOres.Count;
		altOres.Add(this);
	}
}