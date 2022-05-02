using Mono.Cecil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using MonoMod.Utils;
using System.Collections.Generic;
using System.Reflection;

namespace AltLibrary.Common
{
    public static class GenPasses
    {
        private static MethodBase ResetInfo;
        private static MethodBase ShiniesInfo;
        private static MethodBase AltarsInfo;
        private static MethodBase HardmodeWallsInfo;

        public static event ILContext.Manipulator HookGenPassReset
        {
            add => HookEndpointManager.Modify(ResetInfo, value);
            remove => HookEndpointManager.Unmodify(ResetInfo, value);
        }

        public static event ILContext.Manipulator HookGenPassShinies
        {
            add => HookEndpointManager.Modify(ShiniesInfo, value);
            remove => HookEndpointManager.Unmodify(ShiniesInfo, value);
        }

        public static event ILContext.Manipulator HookGenPassAltars
        {
            add => HookEndpointManager.Modify(AltarsInfo, value);
            remove => HookEndpointManager.Unmodify(AltarsInfo, value);
        }

        public static event ILContext.Manipulator HookGenPassHardmodeWalls
        {
            add => HookEndpointManager.Modify(HardmodeWallsInfo, value);
            remove => HookEndpointManager.Unmodify(HardmodeWallsInfo, value);
        }

        public static void ILGenerateWorld(ILContext il)
        {
            ResetInfo = GetGenPassInfo(il, "Reset");
            ShiniesInfo = GetGenPassInfo(il, "Shinies");
            AltarsInfo = GetGenPassInfo(il, "Altars");
        }

        public static void ILSMCallBack(ILContext il)
        {
            HardmodeWallsInfo = GetGenPassInfo(il, "Hardmode Walls");
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
