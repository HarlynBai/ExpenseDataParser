using Serko.ExpenseDataParser.Abstractions;
using Serko.ExpenseDataParser.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Serko.ExpenseDataParser
{
    public class TotalNodeDecorator : ResultDecorator
    {
        public TotalNodeDecorator(IResultDecorator resultDecorator) : base(resultDecorator)
        {
        }

        public override void Process(ref Result result)
        {
            if (!result.ExpenseData.XElementExists("Total"))
            {
                result.Error = true;
                result.ErrorDetials = $"Missing <Total> from the {result.ExpenseData.ToString()}";
                return;
            }
            else
            {
                base.Process(ref result);
            }
        }
    }
}
