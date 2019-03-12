using Serko.ExpenseDataParser.Abstractions;
using Serko.ExpenseDataParser.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Serko.GSTRate.Abstractions;

namespace Serko.ExpenseDataParser
{
    public class TotalNodeDecorator : ResultDecorator
    {
        private IGSTRateProvider _GSTRateProvider;
        public TotalNodeDecorator(IResultDecorator resultDecorator, IGSTRateProvider GSTRateProvider) : base(resultDecorator)
        {
            _GSTRateProvider = GSTRateProvider;
        }

        public override void Process(ref Result result)
        {
            bool found = result.ExpenseData.FindFirstXElementByName("Total", out XElement xElement);
            if (!found)
            {
                result.Error = true;
                result.ErrorDetials = "Missing <Total> from the intput text.";
                return;
            }
            else
            {
                if (decimal.TryParse(xElement.Value, out decimal total))
                {
                    decimal GSTRate = _GSTRateProvider.getGSTRate();
                    xElement.Add(new XElement("GST", total * GSTRate));
                    xElement.Add(new XElement("TotalExcludingGST", total * (1 - GSTRate)));
                }

                base.Process(ref result);
            }
        }
    }
}
