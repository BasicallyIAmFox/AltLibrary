namespace AltLibrary.Common.Data;

public struct ConversionData : IBiomeData {
	public int Stone { get; set; }
	public int Sandstone { get; set; }

	public int ThornBush { get; set; }

	public int Grass { get; set; }
	public int JungleGrass { get; set; }
	public int MowedGrass { get; set; }
	public int Mud { get; set; }
	public bool MudToDirt { get; set; }

	public int Sand { get; set; }
	public int Snow { get; set; }
	public int Ice { get; set; }
}
