using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Cosmos.Build.Common;
using IL2CPU.Debug.Symbols;
using Cosmos.System;
using Console = System.Console;

namespace Cosmos.IL2CPU.Profiler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Program.DoScan();
            Console.WriteLine("Pulse cualquier tecla para continuar.");
            Console.ReadKey();
        }

        private static void DoScan()
        {

            var xSW = new Stopwatch();
            xSW.Start();
            string MDFFile = AppContext.BaseDirectory + "TestKernel.mdf";
            if (File.Exists(MDFFile))
                File.Delete(MDFFile);

            var outFile = AppContext.BaseDirectory + "TestKernel.out";
            if (File.Exists(outFile))
                File.Delete(outFile);

            var logFile = AppContext.BaseDirectory + "TestKernel.log";
            if (File.Exists(logFile))
                File.Delete(logFile);

            var xAsmblr = new AppAssembler(1, "Cosmos.Assembler.Log");
            using (var xScanner = new ILScanner(xAsmblr))
            {
                xScanner.LogException = (Exception e) =>
                {
                    Console.WriteLine("ILScanner exception : " + e.Message);
                };
                using (var xDebugInfo = new DebugInfo(MDFFile, true, true))
                {
                    xAsmblr.DebugInfo = xDebugInfo;
                    xAsmblr.DebugEnabled = true;
                    xAsmblr.DebugMode = DebugMode.Source;
                    xAsmblr.TraceAssemblies = TraceAssemblies.All;
                    xAsmblr.IgnoreDebugStubAttribute = false;

                    xAsmblr.Assembler.Initialize();
                    //TODO: Add plugs into the scanning equation to profile scanning them too
                    Type xFoundType = typeof(FakeKernel);
                    var xCtor = xFoundType.GetConstructor(Type.EmptyTypes);
                    var xEntryPoint = typeof(Kernel).GetMethod("Comienzo", BindingFlags.Public | BindingFlags.Instance);
                    xScanner.EnableLogging(logFile);
                    xScanner.QueueMethod(xEntryPoint);
                    xScanner.Execute(xCtor);
                    using (var xOut = new StreamWriter(File.OpenWrite(outFile)))
                    {
                        xAsmblr.Assembler.FlushText(xOut);
                        xAsmblr.FinalizeDebugInfo();
                    }
                    xSW.Stop();
                    Console.WriteLine("Total time : {0}", xSW.Elapsed);
                    Console.WriteLine("Method count: {0}", xScanner.MethodCount);
                    //Console.WriteLine("Instruction count: {0}", xScanner.InstructionCount);
                }
            }
        }

        static void xScanner_TempDebug(string obj)
        {
            Console.WriteLine(obj);
        }

        // Este es un punto de entrada ficticio para que el escáner comience.
        // Ni siquiera es una aplicación de Cosmos, solo una aplicación de consola estándar de Windows,
        // pero eso está bien para el perfil del escáner como lo hace
        // no compilarlo realmente.
        private static void ScannerEntryPoint()
        {
            Console.WriteLine("¡Hola Mundo!");
            var xInt = 0;
            object xObj = xInt;
            xObj.ToString();
        }
    }
}

