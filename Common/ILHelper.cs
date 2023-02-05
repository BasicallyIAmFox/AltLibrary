using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using MonoMod.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AltLibrary.Common;

public class ILHelper {
	private static List<(MethodInfo, Delegate, bool, bool)> IlsAndDetours = new();

	public static void Load() {
		HookUp(
			(e, m) => $"Failed to modify method {m.DeclaringType.Namespace} {m.Name}!\n{e.Message}",
			(m, d) => HookEndpointManager.Add(m, d),
			(m, d) => HookEndpointManager.Modify(m, d),
			load => load
		);
	}

	public static void PostLoad() {
		HookUp(
			(e, m) => $"Failed to late-modify method {m.DeclaringType.Namespace} {m.Name}!\n{e.Message}",
			(m, d) => HookEndpointManager.Add(m, d),
			(m, d) => HookEndpointManager.Modify(m, d),
			load => !load
		);
	}

	public static void Unload() {
		HookUp(
			(e, m) => $"Failed to unmodify method {m.DeclaringType.Namespace} {m.Name}!\n{e.Message}",
			(m, d) => HookEndpointManager.Add(m, d),
			(m, d) => HookEndpointManager.Modify(m, d),
			load => false
		);

		IlsAndDetours.Clear();
		IlsAndDetours = null;
	}

	private static void HookUp(Func<Exception, MethodInfo, string> errorFunc, Action<MethodInfo, Delegate> actionOn, Action<MethodInfo, Delegate> actionIL, Func<bool, bool> shouldLateLoad) {
		foreach ((MethodInfo method, Delegate callback, bool isDetour, bool lateLoad) in IlsAndDetours) {
			if (shouldLateLoad(lateLoad)) {
				continue;
			}
			try {
				if (isDetour) {
					actionOn(method, callback);
					continue;
				}
				actionIL(method, callback);
			}
			catch (Exception e) {
				AltLibrary.Instance.Logger.Warn(errorFunc(e, method));
			}
		}
	}

	public static void IL<T>(string methodName, ILContext.Manipulator manipulator, bool lateLoading = false) => IL(typeof(T), methodName, manipulator, lateLoading);
	public static void IL(Type type, string methodName, ILContext.Manipulator manipulator, bool lateLoading = false) => IL(type.FindMethod(methodName), manipulator, lateLoading);
	public static void IL(MethodInfo method, ILContext.Manipulator manipulator, bool lateLoading = false) => IlsAndDetours.Add((method, manipulator, false, lateLoading));

	public static void On<T>(string methodName, Delegate del, bool lateLoading = false) => On(typeof(T), methodName, del, lateLoading);
	public static void On(Type type, string methodName, Delegate del, bool lateLoading = false) => On(type.FindMethod(methodName), del, lateLoading);
	public static void On(MethodInfo method, Delegate del, bool lateLoading = false) => IlsAndDetours.Add((method, del, true, lateLoading));
}
