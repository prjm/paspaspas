using System.IO;
using PasPasPas.Api;
using PasPasPas.Globals.Environment;

namespace SampleRunner.Scenarios {

    /// <summary>
    ///     test to create an assembly
    /// </summary>
    public static class CreateAssembly {

        /// <summary>
        ///     run the assembly creator test
        /// </summary>
        /// <param name="b"></param>
        /// <param name="environment"></param>
        /// <param name="testPath"></param>
        /// <param name="reapeat"></param>
        public static void Run(TextWriter b, IAssemblyBuilderEnvironment environment, string testPath, int reapeat) {
            for (var i = 0; i < reapeat; i++) {
                var resolver = CommonApi.CreateAnyFileResolver();
                var opts = Factory.CreateOptions(resolver, environment);
                var api = new AssemblyBuilderApi(environment, opts);
                var file = environment.CreateFileReference(testPath);
                var asm = api.CreateAssemblyForProject(file);

#if DESKTOP

#else
                var crt = new Lokad.ILPack.AssemblyGenerator();

                crt.GenerateAssembly(asm.GeneratedAssembly, @"C:\temp\demo.dll");
                b.WriteLine("Test assembly created.");

#endif

            }
        }
    }
}
