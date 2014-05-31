using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Interpreter
{
    interface IOutput
    {
        void PrintToWatches(string variable, float value);
        void PrintToOutput(string value);
        void PrintToErrors(LexerException e);
        void PrintToErrors(ParserException e);
        void PrintToErrors(InterpreterException e);
    }

    internal sealed class Output : IOutput
    {
        private LinkedList<Form> listOfView;
        public Output(LinkedList<Form> listOfView)
        {
            this.listOfView = listOfView;
        }

        public void PrintToErrors(ParserException e)
        {
            foreach (IDE view in listOfView)
            {
                view.RefreshErrors(e.Message, e.Row, e.ColStartPos);
            }
        }
        
        public void PrintToErrors(InterpreterException e)
        {
            foreach (IDE view in listOfView)
            {
                view.RefreshErrors(e.Message, e.Row, e.Column);
            }
        }

        public void PrintToErrors(LexerException e)
        {
            foreach (IDE view in listOfView)
            {
                view.RefreshErrors(e.Message, e.Row, e.Column);
            }
        }

        public void PrintToWatches(string variable, float value)
        {
            foreach (IDE view in listOfView)
            {
                view.RefreshWatches(variable, value);
            }
        }

        public void PrintToOutput(string value)
        {
            foreach (IDE view in listOfView)
            {
                view.RefreshOutput(value);
            }
        }
    }
}
