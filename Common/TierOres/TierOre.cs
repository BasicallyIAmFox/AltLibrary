using AltLibrary.Common.AltOres;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace AltLibrary.Common.TierOres;

public abstract class TierOre : ModType {
	internal static List<TierOre> tierOres = new(4);

	public List<IAltOre> Ores { get; } = new(3);
	public int Type { get; private set; }

	public float Order { get; set; }

	public void Add(IAltOre ore) => Ores.Add(ore);

	#region Loading
	private void LoadInternal() {
		LibTils.ForEachType(x => !x.IsAbstract && x.IsSubclassOf(typeof(AltOre<>).MakeGenericType(GetType())), (current, mod) => {
			var ore = Activator.CreateInstance(current) as IAltOre;
			mod.AddContent(ore);
			Add(ore);
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

	protected sealed override void Register() {
		ModTypeLookup<TierOre>.Register(this);
		Type = tierOres.Count;
		tierOres.Add(this);
	}
}