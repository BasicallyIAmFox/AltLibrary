using AltLibrary.Common.MaterialContexts;
using AltLibrary.Common.OrderGroups;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace AltLibrary.Common.AltTypes;

public interface IAAltType : IModType {
	int Type { get; }
	IMaterialContext MaterialContext { get; }
}
[Autoload(false)]
public abstract class AAltType<Self, G, I, M> : ModTexturedType
	where Self : AAltType<Self, G, I, M>, I
	where G : class, IAOrderGroup
	where I : IAAltType
	where M : IMaterialContext {
	public int Type { get; private set; }
	public IMaterialContext MaterialContext { get; private set; } = null;

	public G Group => ModContent.GetInstance<G>();

	public IMaterialContext CreateMaterial() {
		if (MaterialContext != null) {
			throw new UsageException("Only one Material Context can be made!");
		}
		return MaterialContext = Activator.CreateInstance<M>();
	}

	public sealed override void SetupContent() {
		SetStaticDefaults();
	}

	protected sealed override void Register() {
		ModTypeLookup<I>.Register((Self)this);
		ModTypeLookup<Self>.Register((Self)this);

		ModContent.Request<Texture2D>(Texture);

		Type = GetListOfTypes().Count;
		GetListOfTypes().Add((Self)this);
	}

	private protected abstract List<I> GetListOfTypes();
}
