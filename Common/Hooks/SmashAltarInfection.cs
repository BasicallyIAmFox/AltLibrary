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
    [Autoload(Side = ModSide.Both)]
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

            if (!c.TryGotoNext(i => i.MatchLdsfld<Main>(nameof(Main.drunkWorld))))
            {
                AltLibrary.Instance.Logger.Info("n $ 0");
                return;
            }

            c.Index++;
            c.Emit(OpCodes.Pop);
            c.EmitDelegate(() =>
            {
                /*if (Main.drunkWorld)
                {
                    int index = AltLibrary.OrderedAdamant[WorldBiomeManager.adamIndex % AltLibrary.OrderedAdamant.Count];
                    WorldGen.SavedOreTiers.Adamantite = index;
                    WorldBiomeManager.adamIndex++;
                    if (WorldBiomeManager.adamIndex >= AltLibrary.OrderedAdamant.Count)
                    {
                        WorldBiomeManager.adamIndex = 0;
                    }
                }*/
                return false;
            });

            for (int j = 0; j < 3; j++)
            {
                if (j == 1)
                {
                    if (!c.TryGotoNext(i => i.MatchLdsfld<Main>(nameof(Main.drunkWorld))))
                    {
                        AltLibrary.Instance.Logger.Info("n $ -1 " + j);
                        return;
                    }

                    c.Index++;
                    c.Emit(OpCodes.Pop);
                    c.EmitDelegate(() =>
                    {
                        /*if (Main.drunkWorld)
                        {
                            int index = AltLibrary.OrderedMythril[WorldBiomeManager.mythIndex % AltLibrary.OrderedMythril.Count];
                            WorldGen.SavedOreTiers.Mythril = index;
                            WorldBiomeManager.mythIndex++;
                            if (WorldBiomeManager.mythIndex >= AltLibrary.OrderedMythril.Count)
                            {
                                WorldBiomeManager.mythIndex = 0;
                            }
                        }*/
                        return false;
                    });
                }
                else if (j == 2)
                {
                    if (!c.TryGotoNext(i => i.MatchLdsfld<Main>(nameof(Main.drunkWorld))))
                    {
                        AltLibrary.Instance.Logger.Info("n $ -2 " + j);
                        return;
                    }

                    c.Index++;
                    c.Emit(OpCodes.Pop);
                    c.EmitDelegate(() =>
                    {
                        /*if (Main.drunkWorld)
                        {
                            int index = AltLibrary.OrderedCobalt[WorldBiomeManager.cobaIndex % AltLibrary.OrderedCobalt.Count];
                            WorldGen.SavedOreTiers.Cobalt = index;
                            WorldBiomeManager.cobaIndex++;
                            if (WorldBiomeManager.cobaIndex >= AltLibrary.OrderedCobalt.Count)
                            {
                                WorldBiomeManager.cobaIndex = 0;
                            }
                            AltLibrary.Instance.Logger.Info(WorldGen.SavedOreTiers.Cobalt.ToString());
                        }*/
                        return false;
                    });
                }

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
                        //int index = AltLibrary.OrderedCobalt[WorldBiomeManager.cobaIndex % AltLibrary.OrderedCobalt.Count];
                        if (WorldGen.SavedOreTiers.Cobalt == TileID.Cobalt)
                        {
                            key = Lang.misc[12].Value;
                        }
                        else if (WorldGen.SavedOreTiers.Cobalt == TileID.Palladium)
                        {
                            key = Lang.misc[21].Value;
                        }
                        else
                        {
                            key = AltLibrary.Ores.First(o => o.OreType == OreType.Cobalt && o.ore == WorldGen.SavedOreTiers.Cobalt).BlessingMessage.GetTranslation(Language.ActiveCulture);
                        }
                    }
                    else if (j == 1)
                    {
                        if (WorldGen.SavedOreTiers.Mythril == TileID.Mythril)
                        {
                            key = Lang.misc[13].Value;
                        }
                        else if (WorldGen.SavedOreTiers.Mythril == TileID.Orichalcum)
                        {
                            key = Lang.misc[22].Value;
                        }
                        else
                        {
                            key = AltLibrary.Ores.First(o => o.OreType == OreType.Mythril && o.ore == WorldGen.SavedOreTiers.Mythril).BlessingMessage.GetTranslation(Language.ActiveCulture);
                        }
                    }
                    else
                    {
                        if (WorldGen.SavedOreTiers.Adamantite == TileID.Adamantite)
                        {
                            key = Lang.misc[14].Value;
                        }
                        else if (WorldGen.SavedOreTiers.Adamantite == TileID.Titanium)
                        {
                            key = Lang.misc[23].Value;
                        }
                        else
                        {
                            key = AltLibrary.Ores.First(o => o.OreType == OreType.Adamantite && o.ore == WorldGen.SavedOreTiers.Adamantite).BlessingMessage.GetTranslation(Language.ActiveCulture);
                        }
                    }
                    return key;
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
                        //int index = AltLibrary.OrderedCobalt[WorldBiomeManager.cobaIndex % AltLibrary.OrderedCobalt.Count];
                        if (WorldGen.SavedOreTiers.Cobalt == TileID.Cobalt)
                        {
                            key = Lang.misc[12].Value;
                        }
                        else if (WorldGen.SavedOreTiers.Cobalt == TileID.Palladium)
                        {
                            key = Lang.misc[21].Value;
                        }
                        else
                        {
                            key = AltLibrary.Ores.First(o => o.OreType == OreType.Cobalt && o.ore == WorldGen.SavedOreTiers.Cobalt).BlessingMessage.GetTranslation(Language.ActiveCulture);
                        }
                    }
                    else if (j == 1)
                    {
                        if (WorldGen.SavedOreTiers.Mythril == TileID.Mythril)
                        {
                            key = Lang.misc[13].Value;
                        }
                        else if (WorldGen.SavedOreTiers.Mythril == TileID.Orichalcum)
                        {
                            key = Lang.misc[22].Value;
                        }
                        else
                        {
                            key = AltLibrary.Ores.First(o => o.OreType == OreType.Mythril && o.ore == WorldGen.SavedOreTiers.Mythril).BlessingMessage.GetTranslation(Language.ActiveCulture);
                        }
                    }
                    else
                    {
                        if (WorldGen.SavedOreTiers.Adamantite == TileID.Adamantite)
                        {
                            key = Lang.misc[14].Value;
                        }
                        else if (WorldGen.SavedOreTiers.Adamantite == TileID.Titanium)
                        {
                            key = Lang.misc[23].Value;
                        }
                        else
                        {
                            key = AltLibrary.Ores.First(o => o.OreType == OreType.Adamantite && o.ore == WorldGen.SavedOreTiers.Adamantite).BlessingMessage.GetTranslation(Language.ActiveCulture);
                        }
                    }
                    return NetworkText.FromLiteral(key);
                });
            }

            if (!c.TryGotoNext(i => i.MatchLdcI4(203)))
            {
                AltLibrary.Instance.Logger.Info("n $ 4");
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
                        orig,
                        TileID.Ebonstone
                    };
                    AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Evil && x.BiomeStone.HasValue)
                                        .ToList()
                                        .ForEach(x => indexToUse.Add(x.BiomeStone.Value));
                    return Main.rand.Next(indexToUse);
                }
                return orig;
            });

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
                        orig,
                        TileID.Crimstone
                    };
                    AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Evil && x.BiomeStone.HasValue)
                                        .ToList()
                                        .ForEach(x => indexToUse.Add(x.BiomeStone.Value));
                    return Main.rand.Next(indexToUse);
                }
                return orig;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(117)))
            {
                AltLibrary.Instance.Logger.Info("n $ 5");
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
                    return Main.rand.Next(indexToUse);
                }
                return orig;
            });
        }
    }
}
