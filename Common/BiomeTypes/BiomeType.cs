using AltLibrary.Common.AltBiomes;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace AltLibrary.Common.BiomeTypes;

public abstract class BiomeType : ModType {
	internal static List<BiomeType> biomeTypes = new(4);

	public List<IAltBiome> Biomes { get; } = new(3);
	public int Type { get; private set; }

	public float Order { get; set; }

	public void Add(IAltBiome biome) => Biomes.Add(biome);

	#region Loading
	private void LoadInternal() {
		LibTils.ForEachType(x => !x.IsAbstract && x.IsSubclassOf(typeof(AltBiome<>).MakeGenericType(GetType())), (current, mod) => {
			Add(mod.GetContent<IAltBiome>().First(x => x.GetType() == current));
		});
	}

	public sealed override void Load() {
		LoadInternal();
		LoadOther();
	}

	public virtual void LoadOther() {
	}
	#endregion

	public override void SetupContent() {
		SetStaticDefaults();
	}

	protected override void Register() {
		ModTypeLookup<BiomeType>.Register(this);
		Type = biomeTypes.Count;
		biomeTypes.Add(this);
	}
}
