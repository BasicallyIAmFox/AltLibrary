using System.Collections.Generic;
using Terraria.ModLoader;

namespace AltLibrary.Common.Solutions;

// TODO: Better implementation.
public interface ISolution : IModType {
	internal static readonly List<ISolution> solutions = new();

	int Type { get; }

	void FillTileEntries(int currentTileId, ref int tileEntry);
	void FillWallEntries(int currentWallId, ref int wallEntry);
}
public abstract class Solution : ISolution {
	public int Type { get; private set; }

	public Mod Mod { get; private set; }
	public string Name { get; private set; }
	public string FullName => $"{Mod.Name}/{Name}";

	public abstract void FillTileEntries(int currentTileId, ref int tileEntry);
	public abstract void FillWallEntries(int currentWallId, ref int wallEntry);

	public void Load(Mod mod) {
		Mod = mod;
		Name = GetType().Name;

		Type = ISolution.solutions.Count;
		ISolution.solutions.Add(this);
	}

	public void Unload() {
	}
}
