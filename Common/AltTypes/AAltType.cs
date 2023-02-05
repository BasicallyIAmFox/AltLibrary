using AltLibrary.Common.Attributes;
using AltLibrary.Common.Data;
using AltLibrary.Common.OrderGroups;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;

namespace AltLibrary.Common.AltTypes;

public interface IAAltType : IModType {
	string Texture { get; }
	int Type { get; }

	IDataHandler DataHandler { get; }
}
[Autoload(false)]
public abstract class AAltType<Self, G, I> : ModTexturedType, IAAltType
	where Self : AAltType<Self, G, I>, I
	where G : class, IAOrderGroup
	where I : IAAltType {
	int IAAltType.Type => Type;
	public int Type { get; private set; }

	IDataHandler IAAltType.DataHandler => DataHandler;
	public IDataHandler DataHandler { get; protected set; }

	string IAAltType.Texture => Texture;

	public G Group => ModContent.GetInstance<G>();

	public override void SetupContent() {
		SetStaticDefaults();

		LibTils.ForEachType(x => x.GetCustomAttribute<DataAlwaysExistsAttribute>() != null, (current, mod) => {
			current.GetMethod("CheckThere", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] { this });
		});
	}

	protected sealed override void Register() {
		ModTypeLookup<IAAltType>.Register(this);
		ModTypeLookup<I>.Register((Self)this);
		ModTypeLookup<Self>.Register((Self)this);

		Type = GetListOfTypes().Count;
		GetListOfTypes().Add((Self)this);
	}

	private protected abstract List<I> GetListOfTypes();
}
