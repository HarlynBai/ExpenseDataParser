using System;
using System.Collections.Generic;
using System.Text;

namespace Serko.ExpenseDataParser.Abstractions
{
    public interface IExpenseDataParser
    {
        Result Parse(string textBlock);
    }
}
