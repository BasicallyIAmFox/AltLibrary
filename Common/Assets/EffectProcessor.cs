using Microsoft.Xna.Framework.Graphics;
using System.Threading;
using Terraria;
using Terraria.ModLoader;

namespace AltLibrary.Common.Assets;

public sealed class EffectProcessor : Processor<Effect> {
	public override Effect Load(string path) {
		ModContent.SplitName(path, out string modName, out string pathToFile);

		var bytes = ModLoader.GetMod(modName).GetFileBytes($"{pathToFile}.fxb");
		Effect effect = null;

		using ManualResetEventSlim manualEventSlim = new();
		Main.QueueMainThreadAction(() => {
			effect = new(Main.graphics.GraphicsDevice, bytes);
			manualEventSlim.Set();
		});
		manualEventSlim.Wait();
		return effect;
	}
}
