using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace AltLibrary.Common.AltBiomes
{
    public abstract class ALBiomeTileCountModifier : ModSystem
    {
        public enum TileCountType
        {
            HALLOW,
            EVIL,
            SUNFLOWER,
        }

        static List<ALBiomeTileCountModifier> instances;

        public virtual TileCountType TileType => TileCountType.HALLOW;

        internal int TileCount;

        public abstract int GetTileCount(ReadOnlySpan<int> TileCounts);

        public abstract void OnReceiveModifiedTileCount(int TileCount);

        public class ALBiomeTileCountModifier_Loader : ILoadable
        {
            public void Load(Mod mod)
            {
                instances = new();

                IL.Terraria.SceneMetrics.ExportTileCountsToMain += SceneMetrics_GetModdedHallowEvil;
            }

            public void Unload()
            {
                instances = null;
                HolyTileCountOriginal = 0;
                EvilTileCountOriginal = 0;
                ModSunflowerCount = 0;
                IL.Terraria.SceneMetrics.ExportTileCountsToMain -= SceneMetrics_GetModdedHallowEvil;
            }
        }


        internal static void SceneMetrics_GetModdedHallowEvil(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchStloc(0)))
                throw new Exception("[SceneMetrics_GetModdedHallowEvil] Failed to find i => i.MatchStloc(0)");
            c.Index++;
            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Ldfld, typeof(SceneMetrics).GetField("_tileCounts", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
            c.EmitDelegate(GetOriginalTileCounts);

            c.Index++;
            if (!c.TryGotoNext(i => i.MatchBge(out ILLabel _)))
                AltLibrary.Instance.Logger.Info("10 $ 1");
            c.Index--;
            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Ldfld, typeof(SceneMetrics).GetField("_tileCounts", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
            c.EmitDelegate(ExportTileCounts);
        }
        protected sealed override void Register()
        {
            instances.Add(this);
        }

        static int HolyTileCountOriginal = 0;
        static int EvilTileCountOriginal = 0;
        static int ModSunflowerCount = 0;

        internal static void GetOriginalTileCounts(SceneMetrics metrics, int[] tileCounts)
        {
            instances.ForEach(i =>
            {
                switch (i.TileType)
                {
                    case TileCountType.SUNFLOWER:
                        int TileCount = i.GetTileCount(tileCounts);
                        i.TileCount = TileCount;
                        ModSunflowerCount += TileCount;
                        break;
                }
            });
            HolyTileCountOriginal = metrics.HolyTileCount;
            metrics.EvilTileCount = Math.Max(0, metrics.EvilTileCount - ModSunflowerCount);
            metrics.BloodTileCount -= Math.Max(0, metrics.BloodTileCount - ModSunflowerCount);
            EvilTileCountOriginal = metrics.EvilTileCount + metrics.BloodTileCount;
        }
        internal static void ExportTileCounts(SceneMetrics metrics, int[] tileCounts)
        {
            int TotalHolyTiles = 0;
            int TotalEvilTiles = 0;
            instances.ForEach(i =>
            {
                if (i.TileType == TileCountType.SUNFLOWER)
                    return;
                int TileCount = i.GetTileCount(tileCounts);
                if (i.TileType == TileCountType.EVIL)
                    Math.Max(0, TileCount - (tileCounts[27] + ModSunflowerCount));
                i.TileCount = TileCount;
                switch (i.TileType)
                {
                    case TileCountType.HALLOW:
                        TotalHolyTiles += TileCount;
                        break;
                    case TileCountType.EVIL:
                        TotalEvilTiles += TileCount;
                        break;
                }
            });

            metrics.HolyTileCount -= TotalEvilTiles;
            metrics.EvilTileCount -= TotalHolyTiles;
            metrics.BloodTileCount -= TotalHolyTiles;

            TotalHolyTiles += HolyTileCountOriginal;
            TotalEvilTiles += EvilTileCountOriginal;

            instances.ForEach(i =>
            {
                switch (i.TileType)
                {
                    case TileCountType.HALLOW:
                        i.OnReceiveModifiedTileCount(Math.Max(i.TileCount - TotalEvilTiles, 0));
                        break;
                    case TileCountType.EVIL:
                        i.OnReceiveModifiedTileCount(Math.Max(i.TileCount - TotalHolyTiles, 0));
                        break;
                }
            });
        }
    }
}
