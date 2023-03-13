using AltLibrary.Common.Conversion;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Terraria.ModLoader.Core;
using Terraria.ModLoader;
using HookList = Terraria.ModLoader.Core.HookList<AltLibrary.Common.Solutions.GlobalSolution>;
using AltLibrary.Core.Attributes;
using AltLibrary.Core;

namespace AltLibrary.Common.Solutions;

[FeedEventCore(nameof(Feed))]
public static class SolutionLoader {
	internal static readonly IList<ModSolution> modSolutions = new List<ModSolution>();
	internal static readonly List<GlobalSolution> globalSolutions = new();

	private static readonly IList<HookList> hooks = new List<HookList>();

	public static int Count => modSolutions.Count;

	private static HookList AddHook<F>(Expression<Func<GlobalSolution, F>> func) where F : Delegate {
		var hook = HookList.Create(func);
		hooks.Add(hook);
		return hook;
	}

	public static ModSolution Get(int id) => (uint)id >= Count ? modSolutions[id] : null;

	internal static void ResizeArrays() {
		foreach (var hook in hooks) {
			hook.Update(globalSolutions);
		}
	}

	internal static int Register(ModSolution solution) {
		ModTypeLookup<ModSolution>.Register(solution);
		return modSolutions.AddIndexReturn(solution);
	}

	internal static int Register(GlobalSolution solution) {
		ModTypeLookup<GlobalSolution>.Register(solution);
		return globalSolutions.AddIndexReturn(solution);
	}

	private static void Feed() {
		EventSystem.UnloadHook += (Mod mod) => {
			modSolutions.Clear();
			globalSolutions.Clear();
			hooks.Clear();
		};
	}

	private static readonly HookList HookSetStaticDefaults = AddHook<Action<ModSolution>>(g => g.SetStaticDefaults);
	internal static void SetStaticDefaults(ModSolution solution) {
		LoaderUtils.InstantiateGlobals(solution, globalSolutions, ref solution.GlobalsArray, solution.SetStaticDefaults);

		foreach (var g in HookSetStaticDefaults.Enumerate(solution.GlobalsArray)) {
			g.SetStaticDefaults(solution);
		}
	}

	internal static void Fill(int count, out ConversionData.Data[] data) {
		data = new ConversionData.Data[count];
		foreach (var s in modSolutions) {
			s.Conversion.Fill(data);
		}
	}
}
