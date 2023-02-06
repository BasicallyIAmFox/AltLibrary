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

	IAOrderGroup Group { get; }
	IDataHandler DataHandler { get; }
}
[Autoload(false)]
public abstract class AAltType<Self, BaseGroup, Interface> : ModTexturedType, IAAltType
	where Self : AAltType<Self, BaseGroup, Interface>, Interface
	where BaseGroup : class, IAOrderGroup
	where Interface : IAAltType {
	string IAAltType.Texture => Texture;

	int IAAltType.Type => Type;
	public int Type { get; private set; }

	IDataHandler IAAltType.DataHandler => DataHandler;
	public IDataHandler DataHandler { get; protected set; }

	IAOrderGroup IAAltType.Group => this.Group;
	public BaseGroup Group => ModContent.GetInstance<BaseGroup>();

	public override void SetupContent() {
		SetStaticDefaults();

		LibUtils.ForEachType(x => x.IsAssignableTo(typeof(IDataAlwaysExists<>).MakeGenericType(typeof(Interface))) && x.GetCustomAttribute<DataAlwaysExistsAttribute>() != null, (current, mod) => {
			current.GetMethod("CheckThere", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] { this });
		});
	}

	protected sealed override void Register() {
		ModTypeLookup<IAAltType>.Register(this);
		ModTypeLookup<Interface>.Register((Self)this);
		ModTypeLookup<Self>.Register((Self)this);

		Type = GetListOfTypes().Count;
		GetListOfTypes().Add((Self)this);
	}

	private protected abstract List<Interface> GetListOfTypes();
}
