using System.Collections.Generic;
using Terraria.ModLoader;

namespace AltLibrary.Common.Groups;

public abstract class Group : ModType {
	public override void SetupContent() {
		SetStaticDefaults();
	}

	public sealed override void Unload() {
		UnloadInternal();
		UnloadSelf();
	}

	public abstract void Add(object type);

	protected abstract void UnloadInternal();

	public virtual void UnloadSelf() {
	}

	protected override void Register() {
	}
}
public abstract class Group<T> : Group {
	internal readonly List<T> types = new();

	public override void Add(object type) => types.Add((T)type);
	public void Add(T type) => types.Add(type);

	protected override void UnloadInternal() {
		types.Clear();
	}
}