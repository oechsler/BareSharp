using BareKit.Lua.Interpreter.DataStructs;
using BareKit.Lua.Interpreter.Execution.VM;

namespace BareKit.Lua.Interpreter.Execution
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
