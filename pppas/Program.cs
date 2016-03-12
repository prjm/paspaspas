using PasPasPas.Api;
using PasPasPas.Infrastructure.Internal.Input;
using System;
using System.Text;

namespace Pppas {

    class Program {

        static void Main(string[] args) {
            var arguments = new InputArguments(args);

            if (arguments.Contains("grammar")) {
                StringBuilder sb = new StringBuilder();
                PascalParser.PrintGrammar(sb);
                Console.Out.WriteLine(sb.ToString());
                Console.In.ReadLine();
                return;
            }


            PascalParser parser = new PascalParser();
            PascalFormatter formatter = new PascalFormatter();
            using (var input = new FileInput()) {
                input.FileName = "C:\\temp\\Unit2.pas";
                parser.Input = input;
                var result = parser.Run();

                foreach (var message in parser.Messages) {
                    Console.Out.WriteLine(message.ToSimpleString());
                }

                if (!parser.HasErrors()) {
                    result.ToFormatter(formatter);
                    Console.Out.WriteLine(formatter.Result);
                }

                Console.In.ReadLine();
            }

        }
    }
}
