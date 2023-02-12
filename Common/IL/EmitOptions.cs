using MonoMod.Cil;
using System;

namespace AltLibrary.Common.IL;

public sealed class EmitOptions {
	public ParameterReferenceType[] ParameterTypes { get; set; } = Array.Empty<ParameterReferenceType>();
	public Action<ILCursor>[] BodyIntrinsics { get; set; } = Array.Empty<Action<ILCursor>>();
	public ILLabel[] LabelIntrinsics { get; set; } = Array.Empty<ILLabel>();
}
