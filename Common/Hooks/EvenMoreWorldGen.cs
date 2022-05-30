using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace AltLibrary.Common.Hooks
{
    [Autoload(Side = ModSide.Both)]
    internal static class EvenMoreWorldGen
    {
        public static void Init()
        {
            IL.Terraria.WorldGen.GenerateWorld += GenPasses.ILGenerateWorld;
            GenPasses.HookGenPassReset += GenPasses_HookGenPassReset;
            GenPasses.HookGenPassShinies += GenPasses_HookGenPassShinies;
            GenPasses.HookGenPassCorruption += ILGenPassCorruption;
            GenPasses.HookGenPassAltars += ILGenPassAltars;
            GenPasses.HookGenPassMicroBiomes += GenPasses_HookGenPassMicroBiomes;
        }

        public static void Unload()
        {
            IL.Terraria.WorldGen.GenerateWorld -= GenPasses.ILGenerateWorld;
            GenPasses.HookGenPassReset -= GenPasses_HookGenPassReset;
            GenPasses.HookGenPassShinies -= GenPasses_HookGenPassShinies;
            GenPasses.HookGenPassCorruption -= ILGenPassCorruption;
            GenPasses.HookGenPassAltars -= ILGenPassAltars;
            GenPasses.HookGenPassMicroBiomes -= GenPasses_HookGenPassMicroBiomes;
        }

        private static void GenPasses_HookGenPassMicroBiomes(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchLdstr("LivingTreeCount")))
            {
                AltLibrary.Instance.Logger.Info("c $ 1");
                return;
            }
            if (!c.TryGotoPrev(i => i.MatchCallvirt<GenerationProgress>("Set")))
            {
                AltLibrary.Instance.Logger.Info("c $ 2");
                return;
            }

            var label = il.DefineLabel();

            c.Index++;
            c.EmitDelegate(() => WorldBiomeManager.WorldJungle == "");
            //c.Emit(OpCodes.Ldsfld, typeof(WorldBiomeManager).GetField(nameof(WorldBiomeManager.worldJungle), BindingFlags.Public | BindingFlags.Static));
            //c.Emit(OpCodes.Ldstr, "");
            //c.Emit(OpCodes.Call, typeof(string).GetMethod("op_Equality", new Type[] { typeof(string), typeof(string) }));
            c.Emit(OpCodes.Brfalse_S, label);

            if (!c.TryGotoNext(i => i.MatchLdstr("..Long Minecart Tracks")))
            {
                AltLibrary.Instance.Logger.Info("c $ 3");
                return;
            }
            if (!c.TryGotoPrev(i => i.MatchLdarg(1)))
            {
                AltLibrary.Instance.Logger.Info("c $ 4");
                return;
            }

            c.MarkLabel(label);
        }

        private static void ILGenPassCorruption(ILContext il)
        {
            ILCursor c = new(il);

            ILLabel label = c.DefineLabel();

            ALUtils.ReplaceIDs(il, TileID.JungleGrass,
                (orig) => (ushort?)AltLibrary.Biomes.Find(x => x.FullName == WorldBiomeManager.WorldJungle).BiomeGrass ?? TileID.JungleGrass,
                (orig) => WorldBiomeManager.WorldJungle != "");

            ALUtils.ReplaceIDs(il, TileID.Crimsand,
                (orig) =>
                {
                    int value1 = TileID.Crimsand;
                    int value2 = TileID.Ebonsand;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeSand ?? TileID.Sand) : TileID.Sand;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                },
                (orig) => true);
            ALUtils.ReplaceIDs(il, TileID.CrimsonGrass,
                (orig) =>
                {
                    int value1 = TileID.CrimsonGrass;
                    int value2 = TileID.CorruptGrass;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeGrass ?? TileID.Grass) : TileID.Grass;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                },
                (orig) => true);
            ALUtils.ReplaceIDs(il, TileID.FleshIce,
                (orig) =>
                {
                    int value1 = TileID.FleshIce;
                    int value2 = TileID.CorruptIce;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeIce ?? TileID.IceBlock) : TileID.IceBlock;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                },
                (orig) => true);
            ALUtils.ReplaceIDs(il, TileID.CrimsonSandstone,
                (orig) =>
                {
                    int value1 = TileID.CrimsonSandstone;
                    int value2 = TileID.CorruptSandstone;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeSandstone ?? TileID.Sandstone) : TileID.Sandstone;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                },
                (orig) => true);
            ALUtils.ReplaceIDs(il, TileID.CrimsonHardenedSand,
                (orig) =>
                {
                    int value1 = TileID.CrimsonHardenedSand;
                    int value2 = TileID.CorruptHardenedSand;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeHardenedSand ?? TileID.HardenedSand) : TileID.HardenedSand;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                },
                (orig) => true);

            ALUtils.ReplaceIDs(il, TileID.Ebonstone,
                (orig) => (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeStone.Value,
                (orig) => WorldBiomeManager.WorldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeStone.HasValue);
            ALUtils.ReplaceIDs(il, TileID.Ebonsand,
                (orig) => (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeSand.Value,
                (orig) => WorldBiomeManager.WorldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeSand.HasValue);
            ALUtils.ReplaceIDs(il, TileID.CorruptGrass,
                (orig) => (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeGrass.Value,
                (orig) => WorldBiomeManager.WorldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeGrass.HasValue);
            ALUtils.ReplaceIDs(il, TileID.CorruptIce,
                (orig) => (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeIce.Value,
                (orig) => WorldBiomeManager.WorldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeIce.HasValue);
            ALUtils.ReplaceIDs(il, TileID.CorruptSandstone,
                (orig) => (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeSandstone.Value,
                (orig) => WorldBiomeManager.WorldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeSandstone.HasValue);
            ALUtils.ReplaceIDs(il, TileID.CorruptHardenedSand,
                (orig) => (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeHardenedSand.Value,
                (orig) => WorldBiomeManager.WorldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeHardenedSand.HasValue);

            ALUtils.ReplaceIDs<ushort>(il, 216,
                (orig) => ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).WallContext.wallsReplacement.TryGetValue(orig, out ushort value) ? value : WallID.None,
                (orig) => WorldBiomeManager.WorldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).WallContext.wallsReplacement.ContainsKey(orig));
            ALUtils.ReplaceIDs<ushort>(il, 187,
                (orig) => ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).WallContext.wallsReplacement.TryGetValue(orig, out ushort value) ? value : WallID.None,
                (orig) => WorldBiomeManager.WorldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).WallContext.wallsReplacement.ContainsKey(orig));

            if (!c.TryGotoNext(i => i.MatchLdarg(1),
                i => i.MatchLdsfld<Lang>(nameof(Lang.gen)),
                i => i.MatchLdcI4(72),
                i => i.MatchLdelemRef(),
                i => i.MatchCallOrCallvirt<LocalizedText>("get_Value"),
                i => i.MatchCallOrCallvirt<GenerationProgress>("set_Message")))
            {
                AltLibrary.Instance.Logger.Info("d $ 1");
                return;
            }

            c.Index += 7;
            c.Emit(OpCodes.Ldarg, 1);
            c.EmitDelegate<Action<GenerationProgress>>((progress) =>
            {
                if (!WorldGen.crimson)
                {
                    if (WorldBiomeManager.drunkEvil == "")
                    {
                        progress.Message = Lang.misc[20].Value;
                    }
                    else
                    {
                        if (WorldBiomeManager.drunkEvil == "Terraria/Corrupt")
                        {
                            progress.Message = Lang.misc[20].Value;
                        }
                        else if (WorldBiomeManager.drunkEvil == "Terraria/Crimson")
                        {
                            progress.Message = Lang.misc[72].Value;
                        }
                        else
                        {
                            progress.Message = AltLibrary.Biomes.Find(x => x.FullName == WorldBiomeManager.drunkEvil).GenPassName.GetTranslation(Language.ActiveCulture) ?? "Creating " + AltLibrary.Biomes.Find(x => x.FullName == WorldBiomeManager.drunkEvil).Name;
                        }
                    }
                }
            });

            if (!c.TryGotoNext(i => i.MatchLdloc(20),
                i => i.MatchLdsfld<WorldGen>(nameof(WorldGen.worldSurfaceLow)),
                i => i.MatchConvI4(),
                i => i.MatchLdcI4(10),
                i => i.MatchSub(),
                i => i.MatchCall<WorldGen>("CrimStart")))
            {
                AltLibrary.Instance.Logger.Info("d $ 2");
                return;
            }

            c.EmitDelegate(() => !WorldGen.crimson);
            c.Emit(OpCodes.Brtrue_S, label);
            c.Index += 6;
            c.MarkLabel(label);

            if (!c.TryGotoNext(i => i.MatchCall<WorldGen>("CrimPlaceHearts")))
            {
                AltLibrary.Instance.Logger.Info("d $ 3");
                return;
            }

            c.EmitDelegate(() => !WorldGen.crimson);
            c.Emit(OpCodes.Brtrue_S, label);
            c.Index++;
            c.MarkLabel(label);

            if (!c.TryGotoNext(i => i.MatchLdarg(1),
                i => i.MatchLdsfld<Lang>(nameof(Lang.gen)),
                i => i.MatchLdcI4(20),
                i => i.MatchLdelemRef(),
                i => i.MatchCallOrCallvirt<LocalizedText>("get_Value"),
                i => i.MatchCallOrCallvirt<GenerationProgress>("set_Message")))
            {
                AltLibrary.Instance.Logger.Info("d $ 4");
                return;
            }

            c.Index += 6;
            c.Emit(OpCodes.Ldarg, 1);
            c.EmitDelegate<Action<GenerationProgress>>((progress) =>
            {
                if (WorldBiomeManager.WorldEvil != "")
                {
                    progress.Message = AltLibrary.Biomes.Find(x => x.FullName == WorldBiomeManager.WorldEvil).GenPassName.GetTranslation(Language.ActiveCulture);
                }
            });

            if (!c.TryGotoNext(i => i.MatchLdloc(48),
                i => i.MatchStloc(63),
                i => i.MatchBr(out _)))
            {
                AltLibrary.Instance.Logger.Info("d $ 5");
                return;
            }
            label = c.DefineLabel();
            c.EmitDelegate(() => WorldBiomeManager.WorldEvil == "");
            c.Emit(OpCodes.Brtrue_S, label);
            if (!c.TryGotoNext(i => i.MatchLdloc(63),
                i => i.MatchLdloc(49),
                i => i.MatchBlt(out _)))
            {
                AltLibrary.Instance.Logger.Info("d $ 6");
                return;
            }
            c.Index += 3;
            c.MarkLabel(label);
        }

        private static void GenPasses_HookGenPassCorruption(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchCall<WorldGen>(nameof(WorldGen.CrimStart))))
            {
                AltLibrary.Instance.Logger.Info("d $ 1");
                return;
            }

            #region Crimson
            if (!c.TryGotoNext(i => i.MatchLdcI4(60)))
            {
                AltLibrary.Instance.Logger.Info("d $ 2");
                return;
            }

            c.Index++;
            c.Emit(OpCodes.Pop);
            c.EmitDelegate(() =>
            {
                foreach (AltBiome biome in AltLibrary.Biomes)
                {
                    if (WorldBiomeManager.WorldJungle == biome.FullName && biome.BiomeType == BiomeType.Jungle && biome.BiomeGrass.HasValue)
                        return biome.BiomeGrass.Value;
                }
                return 60;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(234)))
            {
                AltLibrary.Instance.Logger.Info("d $ 3");
                return;
            }

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldGen.drunkWorldGen)
                {
                    int value1 = TileID.Crimsand;
                    int value2 = TileID.Ebonsand;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeSand ?? TileID.Sand) : TileID.Sand;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                }
                return orig;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(199)))
            {
                AltLibrary.Instance.Logger.Info("d $ 5");
                return;
            }

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldGen.drunkWorldGen)
                {
                    int value1 = TileID.CrimsonGrass;
                    int value2 = TileID.CorruptGrass;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeGrass ?? TileID.Grass) : TileID.Grass;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                }
                return orig;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(203)))
            {
                AltLibrary.Instance.Logger.Info("d $ 6");
                return;
            }

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldGen.drunkWorldGen)
                {
                    int value1 = TileID.Crimstone;
                    int value2 = TileID.Ebonstone;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeStone ?? TileID.Stone) : TileID.Stone;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                }
                return orig;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(199)))
            {
                AltLibrary.Instance.Logger.Info("d $ 7");
                return;
            }

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldGen.drunkWorldGen)
                {
                    int value1 = TileID.CrimsonGrass;
                    int value2 = TileID.CorruptGrass;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeGrass ?? TileID.Grass) : TileID.Grass;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                }
                return orig;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(200)))
            {
                AltLibrary.Instance.Logger.Info("d $ 8");
                return;
            }

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldGen.drunkWorldGen)
                {
                    int value1 = orig;
                    int value2 = TileID.CorruptIce;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeIce ?? TileID.IceBlock) : TileID.IceBlock;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                }
                return orig;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(401)))
            {
                AltLibrary.Instance.Logger.Info("d $ 9");
                return;
            }

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldGen.drunkWorldGen)
                {
                    int value1 = TileID.CrimsonSandstone;
                    int value2 = TileID.CorruptSandstone;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeSandstone ?? TileID.Sandstone) : TileID.Sandstone;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                }
                return orig;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(399)))
            {
                AltLibrary.Instance.Logger.Info("d $ 10");
                return;
            }

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldGen.drunkWorldGen)
                {
                    int value1 = TileID.CrimsonHardenedSand;
                    int value2 = TileID.CorruptHardenedSand;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeHardenedSand ?? TileID.HardenedSand) : TileID.HardenedSand;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                }
                return orig;
            });
            #endregion

            #region Corrupt
            if (!c.TryGotoNext(i => i.MatchLdcI4(60)))
            {
                AltLibrary.Instance.Logger.Info("d $ 11");
                return;
            }

            c.Index++;
            c.Emit(OpCodes.Pop);
            c.EmitDelegate(() =>
            {
                foreach (AltBiome biome in AltLibrary.Biomes)
                {
                    if (WorldBiomeManager.WorldJungle == biome.FullName && biome.BiomeType == BiomeType.Jungle && biome.BiomeGrass.HasValue)
                        return biome.BiomeGrass.Value;
                }
                return 60;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(112)))
            {
                AltLibrary.Instance.Logger.Info("d $ 12");
                return;
            }

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldGen.drunkWorldGen)
                {
                    int value1 = TileID.Crimsand;
                    int value2 = TileID.Ebonsand;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeSand ?? TileID.Sand) : TileID.Sand;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                }
                return orig;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(23)))
            {
                AltLibrary.Instance.Logger.Info("d $ 13");
                return;
            }

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldGen.drunkWorldGen)
                {
                    int value1 = TileID.CrimsonGrass;
                    int value2 = TileID.CorruptGrass;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeGrass ?? TileID.Grass) : TileID.Grass;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                }
                return orig;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(25)))
            {
                AltLibrary.Instance.Logger.Info("d $ 14");
                return;
            }

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldGen.drunkWorldGen)
                {
                    int value1 = TileID.Crimstone;
                    int value2 = TileID.Ebonstone;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeStone ?? TileID.Stone) : TileID.Stone;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                }
                return orig;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(23)))
            {
                AltLibrary.Instance.Logger.Info("d $ 15");
                return;
            }

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldGen.drunkWorldGen)
                {
                    int value1 = TileID.CrimsonGrass;
                    int value2 = TileID.CorruptGrass;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeGrass ?? TileID.Grass) : TileID.Grass;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                }
                return orig;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(163)))
            {
                AltLibrary.Instance.Logger.Info("d $ 16");
                return;
            }

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldGen.drunkWorldGen)
                {
                    int value1 = TileID.FleshIce;
                    int value2 = TileID.CorruptIce;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeIce ?? TileID.IceBlock) : TileID.IceBlock;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                }
                return orig;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(400)))
            {
                AltLibrary.Instance.Logger.Info("d $ 17");
                return;
            }

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldGen.drunkWorldGen)
                {
                    int value1 = TileID.CrimsonSandstone;
                    int value2 = TileID.CorruptSandstone;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeSand ?? TileID.Sandstone) : TileID.Sandstone;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                }
                return orig;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(398)))
            {
                AltLibrary.Instance.Logger.Info("d $ 18");
                return;
            }

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldGen.drunkWorldGen)
                {
                    int value1 = TileID.CrimsonHardenedSand;
                    int value2 = TileID.CorruptHardenedSand;
                    int value3 = WorldBiomeGeneration.worldCrimson3 != null ? (WorldBiomeGeneration.worldCrimson3.BiomeHardenedSand ?? TileID.HardenedSand) : TileID.HardenedSand;
                    return (ushort)(WorldBiomeGeneration.worldCrimson2 ? (WorldGen.crimson ? value2 : value1) : value3);
                }
                return orig;
            });
            #endregion
        }

        public static void ILGenPassAltars(ILContext il)
        {
            ILCursor c = new(il);
            ILLabel endNormalAltar = c.DefineLabel();
            ILLabel startNormalAltar = c.DefineLabel();
            if (!c.TryGotoNext(i => i.MatchLdsfld<WorldGen>(nameof(WorldGen.crimson))))
            {
                AltLibrary.Instance.Logger.Info("e $ 1");
                return;
            }
            c.EmitDelegate(() => WorldGen.crimson || WorldBiomeManager.WorldEvil != "");
            c.Emit(OpCodes.Brfalse, startNormalAltar);
            c.Emit(OpCodes.Ldloc, 3);
            c.Emit(OpCodes.Ldloc, 4);
            c.EmitDelegate((int x, int y) =>
            {
                if (WorldBiomeManager.WorldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).AltarTile.HasValue)
                {
                    if (!WorldGen.IsTileNearby(x, y, ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).AltarTile.Value, 3))
                    {
                        WorldGen.Place3x2(x, y, (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).AltarTile.Value);
                    }
                }
            });
            c.Emit(OpCodes.Br, endNormalAltar);
            c.MarkLabel(startNormalAltar);
            if (!c.TryGotoNext(i => i.MatchLdloc(5)))
            {
                AltLibrary.Instance.Logger.Info("e $ 2");
                return;
            }
            if (!c.TryGotoNext(i => i.MatchLdsflda(out _)))
            {
                AltLibrary.Instance.Logger.Info("e $ 3");
                return;
            }
            c.MarkLabel(endNormalAltar);
        }

        private static void GenPasses_HookGenPassReset(ILContext il)
        {
            ILCursor c = new(il);
            FieldReference copper = null;
            FieldReference iron = null;
            FieldReference silver = null;
            FieldReference gold = null;

            if (!c.TryGotoNext(i => i.MatchStsfld(typeof(WorldGen).GetField(nameof(WorldGen.crimson), BindingFlags.Public | BindingFlags.Static))))
            {
                AltLibrary.Instance.Logger.Info("f $ 1");
                return;
            }
            if (!c.TryGotoPrev(i => i.MatchLdcI4(166)))
            {
                AltLibrary.Instance.Logger.Info("f $ 2");
                return;
            }
            if (!c.TryGotoPrev(i => i.MatchLdcI4(166)))
            {
                AltLibrary.Instance.Logger.Info("f $ 3");
                return;
            }
            if (!c.TryGotoNext(i => i.MatchStfld(out copper)))
            {
                AltLibrary.Instance.Logger.Info("f $ 4");
                return;
            }
            if (!c.TryGotoNext(i => i.MatchLdcI4(167)))
            {
                AltLibrary.Instance.Logger.Info("f $ 5");
                return;
            }
            if (!c.TryGotoNext(i => i.MatchStfld(out iron)))
            {
                AltLibrary.Instance.Logger.Info("f $ 6");
                return;
            }
            if (!c.TryGotoNext(i => i.MatchLdcI4(168)))
            {
                AltLibrary.Instance.Logger.Info("f $ 7");
                return;
            }
            if (!c.TryGotoNext(i => i.MatchStfld(out silver)))
            {
                AltLibrary.Instance.Logger.Info("f $ 8");
                return;
            }
            if (!c.TryGotoNext(i => i.MatchLdcI4(169)))
            {
                AltLibrary.Instance.Logger.Info("f $ 9");
                return;
            }
            if (!c.TryGotoNext(i => i.MatchStfld(out gold)))
            {
                AltLibrary.Instance.Logger.Info("f $ 10");
                return;
            }
            if (!c.TryGotoNext(i => i.MatchStsfld(typeof(WorldGen).GetField(nameof(WorldGen.crimson), BindingFlags.Public | BindingFlags.Static))))
            {
                AltLibrary.Instance.Logger.Info("f $ 11");
                return;
            }
            if (!c.TryGotoNext(i => i.MatchRet()))
            {
                AltLibrary.Instance.Logger.Info("f $ 12");
                return;
            }
            if (!c.TryGotoPrev(i => i.MatchBneUn(out _)))
            {
                AltLibrary.Instance.Logger.Info("f $ 13");
                return;
            }
            if (!c.TryGotoPrev(i => i.MatchLdcI4(-1)))
            {
                AltLibrary.Instance.Logger.Info("f $ 14");
                return;
            }

            c.EmitDelegate<Func<int, int>>(dungeonSide =>
            {
                WorldBiomeGeneration.dungeonSide = dungeonSide;
                return dungeonSide;
            });

            replaceValues(() => WorldBiomeManager.Copper, (-1, TileID.Copper), (-2, TileID.Tin), copper);
            replaceValues(() => WorldBiomeManager.Iron, (-3, TileID.Iron), (-4, TileID.Lead), iron);
            replaceValues(() => WorldBiomeManager.Silver, (-5, TileID.Silver), (-6, TileID.Tungsten), silver);
            replaceValues(() => WorldBiomeManager.Gold, (-7, TileID.Gold), (-8, TileID.Platinum), gold);

            void replaceValues(Func<int> type, ValueTuple<int, int> value1, ValueTuple<int, int> value2, FieldReference field)
            {
                var label = c.DefineLabel();
                var label2 = c.DefineLabel();
                var label3 = c.DefineLabel();
                c.EmitDelegate(type.Invoke);
                c.Emit(OpCodes.Ldc_I4, value1.Item1);
                c.Emit(OpCodes.Bne_Un_S, label);
                c.Emit(OpCodes.Ldarg, 0);
                c.EmitDelegate(() => value1.Item2);
                c.Emit(OpCodes.Stfld, field);
                c.Emit(OpCodes.Br_S, label3);
                c.MarkLabel(label);
                c.EmitDelegate(type.Invoke);
                c.Emit(OpCodes.Ldc_I4, value2.Item1);
                c.Emit(OpCodes.Bne_Un_S, label2);
                c.Emit(OpCodes.Ldarg, 0);
                c.EmitDelegate(() => value2.Item2);
                c.Emit(OpCodes.Stfld, field);
                c.Emit(OpCodes.Br_S, label3);
                c.MarkLabel(label2);
                c.Emit(OpCodes.Ldarg, 0);
                c.EmitDelegate(() => AltLibrary.Ores[type.Invoke() - 1].ore);
                c.Emit(OpCodes.Stfld, field);
                c.MarkLabel(label3);
            }
        }

        private static void GenPasses_HookGenPassShinies(ILContext il)
        {
            ILCursor c = new(il);
            for (int j = 0; j < 3; j++)
            {
                if (!c.TryGotoNext(i => i.MatchLdsfld<WorldGen>(nameof(WorldGen.drunkWorldGen))))
                {
                    AltLibrary.Instance.Logger.Info("g $ 1 " + j);
                    return;
                }
                if (!c.TryGotoNext(i => i.MatchLdcI4(2)))
                {
                    AltLibrary.Instance.Logger.Info("g $ 2 " + j);
                    return;
                }
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldc_I4, 1);
                if (!c.TryGotoNext(i => i.MatchLdcI4(7)))
                {
                    AltLibrary.Instance.Logger.Info("g $ 3 " + j);
                    return;
                }
                c.Index++;
                c.EmitDelegate<Func<int, int>>((value) =>
                {
                    List<int> list = new()
                    {
                        7,
                        166
                    };
                    AltLibrary.Ores.Where(x => x.OreType == OreType.Copper)
                                   .ToList()
                                   .ForEach(x => list.Add(x.ore));
                    return WorldGen.genRand.Next(list);
                });
            }

            for (int j = 0; j < 3; j++)
            {
                if (!c.TryGotoNext(i => i.MatchLdsfld<WorldGen>(nameof(WorldGen.drunkWorldGen))))
                {
                    AltLibrary.Instance.Logger.Info("g $ 4 " + j);
                    return;
                }
                if (!c.TryGotoNext(i => i.MatchLdcI4(2)))
                {
                    AltLibrary.Instance.Logger.Info("g $ 5 " + j);
                    return;
                }
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldc_I4, 1);
                if (!c.TryGotoNext(i => i.MatchLdcI4(6)))
                {
                    AltLibrary.Instance.Logger.Info("g $ 6 " + j);
                    return;
                }
                c.Index++;
                c.EmitDelegate<Func<int, int>>((value) =>
                {
                    List<int> list = new()
                    {
                        6,
                        167
                    };
                    AltLibrary.Ores.Where(x => x.OreType == OreType.Iron)
                                   .ToList()
                                   .ForEach(x => list.Add(x.ore));
                    return WorldGen.genRand.Next(list);
                });
            }

            for (int j = 0; j < 3; j++)
            {
                if (!c.TryGotoNext(i => i.MatchLdsfld<WorldGen>(nameof(WorldGen.drunkWorldGen))))
                {
                    AltLibrary.Instance.Logger.Info("g $ 7 " + j);
                    return;
                }
                if (!c.TryGotoNext(i => i.MatchLdcI4(2)))
                {
                    AltLibrary.Instance.Logger.Info("g $ 8 " + j);
                    return;
                }
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldc_I4, 1);
                if (!c.TryGotoNext(i => i.MatchLdcI4(9)))
                {
                    AltLibrary.Instance.Logger.Info("g $ 9 " + j);
                    return;
                }
                c.Index++;
                c.EmitDelegate<Func<int, int>>((value) =>
                {
                    List<int> list = new()
                    {
                        9,
                        168
                    };
                    AltLibrary.Ores.Where(x => x.OreType == OreType.Silver)
                                   .ToList()
                                   .ForEach(x => list.Add(x.ore));
                    return WorldGen.genRand.Next(list);
                });
            }

            for (int j = 0; j < 2; j++)
            {
                if (!c.TryGotoNext(i => i.MatchLdsfld<WorldGen>(nameof(WorldGen.drunkWorldGen))))
                {
                    AltLibrary.Instance.Logger.Info("g $ 10 " + j);
                    return;
                }
                if (!c.TryGotoNext(i => i.MatchLdcI4(2)))
                {
                    AltLibrary.Instance.Logger.Info("g $ 11 " + j);
                    return;
                }
                c.Index++;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldc_I4, 1);
                if (!c.TryGotoNext(i => i.MatchLdcI4(8)))
                {
                    AltLibrary.Instance.Logger.Info("g $ 12 " + j);
                    return;
                }
                c.Index++;
                c.EmitDelegate<Func<int, int>>((value) =>
                {
                    List<int> list = new()
                    {
                        8,
                        169
                    };
                    AltLibrary.Ores.Where(x => x.OreType == OreType.Gold)
                                   .ToList()
                                   .ForEach(x => list.Add(x.ore));
                    return WorldGen.genRand.Next(list);
                });
            }

            if (!c.TryGotoNext(i => i.MatchRet()))
            {
                AltLibrary.Instance.Logger.Info("g $ 13");
                return;
            }
            if (!c.TryGotoPrev(i => i.MatchLdsfld<WorldGen>(nameof(WorldGen.drunkWorldGen))))
            {
                AltLibrary.Instance.Logger.Info("g $ 14");
                return;
            }
            if (!c.TryGotoNext(i => i.MatchBrfalse(out _)))
            {
                AltLibrary.Instance.Logger.Info("g $ 15");
                return;
            }
            c.Index++;
            c.EmitDelegate(() =>
            {
                if (WorldBiomeManager.WorldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeOre.HasValue)
                {
                    for (int i = 0; i < (int)(Main.maxTilesX * Main.maxTilesY * 2.25E-05 / 2.0); i++)
                    {
                        WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX),
                            WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY), WorldGen.genRand.Next(3, 6),
                            WorldGen.genRand.Next(4, 8), ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeOre.Value);
                    }
                }
            });
            if (!c.TryGotoNext(i => i.MatchRet()))
            {
                AltLibrary.Instance.Logger.Info("g $ 16");
                return;
            }
            if (!c.TryGotoNext(i => i.MatchBr(out _)))
            {
                AltLibrary.Instance.Logger.Info("g $ 17");
                return;
            }
            ILLabel startCorruptionGen = c.DefineLabel();
            c.EmitDelegate(() => !WorldGen.crimson && WorldBiomeManager.WorldEvil != "");
            c.Emit(OpCodes.Brfalse, startCorruptionGen);
            c.EmitDelegate(() =>
            {
                if (WorldBiomeManager.WorldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeOre.HasValue)
                {
                    for (int i = 0; i < (int)(Main.maxTilesX * Main.maxTilesY * 2.25E-05); i++)
                    {
                        WorldGen.TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX),
                            WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY), WorldGen.genRand.Next(3, 6),
                            WorldGen.genRand.Next(4, 8), ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeOre.Value);
                    }
                }
            });
            c.Emit(OpCodes.Ret);
            c.MarkLabel(startCorruptionGen);
        }
    }
}
