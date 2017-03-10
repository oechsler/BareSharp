using System;
using BareKit.Lua.Interop.BasicDescriptors;

namespace BareKit.Lua.Interop.StandardDescriptors.HardwiredDescriptors
{
	public abstract class HardwiredUserDataDescriptor : DispatchingUserDataDescriptor
	{
		protected HardwiredUserDataDescriptor(Type T) :
			base(T, "::hardwired::" + T.Name)
		{

		}

	}
}
