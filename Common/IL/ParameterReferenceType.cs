namespace AltLibrary.Common.IL;

public struct ParameterReferenceType {
	internal ParamRef type;
	internal int index;

	public ParameterReferenceType(ParamRef paramRef, int index) {
		type = paramRef;
		this.index = index;
	}
}
