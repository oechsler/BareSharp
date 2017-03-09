using System.Collections.Generic;
using BareKit.Lua.Interpreter.Execution;
using BareKit.Lua.Interpreter.Execution.VM;

namespace BareKit.Lua.Interpreter.Tree
{
	internal class Loop : ILoop
	{
		public RuntimeScopeBlock Scope;
		public List<Instruction> BreakJumps = new List<Instruction>();

		public void CompileBreak(ByteCode bc)
		{
			bc.Emit_Exit(Scope);
			BreakJumps.Add(bc.Emit_Jump(OpCode.Jump, -1));
		}

		public bool IsBoundary()
		{
			return false;
		}
	}

	internal class LoopBoundary : ILoop
	{
		public void CompileBreak(ByteCode bc)
		{
			throw new InternalErrorException("CompileBreak called on LoopBoundary");
		}

		public bool IsBoundary()
		{
			return true;
		}
	}

}
