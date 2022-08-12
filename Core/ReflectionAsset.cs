using System.Reflection;

namespace AltLibrary.Core
{
	public class ReflectionAsset<T> where T : MemberInfo
	{
		private T ownValue;

		private T _name;

		public T Name
		{
			get
			{
				T old = _name;
				_name = null;
				return old;
			}
			private set
			{
				ownValue = null;
				_name = value;
			}
		}

		public T Value => ownValue ??= Name;

		public ReflectionAsset(T info) => Name = info;

		public static explicit operator T(ReflectionAsset<T> asset) => asset.Value;
	}
}
