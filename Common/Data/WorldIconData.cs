using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Attributes;

namespace AltLibrary.Common.Data;

[DataAlwaysExists]
public struct WorldIconData : IBiomeData, IDataAlwaysExists<IAltBiome> {
	public string NormalWorldIcon { get; set; }
	public string DrunkBaseWorldIcon { get; set; }
	public string DrunkWorldIcon { get; set; }
	public string NotTheBeesWorldIcon { get; set; }
	public string ForTheWorthyWorldIcon { get; set; }
	public string Celebrationmk10WorldIcon { get; set; }
	public string TheConstantWorldIcon { get; set; }
	public string NoTrapsWorldIcon { get; set; }
	public string DontDigUpWorldIcon { get; set; }
	public string GetFixedBoiLeftWorldIcon { get; set; }
	public string GetFixedBoiFullWorldIcon { get; set; }
	public string GetFixedBoiRightWorldIcon { get; set; }

	public static void CheckThere(IAltBiome biome) {
		var worldIconData = biome.DataHandler.Get<WorldIconData>();

		worldIconData.NormalWorldIcon ??= biome.Texture;
		worldIconData.DrunkBaseWorldIcon ??= worldIconData.NormalWorldIcon + "_DrunkBase";
		worldIconData.DrunkWorldIcon ??= worldIconData.NormalWorldIcon + "_Drunk";
		worldIconData.NotTheBeesWorldIcon ??= worldIconData.NormalWorldIcon + "_NotTheBees";
		worldIconData.ForTheWorthyWorldIcon ??= worldIconData.NormalWorldIcon + "_ForTheWorthy";
		worldIconData.Celebrationmk10WorldIcon ??= worldIconData.NormalWorldIcon + "_Anniversary";
		worldIconData.TheConstantWorldIcon ??= worldIconData.NormalWorldIcon + "_Constant";
		worldIconData.NoTrapsWorldIcon ??= worldIconData.NormalWorldIcon + "_NoTraps";
		worldIconData.DontDigUpWorldIcon ??= worldIconData.NormalWorldIcon + "_Remix";
		worldIconData.GetFixedBoiFullWorldIcon ??= worldIconData.NormalWorldIcon + "_ZenithFull";
		worldIconData.GetFixedBoiLeftWorldIcon ??= worldIconData.NormalWorldIcon + "_ZenithLeft";
		worldIconData.GetFixedBoiRightWorldIcon ??= worldIconData.NormalWorldIcon + "_ZenithRight";

		biome.DataHandler.Add(worldIconData);
	}
}
