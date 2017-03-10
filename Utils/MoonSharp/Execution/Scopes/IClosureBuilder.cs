
namespace BareKit.Lua.Execution
{
	internal interface IClosureBuilder
	{
		SymbolRef CreateUpvalue(BuildTimeScope scope, SymbolRef symbol);

	}
}
