using System;
using System.Xml.Linq;

namespace Serko.ExpenseDataParser
{
    public class ExpenseDataParser
    {
        public XDocument Parse(string textBlock)
        {
            XDocument doc = new XDocument();
            doc.Add(new XElement("SerKo.ExpenseData"));
            return doc;
        }
    }
}
