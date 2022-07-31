namespace AltLibrary.Common.AltBiomes
{
	public class AltMaterialContext
	{
		internal int EvilOre = -1;
		internal int EvilBar = -1;
		internal int LightBar = -1;
		internal int UnderworldBar = -1;
		internal int TropicalBar = -1;
		internal int MushroomBar = -1;
		internal int EvilSword = -1;
		internal int LightSword = -1;
		internal int UnderworldSword = -1;
		internal int TropicalSword = -1;
		internal int CombinationSword = -1;
		internal int TrueCombinationSword = -1;
		internal int TrueLightSword = -1;
		internal int VileInnard = -1;
		internal int LightResidue = -1;
		internal int LightInnard = -1;
		internal int LightComponent = -1;
		internal int VileComponent = -1;
		internal int EvilBossDrop = -1;
		internal int TropicalComponent = -1;
		internal int EvilHerb = -1;
		internal int UnderworldHerb = -1;
		internal int TropicalHerb = -1;
		internal int UnderworldForge = -1;

		public AltMaterialContext()
		{
		}

		/// <summary>
		/// For Evil alts.<br/>Vanilla values: Demonite Ore, Crimtane Ore
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public AltMaterialContext SetEvilOre(int value)
		{
			EvilOre = value;
			return this;
		}

		/// <summary>
		/// For Evil alts.<br/>Vanilla values: Demonite Bar, Crimtane Bar
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public AltMaterialContext SetEvilBar(int value)
		{
			EvilBar = value;
			return this;
		}

		/// <summary>
		/// For Hallow alts.<br/>Vanilla values: Hallowed Bar
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public AltMaterialContext SetLightBar(int value)
		{
			LightBar = value;
			return this;
		}

		/// <summary>
		/// For Underworld alts.<br/>Vanilla values: Hellstone Bar
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public AltMaterialContext SetUnderworldBar(int value)
		{
			UnderworldBar = value;
			return this;
		}

		/// <summary>
		/// For Jungle alts.<br/>Vanilla values: Chlorophyte Bar
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public AltMaterialContext SetTropicalBar(int value)
		{
			TropicalBar = value;
			return this;
		}

		/// <summary>
		/// For Jungle alts.<br/>Vanilla values: Shroomite Bar
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public AltMaterialContext SetMushroomBar(int value)
		{
			MushroomBar = value;
			return this;
		}

		/// <summary>
		/// For Evil alts.<br/>Vanilla values: Light's Bane, Blood Butcherer
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public AltMaterialContext SetEvilSword(int value)
		{
			EvilSword = value;
			return this;
		}

		/// <summary>
		/// For Hallow alts.<br/>Vanilla values: Excalibur
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public AltMaterialContext SetLightSword(int value)
		{
			LightSword = value;
			return this;
		}

		/// <summary>
		/// For Underworld alts.<br/>Vanilla values: Fiery Greatsword
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public AltMaterialContext SetUnderworldSword(int value)
		{
			UnderworldSword = value;
			return this;
		}

		/// <summary>
		/// For Jungle alts.<br/>Vanilla values: Blade of Grass
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public AltMaterialContext SetTropicalSword(int value)
		{
			TropicalSword = value;
			return this;
		}

		/// <summary>
		/// For Evil alts.<br/>Vanilla values: Night's Edge
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public AltMaterialContext SetCombinationSword(int value)
		{
			CombinationSword = value;
			return this;
		}

		/// <summary>
		/// For Evil alts.<br/>Vanilla values: True Night's Edge
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public AltMaterialContext SetTrueCombinationSword(int value)
		{
			TrueCombinationSword = value;
			return this;
		}

		/// <summary>
		/// For Hallow alts.<br/>Vanilla values: True Excalibur
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public AltMaterialContext SetTrueLightSword(int value)
		{
			TrueLightSword = value;
			return this;
		}

		/// <summary>
		/// For Evil alts.<br/>Vanilla values: Rotten Chunk, Vertebrae
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public AltMaterialContext SetVileInnard(int value)
		{
			VileInnard = value;
			return this;
		}

		/// <summary>
		/// For Hallow alts.<br/>Vanilla values: Pixie Dust
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public AltMaterialContext SetLightResidue(int value)
		{
			LightResidue = value;
			return this;
		}

		/// <summary>
		/// For Hallow alts.<br/>Vanilla values: Unicorn Horn
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public AltMaterialContext SetLightInnard(int value)
		{
			LightInnard = value;
			return this;
		}

		/// <summary>
		/// For Hallow alts.<br/>Vanilla values: Crystal Shards
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public AltMaterialContext SetLightComponent(int value)
		{
			LightComponent = value;
			return this;
		}

		/// <summary>
		/// For Evil alts.<br/>Vanilla values: Cursed Flame, Ichor
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public AltMaterialContext SetVileComponent(int value)
		{
			VileComponent = value;
			return this;
		}

		/// <summary>
		/// For Evil alts.<br/>Vanilla values: Shadow Scale, Tissue Sample
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public AltMaterialContext SetEvilBossDrop(int value)
		{
			EvilBossDrop = value;
			return this;
		}

		/// <summary>
		/// For Jungle alts.<br/>Vanilla values: Jungle Spores
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public AltMaterialContext SetTropicalComponent(int value)
		{
			TropicalComponent = value;
			return this;
		}

		/// <summary>
		/// For Evil alts.<br/>Vanilla values: Deathweed
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public AltMaterialContext SetEvilHerb(int value)
		{
			EvilHerb = value;
			return this;
		}

		/// <summary>
		/// For Underworld alts.<br/>Vanilla values: Fireblossom
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public AltMaterialContext SetUnderworldHerb(int value)
		{
			UnderworldHerb = value;
			return this;
		}

		/// <summary>
		/// For Jungle alts.<br/>Vanilla values: Moonglow
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public AltMaterialContext SetTropicalHerb(int value)
		{
			TropicalHerb = value;
			return this;
		}

		/// <summary>
		/// For Underworld alts.<br/>Vanilla values: Hellforge
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public AltMaterialContext SetUnderworldForge(int value)
		{
			UnderworldForge = value;
			return this;
		}
	}
}
