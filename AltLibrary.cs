using AltLibrary.Common;
using AltLibrary.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;

namespace AltLibrary;

public class AltLibrary : Mod {
	public static AltLibrary Instance { get; set; }
	private static List<IPostContent> postContentInstances = new();

	public AltLibrary() {
		Instance = this;
	}

	public override void Load() {
		ILHelper.Load();
	}

	public override void PostSetupContent() {
		LibUtils.ForEachType(x => x.GetCustomAttribute<CacheAttribute>() != null, (current, mod)
			=> current.GetMethod(current.GetCustomAttribute<CacheAttribute>().MethodName, BindingFlags.Static | BindingFlags.Public).Invoke(null, null));

		LibUtils.ForEachType(x => !x.IsAbstract && x.IsAssignableTo(typeof(IPostContent)), (current, mod) => {
			var instance = (IPostContent)Activator.CreateInstance(current);
			if (!postContentInstances.Contains(instance)) {
				postContentInstances.Add(instance);
			}
			instance.Load(mod);
		});

		ILHelper.PostLoad();
	}

	public override void Unload() {
		LibUtils.ForEach(postContentInstances, postContent => {
			postContent.Unload();
		});
		postContentInstances.Clear();

		ILHelper.Unload(); // Have to unload ILs and Detours manually.
		StaticCollector.Clean(this);
	}
}