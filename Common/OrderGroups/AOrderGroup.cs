﻿using AltLibrary.Common.AltTypes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;
using static AltLibrary.Common.OrderGroups.IAOrderGroup;

namespace AltLibrary.Common.OrderGroups;

public interface IAOrderGroup : IModType {
	internal static List<IAOrderGroup> tiers = new(4);

	List<IAAltType> Elements { get; }
	string Texture { get; }
	int Type { get; }
	float Order { get; }
}
public interface IStaticOrderGroup {
	static abstract string GetTexture();
	static abstract Rectangle GetSourceRectangle();
}
public abstract class AOrderGroup<Self, T> : ModTexturedType, IAOrderGroup where Self : AOrderGroup<Self, T> where T : IAAltType {
	public const string DefaultTexture = "Terraria/Images/UI/Bestiary/Icon_Tags_Shadow";

	public List<T> Elements { get; } = new(3);
	public int Type { get; private set; }

	public float Order { get; set; }

	List<IAAltType> IAOrderGroup.Elements => Elements.Cast<IAAltType>().ToList();
	string IAOrderGroup.Texture => Texture;

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