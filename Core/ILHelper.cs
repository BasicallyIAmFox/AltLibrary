using AltLibrary.Core.Attributes;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using MonoMod.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AltLibrary.Core;

[LoadableContent(ContentOrder.Init, nameof(Init))]
[LoadableContent(ContentOrder.EarlyContent, nameof(Load), nameof(Unload))]
[LoadableContent(ContentOrder.PostContent, nameof(PostLoad))]
public static class ILHelper {
	private static List<(MethodInfo, Delegate, bool, bool)> IlsAndDetours = new();
	private static MethodInfo ilcursor__insert;
	private static int stackCode = 0;

	private static ILCursor ILCursor__Insert(Func<ILCursor, Instruction, ILCursor> orig, ILCursor self, Instruction instr) {
		stackCode++;
		return orig(self, instr);
	}

	public static void Init() {
		ilcursor__insert = typeof(ILCursor).FindMethod("_Insert");
		if (ilcursor__insert != null) {
			HookEndpointManager.Add(ilcursor__insert, ILCursor__Insert);
		}
	}

	public static void Load() {
		HookUp(
			(e, m) => $"Failed to modify method {m.DeclaringType.Namespace} {m.Name}!",
			HookEndpointManager.Add,
			HookEndpointManager.Modify,
			load => load
		);
	}

	public static void PostLoad() {
		HookUp(
			(e, m) => $"Failed to late-modify method {m.DeclaringType.Namespace} {m.Name}!",
			HookEndpointManager.Add,
			HookEndpointManager.Modify,
			load => !load
		);
	}

	public static void Unload() {
		HookUp(
			(e, m) => $"Failed to unmodify method {m.DeclaringType.Namespace} {m.Name}!",
			HookEndpointManager.Remove,
			HookEndpointManager.Unmodify,
			load => false
		);

		if (ilcursor__insert != null) {
			HookEndpointManager.Remove(ilcursor__insert, ILCursor__Insert);
		}
		IlsAndDetours.Clear();
	}

	#region Hooking
	private static void HookUp(Func<Exception, MethodInfo, string> errorFunc, Action<MethodInfo, Delegate> actionOn, Action<MethodInfo, Delegate> actionIL, Func<bool, bool> shouldLateLoad) {
		int i = 0;
		stackCode = 0;
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
				AltLib.Instance.Logger.Error($"Error Code: {i}-{stackCode:X4}");
				AltLib.Instance.Logger.Warn(errorFunc(e, method));
			}
			stackCode = 0;
			i++;
		}
	}

	public static void IL<T>(string methodName, ILContext.Manipulator manipulator, bool lateLoading = false) => IL(typeof(T), methodName, manipulator, lateLoading);
	public static void IL(Type type, string methodName, ILContext.Manipulator manipulator, bool lateLoading = false) => IL(type.FindMethod(methodName), manipulator, lateLoading);
	public static void IL(MethodInfo method, ILContext.Manipulator manipulator, bool lateLoading = false) => IlsAndDetours.Add((method, manipulator, false, lateLoading));

	public static void On<T>(string methodName, Delegate del, bool lateLoading = false) => On(typeof(T), methodName, del, lateLoading);
	public static void On(Type type, string methodName, Delegate del, bool lateLoading = false) => On(type.FindMethod(methodName), del, lateLoading);
	public static void On(MethodInfo method, Delegate del, bool lateLoading = false) => IlsAndDetours.Add((method, del, true, lateLoading));
	#endregion

	#region Extensions
	public static int AddVariable<T>(this ILCursor c) => c.Context.AddVariable<T>();
	public static int AddVariable(this ILCursor c, Type type) => c.Context.AddVariable(type);
	public static int AddVariable(this ILCursor c, TypeReference typeDefinition) => c.Context.AddVariable(typeDefinition);
	public static int AddVariable(this ILCursor c, VariableDefinition variableDefinition) => c.Context.AddVariable(variableDefinition);

	public static int AddVariable<T>(this ILContext context) => context.AddVariable(typeof(T));
	public static int AddVariable(this ILContext context, Type type) => context.AddVariable(new VariableDefinition(context.Import(type)));
	public static int AddVariable(this ILContext context, TypeReference typeDefinition) => context.AddVariable(new VariableDefinition(typeDefinition));
	public static int AddVariable(this ILContext context, VariableDefinition variableDefinition) {
		context.Body.Variables.Add(variableDefinition);
		return context.Body.Variables.Count - 1;
	}
	#endregion
}
