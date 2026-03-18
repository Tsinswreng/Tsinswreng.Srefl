using Microsoft.Extensions.DependencyInjection;
using Tsinswreng.CsStrAcc;
using Tsinswreng.CsTreeTest;

namespace Tsinswreng.CsStrAcc.Test;

internal class Program{
	public static async Task Main(string[] args){
		IServiceCollection svcColct = new ServiceCollection();
		//svcColct
			
		//;
		var mgr = StrAccTestMgr.Inst;
		_ = mgr.InitSvc(svcColct, sc => sc.BuildServiceProvider());

		ITestExecutor executor = new TreeTestExecutor();
		await executor.RunEtPrint(mgr.TestNode);
	}
}
