using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using static AltLibrary.Common.OrderGroups.IAOrderGroup;

namespace AltLibrary.Common.OrderGroups;

public interface IAOrderGroup : IModType {
	internal static List<IAOrderGroup> tiers = new(4);

	int Type { get; }
	float Order { get; }
}
public abstract class AOrderGroup<Self, T> : ModType, IAOrderGroup where Self : AOrderGroup<Self, T> where T : IModType {
	public List<T> Elements { get; } = new(3);
	public int Type { get; private set; }

	public float Order { get; set; }

	public void Add(T ore) => Elements.Add(ore);

	#region Loading
	private protected abstract Type GetMainSubclass();

	private void LoadInternal() {
		LibTils.ForEachType(x => !x.IsAbstract && x.IsSubclassOf(GetMainSubclass().MakeGenericType(GetType())), (current, mod) => {
			var ore = (T)Activator.CreateInstance(current);
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
		ModTypeLookup<IAOrderGroup>.Register(this);
		ModTypeLookup<Self>.Register((Self)this);
		Type = tiers.Count;
		tiers.Add(this);
	}
}
