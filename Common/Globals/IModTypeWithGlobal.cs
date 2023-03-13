using Terraria.ModLoader;

namespace AltLibrary.Common.Globals;

public interface IModTypeWithGlobal<TType, TGlobal> where TType : ModType where TGlobal : GlobalType<TType, TGlobal> {
	ref Instanced<TGlobal>[] GlobalsArray { get; }
	RefReadOnlyArray<Instanced<TGlobal>> Globals => new(GlobalsArray);

	T GetGlobal<T>(bool exactType = true) where T : TGlobal => GlobalType.GetGlobal<TType, TGlobal, T>(GlobalsArray, exactType);
	T GetGlobal<T>(T baseInstance) where T : TGlobal => GlobalType.GetGlobal<TType, TGlobal, T>(GlobalsArray, baseInstance);
	bool TryGetGlobal<T>(out T result, bool exactType = true) where T : TGlobal => GlobalType.TryGetGlobal(GlobalsArray, exactType, out result);
	bool TryGetGlobal<T>(T baseInstance, out T result) where T : TGlobal => GlobalType.TryGetGlobal(GlobalsArray, baseInstance, out result);
}
