using Terraria.Localization;
using Terraria.ModLoader;

namespace AltLibrary.Common.AltOres
{
    internal sealed class VanillaOre : AltOre
    {
        public override string Texture => oreTexture;
        public override LocalizedText DisplayName => Language.GetText($"Mods.AltLibrary.Ores.{oreTexture}Name");
        public override LocalizedText Description => Language.GetText($"Mods.AltLibrary.Ores.{oreTexture}Desc");

        internal readonly string oreTexture;
        internal VanillaOre(string texture, int type, int ore, int bar)
        {
            oreTexture = texture;
            Type = type;
            this.ore = ore;
            this.bar = bar;
        }

        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
    }
}
