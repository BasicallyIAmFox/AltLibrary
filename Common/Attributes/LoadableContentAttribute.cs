using System;
using System.ComponentModel;

namespace AltLibrary.Common.Attributes;

public enum ContentOrder {
	[EditorBrowsable(EditorBrowsableState.Never)]
	Unload = -1,
	Init,
	EarlyContent,
	[EditorBrowsable(EditorBrowsableState.Never)]
	MidContent,
	PostContent,
}
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class LoadableContentAttribute : Attribute {
	public ContentOrder ContentOrder { get; }
	public string LoadName { get; }
	public string UnloadName { get; set; } = null;

	public LoadableContentAttribute(ContentOrder contentOrder, string loadName) {
		ContentOrder = contentOrder;
		LoadName = loadName;
	}
}
