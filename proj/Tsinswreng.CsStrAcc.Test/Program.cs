using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using Tsinswreng.CsStrAcc;
using Tsinswreng.CsTreeTest;

namespace Tsinswreng.CsStrAcc.Test;

internal class Program{
	[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_PropAccessorMgr")]
	private static extern IPropAccessorMgr? GetPropAccessorMgrUnsafe(SharedModelStrAccRegistry Registry);

	public static async Task Main(string[] args){
		IServiceCollection svcColct = new ServiceCollection();
		svcColct
			.AddSingleton<IPropAccessorMgr>(sp => {
				var registry = new SharedModelStrAccRegistry();
				var accessorMgr = GetPropAccessorMgrUnsafe(registry);
				if(accessorMgr is null){
					throw new Exception($"Property PropAccessorMgr on {nameof(SharedModelStrAccRegistry)} is null or not {nameof(IPropAccessorMgr)}");
				}
				return accessorMgr;
			})
		;
		var mgr = StrAccTestMgr.Inst;
		_ = mgr.InitSvc(svcColct, sc => sc.BuildServiceProvider());

		ITestExecutor executor = new TreeTestExecutor();
		await executor.RunEtPrint(mgr.TestNode);
	}
}
