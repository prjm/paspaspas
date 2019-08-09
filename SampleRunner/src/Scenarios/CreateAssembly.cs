using System.IO;
using Lokad.ILPack;
using PasPasPas.Api;
using PasPasPas.Globals.Environment;

namespace SampleRunner.Scenarios {

    /// <summary>
    ///     test to create an assembly
    /// </summary>
    public static class CreateAssembly {
        public static void Run(TextWriter b, IAssemblyBuilderEnvironment environment, string testPath, int reapeat) {
            for (var i = 0; i < reapeat; i++) {
                var opts = Factory.CreateOptions(environment, default);
                var api = new AssemblyBuilderApi(environment, opts);
                var file = api.Parser.Tokenizer.Readers.CreateFileRef(testPath);
                var resolver = CommonApi.CreateAnyFileResolver(api.Parser.Tokenizer.Readers);
                var asm = api.CreateAssemblyForProject(resolver, file);
                var crt = new AssemblyGenerator();

                crt.GenerateAssembly(asm.GeneratedAssembly, @"C:\temp\demo.dll");
                b.WriteLine("Test assembly created.");
            }
        }
    }
}
