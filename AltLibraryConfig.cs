using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Terraria.ModLoader.Config;

namespace AltLibrary
{
#pragma warning disable CS0649
	internal class AltLibraryConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;
		public static AltLibraryConfig Config;

		public struct WorldDataValues
		{
			public string worldEvil;
			public string worldHallow;
			public string worldHell;
			public string worldJungle;
			public string drunkEvil;
		}

		[Label("$Mods.AltLibrary.Config.VanillaShowUpIfOnlyAltVarExist.Label")]
		[Tooltip("$Mods.AltLibrary.Config.VanillaShowUpIfOnlyAltVarExist.Tooltip")]
		[DefaultValue(true)]
		public bool VanillaShowUpIfOnlyAltVarExist;
		[Label("$Mods.AltLibrary.Config.SpecialSeedWorldPreview.Label")]
		[Tooltip("$Mods.AltLibrary.Config.SpecialSeedWorldPreview.Tooltip")]
		[DefaultValue(true)]
		public bool SpecialSeedWorldPreview;
		[Label("$Mods.AltLibrary.Config.PreviewVisible.Label")]
		[Tooltip("$Mods.AltLibrary.Config.PreviewVisible.Tooltip")]
		[OptionStrings(new string[] { "None", "Hallow only", "Jungle only", "Both" })]
		[DefaultValue("Hallow only")]
		public string PreviewVisible;
		[Label("$Mods.AltLibrary.Config.BiomeIconsVisibleOutsideBiomeUI.Label")]
		[Tooltip("$Mods.AltLibrary.Config.BiomeIconsVisibleOutsideBiomeUI.Tooltip")]
		[DefaultValue(true)]
		public bool BiomeIconsVisibleOutsideBiomeUI;
		[Label("$Mods.AltLibrary.Config.OreIconsVisibleOutsideOreUI.Label")]
		[Tooltip("$Mods.AltLibrary.Config.OreIconsVisibleOutsideOreUI.Tooltip")]
		[DefaultValue(true)]
		public bool OreIconsVisibleOutsideOreUI;

#pragma warning restore CS0649

		public override void OnLoaded() => Config = this;

		[DefaultListValue(false)]
		[JsonProperty]
		private Dictionary<string, WorldDataValues> worldData = new();

		public Dictionary<string, WorldDataValues> GetWorldData() => worldData;
		public void SetWorldData(Dictionary<string, WorldDataValues> newDict) => worldData = newDict;
		public static void Save(ModConfig config)
		{
			Directory.CreateDirectory(ConfigManager.ModConfigPath);
			string filename = config.Mod.Name + "_" + config.Name + ".json";
			string path = Path.Combine(ConfigManager.ModConfigPath, filename);
			string json = JsonConvert.SerializeObject(config, ConfigManager.serializerSettings);
			File.WriteAllText(path, json);
		}
	}
}
