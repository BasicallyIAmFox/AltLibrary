using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace AltLibrary
{
#pragma warning disable CS0649
	internal class AltLibraryServerConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;
		public static AltLibraryServerConfig Config;

		[Label("$Mods.AltLibrary.Config.SecretFeatures.Label")]
		[Tooltip("$Mods.AltLibrary.Config.SecretFeatures.Tooltip")]
		[ReloadRequired]
		[DefaultValue(true)]
		public bool SecretFeatures;

		[Header("$Mods.AltLibrary.Config.Randomization")]

		[Label("$Mods.AltLibrary.Config.SmashingAltarsSpreadsRandom.Label")]
		[Tooltip("$Mods.AltLibrary.Config.SmashingAltarsSpreadsRandom.Tooltip")]
		[DefaultValue(false)]
		public bool SmashingAltarsSpreadsRandom;
		[Label("$Mods.AltLibrary.Config.HardmodeGenRandom.Label")]
		[Tooltip("$Mods.AltLibrary.Config.HardmodeGenRandom.Tooltip")]
		[DefaultValue(false)]
		public bool HardmodeGenRandom;
#pragma warning restore CS0649

		public override void OnLoaded() => Config = this;
	}
}
