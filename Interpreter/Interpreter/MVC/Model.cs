using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter
{
    interface IModel
    {
        //LinkedList<Exception> GetListOfErrors();
        Dictionary<string, Statement> GetListOfLabels();
        LinkedListNode<Statement> GetCurrentNode();
        void SetCurrentNode(LinkedListNode<Statement> currNode);
        Interpreter GetInterpreter();
        //LinkedListNode<Statement> GetNodeWithStatementPosition(int row, int colStartPos);
    }

    public struct BreakpointPosition
    {
        public int row;
        public int column;
    }

    internal sealed class Model : IModel
    {
        private Interpreter interpreter;
        private Parser parser;
        public LinkedListNode<Statement> CurrentNode { get; set; }
        private Dictionary<string, Statement> listOfLabels;

        public Model(string programCode, List<BreakpointPosition> listOfBreakpoints, IOutput outputHandler)
        {
            Lexer lexer = new Lexer(programCode, outputHandler);
            parser = new Parser(lexer, lexer.ListOfErrors, listOfBreakpoints, outputHandler);
            interpreter = new Interpreter(parser.ListOfErrors, outputHandler);
            CurrentNode = (parser.RootNode as Program).StatementList.List.First;
            listOfLabels = new Dictionary<string, Statement>((parser.RootNode as Program).ListOfLabels);
        }

        public Interpreter GetInterpreter()
        {
            return interpreter;
        }

        public Dictionary<string, Statement> GetListOfLabels()
        {
            return listOfLabels;
        }

        public void SetCurrentNode(LinkedListNode<Statement> currNode)
        {
            CurrentNode = currNode;
        }

        public LinkedListNode<Statement> GetCurrentNode()
        {
            return CurrentNode;
        }
    }
}
