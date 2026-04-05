namespace ExpressionEvaluator.Core;

public class Evaluator
{
    public static double Evaluate(string infix)
    {
        var postfix = InfixToPostfix(infix);
        return EvaluatePostfix(postfix);
    }

    private static List<string> InfixToPostfix(string infix)
    {
        var postFix = new List<string>();
        var stack = new Stack<char>();
        int i = 0;
        while (i < infix.Length)
        {
            char item = infix[i];

            if (item == ' ')
            {
                i++;
                continue;
            }

            if (char.IsDigit(item) || item == '.' || item == ',')
            {
                string number = "";
                while (i < infix.Length && (char.IsDigit(infix[i]) || infix[i] == '.' || infix[i] == ','))
                {
                    number += infix[i] == ',' ? '.' : infix[i];
                    i++;
                }
                postFix.Add(number);
                continue;
            }

            if (IsOperator(item))
            {
                if (stack.Count == 0)
                {
                    stack.Push(item);
                }
                else
                {
                    if (item == ')')
                    {
                        do
                        {
                            postFix.Add(stack.Pop().ToString());
                        } while (stack.Peek() != '(');
                        stack.Pop();
                    }
                    else
                    {
                        if (PriorityInfix(item) > PriorityStack(stack.Peek()))
                        {
                            stack.Push(item);
                        }
                        else
                        {
                            postFix.Add(stack.Pop().ToString());
                            stack.Push(item);
                        }
                    }
                }
            }

            i++;
        }

        while (stack.Count > 0)
        {
            postFix.Add(stack.Pop().ToString());
        }
        return postFix;
    }

    private static int PriorityStack(char item) => item switch
    {
        '^' => 3,
        '*' => 2,
        '/' => 2,
        '+' => 1,
        '-' => 1,
        '(' => 0,
        _ => throw new Exception("Sintax error."),
    };

    private static int PriorityInfix(char item) => item switch
    {
        '^' => 4,
        '*' => 2,
        '/' => 2,
        '+' => 1,
        '-' => 1,
        '(' => 5,
        _ => throw new Exception("Sintax error."),
    };

    private static double EvaluatePostfix(List<string> postfix)
    {
        var stack = new Stack<double>();
        foreach (string token in postfix)
        {
            if (token.Length == 1 && IsOperator(token[0]))
            {
                var b = stack.Pop();
                var a = stack.Pop();
                stack.Push(token[0] switch
                {
                    '+' => a + b,
                    '-' => a - b,
                    '*' => a * b,
                    '/' => a / b,
                    '^' => Math.Pow(a, b),
                    _ => throw new Exception("Sintax error."),
                });
            }
            else
            {
                stack.Push(double.Parse(token, System.Globalization.CultureInfo.InvariantCulture));
            }
        }
        return stack.Pop();
    }

    private static bool IsOperator(char item) => "+-*/^()".Contains(item);
}