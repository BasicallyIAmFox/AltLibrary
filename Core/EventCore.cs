using System;
using Terraria.ModLoader;

namespace AltLibrary.Core;

internal static class EventSystem {
	public static event Action<Mod> InitHook;
	public static event Action<Mod> EarlyContentHook;
	public static event Action<Mod> MidContentHook;
	public static event Action<Mod> PostContentHook;
	public static event Action<Mod> UnloadHook;

	public static void InvokeInit(Mod mod) => InitHook?.Invoke(mod);
	public static void InvokeEarlyContent(Mod mod) => EarlyContentHook?.Invoke(mod);
	public static void InvokeMidContent(Mod mod) => MidContentHook?.Invoke(mod);
	public static void InvokePostContent(Mod mod) => PostContentHook?.Invoke(mod);
	public static void InvokeUnload(Mod mod) => UnloadHook?.Invoke(mod);
}
