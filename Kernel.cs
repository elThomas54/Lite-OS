using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cosmos.IL2CPU.Profiler
{
    public class Kernel: Cosmos.System.Kernel
    {
        protected override void BeforeRun()
        {
            Console.WriteLine("El sistema arrancó con éxito. Escriba una línea de texto para que se repita.");
        }

        protected override void Run()
        {
            Console.Write("Ingrese algún texto para que se le repita: ");
            Console.WriteLine(Console.ReadLine());
        }
    }

}