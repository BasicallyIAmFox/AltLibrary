using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.AltOres;
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
using Terraria.Utilities;

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
            c.Emit(OpCodes.Ldloc_0);
            c.EmitDelegate(GetDrunkSmashingData);

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
                    c.EmitDelegate(() => false);
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
                    c.EmitDelegate(() => false);
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
                c.EmitDelegate(GetSmashAltarText);

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
                c.EmitDelegate<Func<int, NetworkText>>((j) => NetworkText.FromLiteral(GetSmashAltarText(j)));
            }

            if (!c.TryGotoNext(i => i.MatchLdcI4(203)))
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

        //move to utility function later
        private static void ShuffleArrayUsingSeed<T>(T[] list, UnifiedRandom seed)
        {
            int randIters = list.Length;
            while (randIters > 0)
            {
                int thisRand = seed.Next(randIters);
                randIters--;
                if (thisRand != randIters)
                {
                    (list[thisRand], list[randIters]) = (list[randIters], list[thisRand]);
                }
            }
        }

        private static bool GetDrunkSmashingData(bool drunk, int smashType)
        {
            if (!drunk || smashType != 0)
                return false;

            //bake this entire block later
            #region inefficientSetupWhichShouldBeBaked
            UnifiedRandom rngSeed = new(WorldGen._genRandSeed); //bake seed later
            List<AltOre> hardmodeListing = new();
            hardmodeListing.Clear();
            hardmodeListing.Add(new VanillaOre("Cobalt", "Cobalt", -9, TileID.Cobalt, ItemID.CobaltBar, OreType.Cobalt));
            hardmodeListing.Add(new VanillaOre("Palladium", "Palladium", -10, TileID.Palladium, ItemID.PalladiumBar, OreType.Cobalt));
            hardmodeListing.AddRange(AltLibrary.Ores.Where(x => x.OreType == OreType.Cobalt && x.Selectable));
            hardmodeListing.Add(new VanillaOre("Mythril", "Mythril", -11, TileID.Mythril, ItemID.MythrilBar, OreType.Mythril));
            hardmodeListing.Add(new VanillaOre("Orichalcum", "Orichalcum", -12, TileID.Orichalcum, ItemID.OrichalcumBar, OreType.Mythril));
            hardmodeListing.AddRange(AltLibrary.Ores.Where(x => x.OreType == OreType.Mythril && x.Selectable));
            hardmodeListing.Add(new VanillaOre("Adamantite", "Adamantite", -13, TileID.Adamantite, ItemID.AdamantiteBar, OreType.Adamantite));
            hardmodeListing.Add(new VanillaOre("Titanium", "Titanium", -14, TileID.Titanium, ItemID.TitaniumBar, OreType.Adamantite));
            hardmodeListing.AddRange(AltLibrary.Ores.Where(x => x.OreType == OreType.Adamantite && x.Selectable));

            AltOre[] cobaltAlts = hardmodeListing.Where(x => x.OreType == OreType.Cobalt).ToArray();
            AltOre[] mythrilOres = hardmodeListing.Where(x => x.OreType == OreType.Mythril).ToArray();
            AltOre[] adamantiteores = hardmodeListing.Where(x => x.OreType == OreType.Adamantite).ToArray();
            ShuffleArrayUsingSeed(cobaltAlts, rngSeed);
            ShuffleArrayUsingSeed(mythrilOres, rngSeed);
            ShuffleArrayUsingSeed(adamantiteores, rngSeed);
            #endregion

            WorldGen.SavedOreTiers.Cobalt = cobaltAlts[WorldBiomeManager.hmOreIndex % cobaltAlts.Length].ore;
            WorldGen.SavedOreTiers.Mythril = mythrilOres[WorldBiomeManager.hmOreIndex % mythrilOres.Length].ore;
            WorldGen.SavedOreTiers.Adamantite = adamantiteores[WorldBiomeManager.hmOreIndex % adamantiteores.Length].ore;
            WorldBiomeManager.hmOreIndex++;

            return false;
        }

        private static string GetTranslation(AltOre ore)
        {
            return ore.BlessingMessage.GetTranslation(Language.ActiveCulture) ?? Language.GetTextValue("Mods.AltLibrary.BlessBase", ore.DisplayName.GetTranslation(Language.ActiveCulture));
        }

        private static string GetSmashAltarText(int j)
        {
            string key = "";
            key = j switch
            {
                0 => WorldGen.SavedOreTiers.Cobalt switch
                {
                    TileID.Cobalt => Lang.misc[12].Value,
                    TileID.Palladium => Lang.misc[21].Value,
                    _ => GetTranslation(AltLibrary.Ores.First(o => o.OreType == OreType.Cobalt && o.ore == WorldGen.SavedOreTiers.Cobalt)),
                },
                1 => WorldGen.SavedOreTiers.Mythril switch
                {
                    TileID.Mythril => Lang.misc[13].Value,
                    TileID.Orichalcum => Lang.misc[22].Value,
                    _ => GetTranslation(AltLibrary.Ores.First(o => o.OreType == OreType.Mythril && o.ore == WorldGen.SavedOreTiers.Mythril)),
                },
                _ => WorldGen.SavedOreTiers.Adamantite switch
                {
                    TileID.Adamantite => Lang.misc[14].Value,
                    TileID.Titanium => Lang.misc[23].Value,
                    _ => GetTranslation(AltLibrary.Ores.First(o => o.OreType == OreType.Adamantite && o.ore == WorldGen.SavedOreTiers.Adamantite)),
                },
            };
            return key;
        }
    }
}
