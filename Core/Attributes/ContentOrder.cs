using System.ComponentModel;

namespace AltLibrary.Core.Attributes;

internal enum ContentOrder {
	[EditorBrowsable(EditorBrowsableState.Never)]
	Unload = -1,
	Init,
	EarlyContent,
	[EditorBrowsable(EditorBrowsableState.Never)]
	MidContent,
	PostContent,
}
