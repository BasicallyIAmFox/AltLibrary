using AltLibrary.Core;
using AltLibrary.Core.Attributes;
using MonoMod.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;

namespace AltLibrary;

public class AltLib : Mod {
	public static AltLib Instance { get; private set; }
	private static readonly Dictionary<Type, object> loadableContent = new();

	public AltLib() {
		Instance = this;
		LoadLoadables(ContentOrder.Init);
	}

	public override void Load() {
		LoadLoadables(ContentOrder.EarlyContent);
		LoadLoadables(ContentOrder.MidContent);
	}

	public override void PostSetupContent() {
		LoadLoadables(ContentOrder.PostContent);
	}

	public override void Unload() {
		LoadLoadableContents(ContentOrder.Unload, true);
		StaticCollector.Clean();
	}

	private static void LoadLoadables(ContentOrder contentOrder) {
		LoadLoadableContents(contentOrder);
	}

	private static void LoadLoadableContents(ContentOrder contentOrder, bool unload = false) {
		LibUtils.ForEachType(x => x.GetCustomAttributes<LoadableContentAttribute>()?.Any(x => unload && x.UnloadName != null || x.ContentOrder == contentOrder) == true, (current, mod) => {
			var attris = current.GetCustomAttributes<LoadableContentAttribute>().ToArray();
			for (int i = 0, c = attris.Length; i < c; i++) {
				var attri = attris[i];
				if (attri.ContentOrder != contentOrder) {
					continue;
				}

				MethodInfo method = null;
				if (attri.UnloadName == null && !unload) {
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