using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Serko.ExpenseDataParser.Abstractions
{
    public class Result
    {
        public bool Error { get; set; } = true;
        public string ErrorDetials { get; set; } = "Unknown";
        public XDocument ExpenseData { get; set; } = null;
    }
}
