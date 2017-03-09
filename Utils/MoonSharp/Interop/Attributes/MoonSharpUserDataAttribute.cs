using System;

namespace BareKit.Lua.Interpreter
{
	/// <summary>
	/// Marks a type of automatic registration as userdata (which happens only if UserData.RegisterAssembly is called).
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
	public sealed class BareKit.LuaUserDataAttribute : Attribute
	{
		/// <summary>
		/// The interop access mode
		/// </summary>
		public InteropAccessMode AccessMode { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="BareKit.LuaUserDataAttribute"/> class.
		/// </summary>
		public BareKit.LuaUserDataAttribute()
		{
			AccessMode = InteropAccessMode.Default;
		}
	}
}
