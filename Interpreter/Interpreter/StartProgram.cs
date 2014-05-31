using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Interpreter
{
    internal sealed class StartProgram
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            LinkedList<Form> listOfView = new LinkedList<Form>();
            listOfView.AddLast((new IDE()) as Form);
            Controller controller = new Controller(listOfView);
            controller.Run();
        }
    }
}
