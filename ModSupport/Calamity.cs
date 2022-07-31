using Terraria.ModLoader;

namespace AltLibrary.ModSupport
{
	internal abstract class ModSupport
	{
		public virtual string ModName => "";

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
		public override string ModName => "CalamityMod";
		public override void OnHook(Mod Mod)
		{
			AltLibraryGlobalItem.HallowedOreList.Add(Mod.Find<ModTile>("HallowedOre").Type, true);
		}
	}
}
