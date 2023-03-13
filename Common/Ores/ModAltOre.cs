using System;
using AltLibrary.Common.Groups;
using AltLibrary.Content.Groups;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace AltLibrary.Common.Ores;

public abstract class ModAltOre : ModTexturedType {
	public int Type { get; internal set; }
	public Group<ModAltOre> Group { get; protected set; }

	public override void SetupContent() {
		SetStaticDefaults();
	}

	protected override void Register() {
		throw new NotSupportedException($"Use {typeof(ModAltOre<>).FullName} instead.");
	}
}
public abstract class ModAltOre<T> : ModAltOre where T : Group<ModAltOre> {
	public sealed override void SetupContent() {
		Group = OreGroup.Instance;
		Group.Add(this);

		base.SetupContent();
	}

	protected sealed override void Register() {
		ModContent.Request<Texture2D>(Texture, AssetRequestMode.ImmediateLoad);

		Type = AltOreLoader.Register(this);
	}
}
