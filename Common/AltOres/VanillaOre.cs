using Terraria.ModLoader;

namespace AltLibrary.Common.AltOres
{
    internal sealed class VanillaOre : AltOre
    {
        public override string Texture => oreTexture;
        public override string Name => oreTexture;

        internal readonly string oreTexture;
        private readonly string name;
        private readonly string desc;
        internal VanillaOre(string texture, string name, int type, int ore, int bar, OreType oreType, string desc = "", bool avalon = false)
        {
            oreTexture = texture;
            OreType = oreType;
            Type = type;
            this.name = name;
            this.ore = ore;
            this.bar = bar;
            this.desc = desc;
            IsForAvalon = avalon;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(name);
            Description.SetDefault(desc);
        }

        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
    }
}
