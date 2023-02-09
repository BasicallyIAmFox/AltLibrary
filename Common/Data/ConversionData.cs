using AltLibrary.Common.CID;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AltLibrary.Common.Data;

public struct ConversionData : IBiomeData {
	public ConversionData() {
	}

	public int Stone { get; set; } = CIData.Keep;
	public int Sandstone { get; set; } = CIData.Keep;
	public int HardSand { get; set; } = CIData.Keep;

	public int ThornBush { get; set; } = CIData.Keep;

	public int Grass { get; set; } = CIData.Keep;
	public int JungleGrass { get; set; } = CIData.Keep;
	public int MowedGrass { get; set; } = CIData.Keep;
	public int Mud { get; set; } = CIData.Keep;

	public int Sand { get; set; } = CIData.Keep;
	public int Snow { get; set; } = CIData.Keep;
	public int Ice { get; set; } = CIData.Keep;

	public IReadOnlyList<int> AsList() {
		var self = this;
		return GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
			.Where(x => x.DeclaringType == typeof(int))
			.Select(x => (int)x.GetValue(self))
			.ToList();
	}
}
