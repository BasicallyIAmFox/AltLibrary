using System;
using System.Collections.Generic;

namespace AltLibrary.Common.Data;

public interface IReadonlyDataHandler {
	V Get<V>() where V : IAltData;
}
public interface IDataHandler : IReadonlyDataHandler {
	void Add<T>(T data) where T : IAltData;
}

public abstract class DataHandler : IDataHandler {
	private readonly Dictionary<Type, IAltData> database = new();

	public void Add<T>(T data) where T : IAltData {
		if (!database.ContainsKey(typeof(T))) {
			database[typeof(T)] = data;
		}
	}

	public T Get<T>() where T : IAltData {
		return database.TryGetValue(typeof(T), out IAltData data) ? (T)data : default;
	}
}

public interface IAltData { }
public interface IOreData : IAltData { }
public interface IBiomeData : IAltData { }
public sealed class OreDataHandler : DataHandler {
}
public sealed class BiomeDataHandler : DataHandler {
}
