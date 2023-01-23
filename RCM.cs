using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestPlugin
{
    class RCM
    {
    }

    class RC_IMPORT : IRCFunction
    {
        public string GetName() { return "IMPORT"; }
        public int GetArgCount() { return 0; }
        public bool Run(string[] arguments, ref string res)
        {
            Base.Settings.main_window.BeginInvoke(new MethodInvoker(() => {
                DMC.Actions.ImportCAD(Core.Commands.FileImportType.Any);
            }));



            return true;
        }
    }

}
