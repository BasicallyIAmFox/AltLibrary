using AltLibrary.Common.Conversion;
using AltLibrary.Core.Attributes;
using System.Collections.Generic;

namespace AltLibrary.Common.Solutions;

[LoadableContent(ContentOrder.Unload, nameof(Unload))]
public static class SolutionLoader {
	internal static readonly IList<ModSolution> solutions = new List<ModSolution>();

	public static int Count => solutions.Count;

	internal static void Unload() {
		solutions.Clear();
	}

	public static ModSolution Get(int type) => type >= 0 && type < Count ? solutions[type] : null;

	internal static void Fill(int count, out ConversionData.Data[] data) {
		data = new ConversionData.Data[count];
		foreach (var s in solutions) {
			s.Conversion.Fill(data);
		}
	}
}
