using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using BareKit.Lua.Compatibility.Frameworks;

namespace BareKit.Lua.Compatibility
{
	public static class Framework
	{
		static FrameworkCurrent s_FrameworkCurrent = new FrameworkCurrent();

		public static FrameworkBase Do { get { return s_FrameworkCurrent; } }
	}
}
