using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IL2CPU.Debug.Symbols;

namespace Cosmos.IL2CPU.Profiler {
    public class Assembler : Cosmos.IL2CPU.AppAssembler
    {

        public Assembler(string assemblerLogFile)
            : base(0, assemblerLogFile)
        {
        }

        protected override void InitILOps(Type aAssemblerBaseOp)
        {
            var xILOp = new ILOp(this.Assembler);
            DebugInfo = new DebugInfo(AppContext.BaseDirectory + "DebugInfo.mdf", true, true);
            // No cambie el tipo en el foreach a una var, es necesario como ahora
            // para encasillarlo, de modo que podamos volver a convertirlo en un int.
            foreach (ILOpCode.Code xCode in Enum.GetValues(typeof(ILOpCode.Code)))
            {
                int xCodeValue = (int)xCode;
                if (xCodeValue <= 0xFF)
                {
                    mILOpsLo[xCodeValue] = xILOp;
                }
                else
                {
                    mILOpsHi[xCodeValue & 0xFF] = xILOp;
                }
            }
        }

    }
}

