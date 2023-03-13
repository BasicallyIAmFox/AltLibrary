using AltLibrary.Common.Loaders;
using AltLibrary.Common.Solutions;
using AltLibrary.Core;
using AltLibrary.Core.Attributes;
using Terraria.ModLoader;

namespace AltLibrary;

public class AltLib : Mod {
	public static AltLib Instance { get; private set; }

	public AltLib() {
		Instance = this;

		LibUtils.ForEachSpecificMod(Instance,
			static t => FeedEventCoreAttribute.GetName(t) != null,
			static (t, _) => {
				FeedEventCoreAttribute.GetMethod(t).Invoke(null, null);
			});

		ILHelper.Init();

		EventSystem.InvokeInit(this);
	}

	public override void Load() {
		SolutionLoader.ResizeArrays();

		EventSystem.InvokeEarlyContent(this);
		EventSystem.InvokeMidContent(this);

		ILHelper.Load();
	}

	public override void PostSetupContent() {
		SolutionLoader.ResizeArrays();

		EventSystem.InvokePostContent(this);

		ILHelper.PostLoad();
	}

	public override void Unload() {
		ILHelper.Unload();

		EventSystem.InvokeUnload(this);

		StaticCollector.Clean();
	}
}