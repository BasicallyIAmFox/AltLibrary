using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Attributes;

namespace AltLibrary.Common.Data;

[DataAlwaysExists]
public struct WorldIconData : IAltData, IDataAlwaysExists<IAltBiome> {
	public string NormalWorldIcon { readonly get; set; }
	public string DrunkBaseWorldIcon { readonly get; set; }
	public string DrunkWorldIcon { readonly get; set; }
	public string NotTheBeesWorldIcon { readonly get; set; }
	public string ForTheWorthyWorldIcon { readonly get; set; }
	public string Celebrationmk10WorldIcon { readonly get; set; }
	public string TheConstantWorldIcon { readonly get; set; }
	public string NoTrapsWorldIcon { readonly get; set; }
	public string DontDigUpWorldIcon { readonly get; set; }
	public string GetFixedBoiLeftWorldIcon { readonly get; set; }
	public string GetFixedBoiFullWorldIcon { readonly get; set; }
	public string GetFixedBoiRightWorldIcon { readonly get; set; }

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
