using AltLibrary.Common.AltBiomes;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.Localization;

namespace AltLibrary.Common.Hooks
{
    internal class DryadText
    {
        public static void Init()
        {
            IL.Terraria.Lang.GetDryadWorldStatusDialog += Lang_GetDryadWorldStatusDialog;
            IL.Terraria.WorldGen.AddUpAlignmentCounts += WorldGen_AddUpAlignmentCounts;
        }

        private static void WorldGen_AddUpAlignmentCounts(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchLdcI4(200)))
                return;
            if (!c.TryGotoNext(i => i.MatchLdcI4(200)))
                return;
            FieldReference tileCounts = null;
            if (!c.TryGotoNext(i => i.MatchLdsfld(out tileCounts)))
                return;

            c.Remove();
            c.EmitDelegate(() =>
            {
                foreach (AltBiome biome in AltLibrary.biomes)
                {
                    if (biome.BiomeType == BiomeType.Hallow)
                    {
                        if (biome.BiomeIce.HasValue)
                        {
                            WorldGen.totalGood2 += WorldGen.tileCounts[biome.BiomeIce.Value];
                        }
                        if (biome.BiomeGrass.HasValue)
                        {
                            WorldGen.totalGood2 += WorldGen.tileCounts[biome.BiomeIce.Value];
                        }
                        if (biome.BiomeStone.HasValue)
                        {
                            WorldGen.totalGood2 += WorldGen.tileCounts[biome.BiomeIce.Value];
                        }
                        if (biome.BiomeSand.HasValue)
                        {
                            WorldGen.totalGood2 += WorldGen.tileCounts[biome.BiomeIce.Value];
                        }
                    }
                    else if (biome.BiomeType == BiomeType.Evil)
                    {
                        if (biome.BiomeIce.HasValue)
                        {
                            WorldGen.totalEvil2 += WorldGen.tileCounts[biome.BiomeIce.Value];
                        }
                        if (biome.BiomeGrass.HasValue)
                        {
                            WorldGen.totalEvil2 += WorldGen.tileCounts[biome.BiomeGrass.Value];
                        }
                        if (biome.BiomeStone.HasValue)
                        {
                            WorldGen.totalEvil2 += WorldGen.tileCounts[biome.BiomeStone.Value];
                        }
                        if (biome.BiomeSand.HasValue)
                        {
                            WorldGen.totalEvil2 += WorldGen.tileCounts[biome.BiomeSand.Value];
                        }
                    }
                }
            });
            c.Emit(OpCodes.Ldsfld, tileCounts);
        }

        private static void Lang_GetDryadWorldStatusDialog(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchLdstr("")))
                return;

            c.Remove();
            c.EmitDelegate(() =>
            {
                string text2 = "";
                int tGood = WorldGen.tGood;
                int tEvil = WorldGen.tEvil + WorldGen.tBlood;
				if (tGood > 0 && tEvil > 0)
				{
					text2 = Language.GetTextValue("Mods.AltLibrary.DryadSpecialText.WorldStatusGoodEvil", Main.worldName, tGood, tEvil);
				}
				else if (tEvil > 0)
				{
					text2 = Language.GetTextValue("Mods.AltLibrary.DryadSpecialText.WorldStatusEvil", Main.worldName, tEvil);
				}
				else
				{
					if (tGood <= 0)
					{
						return Language.GetTextValue("DryadSpecialText.WorldStatusPure", Main.worldName);
					}
					text2 = Language.GetTextValue("Mods.AltLibrary.DryadSpecialText.WorldStatusGood", Main.worldName, tGood);
				}
				string arg = (tGood * 1.2 >= tEvil && tGood * 0.8 <= tEvil) ? Language.GetTextValue("DryadSpecialText.WorldDescriptionBalanced") : ((tGood >= tEvil) ? Language.GetTextValue("DryadSpecialText.WorldDescriptionFairyTale") : ((tEvil > tGood + 20) ? Language.GetTextValue("DryadSpecialText.WorldDescriptionGrim") : ((tEvil <= 5) ? Language.GetTextValue("DryadSpecialText.WorldDescriptionClose") : Language.GetTextValue("DryadSpecialText.WorldDescriptionWork"))));
				return text2 + " " + arg;
			});
            c.Emit(OpCodes.Ret);
            c.Emit(OpCodes.Ldstr, "");
        }
    }
}
