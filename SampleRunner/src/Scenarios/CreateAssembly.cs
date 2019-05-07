using System.IO;
using PasPasPas.Api;
using PasPasPas.Globals.Environment;

namespace SampleRunner.Scenarios {

    /// <summary>
    ///     test to create an assembly
    /// </summary>
    public static class CreateAssembly {
        public static void Run(TextWriter b, IAssemblyBuilderEnvironment environment, string testPath, int reapeat) {
            for (var i = 0; i < reapeat; i++) {
                var api = new AssemblyBuilderApi(environment);
                api.CreateAssemblyForProject(testPath);
                b.WriteLine("Test assembly created.");
            }
        }
    }
}
