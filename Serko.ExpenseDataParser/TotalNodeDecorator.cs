using Serko.ExpenseDataParser.Abstractions;
using Serko.ExpenseDataParser.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Serko.ExpenseDataParser
{
    public class TotalNodeDecorator : ResultDecorator
    {
        private readonly decimal _GSTRate = 0.05M;
        public TotalNodeDecorator(IResultDecorator resultDecorator) : base(resultDecorator)
        {
        }

        public override void Process(ref Result result)
        {
            XElement xElement;
            bool found = result.ExpenseData.FindFirstXElementByName("Total", out xElement);
            if (!found)
            {
                result.Error = true;
                result.ErrorDetials = $"Missing <Total> from the {result.ExpenseData.ToString()}";
                return;
            }
            else
            {
                decimal total;
                if(decimal.TryParse(xElement.Value, out total))
                {
                    xElement.Add(new XElement("GST", total * _GSTRate));
                    xElement.Add(new XElement("TotalExcludingGST", total * (1-_GSTRate)));
                }

                base.Process(ref result);
            }
        }
    }
}
