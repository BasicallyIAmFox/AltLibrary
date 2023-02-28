using System;

namespace AltLibrary.Core.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
internal class LoadableContentAttribute : Attribute {
	public ContentOrder ContentOrder { get; }
	public string LoadName { get; }
	public string UnloadName { get; }

	public LoadableContentAttribute(ContentOrder contentOrder, string loadName, string unloadName = null) {
		ContentOrder = contentOrder;
		LoadName = loadName;
		UnloadName = unloadName;
	}
}