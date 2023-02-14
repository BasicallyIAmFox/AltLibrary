using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Attributes;
using AltLibrary.Common.IL;
using AltLibrary.Common.OrderGroups;
using AltLibrary.Common.SelectableUIs;
using AltLibrary.Content.Biomes;
using AltLibrary.Content.Groups;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Terraria.GameContent.UI.Elements;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Terraria.Utilities;

namespace AltLibrary.Common.IO;

[LoadableContent(ContentOrder.Content, nameof(Load))]
public static class LibWorldsData {
	private static readonly ConditionalWeakTable<WorldFileData, TagCompound> dataPerWorld = new();
	private static readonly FieldInfo AWorldListItem__data = typeof(AWorldListItem).GetField("_data", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

	private static void Load() {
		ILHelper.On<AWorldListItem>("GetIconElement", (Func<AWorldListItem, UIElement> orig, AWorldListItem self) => {
			var result = orig(self);
			if (self is UIWorldListItem) {
				var data = (WorldFileData)AWorldListItem__data.GetValue(self);
				return new LayeredWorldIconElement(data, dataPerWorld.GetValue(data, m => GetNewInstanceOfTagCompound(m)));
			}
			return result;
		});
		ILHelper.On<UIWorldListItem>("ctor", (Action<WorldFileData, int, bool> orig, WorldFileData data, int orderInList, bool canBePlayed) => {
			CreateDataPerWorldForFileData(data);
			orig(data, orderInList, canBePlayed);
		});
	}

	private static void CreateDataPerWorldForFileData(WorldFileData data) {
		string path = Path.ChangeExtension(data.Path, ".twld");
		if (File.Exists(path)) {
			var buf = FileUtilities.ReadAllBytes(path, data.IsCloudSave);
			var tag = TagIO.FromStream(new MemoryStream(buf));

			dataPerWorld.Add(data,
				tag.GetList<TagCompound>("modData")
				.FirstOrDefault(m =>
					m.Get<string>("mod") == AltLibrary.Instance.Name &&
					m.Get<string>("name") == nameof(WorldDataManager),
				GetNewInstanceOfTagCompound(data)
				)
			);
		}
	}

	private static TagCompound GetNewInstanceOfTagCompound(WorldFileData data) {
		return dataPerWorld.GetValue(data, data => {
			return new() {
				[WorldDataManager.BiomeDataKey] = new Dictionary<BiomeGroup, ModTypeData<IAltBiome>>() {
					[ModContent.GetInstance<EvilBiomeGroup>()] = data.HasCorruption ? ModContent.GetInstance<CorruptBiome>() : ModContent.GetInstance<CrimsonBiome>(),
					[ModContent.GetInstance<GoodBiomeGroup>()] = ModContent.GetInstance<HallowBiome>(),
					[ModContent.GetInstance<TropicsBiomeGroup>()] = ModContent.GetInstance<JungleBiome>(),
					[ModContent.GetInstance<UnderworldBiomeGroup>()] = ModContent.GetInstance<UnderworldBiome>(),
				},
				[WorldDataManager.OreDataKey] = new Dictionary<OreGroup, ModTypeData<IAltOre>>()
			};
		});
	}
}
