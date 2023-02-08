using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AltLibrary.Common.Data;

public struct ConversionData : IBiomeData {
	public int Stone { get; set; }
	public int Sandstone { get; set; }
	public int HardSand { get; set; }

	public int ThornBush { get; set; }

	public int Grass { get; set; }
	public int JungleGrass { get; set; }
	public int MowedGrass { get; set; }
	public int Mud { get; set; }
	public bool MudToDirt { get; set; }

	public int Sand { get; set; }
	public int Snow { get; set; }
	public int Ice { get; set; }

	public IReadOnlyList<int> AsList() {
		var self = this;
		return GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
			.Where(x => x.DeclaringType == typeof(int))
			.Select(x => (int)x.GetValue(self))
			.ToList();
	}
}
