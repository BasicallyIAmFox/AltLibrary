using AltLibrary.Common;
using AltLibrary.Common.Attributes;
using AltLibrary.Common.IL;
using MonoMod.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;

namespace AltLibrary;

public class AltLibrary : Mod {
#if DEBUG
	internal static bool MonoModDumbps => false;
#endif

	public static AltLibrary Instance { get; set; }
	private static readonly Dictionary<Type, object> loadableContent = new();

	public AltLibrary() {
		Instance = this;
		LoadLoadables(ContentOrder.Init);
	}

	public override void Load() {
		LoadLoadables(ContentOrder.Content);
	}

	public override void PostSetupContent() {
		LoadLoadables(ContentOrder.PostContent);
	}

	public override void Unload() {
		LoadLoadableContents(ContentOrder.Unload, true);
		StaticCollector.Clean(this);
	}

	private static void LoadLoadables(ContentOrder contentOrder) {
#if DEBUG
		ILHelper.EnableMonoModDump();
#endif
		LoadLoadableContents(contentOrder);
#if DEBUG
		Debug.Assert(true);
		ILHelper.DisableMonModDump();
#endif
	}

	private static void LoadLoadableContents(ContentOrder contentOrder, bool unload = false) {
		LibUtils.ForEachType(x => unload || x.GetCustomAttributes<LoadableContentAttribute>()?.Any(x => x.ContentOrder == contentOrder) == true, (current, mod) => {
			var attris = current.GetCustomAttributes<LoadableContentAttribute>().ToArray();
			for (int i = 0, c = attris.Length; i < c; i++) {
				var attri = attris[i];
				if (attri.ContentOrder != contentOrder) {
					continue;
				}

				MethodInfo method = null;
				if (!unload) {
					method = current.FindMethod(attri.LoadName);
				}
				else if (attri.UnloadName != null) {
					method = current.FindMethod(attri.UnloadName);
				}

				if (method == null) {
					continue;
				}

				object[] parameters;
				if (method.GetParameters().Length == 1 && method.GetParameters()[0].ParameterType == typeof(Mod)) {
					parameters = new object[] { mod };
				}
				else if (method.GetParameters().Length > 1) {
					throw new ArgumentException($"Too many parameters for {method.Name} from {current.FullName}!");
				}
				else {
					parameters = Array.Empty<object>();
				}

				loadableContent.TryGetValue(current, out object obj);
				if (obj == null && !method.IsStatic) {
					loadableContent[current] = Activator.CreateInstance(current);
				}
				else if (obj != null && method.IsStatic) {
					obj = null;
				}
				method.Invoke(obj, parameters);
			}
		});
	}
}