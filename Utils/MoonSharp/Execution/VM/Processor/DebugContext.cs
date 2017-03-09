using System.Collections.Generic;
using BareKit.Lua.Interpreter.Debugging;

namespace BareKit.Lua.Interpreter.Execution.VM
{
	sealed partial class Processor
	{
		private class DebugContext
		{
			public bool DebuggerEnabled = true;
			public IDebugger DebuggerAttached = null;
			public DebuggerAction.ActionType DebuggerCurrentAction = DebuggerAction.ActionType.None;
			public int DebuggerCurrentActionTarget = -1;
			public SourceRef LastHlRef = null;
			public int ExStackDepthAtStep = -1;
			public List<SourceRef> BreakPoints = new List<SourceRef>();
			public bool LineBasedBreakPoints = false;
		}
	}
}
