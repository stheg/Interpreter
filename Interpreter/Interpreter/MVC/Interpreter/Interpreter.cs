using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter
{
    internal sealed class Interpreter
    {
        public Dictionary<string, float> DefinedVariables { get; set; }
        private LinkedListNode<Statement> firstNode;
        private LinkedList<LinkedListNode<Statement>> temporaryCurrentNode;
        public LinkedList<Exception> ListOfErrors { get; set; }
        private bool isFirstStep = true;
        public LinkedList<string> ListToPrint { get; set; }
        private IOutput outputHandler;

        public Interpreter(LinkedList<Exception> listOfErrors, IOutput outputHandler)
        {
            ListToPrint = new LinkedList<string>();
            ListOfErrors = new LinkedList<Exception>(listOfErrors);
            temporaryCurrentNode = new LinkedList<LinkedListNode<Statement>>();
            DefinedVariables = new Dictionary<string, float>();
            this.outputHandler = outputHandler;
        }

        public LinkedListNode<Statement> InterpreterStep(LinkedListNode<Statement> currNode, Dictionary<string, Statement> listOfLabels)
        {
            if (isFirstStep)
            {
                isFirstStep = false;
                firstNode = currNode;
            }
            if (currNode != null && currNode.Value != null)
            {
                try
                {
                    currNode = InterpretStatement(currNode, listOfLabels);//currNode.Value, currNode.Next);
                }
                catch (InterpreterException e)
                {
                    //ListOfErrors.AddLast(e);
                    outputHandler.PrintToErrors(e);
                    currNode = currNode.Next;
                }
            }
            return currNode;
        }

        private LinkedListNode<Statement> GetNextTempNode(LinkedListNode<Statement> tempNode, LinkedList<LinkedListNode<Statement>> gotoTemporaryNode)
        {
            if (tempNode.Value is While)
            {
                gotoTemporaryNode.AddFirst(tempNode.Next);
                temporaryCurrentNode.AddFirst(tempNode);
                tempNode = (tempNode.Value as While).Body.List.First;
                return tempNode;
            }
            if (tempNode.Value is For)
            {
                gotoTemporaryNode.AddFirst(tempNode.Next);
                temporaryCurrentNode.AddFirst(tempNode);
                tempNode = (tempNode.Value as For).Body.List.First;
                return tempNode;
            }
            if (tempNode.Value is If)
            {
                gotoTemporaryNode.AddFirst(tempNode);
                temporaryCurrentNode.AddFirst(tempNode.Next);
                tempNode = (tempNode.Value as If).ThenBody.List.First;
                return tempNode;
            }
            if (tempNode.Value.EndThenBody)
            {
                tempNode = (gotoTemporaryNode.First.Value.Value as If).ElseBody.List.First;
                gotoTemporaryNode.RemoveFirst();
                temporaryCurrentNode.RemoveFirst();
                return tempNode;
            }
            if (tempNode.Value.EndElseBody)
            {
                tempNode = gotoTemporaryNode.First.Value.Next;
                gotoTemporaryNode.RemoveFirst();
                temporaryCurrentNode.RemoveFirst();
                return tempNode;
            }
            if (tempNode.Value.EndForBody)
            {
                tempNode = gotoTemporaryNode.First.Value;
                gotoTemporaryNode.RemoveFirst();
                temporaryCurrentNode.RemoveFirst();
                return tempNode;
            }
            if (tempNode.Value.EndWhile)
            {
                tempNode = gotoTemporaryNode.First.Value;
                gotoTemporaryNode.RemoveFirst();
                temporaryCurrentNode.RemoveFirst();
                return tempNode;
            }
            tempNode = tempNode.Next;
            return tempNode;
        }

        private LinkedListNode<Statement> InterpretStatement(LinkedListNode<Statement> currentNode, Dictionary<string, Statement> listOfLabels)//Statement statement, LinkedListNode<Statement> nextNode)
        {
            Statement statement = currentNode.Value;
            LinkedListNode<Statement> nextNode = currentNode.Next;

            if (statement is Assignment)
            {
                InterpretAssignment(statement as Assignment);
                return nextNode;
            }
            if (statement is Goto)
            {
                LinkedList<LinkedListNode<Statement>> gotoTemporaryNode = new LinkedList<LinkedListNode<Statement>>();
                LinkedListNode<Statement> tempNode = firstNode;
                while (tempNode != null &&
                    tempNode.Value != listOfLabels[(statement as Goto).Identificator.Value])
                {
                    tempNode = GetNextTempNode(tempNode, gotoTemporaryNode);
                }
                if (tempNode == null)
                {
                    throw new InterpreterException("Undefined label", currentNode.Value.Row, currentNode.Value.ColumnStartPos);
                }
                currentNode = tempNode;
                return currentNode;
            }
            if (statement.EndWhile)
            {
                currentNode = temporaryCurrentNode.First.Value;
                temporaryCurrentNode.RemoveFirst();
                return currentNode;
            }
            if (statement is While)
            {
                temporaryCurrentNode.AddFirst(currentNode);
                if (InterpretLogicCondition((statement as While).LogicCondition))
                {
                    currentNode = (statement as While).Body.List.First;
                }
                else
                {
                    temporaryCurrentNode.RemoveFirst();
                    currentNode = nextNode;
                }
                return currentNode;
            }
            if (statement.EndThenBody || statement.EndElseBody)
            {
                currentNode = temporaryCurrentNode.First.Value;
                temporaryCurrentNode.RemoveFirst();
                return currentNode;
            }
            if (statement is If)
            {
                temporaryCurrentNode.AddFirst(currentNode.Next);
                currentNode = InterpretLogicCondition((statement as If).LogicCondition) ?
                    (statement as If).ThenBody.List.First :
                    (statement as If).ElseBody.List.First;
                return currentNode;
            }
            if (statement.EndForBody)
            {
                InterpretAssignment((temporaryCurrentNode.First.Value.Value as For).ExprForAction);
                if (InterpretLogicCondition((temporaryCurrentNode.First.Value.Value as For).LogicCondition))
                {
                    currentNode = (temporaryCurrentNode.First.Value.Value as For).Body.List.First;
                }
                else
                {
                    currentNode = temporaryCurrentNode.First.Value.Next;
                    temporaryCurrentNode.RemoveFirst();
                }
                return currentNode;
            }
            if (statement is For)
            {
                temporaryCurrentNode.AddFirst(currentNode);
                InterpretAssignment((statement as For).Initialization);
                if (InterpretLogicCondition((statement as For).LogicCondition))
                {
                    currentNode = (statement as For).Body.List.First;
                }
                else
                {
                    currentNode = nextNode;
                }
                return currentNode;
            }
            if (statement is Print)
            {
                InterpretPrint(statement as Print);
                return nextNode;
            }
            else
                throw new InterpreterException("Undefined statement", statement.Row, statement.ColumnStartPos);
        }

        private void InterpretPrint(Print print)
        {
            if (print.TextToPrint != null)
            {
                outputHandler.PrintToOutput(print.TextToPrint);
            }
            else if (print.ExprToPrint != null)
            {
                outputHandler.PrintToOutput(InterpretExpression(print.ExprToPrint).ToString());
            }
            else
            {
                throw new InterpreterException("Undefined argument to print", print.Row, print.ColumnStartPos);
            }
        }

        private void InterpretAssignment(Assignment assignment)
        {
            float value = InterpretExpression(assignment.Source);
            if (DefinedVariables.ContainsKey(assignment.Destination.Value))
            {
                DefinedVariables[assignment.Destination.Value] = value;
            }
            else
            {
                DefinedVariables.Add(assignment.Destination.Value, value);
            }
            outputHandler.PrintToWatches(assignment.Destination.Value, value);
        }

        private float InterpretExpression(Expression expression)
        {
            if (expression is Number)
            {
                return (expression as Number).Value;
            }
            if (expression is Variable)
            {
                if (DefinedVariables.ContainsKey((expression as Variable).Value))
                {
                    return DefinedVariables[(expression as Variable).Value];
                }
                else
                {
                    throw new InterpreterException
                        ("Undefined variable " + (expression as Variable).Value, expression.Row, expression.ColumnStartPos);
                }
            }
            if (expression is BinaryOperation)
            {
                BinaryOperation binOperation = expression as BinaryOperation;
                if (binOperation.Type == BinaryOperation.BinaryOperationType.Add)
                {
                    float leftExpression = InterpretExpression(binOperation.Left);
                    float rightExpression = InterpretExpression(binOperation.Right);
                    return leftExpression + rightExpression;
                }
                if (binOperation.Type == BinaryOperation.BinaryOperationType.Deg)
                {
                    return (float)Math.Pow((double)InterpretExpression(binOperation.Left), (double)InterpretExpression(binOperation.Right));
                }
                if (binOperation.Type == BinaryOperation.BinaryOperationType.Div)
                {
                    return InterpretExpression(binOperation.Left) / InterpretExpression(binOperation.Right);
                }
                if (binOperation.Type == BinaryOperation.BinaryOperationType.Mult)
                {
                    return InterpretExpression(binOperation.Left) * InterpretExpression(binOperation.Right);
                }
                else
                {
                    float leftExpression = InterpretExpression(binOperation.Left);
                    float rightExpression = InterpretExpression(binOperation.Right);
                    return leftExpression - rightExpression;
                }
            }
            else
            {
                return 0;
            }
        }

        private bool InterpretLogicCondition(LogicExpression logicCondition)
        {
            if (logicCondition.LogicOpearation == LogicExpression.LogicOperation.Equal)
            {
                return InterpretExpression(logicCondition.LeftExpr) == InterpretExpression(logicCondition.RightExpr);
            }
            if (logicCondition.LogicOpearation == LogicExpression.LogicOperation.Less)
            {
                return InterpretExpression(logicCondition.LeftExpr) < InterpretExpression(logicCondition.RightExpr);
            }
            if (logicCondition.LogicOpearation == LogicExpression.LogicOperation.LessEqual)
            {
                return InterpretExpression(logicCondition.LeftExpr) <= InterpretExpression(logicCondition.RightExpr);
            }
            if (logicCondition.LogicOpearation == LogicExpression.LogicOperation.More)
            {
                return InterpretExpression(logicCondition.LeftExpr) > InterpretExpression(logicCondition.RightExpr);
            }
            if (logicCondition.LogicOpearation == LogicExpression.LogicOperation.MoreEqual)
            {
                return InterpretExpression(logicCondition.LeftExpr) >= InterpretExpression(logicCondition.RightExpr);
            }
            else
            {
                return InterpretExpression(logicCondition.LeftExpr) != InterpretExpression(logicCondition.RightExpr);
            }
        }
    }
}
