using AltLibrary.Common.AltBiomes;
using AltLibrary.Core;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace AltLibrary
{
    public class AltLibrary : Mod
    {
        private static AltLibrary instance;

        public static AltLibrary Instance { get => instance; set => instance = value; }

        public static List<AltBiome> biomes = new();

        public static Dictionary<string, float> hellAltTrans = new();

        public AltLibrary()
        {
            Instance = this;
        }

        public override void Load()
        {
            ILHooks.OnInitialize();
        }

        public override void Unload()
        {
            AltLibraryConfig.Config = null;
        }
    }
}