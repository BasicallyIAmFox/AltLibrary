using System;

namespace AltLibrary.Common.Assets;

public interface IProcessor {
	Type ProcessType { get; }
	object Load(string path);
}
public abstract class Processor<T> : IProcessor where T : class {
	public Type ProcessType => typeof(T);

	object IProcessor.Load(string path) => Load(path);
	public abstract T Load(string path);
}
