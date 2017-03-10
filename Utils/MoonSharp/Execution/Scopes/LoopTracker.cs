using BareKit.Lua.DataStructs;
using BareKit.Lua.Execution.VM;

namespace BareKit.Lua.Execution
{
	interface ILoop
	{
		void CompileBreak(ByteCode bc);
		bool IsBoundary();
	}


	internal class LoopTracker
	{
		public FastStack<ILoop> Loops = new FastStack<ILoop>(16384);
	}
}
