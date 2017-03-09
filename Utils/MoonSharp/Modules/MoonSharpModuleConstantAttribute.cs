using System;

namespace BareKit.Lua.Interpreter
{
	/// <summary>
	/// In a module type, mark fields with this attribute to have them exposed as a module constant.
	/// 
	/// See <see cref="BareKit.LuaModuleAttribute"/> for more information about modules.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public sealed class BareKit.LuaModuleConstantAttribute : Attribute
	{
		/// <summary>
		/// Gets or sets the name of the constant - if different from the name of the field itself
		/// </summary>
		public string Name { get; set; }
	}
}
