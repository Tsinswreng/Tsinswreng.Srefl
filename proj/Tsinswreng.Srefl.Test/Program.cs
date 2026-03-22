using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using Tsinswreng.Srefl;
using Tsinswreng.CsTreeTest;

namespace Tsinswreng.Srefl.Test;

internal class Program{
	[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_PropAccessorMgr")]
	private static extern IPropAccessorReg? GetPropAccessorMgrUnsafe(SharedModelStrAccRegistry Registry);

	public static async Task Main(string[] args){
		IServiceCollection svcColct = new ServiceCollection();
		svcColct
			.AddSingleton<IPropAccessorReg>(sp => {
				var registry = new SharedModelStrAccRegistry();
				var accessorMgr = GetPropAccessorMgrUnsafe(registry);
				if(accessorMgr is null){
					throw new Exception($"Property PropAccessorMgr on {nameof(SharedModelStrAccRegistry)} is null or not {nameof(IPropAccessorReg)}");
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
