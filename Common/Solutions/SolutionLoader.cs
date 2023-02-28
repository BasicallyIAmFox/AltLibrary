using AltLibrary.Core.Attributes;
using System.Collections.Generic;

namespace AltLibrary.Common.Solutions;

[LoadableContent(ContentOrder.Unload, nameof(Unload))]
public static class SolutionLoader {
	internal static readonly IList<ModSolution> solutions = new List<ModSolution>();

	private static int next = 0;

	public static int Count => next;

	internal static int ReserveID() {
		return ++next - 1;
	}

	internal static void Unload() {
		solutions.Clear();
		next = 0;
	}

	public static ModSolution Get(int type) {
		return type >= 0 && type < Count ? solutions[type] : null;
	}
}
