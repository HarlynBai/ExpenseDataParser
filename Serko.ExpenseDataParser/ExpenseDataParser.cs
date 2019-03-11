using System;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Serko.ExpenseDataParser
{
    public class ExpenseDataParser
    {
        public XDocument Parse(string textBlock)
        {
            XDocument doc = new XDocument();
            doc.Add(new XElement("SerKo.ExpenseData"));

            // The fllowing Regex will try to match the most outside XML node. 
            // As a result it will try to return all the possible XML islands.
            // '<(.+?)>'  non greedy search for opening XML tag
            // '</\1 >'   matching the closing XML tag
            // '[\d\D]*'  matching anything including \n\r
            var matches = Regex.Matches(textBlock, @"<(.+?)>[\d\D]*</\1>");
            foreach (Match match in matches)
            {
                doc.Root.Add(XDocument.Parse(match.ToString()).Root);
            }
            return doc;
        }
    }
}
