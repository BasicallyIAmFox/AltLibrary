using AltLibrary.Core;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using MonoMod.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;

namespace AltLibrary.Common {
	internal class EditsHelper : ILoadable {
		private static List<(MethodInfo, Delegate, bool, bool)> IlsAndDetours;

		public static void IL<T>(string methodName, ILContext.Manipulator manipulator, bool lateLoading = false) {
			IL(typeof(T), methodName, manipulator, lateLoading);
		}

		public static void IL(Type type, string methodName, ILContext.Manipulator manipulator, bool lateLoading = false) {
			try {
				IL(type.FindMethod(methodName), manipulator, lateLoading);
			}
			catch (Exception e) {
				AltLibrary.Instance.Logger.Info($"IL {type.FullName} {methodName} failed to load!", e);
			}
		}

		public static void IL(MethodInfo method, ILContext.Manipulator manipulator, bool lateLoading = false) {
			IlsAndDetours.Add((method, manipulator, false, lateLoading));
		}

		public static void On<T>(string methodName, Delegate del, bool lateLoading = false) {
			try {
				On(typeof(T).FindMethod(methodName), del, lateLoading);
			}
			catch (Exception e) {
				AltLibrary.Instance.Logger.Info($"On {typeof(T).FullName} {methodName} failed to load!", e);
			}
		}

		public static void On(MethodInfo method, Delegate del, bool lateLoading = false) {
			IlsAndDetours.Add((method, del, true, lateLoading));
		}

		public void Load(Mod mod) {
			IlsAndDetours = new();
			ILHooks.OnInitialize();
			foreach ((MethodInfo method, Delegate callback, bool isDetour, bool lateLoad) in IlsAndDetours) {
				if (lateLoad) {
					continue;
				}
				try {
					if (isDetour) {
						HookEndpointManager.Add(method, callback);
						continue;
					}
					HookEndpointManager.Modify(method, callback);
				}
				catch (Exception e) {
					AltLibrary.Instance.Logger.Error($"Failed to modify method {method.DeclaringType.Namespace} {method.Name}!", e);
				}
			}
		}

		internal static void PostLoad() {
			foreach ((MethodInfo method, Delegate callback, bool isDetour, bool lateLoad) in IlsAndDetours) {
				if (!lateLoad) {
					continue;
				}
				try {
					if (isDetour) {
						HookEndpointManager.Add(method, callback);
						continue;
					}
					HookEndpointManager.Modify(method, callback);
				}
				catch (Exception e) {
					AltLibrary.Instance.Logger.Error($"Failed to late-modify method {method.DeclaringType.Namespace} {method.Name}!", e);
				}
			}
		}

		public void Unload() {
			foreach ((MethodInfo method, Delegate callback, bool isDetour, _) in IlsAndDetours) {
				try {
					if (isDetour) {
						HookEndpointManager.Remove(method, callback);
						continue;
					}
					HookEndpointManager.Unmodify(method, callback);
				}
				catch (Exception e) {
					AltLibrary.Instance.Logger.Error($"Failed to unmodify method {method.DeclaringType.Namespace} {method.Name}!", e);
				}
			}
			IlsAndDetours.Clear();
			IlsAndDetours = null;
		}
	}
}
