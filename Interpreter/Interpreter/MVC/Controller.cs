using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Interpreter
{
    internal sealed class Controller
    {
        private LinkedList<Form> listOfView;
        private List<BreakpointPosition> listOfBreakpointPosition = new List<BreakpointPosition>();
        private IModel model;
        private IOutput outputHandler;

        public Controller(LinkedList<Form> listOfView)
        {
            this.listOfView = listOfView;
            outputHandler = new Output(listOfView);
        }

        public void Run()
        {
            LinkedListNode<Form> currentView = listOfView.First;
            while (currentView != null)
            {
                (currentView.Value as IDE).CallCompiling += OnCompile;
                (currentView.Value as IDE).CallBreakpoint += OnBreakpoint;
                (currentView.Value as IDE).CallInterpretation += OnInterpretation;
                (currentView.Value as IDE).CallTurnBasedInterpretation += OnTurnBasedInterpretation;
                (currentView.Value as IDE).CallOutput += OnOutput;
                (currentView.Value as IDE).CallHandlerException += OnHandleException;
                currentView.Value.Show();
                currentView = currentView.Next;
            }
            Application.Run();
        }

        private void OnBreakpoint(object sender, BreakpointEventArgs args)
        {
        }

        private void OnCompile(object sender, InterpretationEventArgs args)
        {
            BreakpointPosition breakpointPosZeroZero = new BreakpointPosition();
            breakpointPosZeroZero.row = 0;
            breakpointPosZeroZero.column = 0;
            if (listOfBreakpointPosition.Contains(breakpointPosZeroZero))
            {
                listOfBreakpointPosition.Remove(breakpointPosZeroZero);
            }
            for (int breakpointIndex = 0; breakpointIndex < listOfBreakpointPosition.Count; breakpointIndex++)
            {
                if (listOfBreakpointPosition[breakpointIndex].row == args.BreakpointPosition.row)
                {
                    listOfBreakpointPosition.RemoveAt(breakpointIndex);
                    model = new Model(args.ProgramCode, listOfBreakpointPosition, outputHandler);
                    foreach (var position in listOfBreakpointPosition)
                    {
                        (sender as IDE).RefreshBreakpoints(position);
                    }
                    return;
                }
            }
            listOfBreakpointPosition.Add(args.BreakpointPosition);
            model = new Model(args.ProgramCode, listOfBreakpointPosition, outputHandler);
            foreach (var position in listOfBreakpointPosition)
            {
                (sender as IDE).RefreshBreakpoints(position);
            }
        }

        private void OnHandleException(object sender, ErrorEventArgs args)
        {
            /*foreach (var error in model.GetInterpreter().ListOfErrors)
            {
                if (error is ParserException)
                {
                    (sender as IDE).RefreshErrors(
                        (error as ParserException).Message, (error as ParserException).Row, (error as ParserException).ColStartPos);
                }
                else if (error is LexerException)
                {
                    (sender as IDE).RefreshErrors(
                        (error as LexerException).Message, (error as LexerException).Row, (error as LexerException).Column);
                }
                else
                {
                    (sender as IDE).RefreshErrors(
                        (error as InterpreterException).Message, (error as InterpreterException).Row, (error as InterpreterException).Column);
                }
            }*/
        }

        private void OnTurnBasedInterpretation(object sender, InterpretationEventArgs args)
        {
            if (model.GetCurrentNode() != null)
            {
                //int row = model.GetCurrentNode().Value.Row;
                //(sender as IDE).ColoredStatement(row);
                model.SetCurrentNode(model.GetInterpreter().InterpreterStep(model.GetCurrentNode(), model.GetListOfLabels()));
            }
        }

        private void OnInterpretation(object sender, InterpretationEventArgs args)
        {
            while (model.GetCurrentNode() != null)
            {
                if (!model.GetCurrentNode().Value.IsBreakpoint)
                {
                    model.SetCurrentNode(model.GetInterpreter().InterpreterStep(model.GetCurrentNode(), model.GetListOfLabels()));
                }
                else
                {
                    model.SetCurrentNode(model.GetInterpreter().InterpreterStep(model.GetCurrentNode(), model.GetListOfLabels()));
                    break;
                }
            }
            if (model.GetCurrentNode() == null && model.GetInterpreter().ListToPrint.Count != 0 && !model.GetInterpreter().ListToPrint.Last.Value.Equals("End of program."))
            {
                model.GetInterpreter().ListToPrint.AddLast("End of program.");
            }
        }

        private void OnOutput(object sender, OutputEventArgs args)
        {
        }
    }
}
