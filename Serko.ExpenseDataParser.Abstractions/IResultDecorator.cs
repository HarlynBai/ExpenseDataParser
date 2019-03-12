using System;
using System.Collections.Generic;
using System.Text;

namespace Serko.ExpenseDataParser.Abstractions
{
    public interface IResultDecorator
    {
        void Process(ref Result result);
    }
}
