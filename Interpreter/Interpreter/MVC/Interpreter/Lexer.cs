using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Interpreter
{
    interface ILexer
    {
        Lexem GetNextLexem();
        Lexem Lookahead();
    }

    public struct Lexem
    {
        public LexType LexemType;
        public string Value;
        public int Row;
        public Column Col;
    }

    public struct Column
    {
        public int StartPos;
        public int LastPos;
    }

    public enum LexType
    {
        Goto,
        While,
        For,
        Print,
        Number,
        String,
        Comment,
        If,
        Else,
        Plus,//'+'
        Minus,//'-'
        Semicolon,//';'
        Colon,//':'
        Multiply,//'*'
        Divide,//'/'
        Degree,//'^'
        Equal,//'=='
        NotEqual,//'!='
        Assignment,//'='
        More,//'>'
        MoreEqual,//'>='
        Less,//'<'
        LessEqual,//'<='
        OpenFigureBracket,//'{'
        CloseFigureBracket,//'}'
        OpenBracket,//'('
        CloseBracket,//')'
        Variable,
    }

    internal sealed class Lexer : ILexer
    {
        public LinkedList<Exception> ListOfErrors { get; set; }
        private LinkedList<Lexem> ListOfLexem { get; set; }
        private LinkedListNode<Lexem> lexemNode;
        private LexType keywordType;
        private string programCode;
        private int currentPosition;
        private int col;
        private int row;

        public Lexer(string programCode, IOutput outputHandler)
        {
            ListOfErrors = new LinkedList<Exception>();
            ListOfLexem = new LinkedList<Lexem>();
            this.programCode = programCode;
            col = 0;
            row = 1;
            for (currentPosition = 0; currentPosition < programCode.Length; currentPosition++)
            {
                try
                {
                    Lexem lexem = CreateLexem();
                    if (lexem.Value != null)
                    {
                        ListOfLexem.AddLast(lexem);
                    }
                }
                catch (LexerException e)
                {
                    //ListOfErrors.AddLast(e);
                    outputHandler.PrintToErrors(e);
                }
            }
            lexemNode = ListOfLexem.First;
        }

        public Lexem GetNextLexem()
        {
            Lexem lexem = Lookahead();
            if (lexemNode != null)
            {
                lexemNode = lexemNode.Next;
            }
            return lexem;
        }

        public Lexem Lookahead()
        {
            if (lexemNode != null)
            {
                return lexemNode.Value;
            }
            else
            {
                return new Lexem();
            }
        }

        private bool IsKeyword()
        {
            string kwIf = "if";
            string kwWhile = "while";
            string kwPrint = "print";
            string kwGoto = "goto";
            string kwFor = "for";
            string kwElse = "else";
            string keyword;
            if (programCode.Length > currentPosition + kwIf.Length + 1 &&
                (keyword = programCode.Substring(currentPosition, kwIf.Length)).Equals(kwIf) &&
                (!char.IsLetterOrDigit(programCode[currentPosition + keyword.Length]) &&
                programCode[currentPosition + keyword.Length] != '_'))
            {
                keywordType = LexType.If;
                currentPosition += keyword.Length - 1;
                col += keyword.Length - 1;
                return true;
            }
            if (programCode.Length > currentPosition + kwElse.Length + 1 &&
                (keyword = programCode.Substring(currentPosition, kwElse.Length)).Equals(kwElse) &&
                (!char.IsLetterOrDigit(programCode[currentPosition + keyword.Length]) &&
                programCode[currentPosition + keyword.Length] != '_'))
            {
                keywordType = LexType.Else;
                currentPosition += keyword.Length - 1;
                col += keyword.Length - 1;
                return true;
            }
            if (programCode.Length > currentPosition + kwFor.Length + 1 &&
                (keyword = programCode.Substring(currentPosition, kwFor.Length)).Equals(kwFor) &&
                (!char.IsLetterOrDigit(programCode[currentPosition + keyword.Length]) &&
                programCode[currentPosition + keyword.Length] != '_'))
            {
                keywordType = LexType.For;
                currentPosition += keyword.Length - 1;
                col += keyword.Length - 1;
                return true;
            }
            if (programCode.Length > currentPosition + kwGoto.Length + 1 &&
                (keyword = programCode.Substring(currentPosition, kwGoto.Length)).Equals(kwGoto) &&
                (!char.IsLetterOrDigit(programCode[currentPosition + keyword.Length]) &&
                programCode[currentPosition + keyword.Length] != '_'))
            {
                keywordType = LexType.Goto;
                currentPosition += keyword.Length - 1;
                col += keyword.Length - 1;
                return true;
            }
            if (programCode.Length > currentPosition + kwPrint.Length + 1 &&
                (keyword = programCode.Substring(currentPosition, kwPrint.Length)).Equals(kwPrint) &&
                (!char.IsLetterOrDigit(programCode[currentPosition + keyword.Length]) &&
                programCode[currentPosition + keyword.Length] != '_'))
            {
                keywordType = LexType.Print;
                currentPosition += keyword.Length - 1;
                col += keyword.Length - 1;
                return true;
            }
            if (programCode.Length > currentPosition + kwWhile.Length + 1 &&
                (keyword = programCode.Substring(currentPosition, kwWhile.Length)).Equals(kwWhile) &&
                (!char.IsLetterOrDigit(programCode[currentPosition + keyword.Length]) &&
                programCode[currentPosition + keyword.Length] != '_'))
            {
                keywordType = LexType.While;
                currentPosition += keyword.Length - 1;
                col += keyword.Length - 1;
                return true;
            }
            return false;
        }

        private Lexem CreateLexem()
        {
            
            Lexem lexem = new Lexem();
            char symbol;
            symbol = programCode[currentPosition];
            col++;
            lexem.Col.StartPos = col;
            lexem.Row = row;
            if (symbol == '_')
            {
                throw new LexerException("Undefined lexem", row, col);
            }
            if (char.IsDigit(symbol))
            {
                lexem.LexemType = LexType.Number;
                lexem.Value = GetLexemNumber();
            }
            else if (IsKeyword())
            {
                lexem.LexemType = keywordType;
                lexem.Value = keywordType.ToString();
            }
            else if (char.IsLetter(symbol))
            {
                lexem.LexemType = LexType.Variable;
                lexem.Value = string.Empty;
                while (currentPosition + 1 < programCode.Length && (char.IsLetterOrDigit(symbol) || symbol == '_'))
                {
                    lexem.Value += symbol;
                    currentPosition++;
                    col++;
                    symbol = programCode[currentPosition];
                }
                if (currentPosition + 1 < programCode.Length)
                {
                    currentPosition--;
                    col--;
                }
            }
            else switch (symbol)
                {
                    case ':':
                        {
                            lexem.LexemType = LexType.Colon;
                            lexem.Value = ":";
                            break;
                        }
                    case '"':
                        {
                            int length = 0;
                            lexem.LexemType = LexType.String;
                            lexem.Value = string.Empty;
                            currentPosition++;
                            col++;
                            while (currentPosition < programCode.Length && programCode[currentPosition] != '"')
                            {
                                length++;
                                if (length <= 255)
                                {
                                    lexem.Value += programCode[currentPosition].ToString();
                                }
                                else
                                {
                                    throw new LexerException("Max length of string must be less 255", row, col);
                                }
                                currentPosition++;
                                col++;
                            }
                            if (currentPosition >= programCode.Length)
                            {
                                throw new LexerException("Not found end of string", row, col);
                            }
                            break;
                        }
                    case '!':
                        {
                            if (programCode[currentPosition + 1] == '=')
                            {
                                currentPosition++;
                                col++;
                                lexem.LexemType = LexType.NotEqual;
                                lexem.Value = "!=";
                            }
                            else
                            {
                                throw new LexerException("Undefined lexem", row, col);
                            }
                            break;
                        }
                    case ')':
                        {
                            lexem.LexemType = LexType.CloseBracket;
                            lexem.Value = ")";
                            break;
                        }
                    case '(':
                        {
                            lexem.LexemType = LexType.OpenBracket;
                            lexem.Value = "(";
                            break;
                        }
                    case '}':
                        {
                            lexem.LexemType = LexType.CloseFigureBracket;
                            lexem.Value = "}";
                            break;
                        }
                    case '{':
                        {
                            lexem.LexemType = LexType.OpenFigureBracket;
                            lexem.Value = "{";
                            break;
                        }
                    case '=':
                        {
                            if (programCode[currentPosition + 1] == '=')
                            {
                                currentPosition++;
                                col++;
                                lexem.LexemType = LexType.Equal;
                                lexem.Value = "==";
                            }
                            else
                            {
                                lexem.LexemType = LexType.Assignment;
                                lexem.Value = "=";
                            }
                            break;
                        }
                    case '>':
                        {
                            if (programCode[currentPosition + 1] == '=')
                            {
                                currentPosition++;
                                col++;
                                lexem.LexemType = LexType.MoreEqual;
                                lexem.Value = ">=";
                            }
                            else
                            {
                                lexem.LexemType = LexType.More;
                                lexem.Value = ">";
                            }
                            break;
                        }
                    case '<':
                        {
                            if (programCode[currentPosition + 1] == '=')
                            {
                                currentPosition++;
                                col++;
                                lexem.LexemType = LexType.LessEqual;
                                lexem.Value = "<=";
                            }
                            else
                            {
                                lexem.LexemType = LexType.Less;
                                lexem.Value = "<";
                            }
                            break;
                        }
                    case ';':
                        {
                            lexem.LexemType = LexType.Semicolon;
                            lexem.Value = ";";
                            break;
                        }
                    case '+':
                        {
                            lexem.LexemType = LexType.Plus;
                            lexem.Value = "+";
                            break;
                        }
                    case '-':
                        {
                            lexem.LexemType = LexType.Minus;
                            lexem.Value = "-";
                            break;
                        }
                    case '*':
                        {
                            if (programCode[currentPosition + 1] == '*')
                            {
                                currentPosition++;
                                col++;
                                lexem.LexemType = LexType.Degree;
                                lexem.Value = "**";
                            }
                            else
                            {
                                lexem.LexemType = LexType.Multiply;
                                lexem.Value = "*";
                            }
                            break;
                        }
                    case '/':
                        {
                            if (programCode[currentPosition + 1] == '/')
                            {
                                lexem.LexemType = LexType.Comment;
                                while (programCode[currentPosition++] != '\n') ;
                                break;
                            }
                            else
                            {
                                lexem.LexemType = LexType.Divide;
                                lexem.Value = "/";
                                break;
                            }
                        }
                    case ' ':
                    case '\t':
                    case '\r':
                        {
                            Lexem lex = new Lexem();
                            lex.Value = null;
                            return lex;
                        }
                    case '\n':
                        {
                            row++;
                            col = 0;
                            Lexem lex = new Lexem();
                            lex.Value = null;
                            return lex;
                        }
                    default:
                        {
                            throw new LexerException(string.Format("Undefined lexem '{0}'", programCode[currentPosition]), row, col);
                        }
                }
            lexem.Col.LastPos = col;
            return lexem;
        }
        

        private enum State
        {
            Start,
            GetExp,
            GetPoint,
            GetSignInDecimalPart,
            GetNumberInDecimalPart,
            GetNumberAfterPoint,
            End,
            Error,
        }

        private string GetLexemNumber()
        {
            State state = State.Start;
            string value = string.Empty;
            char symbol = programCode[currentPosition];

            while (true)
            {
                switch (state)
                {
                    case State.Start:
                        {
                            if (char.IsDigit(symbol))
                            {
                                break;
                            }
                            if (symbol == 'e' || symbol == 'E')
                            {
                                state = State.GetExp;
                                break;
                            }
                            if (symbol == ',')
                            {
                                state = State.GetPoint;
                                break;
                            }
                            else
                            {
                                state = State.End;
                                continue;
                            }
                        }
                    case State.GetExp:
                        {
                            if (symbol == '+' || symbol == '-')
                            {
                                state = State.GetSignInDecimalPart;
                                break;
                            }
                            if (char.IsDigit(symbol))
                            {
                                state = State.GetNumberInDecimalPart;
                                break;
                            }
                            else
                            {
                                state = State.Error;
                                continue;
                            }
                        }
                    case State.GetPoint:
                        {
                            if (char.IsDigit(symbol))
                            {
                                state = State.GetNumberAfterPoint;
                                break;
                            }
                            else
                            {
                                state = State.Error;
                                continue;
                            }
                        }
                    case State.GetSignInDecimalPart:
                        {
                            if (char.IsDigit(symbol))
                            {
                                state = State.GetNumberInDecimalPart;
                                break;
                            }
                            else
                            {
                                state = State.Error;
                                continue;
                            }
                        }
                    case State.GetNumberInDecimalPart:
                        {
                            if (char.IsDigit(symbol))
                            {
                                break;
                            }
                            else
                            {
                                state = State.End;
                                continue;
                            }
                        }
                    case State.GetNumberAfterPoint:
                        {
                            if (char.IsDigit(symbol))
                            {
                                break;
                            }
                            if (symbol == 'e' || symbol == 'E')
                            {
                                state = State.GetExp;
                                break;
                            }
                            else
                            {
                                state = State.End;
                                continue;
                            }
                        }
                    case State.End:
                        {
                            currentPosition--;
                            col--;
                            return value.ToString();
                        }
                    case State.Error:
                        {
                            throw new LexerException("Bad type of number", row, col);
                        }
                }
                value += symbol.ToString();
                currentPosition++;
                col++;
                if (currentPosition < programCode.Length)
                {
                    symbol = programCode[currentPosition];
                }
                else
                {
                    symbol = ';';
                }
            }
        }
    }
}
