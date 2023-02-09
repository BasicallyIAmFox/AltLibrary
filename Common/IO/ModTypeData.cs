using Terraria.ModLoader;

namespace AltLibrary.Common.IO;

public readonly struct ModTypeData<T> where T : IModType {
	public readonly string Mod { get; }
	public readonly string Name { get; }

	public readonly string FullName => $"{Mod}/{Name}";

	public ModTypeData(T instance) {
		Mod = instance.Mod.Name;
		Name = instance.Name;
	}

	public static implicit operator T(ModTypeData<T> modTypeData) => modTypeData.FullName.To<T>();
	public static implicit operator ModTypeData<T>(T modType) => new(modType);
}
