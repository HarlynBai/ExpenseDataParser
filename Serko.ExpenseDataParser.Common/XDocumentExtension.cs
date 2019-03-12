using System;
using System.Linq;
using System.Xml.Linq;

namespace Serko.ExpenseDataParser.Common
{
    public static class XDocumentExtension
    {
        public static bool FindFirstXElementByName(this XDocument doc, string name, out XElement xElement)
        {
            var xElements = doc.Descendants().Where(element => element.Name.LocalName == name);
            if (0 == xElements.Count())
            {
                xElement = null;
                return false;
            }
            else
            {
                xElement = xElements.First();
                return true;
            }
        }

        public static bool XElementExists(this XDocument doc, string name)
        {
            XElement xElement;
            return doc.FindFirstXElementByName(name, out xElement);
        }
    }
}
