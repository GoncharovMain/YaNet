using System.Reflection;

namespace YaNet.Structures
{
	public abstract class Structure
	{
		protected Marker _marker;
		protected PropertyInfo _propertyInfo;
		protected object _obj;

		protected Structure(Marker marker, object obj)
		{
			_marker = marker;
			_obj = obj;
		}

		public abstract void Init();
	}
}