using System;

namespace BareKit.Lua.Interpreter
{
	/// <summary>
	/// Forces a class member visibility to scripts. Can be used to hide public members. Equivalent to BareKit.LuaVisible(false).
	/// </summary>
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field
		| AttributeTargets.Constructor | AttributeTargets.Event, Inherited = true, AllowMultiple = false)]
	public sealed class BareKit.LuaHiddenAttribute : Attribute
	{
	}
}
