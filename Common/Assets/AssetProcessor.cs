using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace AltLibrary.Common.Assets;

public sealed class AssetProcessor : Processor<Asset<Texture2D>> {
	public override Asset<Texture2D> Load(string path) {
		if (string.IsNullOrWhiteSpace(path)) {
			return null;
		}
		return ModContent.Request<Texture2D>(path, AssetRequestMode.ImmediateLoad);
	}
}
