using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace AltLibrary.Common.Hooks
{
    [Autoload(Side = ModSide.Both)]
    internal class FishingCrateLoot
    {
        internal static void Load()
        {
            IL.Terraria.Player.OpenFishingCrate += Player_OpenFishingCrate;
        }

        internal static void Unload()
        {
            IL.Terraria.Player.OpenFishingCrate -= Player_OpenFishingCrate;
        }

        private static void Player_OpenFishingCrate(ILContext il)
        {
            ILCursor c = new(il);
            // 'value' is randomization of item id that going to get chosen
            // 'rng' is item id that is going to drop

            #region Wooden Crate's tier 1-2 ores
            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand"),
                i => i.MatchLdcI4(4),
                i => i.MatchCallvirt<UnifiedRandom>(nameof(UnifiedRandom.Next)),
                i => i.MatchStloc(23)))
            {
                AltLibrary.Instance.Logger.Info("9 $ 1");
                return;
            }

            c.Index += 2;
            c.EmitDelegate<Func<int, int>>((value) =>
            {
                return value;
            });

            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand")))
            {
                AltLibrary.Instance.Logger.Info("9 $ 2");
                return;
            }

            c.Emit(OpCodes.Ldloc, 23);
            c.EmitDelegate<Func<int, int>>((rng) =>
            {
                return rng;
            });
            c.Emit(OpCodes.Stloc, 23);
            #endregion

            #region Pearlwood Crate's cobalt/palladium
            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand"),
                i => i.MatchLdcI4(2),
                i => i.MatchCallvirt<UnifiedRandom>(nameof(UnifiedRandom.Next)),
                i => i.MatchStloc(23)))
            {
                AltLibrary.Instance.Logger.Info("9 $ 3");
                return;
            }

            c.Index += 2;
            c.EmitDelegate<Func<int, int>>((value) =>
            {
                return value;
            });

            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand")))
            {
                AltLibrary.Instance.Logger.Info("9 $ 4");
                return;
            }

            c.Emit(OpCodes.Ldloc, 23);
            c.EmitDelegate<Func<int, int>>((rng) =>
            {
                return rng;
            });
            c.Emit(OpCodes.Stloc, 23);
            #endregion

            #region Wooden Crate's tier 1-2 bars
            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand"),
                i => i.MatchLdcI4(4),
                i => i.MatchCallvirt<UnifiedRandom>(nameof(UnifiedRandom.Next)),
                i => i.MatchStloc(26)))
            {
                AltLibrary.Instance.Logger.Info("9 $ 5");
                return;
            }

            c.Index += 2;
            c.EmitDelegate<Func<int, int>>((value) =>
            {
                return value;
            });

            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand")))
            {
                AltLibrary.Instance.Logger.Info("9 $ 6");
                return;
            }

            c.Emit(OpCodes.Ldloc, 26);
            c.EmitDelegate<Func<int, int>>((rng) =>
            {
                return rng;
            });
            c.Emit(OpCodes.Stloc, 26);
            #endregion

            #region Pearlwood Crate's hardmode tier 1 bars
            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand"),
                i => i.MatchLdcI4(2),
                i => i.MatchCallvirt<UnifiedRandom>(nameof(UnifiedRandom.Next)),
                i => i.MatchStloc(26)))
            {
                AltLibrary.Instance.Logger.Info("9 $ 7");
                return;
            }

            c.Index += 2;
            c.EmitDelegate<Func<int, int>>((value) =>
            {
                return value;
            });

            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand")))
            {
                AltLibrary.Instance.Logger.Info("9 $ 8");
                return;
            }

            c.Emit(OpCodes.Ldloc, 26);
            c.EmitDelegate<Func<int, int>>((rng) =>
            {
                return rng;
            });
            c.Emit(OpCodes.Stloc, 26);
            #endregion

            if (!c.TryGotoNext(i => i.MatchLdarg(1),
                i => i.MatchLdcI4(ItemID.IronCrate)))
            {
                return;
            }

            #region Iron Crate's tier 1-3 ores
            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand"),
                i => i.MatchLdcI4(6),
                i => i.MatchCallvirt<UnifiedRandom>(nameof(UnifiedRandom.Next)),
                i => i.MatchStloc(56)))
            {
                AltLibrary.Instance.Logger.Info("9 $ 9");
                return;
            }

            c.Index += 2;
            c.EmitDelegate<Func<int, int>>((value) =>
            {
                return value;
            });

            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand")))
            {
                AltLibrary.Instance.Logger.Info("9 $ 10");
                return;
            }

            c.Emit(OpCodes.Ldloc, 56);
            c.EmitDelegate<Func<int, int>>((rng) =>
            {
                return rng;
            });
            c.Emit(OpCodes.Stloc, 56);
            #endregion

            #region Mythril Crate's hardmode tier 1-2 ores
            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand"),
                i => i.MatchLdcI4(4),
                i => i.MatchCallvirt<UnifiedRandom>(nameof(UnifiedRandom.Next)),
                i => i.MatchStloc(56)))
            {
                AltLibrary.Instance.Logger.Info("9 $ 11");
                return;
            }

            c.Index += 2;
            c.EmitDelegate<Func<int, int>>((value) =>
            {
                return value;
            });

            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand")))
            {
                AltLibrary.Instance.Logger.Info("9 $ 12");
                return;
            }

            c.Emit(OpCodes.Ldloc, 56);
            c.EmitDelegate<Func<int, int>>((rng) =>
            {
                return rng;
            });
            c.Emit(OpCodes.Stloc, 56);
            #endregion

            #region Iron Crate's tier 1-3 bars
            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand"),
                i => i.MatchLdcI4(6),
                i => i.MatchCallvirt<UnifiedRandom>(nameof(UnifiedRandom.Next)),
                i => i.MatchStloc(59)))
            {
                AltLibrary.Instance.Logger.Info("9 $ 13");
                return;
            }

            c.Index += 2;
            c.EmitDelegate<Func<int, int>>((value) =>
            {
                return value;
            });

            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand")))
            {
                AltLibrary.Instance.Logger.Info("9 $ 14");
                return;
            }

            c.Emit(OpCodes.Ldloc, 59);
            c.EmitDelegate<Func<int, int>>((rng) =>
            {
                return rng;
            });
            c.Emit(OpCodes.Stloc, 59);
            #endregion

            #region Mythril Crate's hardmode tier 1-2 bars
            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand"),
                i => i.MatchLdcI4(4),
                i => i.MatchCallvirt<UnifiedRandom>(nameof(UnifiedRandom.Next)),
                i => i.MatchStloc(59)))
            {
                AltLibrary.Instance.Logger.Info("9 $ 15");
                return;
            }

            c.Index += 2;
            c.EmitDelegate<Func<int, int>>((value) =>
            {
                return value;
            });

            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand")))
            {
                AltLibrary.Instance.Logger.Info("9 $ 16");
                return;
            }

            c.Emit(OpCodes.Ldloc, 59);
            c.EmitDelegate<Func<int, int>>((rng) =>
            {
                return rng;
            });
            c.Emit(OpCodes.Stloc, 59);
            #endregion

            if (!c.TryGotoNext(i => i.MatchLdarg(1),
                i => i.MatchLdcI4(ItemID.GoldenCrate)))
            {
                return;
            }

            #region Golden Crate's tier 3-4 ores
            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand"),
                i => i.MatchLdcI4(4),
                i => i.MatchCallvirt<UnifiedRandom>(nameof(UnifiedRandom.Next)),
                i => i.MatchStloc(85)))
            {
                AltLibrary.Instance.Logger.Info("9 $ 21");
                return;
            }

            c.Index += 2;
            c.EmitDelegate<Func<int, int>>((value) =>
            {
                return value;
            });

            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand")))
            {
                AltLibrary.Instance.Logger.Info("9 $ 22");
                return;
            }

            c.Emit(OpCodes.Ldloc, 85);
            c.EmitDelegate<Func<int, int>>((rng) =>
            {
                return rng;
            });
            c.Emit(OpCodes.Stloc, 85);
            #endregion

            #region Titanium Crate's hardmode tier 1-2 ores
            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand"),
                i => i.MatchLdcI4(4),
                i => i.MatchCallvirt<UnifiedRandom>(nameof(UnifiedRandom.Next)),
                i => i.MatchStloc(85)))
            {
                AltLibrary.Instance.Logger.Info("9 $ 23");
                return;
            }

            c.Index += 2;
            c.EmitDelegate<Func<int, int>>((value) =>
            {
                return value;
            });

            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand")))
            {
                AltLibrary.Instance.Logger.Info("9 $ 24");
                return;
            }

            c.Emit(OpCodes.Ldloc, 85);
            c.EmitDelegate<Func<int, int>>((rng) =>
            {
                return rng;
            });
            c.Emit(OpCodes.Stloc, 85);
            #endregion

            #region Golden Crate's tier 3-4 bars
            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand"),
                i => i.MatchLdcI4(4),
                i => i.MatchCallvirt<UnifiedRandom>(nameof(UnifiedRandom.Next)),
                i => i.MatchStloc(85)))
            {
                AltLibrary.Instance.Logger.Info("9 $ 25");
                return;
            }

            c.Index += 2;
            c.EmitDelegate<Func<int, int>>((value) =>
            {
                return value;
            });

            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand")))
            {
                AltLibrary.Instance.Logger.Info("9 $ 26");
                return;
            }

            c.Emit(OpCodes.Ldloc, 85);
            c.EmitDelegate<Func<int, int>>((rng) =>
            {
                return rng;
            });
            c.Emit(OpCodes.Stloc, 85);
            #endregion

            #region Titanium Crate's hardmode tier 1-2 ores
            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand"),
                i => i.MatchLdcI4(4),
                i => i.MatchCallvirt<UnifiedRandom>(nameof(UnifiedRandom.Next)),
                i => i.MatchStloc(85)))
            {
                AltLibrary.Instance.Logger.Info("9 $ 27");
                return;
            }

            c.Index += 2;
            c.EmitDelegate<Func<int, int>>((value) =>
            {
                return value;
            });

            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand")))
            {
                AltLibrary.Instance.Logger.Info("9 $ 28");
                return;
            }

            c.Emit(OpCodes.Ldloc, 85);
            c.EmitDelegate<Func<int, int>>((rng) =>
            {
                return rng;
            });
            c.Emit(OpCodes.Stloc, 85);
            #endregion

            if (!c.TryGotoNext(i => i.MatchLdarg(1),
                i => i.MatchLdcI4(ItemID.OceanCrate)))
            {
                return;
            }

            #region Other Crates tier 1-4 ores
            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand"),
                i => i.MatchLdcI4(8),
                i => i.MatchCallvirt<UnifiedRandom>(nameof(UnifiedRandom.Next)),
                i => i.MatchStloc(141)))
            {
                AltLibrary.Instance.Logger.Info("9 $ 29");
                return;
            }

            c.Index += 2;
            c.EmitDelegate<Func<int, int>>((value) =>
            {
                return value;
            });

            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand")))
            {
                AltLibrary.Instance.Logger.Info("9 $ 30");
                return;
            }

            c.Emit(OpCodes.Ldloc, 141);
            c.EmitDelegate<Func<int, int>>((rng) =>
            {
                return rng;
            });
            c.Emit(OpCodes.Stloc, 141);
            #endregion

            #region Other Crates hardmode tier 1-3 ores
            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand"),
                i => i.MatchLdcI4(6),
                i => i.MatchCallvirt<UnifiedRandom>(nameof(UnifiedRandom.Next)),
                i => i.MatchStloc(141)))
            {
                AltLibrary.Instance.Logger.Info("9 $ 31");
                return;
            }

            c.Index += 2;
            c.EmitDelegate<Func<int, int>>((value) =>
            {
                return value;
            });

            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand")))
            {
                AltLibrary.Instance.Logger.Info("9 $ 32");
                return;
            }

            c.Emit(OpCodes.Ldloc, 141);
            c.EmitDelegate<Func<int, int>>((rng) =>
            {
                return rng;
            });
            c.Emit(OpCodes.Stloc, 141);
            #endregion

            #region Other Crates tier 2-4 bars
            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand"),
                i => i.MatchLdcI4(6),
                i => i.MatchCallvirt<UnifiedRandom>(nameof(UnifiedRandom.Next)),
                i => i.MatchStloc(144)))
            {
                AltLibrary.Instance.Logger.Info("9 $ 33");
                return;
            }

            c.Index += 2;
            c.EmitDelegate<Func<int, int>>((value) =>
            {
                return value;
            });

            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand")))
            {
                AltLibrary.Instance.Logger.Info("9 $ 34");
                return;
            }

            c.Emit(OpCodes.Ldloc, 144);
            c.EmitDelegate<Func<int, int>>((rng) =>
            {
                return rng;
            });
            c.Emit(OpCodes.Stloc, 144);
            #endregion

            #region Other Crates hardmode tier 2-3 bars
            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand"),
                i => i.MatchLdcI4(6),
                i => i.MatchCallvirt<UnifiedRandom>(nameof(UnifiedRandom.Next)),
                i => i.MatchStloc(144)))
            {
                AltLibrary.Instance.Logger.Info("9 $ 35");
                return;
            }

            c.Index += 2;
            c.EmitDelegate<Func<int, int>>((value) =>
            {
                return value;
            });

            if (!c.TryGotoNext(i => i.MatchCall<Main>("get_rand")))
            {
                AltLibrary.Instance.Logger.Info("9 $ 36");
                return;
            }

            c.Emit(OpCodes.Ldloc, 144);
            c.EmitDelegate<Func<int, int>>((rng) =>
            {
                return rng;
            });
            c.Emit(OpCodes.Stloc, 144);
            #endregion
        }
    }
}
