using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using Tsinswreng.Srefl;
using Tsinswreng.CsTreeTest;

namespace Tsinswreng.Srefl.Test;

internal class Program{
	[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_PropAccessorReg")]
	private static extern IPropAccessorReg? GetPropAccessorRegUnsafe(SharedModelStrAccRegistry Registry);

	[UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_InstMkrReg")]
	private static extern IInstMkrReg? GetInstMkrRegUnsafe(SharedModelStrAccRegistry Registry);

	public static async Task Main(string[] args){
		IServiceCollection svcColct = new ServiceCollection();
		svcColct
			.AddSingleton<IPropAccessorReg>(sp => {
				var registry = new SharedModelStrAccRegistry();
				var accessorMgr = GetPropAccessorRegUnsafe(registry);
				if(accessorMgr is null){
					throw new Exception($"Property PropAccessorReg on {nameof(SharedModelStrAccRegistry)} is null or not {nameof(IPropAccessorReg)}");
				}
				return accessorMgr;
			})
			.AddSingleton<IInstMkrReg>(sp => {
				var registry = new SharedModelStrAccRegistry();
				var instMkrReg = GetInstMkrRegUnsafe(registry);
				if(instMkrReg is null){
					throw new Exception($"Property InstMkrReg on {nameof(SharedModelStrAccRegistry)} is null or not {nameof(IInstMkrReg)}");
				}
				return instMkrReg;
			})
		;
		var mgr = StrAccTestMgr.Inst;
		_ = mgr.InitSvc(svcColct, sc => sc.BuildServiceProvider());

		ITestExecutor executor = new TreeTestExecutor();
		await executor.RunEtPrint(mgr.TestNode);
	}
}
