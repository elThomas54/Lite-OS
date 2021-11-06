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
            Console.WriteLine("El sistema arranc� con �xito. Escriba una l�nea de texto para que se repita.");
        }

        protected override void Run()
        {
            Console.Write("Ingrese alg�n texto para que se le repita: ");
            Console.WriteLine(Console.ReadLine());
        }
    }

}