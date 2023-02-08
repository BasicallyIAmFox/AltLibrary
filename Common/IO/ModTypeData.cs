using Terraria.ModLoader;

namespace AltLibrary.Common.IO;

public readonly struct ModTypeData<T> where T : IModType {
	public readonly string Mod { get; }
	public readonly string Name { get; }

	public readonly bool IsEnabled => ModLoader.HasMod(Mod);

	public ModTypeData(T inst) {
		Mod = inst.Mod.Name;
		Name = inst.Name;
	}
}