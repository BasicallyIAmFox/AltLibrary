using AltLibrary.Common.Attributes;
using AltLibrary.Common.Solutions;
using AltLibrary.Content.Biomes;
using Terraria.ID;
using Terraria.ModLoader;
using static AltLibrary.Common.CID.ConversionInheritanceData;

namespace AltLibrary.Common.CID;

// TODO: More optimization.
public abstract class ConversionInheritanceData {
	public const int Keep = -1;
	public const int Break = -2;

	internal readonly int[] tiles;
	internal readonly int count;

	public ConversionInheritanceData(int count) {
		this.count = count;
		tiles = new SetFactory(ISolution.solutions.Count * count).CreateIntSet(defaultState: Keep);
	}

	internal int GetId(int id, int tile) => id * count + tile;
	public static int GetIdOf(int conversionType) => conversionType switch {
		BiomeConversionID.Purity => GetIdOf<GreenSolution>(),
		BiomeConversionID.Sand => GetIdOf<YellowSolution>(),
		BiomeConversionID.Dirt => GetIdOf<BrownSolution>(),
		BiomeConversionID.Snow => GetIdOf<WhiteSolution>(),
		BiomeConversionID.GlowingMushroom => GetIdOf<DarkBlueSolution>(),

		BiomeConversionID.Corruption => GetIdOf<CorruptBiome>(),
		BiomeConversionID.Crimson => GetIdOf<CrimsonBiome>(),
		BiomeConversionID.Hallow => GetIdOf<HallowBiome>(),
		_ => conversionType
	};
	public static int GetIdOf(ISolution solution) => solution.Type;
	public static int GetIdOf<T>() where T : class, ISolution => ModContent.GetInstance<T>().Type;

	public abstract void Bake();

	public int Get(int id, int tile) => tiles[GetId(id, tile)];
	public int Get<T>(int tile) where T : class, ISolution => Get(GetIdOf<T>(), tile);

	public void Set(int id, int baseTile, int tile) => tiles[GetId(id, baseTile)] = tile;
	public void Set<T>(int baseTile, int tile) where T : class, ISolution => Set(GetIdOf<T>(), baseTile, tile);
}
[LoadableContent(ContentOrder.PostContent, nameof(Load))]
public static class ConversionInheritanceDatabase {
	private static ConversionInheritanceDataTile TileData;
	private static ConversionInheritanceDataWall WallData;

	public static void Load() {
		(TileData = new()).Bake();
		(WallData = new()).Bake();
	}

	public static int GetConvertedTile(int conversionType, int baseTile) => TileData.Get(baseTile, conversionType);
	public static int GetConvertedTile(ISolution solution, int baseTile) => GetConvertedTile(GetIdOf(solution), baseTile);
	public static int GetConvertedTile<T>(int baseTile) where T : class, ISolution => GetConvertedTile(GetIdOf<T>(), baseTile);

	public static int GetConvertedWall(int conversionType, int baseTile) => WallData.Get(baseTile, conversionType);
	public static int GetConvertedWall(ISolution biome, int baseTile) => GetConvertedWall(GetIdOf(biome), baseTile);
	public static int GetConvertedWall<T>(int baseTile) where T : class, ISolution => GetConvertedWall(GetIdOf<T>(), baseTile);
}
