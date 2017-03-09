using System;

namespace BareKit.Lua.Interpreter.Diagnostics.PerformanceCounters
{
	internal interface IPerformanceStopwatch
	{
		IDisposable Start();
		PerformanceResult GetResult();
	}
}
