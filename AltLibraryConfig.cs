using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Terraria.ModLoader.Config;

namespace AltLibrary
{
    internal class AltLibraryConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        public static AltLibraryConfig Config;

        public struct WorldDataValues
        {
            public string worldEvil;
            public string worldHallow;
            public string worldHell;
            public string worldJungle;
        }

        public override void OnLoaded()
        {
            Config = this;
        }

        [DefaultListValue(false)]
        [JsonProperty]
        private Dictionary<string, WorldDataValues> worldData = new();

        public Dictionary<string, WorldDataValues> GetWorldData() { return worldData; }
        public void SetWorldData(Dictionary<string, WorldDataValues> newDict) { worldData = newDict; }
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
