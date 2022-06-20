using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.Graphics;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Terraria.WorldBuilding;

namespace AltLibrary.Common.AltBiomes
{
    public abstract class ALBiomeTileCountModifier : ModSystem
    {
        public enum TileCountType
        {
            HALLOW,
            EVIL,
        }

        public virtual TileCountType TileType => TileCountType.HALLOW;

        internal int TileCount;

        public abstract int GetTileCount(ReadOnlySpan<int> TileCounts);

        public abstract void OnReceiveModifiedTileCount(int TileCount);

        internal class ALBiomeTileCountModifier_Loader : ILoadable
        {
            public void Load(Mod mod)
            {
                IL.Terraria.SceneMetrics.ExportTileCountsToMain += SceneMetrics_GetModdedHallowEvil;
            }

            public void Unload()
            {
                AltLibrary.ALBiomeTileCountModifiers = null;

                IL.Terraria.SceneMetrics.ExportTileCountsToMain -= SceneMetrics_GetModdedHallowEvil;

                HolyTileCountOriginal = 0;
                EvilTileCountOriginal = 0;
            }
        }


        internal static void SceneMetrics_GetModdedHallowEvil(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchStloc(0)))
                throw new Exception("[SceneMetrics_GetModdedHallowEvil] Failed to find i => i.MatchStloc(0)");
            c.Index++;
            c.Emit(OpCodes.Ldarg_0);
            c.EmitDelegate(GetOriginalTileCounts);

            c.Index++;
            if (!c.TryGotoNext(i => i.MatchBge(out ILLabel _)))
                AltLibrary.Instance.Logger.Info("13 $ 1");
            c.Index--;
            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Ldfld, typeof(SceneMetrics).GetField("_tileCounts", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
            c.EmitDelegate(ExportTileCounts);
        }

        protected sealed override void Register()
        {
            AltLibrary.ALBiomeTileCountModifiers.Add(this);
        }

        internal static int HolyTileCountOriginal = 0;
        internal static int EvilTileCountOriginal = 0;

        internal static void GetOriginalTileCounts(SceneMetrics metrics)
        {
            HolyTileCountOriginal = metrics.HolyTileCount;
            EvilTileCountOriginal = metrics.EvilTileCount + metrics.BloodTileCount;
        }

        internal static void ExportTileCounts(SceneMetrics metrics, int[] tileCounts)
        {
            int TotalHolyTiles = 0;
            int TotalEvilTiles = 0;
            AltLibrary.ALBiomeTileCountModifiers.ForEach(i =>
            {
                int TileCount = i.GetTileCount(tileCounts);
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

            AltLibrary.ALBiomeTileCountModifiers.ForEach(i =>
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

        public sealed override void AddRecipeGroups()
        {
        }

        public sealed override void AddRecipes()
        {
        }

        public sealed override bool CanWorldBePlayed(PlayerFileData playerData, WorldFileData worldFileData) => base.CanWorldBePlayed(playerData, worldFileData);

        public sealed override bool HijackGetData(ref byte messageType, ref BinaryReader reader, int playerNumber) => base.HijackGetData(ref messageType, ref reader, playerNumber);

        public sealed override bool HijackSendData(int whoAmI, int msgType, int remoteClient, int ignoreClient, NetworkText text, int number, float number2, float number3, float number4, int number5, int number6, int number7) => base.HijackSendData(whoAmI, msgType, remoteClient, ignoreClient, text, number, number2, number3, number4, number5, number6, number7);

        public sealed override void LoadWorldData(TagCompound tag)
        {
        }

        public sealed override void ModifyHardmodeTasks(List<GenPass> list)
        {
        }

        public sealed override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
        }

        public sealed override void ModifyLightingBrightness(ref float scale)
        {
        }

        public sealed override void ModifyScreenPosition()
        {
        }

        public sealed override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
        {
        }

        public sealed override void ModifyTimeRate(ref double timeRate, ref double tileUpdateRate, ref double eventUpdateRate)
        {
        }

        public sealed override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
        {
        }

        public sealed override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
        }

        public sealed override void NetReceive(BinaryReader reader)
        {
        }

        public sealed override void NetSend(BinaryWriter writer)
        {
        }

        public sealed override void OnWorldLoad()
        {
        }

        public sealed override void OnWorldUnload()
        {
        }

        public sealed override void PostAddRecipes()
        {
        }

        public sealed override void PostDrawFullscreenMap(ref string mouseText)
        {
        }

        public sealed override void PostDrawInterface(SpriteBatch spriteBatch)
        {
        }

        public sealed override void PostDrawTiles()
        {
        }

        public sealed override void PostSetupContent()
        {
        }

        public sealed override void PostSetupRecipes()
        {
        }

        public sealed override void PostUpdateDusts()
        {
        }

        public sealed override void PostUpdateEverything()
        {
        }

        public sealed override void PostUpdateGores()
        {
        }

        public sealed override void PostUpdateInput()
        {
        }

        public sealed override void PostUpdateInvasions()
        {
        }

        public sealed override void PostUpdateItems()
        {
        }

        public sealed override void PostUpdateNPCs()
        {
        }

        public sealed override void PostUpdatePlayers()
        {
        }

        public sealed override void PostUpdateProjectiles()
        {
        }

        public sealed override void PostUpdateTime()
        {
        }

        public sealed override void PostUpdateWorld()
        {
        }

        public sealed override void PostWorldGen()
        {
        }

        public sealed override void PreDrawMapIconOverlay(IReadOnlyList<IMapLayer> layers, MapOverlayDrawContext mapOverlayDrawContext)
        {
        }

        public sealed override void PreSaveAndQuit()
        {
        }

        public sealed override void PreUpdateDusts()
        {
        }

        public sealed override void PreUpdateEntities()
        {
        }

        public sealed override void PreUpdateGores()
        {
        }

        public sealed override void PreUpdateInvasions()
        {
        }

        public sealed override void PreUpdateItems()
        {
        }

        public sealed override void PreUpdateNPCs()
        {
        }

        public sealed override void PreUpdatePlayers()
        {
        }

        public sealed override void PreUpdateProjectiles()
        {
        }

        public sealed override void PreUpdateTime()
        {
        }

        public sealed override void PreUpdateWorld()
        {
        }

        public sealed override void PreWorldGen()
        {
        }

        public sealed override void ResetNearbyTileEffects()
        {
        }

        public sealed override void SaveWorldData(TagCompound tag)
        {
        }

        public sealed override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
        }

        public sealed override void UpdateUI(GameTime gameTime)
        {
        }

        public sealed override string WorldCanBePlayedRejectionMessage(PlayerFileData playerData, WorldFileData worldData) => base.WorldCanBePlayedRejectionMessage(playerData, worldData);
    }
}
