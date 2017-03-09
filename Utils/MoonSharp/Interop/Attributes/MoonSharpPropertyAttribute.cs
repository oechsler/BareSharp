using System;

namespace BareKit.Lua.Interpreter
{

	/// <summary>
	/// Marks a property as a configruation property
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
	public sealed class BareKit.LuaPropertyAttribute : Attribute
	{
		/// <summary>
		/// The metamethod name (like '__div', '__ipairs', etc.)
		/// </summary>
		public string Name { get; private set; }


		/// <summary>
		/// Initializes a new instance of the <see cref="BareKit.LuaPropertyAttribute"/> class.
		/// </summary>
		public BareKit.LuaPropertyAttribute()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BareKit.LuaPropertyAttribute"/> class.
		/// </summary>
		/// <param name="name">The name for this property</param>
		public BareKit.LuaPropertyAttribute(string name)
		{
			Name = name;
		}
	}

}
