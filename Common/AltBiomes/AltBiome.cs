using AltLibrary.Common.BiomeTypes;
using AltLibrary.Common.MaterialContexts;
using Terraria.ModLoader;
using static AltLibrary.Common.AltBiomes.IAltBiome;

namespace AltLibrary.Common.AltBiomes;

[Autoload(false)]
public abstract class AltBiome<T> : ModType, IAltBiome where T : BiomeType {
	public int Type { get; private set; }
	public IMaterialContext MaterialContext { get; private set; } = null;

	public IMaterialContext CreateMaterial() {
		if (MaterialContext != null) {
			throw new UsageException("Only one Material Context can be made!");
		}
		return MaterialContext = new BiomeMaterialContext();
	}

	public sealed override void SetupContent() {
		SetStaticDefaults();
	}

	protected sealed override void Register() {
		ModTypeLookup<IAltBiome>.Register(this);
		ModTypeLookup<AltBiome<T>>.Register(this);

		Type = altBiomes.Count;
		altBiomes.Add(this);
	}
}