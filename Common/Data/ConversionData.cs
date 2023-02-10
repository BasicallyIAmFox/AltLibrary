using AltLibrary.Common.CID;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AltLibrary.Common.Data;

public struct ConversionData : IBiomeData {
	public ConversionData() {
	}

	public int Stone { get; set; } = ConversionInheritanceData.Keep;
	public int Sandstone { get; set; } = ConversionInheritanceData.Keep;
	public int HardSand { get; set; } = ConversionInheritanceData.Keep;

	public int ThornBush { get; set; } = ConversionInheritanceData.Keep;

	public int Grass { get; set; } = ConversionInheritanceData.Keep;
	public int JungleGrass { get; set; } = ConversionInheritanceData.Keep;
	public int MowedGrass { get; set; } = ConversionInheritanceData.Keep;
	public int Mud { get; set; } = ConversionInheritanceData.Keep;

	public int Sand { get; set; } = ConversionInheritanceData.Keep;
	public int Snow { get; set; } = ConversionInheritanceData.Keep;
	public int Ice { get; set; } = ConversionInheritanceData.Keep;

	public IReadOnlyList<int> AsList() {
		var self = this;
		return GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
			.Where(x => x.DeclaringType == typeof(int))
			.Select(x => (int)x.GetValue(self))
			.ToList();
	}
}
