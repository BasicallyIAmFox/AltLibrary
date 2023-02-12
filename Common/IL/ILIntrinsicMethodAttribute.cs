using System;

namespace AltLibrary.Common.IL;

[AttributeUsage(AttributeTargets.Method)]
public sealed class ILIntrinsicMethodAttribute<T> : Attribute where T : ILIntrinsicMethodImpl {
	public IntrisicType IntrisicType { get; }

	public ILIntrinsicMethodAttribute(IntrisicType intrisicType) {
		IntrisicType = intrisicType;
	}
}