using Terraria.ModLoader;

namespace AltLibrary.Common;

public interface IPostContent {
	void Load(Mod mod);
	void Unload();
}
