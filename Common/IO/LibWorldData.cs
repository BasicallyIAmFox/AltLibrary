using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Attributes;
using AltLibrary.Content.Biomes;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.IO;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;

namespace AltLibrary.Common.IO;

[LoadableContent(ContentOrder.Content, nameof(Load), UnloadName = nameof(Unload))]
public class LibWorldData {
	internal static ConcurrentDictionary<WorldFileData, LibWorldData> libWorldData = new();
	private static readonly Type libWorldDataType = typeof(ConcurrentDictionary<WorldFileData, LibWorldData>);

	private static readonly string path = Path.Combine(Program.SavePath, "AltLibrary stuff", "LibWorldData.str");

	private static void Load() {
		if (File.Exists(path)) {
			using var reader = File.OpenText(path);
			libWorldData = (ConcurrentDictionary<WorldFileData, LibWorldData>)JsonSerializer.Create().Deserialize(reader, libWorldDataType);
		}

		ILHelper.On<WorldFile>("CreateMetadata", (Func<string, bool, int, WorldFileData> orig, string name, bool cloudSave, int GameMode) => {
			var ret = orig(name, cloudSave, GameMode);
			if (ret != null) {
				libWorldData.TryAdd(ret, new LibWorldData {
					WorldEvil = ret.HasCorruption ? new(ModContent.GetInstance<CorruptBiome>()) : new(ModContent.GetInstance<CrimsonBiome>()),
					WorldGood = new(ModContent.GetInstance<HallowBiome>()),
					WorldTropics = new(ModContent.GetInstance<JungleBiome>()),
					WorldUnderworld = new(ModContent.GetInstance<UnderworldBiome>()),
				});
			}
			return ret;
		});
	}

	private static void Unload() {
		using var writer = File.CreateText(path);
		JsonSerializer.Create().Serialize(writer, libWorldData, libWorldDataType);
	}

	public ModTypeData<IAltBiome> WorldEvil;
	public ModTypeData<IAltBiome> WorldGood;
	public ModTypeData<IAltBiome> WorldTropics;
	public ModTypeData<IAltBiome> WorldUnderworld;
}
