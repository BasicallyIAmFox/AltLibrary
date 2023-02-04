using AltLibrary.Common.Data;
using AltLibrary.Common.OrderGroups;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace AltLibrary.Common.AltTypes;

public interface IAAltType : IModType {
	int Type { get; }

	IReadonlyDataHandler DataHandler { get; }
}
[Autoload(false)]
public abstract class AAltType<Self, G, I> : ModTexturedType, IAAltType
	where Self : AAltType<Self, G, I>, I
	where G : class, IAOrderGroup
	where I : IAAltType {
	int IAAltType.Type => Type;
	public int Type { get; private set; }

	IReadonlyDataHandler IAAltType.DataHandler { get; }
	public IDataHandler DataHandler { get; protected set; }

	public G Group => ModContent.GetInstance<G>();

	public override void SetupContent() {
		SetStaticDefaults();
	}

	protected sealed override void Register() {
		ModTypeLookup<IAAltType>.Register(this);
		ModTypeLookup<I>.Register((Self)this);
		ModTypeLookup<Self>.Register((Self)this);

		ModContent.Request<Texture2D>(Texture);

		Type = GetListOfTypes().Count;
		GetListOfTypes().Add((Self)this);
	}

	private protected abstract List<I> GetListOfTypes();
}
