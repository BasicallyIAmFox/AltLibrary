using AltLibrary.Common.Attributes;
using AltLibrary.Common.Data;
using AltLibrary.Common.OrderGroups;
using System.Collections.Generic;
using System.Reflection;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AltLibrary.Common.AltTypes;

public interface IAAltType : IModType, ILocalizedModType {
	string Texture { get; }
	int Type { get; }

	IAOrderGroup Group { get; }
	IDataHandler DataHandler { get; }

	LocalizedText DisplayName { get; }
	LocalizedText Description { get; }
}
[Autoload(false)]
public abstract class AAltType<Self, BaseGroup, Interface> : ModTexturedType, IAAltType
	where Self : AAltType<Self, BaseGroup, Interface>, Interface
	where BaseGroup : class, IAOrderGroup
	where Interface : IAAltType {
	IAOrderGroup IAAltType.Group => this.Group;

	public int Type { get; private set; }
	public IDataHandler DataHandler { get; protected set; }
	public BaseGroup Group => ModContent.GetInstance<BaseGroup>();

	public virtual LocalizedText DisplayName => this.GetLocalization(nameof(DisplayName), PrettyPrintName);
	public virtual LocalizedText Description => this.GetLocalization(nameof(Description));

	public abstract string LocalizationCategory { get; }

	public sealed override void Load() {
		LoadInternal();
		LoadSelf();
	}

	public virtual void LoadSelf() { }
	private void LoadInternal() {
		
	}

	public override void SetupContent() {
		SetStaticDefaults();

		LibUtils.ForEachType(x => x.IsAssignableTo(typeof(IDataAlwaysExists<Interface>)) && x.GetCustomAttribute<DataAlwaysExistsAttribute>() != null, (current, mod) => {
			current.GetMethod("CheckThere", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] { this });
		});
	}

	protected sealed override void Register() {
		ModTypeLookup<IAAltType>.Register(this);
		ModTypeLookup<Interface>.Register(this.As<Self>());
		ModTypeLookup<Self>.Register(this.As<Self>());

		Type = GetListOfTypes().Count;
		GetListOfTypes().Add(this.As<Self>());
	}

	private protected abstract List<Interface> GetListOfTypes();
}
