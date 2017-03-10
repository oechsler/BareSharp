using BareKit.Lua.Execution;

namespace BareKit.Lua.Tree.Statements
{
	class EmptyStatement : Statement
	{
		public EmptyStatement(ScriptLoadingContext lcontext)
			: base(lcontext)
		{
		}


		public override void Compile(Execution.VM.ByteCode bc)
		{
		}
	}
}
