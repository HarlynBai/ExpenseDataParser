using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Serko.ExpenseDataParser.Abstractions;
using Serko.ExpenseDataParser.Common;

namespace Serko.ExpenseDataParser
{
    public class CostCentreNodeDecorator : ResultDecorator
    {
        public CostCentreNodeDecorator(IResultDecorator resultDecorator) : base(resultDecorator)
        {
        }

        public override void Process(ref Result result)
        {
            if (!result.ExpenseData.XElementExists("Cost_centre"))
            {
                result.ExpenseData.Root.Add(new XElement("Cost_centre", "UNKNOWN"));
            }
            base.Process(ref result);
        }
    }
}
