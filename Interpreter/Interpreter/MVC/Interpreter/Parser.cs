using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter
{
    internal sealed class Parser
    {
        private Lexer lexer;
        private Dictionary<string, Statement> nextStatementAfterLabel;
        public Node RootNode { get; private set; }
        public LinkedList<Exception> ListOfErrors { get; set; }
        private List<BreakpointPosition> listOfBreakpoints;
        private IOutput outputHandler;

        public Parser(Lexer lexer, LinkedList<Exception> listOfExceptions, IOutput outputHandler)
        {
            this.outputHandler = outputHandler;
            this.lexer = lexer;
            ListOfErrors = new LinkedList<Exception>(listOfExceptions);
            nextStatementAfterLabel = new Dictionary<string, Statement>();
            RootNode = EatProgram(lexer.GetNextLexem());
            RootNode.ListOfLabels = new Dictionary<string, Statement>(nextStatementAfterLabel);
        }

        public Parser(Lexer lexer, LinkedList<Exception> listOfExceptions, List<BreakpointPosition> listOfBreakpoints, IOutput outputHandler)
        {
            this.lexer = lexer;
            this.outputHandler = outputHandler;
            this.listOfBreakpoints = listOfBreakpoints;
            ListOfErrors = new LinkedList<Exception>(listOfExceptions);
            nextStatementAfterLabel = new Dictionary<string, Statement>();
            RootNode = EatProgram(lexer.GetNextLexem());
            RootNode.ListOfLabels = new Dictionary<string, Statement>(nextStatementAfterLabel);
        }

        private Program EatProgram(Lexem lexem)
        {
            Program program = new Program();
            program.StatementList = EatStatementList(lexem);

            return program;
        }

        private StatementList EatStatementList(Lexem lexem)
        {
            StatementList StatementList = new StatementList();
            StatementList.List = new LinkedList<Statement>();

            while (lexem.Value != null)
            {
                try
                {
                    if (lexem.LexemType == LexType.If ||
                        lexem.LexemType == LexType.For ||
                        lexem.LexemType == LexType.Print ||
                        lexem.LexemType == LexType.Goto ||
                        lexem.LexemType == LexType.While ||
                        (lexem.LexemType == LexType.Variable && lexer.Lookahead().LexemType == LexType.Assignment))
                    {
                        StatementList.List.AddLast(EatStatement(lexem));
                    }
                    else if (lexem.LexemType == LexType.Variable && lexer.Lookahead().LexemType == LexType.Colon)
                    {
                        StatementList.List.AddLast(EatLabel(lexem));
                    }
                    else throw new ParserException("Undefined statement", lexem.Row, lexem.Col.StartPos, lexem.Col.LastPos);
                    lexem = lexer.Lookahead();
                    if (lexem.Value != null)
                    {
                        lexer.GetNextLexem();
                    }
                }
                catch (ParserException e)
                {
                    //ListOfErrors.AddLast(e);
                    outputHandler.PrintToErrors(e);
                    while (lexem.LexemType != LexType.Semicolon &&
                        lexem.LexemType != LexType.CloseFigureBracket &&
                        lexem.LexemType != LexType.OpenFigureBracket &&
                        lexem.Value != null)
                    {
                        lexem = lexer.GetNextLexem();
                    }
                    lexem = lexer.GetNextLexem();
                    if(lexem.LexemType == LexType.CloseFigureBracket)
                    {
                        lexem = lexer.GetNextLexem();
                    }
                }
            }
            return StatementList;
        }

        private bool IsBreakpoint(int row, int colStart, int colLast)
        {
            if (listOfBreakpoints != null)
            {
                foreach (var position in listOfBreakpoints)
                {
                    if (position.row == row && position.column >= colStart && position.column <= colLast)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private Statement EatStatement(Lexem lexem)
        {
            Statement statement = new Statement();

            if (lexem.LexemType == LexType.If)
            {
                statement = EatIf(lexem);
                statement.Row = lexem.Row;
                statement.ColumnStartPos = lexem.Col.StartPos;
                statement.ColumnLastPos = lexem.Col.LastPos;
                statement.IsBreakpoint = IsBreakpoint(statement.Row, statement.ColumnStartPos, statement.ColumnLastPos);

                return statement;
            }
            if (lexem.LexemType == LexType.Variable && lexer.Lookahead().LexemType == LexType.Assignment)
            {
                statement = EatAssignment(lexem);
                statement.Row = lexem.Row;
                statement.ColumnStartPos = lexem.Col.StartPos;
                if ((lexem = lexer.GetNextLexem()).LexemType != LexType.Semicolon)
                {
                    if (lexem.Value != null)
                        throw new ParserException("Expected ';'", lexem.Row, lexem.Col.StartPos, lexem.Col.LastPos);
                    else
                        throw new ParserException("Expected ';'", statement.Row, lastPos, statement.ColumnLastPos);
                }
                statement.ColumnLastPos = lexem.Col.LastPos;
                statement.IsBreakpoint = IsBreakpoint(statement.Row, statement.ColumnStartPos, statement.ColumnLastPos);

                return statement;
            }
            if (lexem.LexemType == LexType.While)
            {
                statement = EatWhile(lexem);
                statement.Row = lexem.Row;
                statement.ColumnStartPos = lexem.Col.StartPos;
                statement.ColumnLastPos = lexem.Col.LastPos;
                statement.IsBreakpoint = IsBreakpoint(statement.Row, statement.ColumnStartPos, statement.ColumnLastPos);

                return statement;
            }
            if (lexem.LexemType == LexType.For)
            {
                statement = EatFor(lexem);
                statement.Row = lexem.Row;
                statement.ColumnStartPos = lexem.Col.StartPos;
                statement.ColumnLastPos = lexem.Col.LastPos;
                statement.IsBreakpoint = IsBreakpoint(statement.Row, statement.ColumnStartPos, statement.ColumnLastPos);

                return statement;
            }
            if (lexem.LexemType == LexType.Print)
            {
                statement = EatPrint(lexem);
                statement.Row = lexem.Row;
                statement.ColumnStartPos = lexem.Col.StartPos;
                statement.IsBreakpoint = IsBreakpoint(statement.Row, statement.ColumnStartPos, statement.ColumnLastPos);
                if ((lexem = lexer.GetNextLexem()).LexemType != LexType.Semicolon)
                {
                    throw new ParserException("Expected ';'", lexem.Row, lexem.Col.StartPos, lexem.Col.LastPos);
                }
                statement.ColumnLastPos = lexem.Col.LastPos;

                return statement;
            }
            if (lexem.LexemType == LexType.Goto)
            {
                statement = EatGoto(lexem);
                statement.Row = lexem.Row;
                statement.ColumnStartPos = lexem.Col.StartPos;
                statement.IsBreakpoint = IsBreakpoint(statement.Row, statement.ColumnStartPos, statement.ColumnLastPos);
                if ((lexem = lexer.GetNextLexem()).LexemType != LexType.Semicolon)
                {
                    throw new ParserException("Expected ';'", lexem.Row, lexem.Col.StartPos, lexem.Col.LastPos);
                }
                statement.ColumnLastPos = lexem.Col.LastPos;

                return statement;
            }
            else
            {
                throw new ParserException("Undefined statement", lexem.Row, lexem.Col.StartPos, lexem.Col.LastPos);
            }
        }

        private Statement EatGoto(Lexem lexem)
        {
            Goto statementGoto = new Goto();

            statementGoto.Identificator = new Variable();
            statementGoto.Identificator.Value = EatIdentificator(lexer.GetNextLexem()).Value;
            if (!nextStatementAfterLabel.ContainsKey(statementGoto.Identificator.Value))
            {
                nextStatementAfterLabel.Add(statementGoto.Identificator.Value, null);
            }

            return statementGoto;
        }

        private Variable EatIdentificator(Lexem lexem)
        {
            Variable var = new Variable();
            var.Value = lexem.Value;

            return var;
        }

        private Statement EatLabel(Lexem lexem)
        {
            Lexem lex;
            Label label = new Label();
            label.Identificator = new Variable();
            label.Identificator.Value = EatIdentificator(lexem).Value;
            if ((lex = lexer.GetNextLexem()).LexemType != LexType.Colon)
            {
                throw new ParserException("Expected ':'", lex.Row, lex.Col.StartPos, lex.Col.LastPos);
            }

            label.NextStatement = EatStatement(lexer.GetNextLexem());
            if (!nextStatementAfterLabel.ContainsKey(label.Identificator.Value))
            {
                nextStatementAfterLabel.Add(label.Identificator.Value, label.NextStatement);
            }
            else
            {
                nextStatementAfterLabel[label.Identificator.Value] = label.NextStatement;
            }
            return label.NextStatement;
        }

        private For EatFor(Lexem lexem)
        {
            Lexem lex;
            For statementFor = new For();

            if ((lex = lexer.GetNextLexem()).LexemType != LexType.OpenBracket)
            {
                throw new ParserException("Expected '('", lex.Row, lex.Col.StartPos, lex.Col.LastPos);
            }
            statementFor.Initialization = EatAssignment(lexer.GetNextLexem());
            if ((lex = lexer.GetNextLexem()).LexemType != LexType.Semicolon)
            {
                throw new ParserException("Expected ';'", lex.Row, lex.Col.StartPos, lex.Col.LastPos);
            }
            statementFor.LogicCondition = EatLogicExpression(lexer.GetNextLexem());
            if ((lex = lexer.GetNextLexem()).LexemType != LexType.Semicolon)
            {
                throw new ParserException("Expected ';'", lex.Row, lex.Col.StartPos, lex.Col.LastPos);
            }
            statementFor.ExprForAction = EatAssignment(lexer.GetNextLexem());
            if ((lex = lexer.GetNextLexem()).LexemType != LexType.CloseBracket)
            {
                throw new ParserException("Expected ')'", lex.Row, lex.Col.StartPos, lex.Col.LastPos);
            }
            if ((lex = lexer.GetNextLexem()).LexemType != LexType.OpenFigureBracket)
            {
                throw new ParserException("Expected '{'", lex.Row, lex.Col.StartPos, lex.Col.LastPos);
            }
            statementFor.Body = new StatementList();
            statementFor.Body.List = new LinkedList<Statement>();
            while (lexer.Lookahead().LexemType != LexType.CloseFigureBracket)
            {
                Lexem tempLexem = lexer.GetNextLexem();
                if (tempLexem.LexemType == LexType.If ||
                    tempLexem.LexemType == LexType.For ||
                    tempLexem.LexemType == LexType.Print ||
                    tempLexem.LexemType == LexType.Goto ||
                    tempLexem.LexemType == LexType.While ||
                    (tempLexem.LexemType == LexType.Variable && lexer.Lookahead().LexemType == LexType.Assignment))
                {
                    statementFor.Body.List.AddLast(EatStatement(tempLexem));
                }
                else if (tempLexem.LexemType == LexType.Variable && lexer.Lookahead().LexemType == LexType.Colon)
                {
                    statementFor.Body.List.AddLast(EatLabel(tempLexem));
                }
                else throw new ParserException("Undefined statement", tempLexem.Row, tempLexem.Col.StartPos, tempLexem.Col.LastPos);
            }
            Statement endStatement = new Statement();
            endStatement.EndForBody = true;
            statementFor.Body.List.AddLast(endStatement);

            lexer.GetNextLexem();
            return statementFor;
        }

        private Print EatPrint(Lexem lexem)
        {
            Lexem lex;
            Print print = new Print();

            if ((lex = lexer.GetNextLexem()).LexemType != LexType.OpenBracket)
            {
                throw new ParserException("Expected '('", lexem.Row, lexem.Col.StartPos, lexem.Col.LastPos);
            }
            Lexem tempLexem;
            if ((tempLexem = lexer.GetNextLexem()).LexemType == LexType.String)
            {
                print.TextToPrint = tempLexem.Value;
            }
            else
            {
                print.ExprToPrint = EatExpression(tempLexem);
            }
            if ((lex = lexer.GetNextLexem()).LexemType != LexType.CloseBracket)
            {
                throw new ParserException("Expected ')'", lexem.Row, lexem.Col.StartPos, lexem.Col.LastPos);
            }
            return print;
        }

        private If EatIf(Lexem lexem)
        {
            Lexem lex;
            If statementIf = new If();

            if ((lex = lexer.GetNextLexem()).LexemType != LexType.OpenBracket)
            {
                throw new ParserException("Expected '('", lex.Row, lex.Col.StartPos, lex.Col.LastPos);
            }
            statementIf.LogicCondition = EatLogicExpression(lexer.GetNextLexem());
            if ((lex = lexer.GetNextLexem()).LexemType != LexType.CloseBracket)
            {
                throw new ParserException("Expected ')'", lex.Row, lex.Col.StartPos, lex.Col.LastPos);
            }
            if ((lex = lexer.GetNextLexem()).LexemType != LexType.OpenFigureBracket)
            {
                throw new ParserException("Expected '{'", lex.Row, lex.Col.StartPos, lex.Col.LastPos);
            }
            statementIf.ThenBody = new StatementList();
            statementIf.ThenBody.List = new LinkedList<Statement>();
            while (lexer.Lookahead().LexemType != LexType.CloseFigureBracket)
            {
                Lexem tempLexem = lexer.GetNextLexem();
                if (tempLexem.LexemType == LexType.If ||
                    tempLexem.LexemType == LexType.For ||
                    tempLexem.LexemType == LexType.Print ||
                    tempLexem.LexemType == LexType.Goto ||
                    tempLexem.LexemType == LexType.While ||
                    (tempLexem.LexemType == LexType.Variable && lexer.Lookahead().LexemType == LexType.Assignment))
                {
                    statementIf.ThenBody.List.AddLast(EatStatement(tempLexem));
                }
                else if (tempLexem.LexemType == LexType.Variable && lexer.Lookahead().LexemType == LexType.Colon)
                {
                    statementIf.ThenBody.List.AddLast(EatLabel(tempLexem));
                }
                else throw new ParserException("Undefined statement", tempLexem.Row, tempLexem.Col.StartPos, tempLexem.Col.LastPos);
            }
            Statement endStatement = new Statement();
            endStatement.EndThenBody = true;
            statementIf.ThenBody.List.AddLast(endStatement);

            lexer.GetNextLexem();//пропускаем }
            if (lexer.Lookahead().LexemType == LexType.Else)
            {
                lexer.GetNextLexem();
                if ((lex = lexer.GetNextLexem()).LexemType != LexType.OpenFigureBracket)
                {
                    throw new ParserException("Expected '{'",  lex.Row, lex.Col.StartPos, lex.Col.LastPos);
                }
                statementIf.ElseBody = new StatementList();
                statementIf.ElseBody.List = new LinkedList<Statement>();
                while (lexer.Lookahead().LexemType != LexType.CloseFigureBracket)
                {
                    Lexem tempLexem = lexer.GetNextLexem();
                    if (tempLexem.LexemType == LexType.If ||
                        tempLexem.LexemType == LexType.While ||
                        tempLexem.LexemType == LexType.For ||
                        tempLexem.LexemType == LexType.Goto ||
                        tempLexem.LexemType == LexType.Print ||
                        (tempLexem.LexemType == LexType.Variable && lexer.Lookahead().LexemType == LexType.Assignment))
                    {
                        statementIf.ElseBody.List.AddLast(EatStatement(tempLexem));
                    }
                    else if (tempLexem.LexemType == LexType.Variable && lexer.Lookahead().LexemType == LexType.Colon)
                    {
                        statementIf.ElseBody.List.AddLast(EatLabel(tempLexem));
                    }
                    else throw new ParserException("Undefined statement", tempLexem.Row, tempLexem.Col.StartPos, tempLexem.Col.LastPos);
                }
                Statement endStatement2 = new Statement();
                endStatement2.EndElseBody = true;
                statementIf.ElseBody.List.AddLast(endStatement2);

                lexer.GetNextLexem();//пропускаем }
                return statementIf;
            }
            else
            {
                statementIf.ElseBody = null;
                return statementIf;
            }
        }

        private While EatWhile(Lexem lexem)
        {
            While statementWhile = new While();
            Lexem lex;

            if ((lex = lexer.GetNextLexem()).LexemType != LexType.OpenBracket)
            {
                throw new ParserException("Expected '('", lex.Row, lex.Col.StartPos, lex.Col.LastPos);
            }
            statementWhile.LogicCondition = EatLogicExpression(lexer.GetNextLexem());
            if ((lex = lexer.GetNextLexem()).LexemType != LexType.CloseBracket)
            {
                throw new ParserException("Expected ')'", lex.Row, lex.Col.StartPos, lex.Col.LastPos);
            }
            if ((lex = lexer.GetNextLexem()).LexemType != LexType.OpenFigureBracket)
            {
                throw new ParserException("Expected '{'", lex.Row, lex.Col.StartPos, lex.Col.LastPos);
            }
            statementWhile.Body = new StatementList();
            statementWhile.Body.List = new LinkedList<Statement>();
            while (lexer.Lookahead().LexemType != LexType.CloseFigureBracket)
            {
                Lexem tempLexem = lexer.GetNextLexem();
                if (tempLexem.LexemType == LexType.If ||
                    tempLexem.LexemType == LexType.For ||
                    tempLexem.LexemType == LexType.Print ||
                    tempLexem.LexemType == LexType.Goto ||
                    tempLexem.LexemType == LexType.While ||
                    (tempLexem.LexemType == LexType.Variable && lexer.Lookahead().LexemType == LexType.Assignment))
                {
                    statementWhile.Body.List.AddLast(EatStatement(tempLexem));
                }
                else if (tempLexem.LexemType == LexType.Variable && lexer.Lookahead().LexemType == LexType.Colon)
                {
                    statementWhile.Body.List.AddLast(EatLabel(tempLexem));
                }
                else throw new ParserException("Undefined statement", tempLexem.Row, tempLexem.Col.StartPos, tempLexem.Col.LastPos);
            }
            Statement endStatement = new Statement();
            endStatement.EndWhile = true;
            statementWhile.Body.List.AddLast(endStatement);

            lexer.GetNextLexem();//пропускаем }
            return statementWhile;
        }

        private LogicExpression EatLogicExpression(Lexem lexem)
        {
            LogicExpression logicExpression = new LogicExpression();

            logicExpression.LeftExpr = EatExpression(lexem);
            Lexem lex = lexer.GetNextLexem();
            switch (lex.LexemType)
            {
                case LexType.More:
                    {
                        logicExpression.LogicOpearation = LogicExpression.LogicOperation.More;
                        break;
                    }
                case LexType.MoreEqual:
                    {
                        logicExpression.LogicOpearation = LogicExpression.LogicOperation.MoreEqual;
                        break;
                    }
                case LexType.Equal:
                    {
                        logicExpression.LogicOpearation = LogicExpression.LogicOperation.Equal;
                        break;
                    }
                case LexType.Less:
                    {
                        logicExpression.LogicOpearation = LogicExpression.LogicOperation.Less;
                        break;
                    }
                case LexType.LessEqual:
                    {
                        logicExpression.LogicOpearation = LogicExpression.LogicOperation.LessEqual;
                        break;
                    }
                case LexType.NotEqual:
                    {
                        logicExpression.LogicOpearation = LogicExpression.LogicOperation.NotEqual;
                        break;
                    }
                default:
                    {
                        throw new ParserException("Undefined logic operation", lex.Row, lex.Col.StartPos, lex.Col.LastPos);
                    }
            }
            logicExpression.RightExpr = EatExpression(lexer.GetNextLexem());
            return logicExpression;
        }

        private int lastPos;
        private Assignment EatAssignment(Lexem lexem)
        {
            Assignment assignment = new Assignment();
            Lexem lex;

            assignment.Row = lexem.Row;
            assignment.Destination = EatIdentificator(lexem);
            if ((lex = lexer.GetNextLexem()).LexemType != LexType.Assignment)
            {
                throw new ParserException("Expected '='", lex.Row, lex.Col.StartPos, lex.Col.LastPos);
            }
            lastPos = lexer.Lookahead().Col.LastPos;
            assignment.Source = EatExpression(lexer.GetNextLexem());
            
            return assignment;
        }

        private Expression EatFactor(Lexem lexem)
        {
            if (lexem.LexemType == LexType.Number)
            {
                Number factor = new Number();
                factor.Value = float.Parse(lexem.Value);
                return factor;
            }
            if (lexem.LexemType == LexType.Variable)
            {
                Variable factor = new Variable();
                factor.Value = lexem.Value;
                return factor;
            }
            if (lexem.LexemType == LexType.OpenBracket)
            {
                Lexem lex;
                Expression factor = EatExpression(lexer.GetNextLexem());
                if ((lex = lexer.GetNextLexem()).LexemType == LexType.CloseBracket)
                {
                    return factor;
                }
                else
                {
                    throw new ParserException("Expected ')'", lexem.Row, lexem.Col.StartPos, lexem.Col.LastPos);
                }
            }
            else
            {
                throw new ParserException("Expected '('", lexem.Row, lexem.Col.StartPos, lexem.Col.LastPos);
            }
        }

        private Expression EatTerm(Lexem lexem)
        {
            BinaryOperation term = new BinaryOperation();
            term.Left = EatDegree(lexem);
            while (lexer.Lookahead().LexemType == LexType.Multiply || lexer.Lookahead().LexemType == LexType.Divide)
            {
                if ((lexer.GetNextLexem()).LexemType == LexType.Multiply)
                {
                    term.Type = BinaryOperation.BinaryOperationType.Mult;
                }
                else
                {
                    term.Type = BinaryOperation.BinaryOperationType.Div;
                }
                term.Right = EatDegree(lexer.GetNextLexem());
                BinaryOperation binOp = new BinaryOperation();
                binOp.Left = term.Left;
                binOp.Type = term.Type;
                binOp.Right = term.Right;
                term.Left = binOp;
            }
            return term.Left;
        }

        private Expression EatExpression(Lexem lexem)
        {
            BinaryOperation expression = new BinaryOperation();
            expression.Left = EatTerm(lexem);
            while (lexer.Lookahead().LexemType == LexType.Plus || lexer.Lookahead().LexemType == LexType.Minus)
            {
                if (lexer.GetNextLexem().LexemType == LexType.Plus)
                {
                    expression.Type = BinaryOperation.BinaryOperationType.Add;
                }
                else
                {
                    expression.Type = BinaryOperation.BinaryOperationType.Sub;
                }
                expression.Right = EatTerm(lexer.GetNextLexem());
                BinaryOperation binOP = new BinaryOperation();
                binOP.Left = expression.Left;
                binOP.Type = expression.Type;
                binOP.Right = expression.Right;
                expression.Left = binOP;
            }
            return expression.Left;
        }

        private Expression EatDegree(Lexem lexem)
        {
            BinaryOperation degree = new BinaryOperation();
            degree.Left = EatFactor(lexem);
            if (lexer.Lookahead().LexemType == LexType.Degree)
            {
                lexer.GetNextLexem();
                degree.Type = BinaryOperation.BinaryOperationType.Deg;
                degree.Right = EatDegree(lexer.GetNextLexem());
            }
            return degree;
        }
    }
}
