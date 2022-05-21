using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AltLibrary.Common.Hooks
{
    internal class SmashAltarInfection
    {
        public static void Init()
        {
            IL.Terraria.WorldGen.SmashAltar += WorldGen_SmashAltar;
        }

        public static void Unload()
        {
            IL.Terraria.WorldGen.SmashAltar -= WorldGen_SmashAltar;
        }

        private static void WorldGen_SmashAltar(ILContext il)
        {
            ILCursor c = new(il);

            for (int j = 0; j < 3; j++)
            {
                if (!c.TryGotoNext(i => i.MatchLdsfld<Lang>(nameof(Lang.misc)),
                    i => i.MatchLdloc(7 + j),
                    i => i.MatchLdelemRef(),
                    i => i.MatchCallvirt<LocalizedText>("get_Value")))
                {
                    AltLibrary.Instance.Logger.Info("n $ 1 " + j);
                    return;
                }
                c.Index += 4;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldc_I4, j);
                c.EmitDelegate<Func<int, string>>((j) =>
                {
                    string key = "";
                    if (j == 0)
                    {
                        if (WorldGen.SavedOreTiers.Cobalt == TileID.Cobalt)
                        {
                            key = Language.GetTextValue("Mods.AltLibrary.AltOreName.Cobalt");
                        }
                        else if (WorldGen.SavedOreTiers.Cobalt == TileID.Palladium)
                        {
                            key = Language.GetTextValue("Mods.AltLibrary.AltOreName.Palladium");
                        }
                        else
                        {
                            key = AltLibrary.Ores.First(o => o.OreType == OreType.Cobalt && o.ore == WorldGen.SavedOreTiers.Cobalt).DisplayName.GetTranslation(Language.ActiveCulture);
                        }
                    }
                    else if (j == 1)
                    {
                        if (WorldGen.SavedOreTiers.Mythril == TileID.Mythril)
                        {
                            key = Language.GetTextValue("Mods.AltLibrary.AltOreName.Mythril");
                        }
                        else if (WorldGen.SavedOreTiers.Mythril == TileID.Orichalcum)
                        {
                            key = Language.GetTextValue("Mods.AltLibrary.AltOreName.Orichalcum");
                        }
                        else
                        {
                            key = AltLibrary.Ores.First(o => o.OreType == OreType.Mythril && o.ore == WorldGen.SavedOreTiers.Mythril).DisplayName.GetTranslation(Language.ActiveCulture);
                        }
                    }
                    else
                    {
                        if (WorldGen.SavedOreTiers.Adamantite == TileID.Adamantite)
                        {
                            key = Language.GetTextValue("Mods.AltLibrary.AltOreName.Adamantite");
                        }
                        else if (WorldGen.SavedOreTiers.Adamantite == TileID.Titanium)
                        {
                            key = Language.GetTextValue("Mods.AltLibrary.AltOreName.Titanium");
                        }
                        else
                        {
                            key = AltLibrary.Ores.First(o => o.OreType == OreType.Adamantite && o.ore == WorldGen.SavedOreTiers.Adamantite).DisplayName.GetTranslation(Language.ActiveCulture);
                        }
                    }
                    return Language.GetTextValue("Mods.AltLibrary.HardmodeOre", key);
                });

                if (!c.TryGotoNext(i => i.MatchLdsfld<Lang>(nameof(Lang.misc)),
                    i => i.MatchLdloc(7 + j),
                    i => i.MatchLdelemRef(),
                    i => i.MatchLdfld<LocalizedText>(nameof(LocalizedText.Key)),
                    i => i.MatchCall(out _),
                    i => i.MatchCall<NetworkText>(nameof(NetworkText.FromKey))))
                {
                    AltLibrary.Instance.Logger.Info("n $ 2 " + j);
                    return;
                }
                c.Index += 6;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldc_I4, j);
                c.EmitDelegate<Func<int, NetworkText>>((j) =>
                {
                    string key = "";
                    if (j == 0)
                    {
                        if (WorldGen.SavedOreTiers.Cobalt == TileID.Cobalt)
                        {
                            key = Language.GetTextValue("Mods.AltLibrary.AltOreName.Cobalt");
                        }
                        else if (WorldGen.SavedOreTiers.Cobalt == TileID.Palladium)
                        {
                            key = Language.GetTextValue("Mods.AltLibrary.AltOreName.Palladium");
                        }
                        else
                        {
                            key = AltLibrary.Ores.First(o => o.OreType == OreType.Cobalt && o.ore == WorldGen.SavedOreTiers.Cobalt).DisplayName.GetTranslation(Language.ActiveCulture);
                        }
                    }
                    else if (j == 1)
                    {
                        if (WorldGen.SavedOreTiers.Mythril == TileID.Mythril)
                        {
                            key = Language.GetTextValue("Mods.AltLibrary.AltOreName.Mythril");
                        }
                        else if (WorldGen.SavedOreTiers.Mythril == TileID.Orichalcum)
                        {
                            key = Language.GetTextValue("Mods.AltLibrary.AltOreName.Orichalcum");
                        }
                        else
                        {
                            key = AltLibrary.Ores.First(o => o.OreType == OreType.Mythril && o.ore == WorldGen.SavedOreTiers.Mythril).DisplayName.GetTranslation(Language.ActiveCulture);
                        }
                    }
                    else
                    {
                        if (WorldGen.SavedOreTiers.Adamantite == TileID.Adamantite)
                        {
                            key = Language.GetTextValue("Mods.AltLibrary.AltOreName.Adamantite");
                        }
                        else if (WorldGen.SavedOreTiers.Adamantite == TileID.Titanium)
                        {
                            key = Language.GetTextValue("Mods.AltLibrary.AltOreName.Titanium");
                        }
                        else
                        {
                            key = AltLibrary.Ores.First(o => o.OreType == OreType.Adamantite && o.ore == WorldGen.SavedOreTiers.Adamantite).DisplayName.GetTranslation(Language.ActiveCulture);
                        }
                    }
                    return NetworkText.FromKey("Mods.AltLibrary.HardmodeOre", key);
                });
            }

            if (!c.TryGotoNext(i => i.MatchLdcI4(25)))
            {
                AltLibrary.Instance.Logger.Info("n $ 3");
                return;
            }

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (!AltLibraryConfig.Config.SmashingAltarsSpreadsRandom)
                {
                    if (WorldBiomeManager.WorldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeStone.HasValue)
                    {
                        return ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeStone.Value;
                    }
                }
                else
                {
                    List<int> indexToUse = new()
                    {
                        orig
                    };
                    AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Evil && x.BiomeStone.HasValue)
                                        .ToList()
                                        .ForEach(x => indexToUse.Add(x.BiomeStone.Value));
                    return indexToUse[Main.rand.Next(indexToUse.Count)];
                }
                return orig;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(117)))
            {
                AltLibrary.Instance.Logger.Info("n $ 4");
                return;
            }

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (!AltLibraryConfig.Config.SmashingAltarsSpreadsRandom)
                {
                    if (WorldBiomeManager.WorldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeStone.HasValue)
                    {
                        return ModContent.Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeStone.Value;
                    }
                }
                else
                {
                    List<int> indexToUse = new()
                    {
                        orig
                    };
                    AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Hallow && x.BiomeStone.HasValue)
                                        .ToList()
                                        .ForEach(x => indexToUse.Add(x.BiomeStone.Value));
                    return indexToUse[Main.rand.Next(indexToUse.Count)];
                }
                return orig;
            });
        }
    }
}
