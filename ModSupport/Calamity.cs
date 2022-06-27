using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace AltLibrary.ModSupport
{
    internal abstract class ModSupport
    {
        string ModName;

        void Hook()
        {
            if (ModLoader.TryGetMod(ModName, out Mod Mod))
                OnHook(Mod);
        }

        public abstract void OnHook(Mod Mod);

        public ModSupport()
        {
            Hook();
        }

        public static void HookAll()
        {
            new CalamityModSupport();
        }
    }
    internal class CalamityModSupport : ModSupport
    {
        public override void OnHook(Mod Mod)
        {
            AltLibraryGlobalItem.HallowedOreList.Add(Mod.Find<ModTile>("HallowedOre").Name, true);
        }
    }
}
