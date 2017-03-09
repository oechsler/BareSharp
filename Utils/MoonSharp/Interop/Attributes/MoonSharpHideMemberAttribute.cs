using System;

namespace BareKit.Lua.Interpreter
{
	/// <summary>
	/// Lists a userdata member not to be exposed to scripts referencing it by name.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = true, AllowMultiple = true)]
	public sealed class BareKit.LuaHideMemberAttribute : Attribute
	{
		/// <summary>
		/// Gets the name of the member to be hidden.
		/// </summary>
		public string MemberName { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="BareKit.LuaHideMemberAttribute"/> class.
		/// </summary>
		/// <param name="memberName">Name of the member to hide.</param>
		public BareKit.LuaHideMemberAttribute(string memberName)
		{
			MemberName = memberName;
		}
	}
}
