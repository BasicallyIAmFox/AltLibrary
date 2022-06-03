using Mono.Cecil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using MonoMod.Utils;
using System.Collections.Generic;
using System.Reflection;

namespace AltLibrary.Common
{
    internal static class GenPasses
    {
        private static MethodBase ResetInfo;
        private static MethodBase ShiniesInfo;
        private static MethodBase UnderworldInfo;
        private static MethodBase AltarsInfo;
        private static MethodBase MicroBiomesInfo;
        private static MethodBase HardmodeWallsInfo;

        internal static event ILContext.Manipulator HookGenPassReset
        {
            add => HookEndpointManager.Modify(ResetInfo, value);
            remove => HookEndpointManager.Unmodify(ResetInfo, value);
        }

        internal static event ILContext.Manipulator HookGenPassShinies
        {
            add => HookEndpointManager.Modify(ShiniesInfo, value);
            remove => HookEndpointManager.Unmodify(ShiniesInfo, value);
        }

        internal static event ILContext.Manipulator HookGenPassUnderworld
        {
            add => HookEndpointManager.Modify(UnderworldInfo, value);
            remove => HookEndpointManager.Unmodify(UnderworldInfo, value);
        }

        internal static event ILContext.Manipulator HookGenPassAltars
        {
            add => HookEndpointManager.Modify(AltarsInfo, value);
            remove => HookEndpointManager.Unmodify(AltarsInfo, value);
        }

        internal static event ILContext.Manipulator HookGenPassMicroBiomes
        {
            add => HookEndpointManager.Modify(MicroBiomesInfo, value);
            remove => HookEndpointManager.Unmodify(MicroBiomesInfo, value);
        }

        internal static event ILContext.Manipulator HookGenPassHardmodeWalls
        {
            add => HookEndpointManager.Modify(HardmodeWallsInfo, value);
            remove => HookEndpointManager.Unmodify(HardmodeWallsInfo, value);
        }

        internal static void ILGenerateWorld(ILContext il)
        {
            ResetInfo = GetGenPassInfo(il, "Reset");
            ShiniesInfo = GetGenPassInfo(il, "Shinies");
            UnderworldInfo = GetGenPassInfo(il, "Underworld");
            AltarsInfo = GetGenPassInfo(il, "Altars");
            MicroBiomesInfo = GetGenPassInfo(il, "Micro Biomes");
        }

        internal static void ILSMCallBack(ILContext il)
        {
            HardmodeWallsInfo = GetGenPassInfo(il, "Hardmode Walls");
        }

        public static void Unload()
        {
            ResetInfo = null;
            ShiniesInfo = null;
            UnderworldInfo = null;
            AltarsInfo = null;
            MicroBiomesInfo = null;
            HardmodeWallsInfo = null;
        }

        private static MethodBase GetGenPassInfo(ILContext il, string name)
        {
            try
            {
                var c = new ILCursor(il);
                MethodReference methodReference = null;
                c.GotoNext(i => i.MatchLdstr(name));
                c.GotoNext(i => i.MatchLdftn(out methodReference));
                return methodReference.ResolveReflection();
            }
            catch (KeyNotFoundException e)
            {
                AltLibrary.Instance.Logger.Error($"Could not find GenPass with name {name}", e);
                return null;
            }
        }
    }
}
