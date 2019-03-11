using System;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using Serko.ExpenseDataParser.Abstractions;

namespace Serko.ExpenseDataParser
{
    public class ExpenseDataParser : IExpenseDataParser
    {
        private ILogger<ExpenseDataParser> _logger;

        public ExpenseDataParser(ILogger<ExpenseDataParser> logger)
        {
            _logger = logger;
        }

        public Result Parse(string textBlock)
        {
            Result result = new Result();
            if (ClosingTagIsMissing(textBlock, ref result))
            {
                return result;
            }

            XDocument doc = new XDocument();
            doc.Add(new XElement("SerKo.ExpenseData"));

            // The fllowing Regex will try to match the most outside XML node. 
            // As a result it will try to return all the possible XML islands.
            // '<(.+?)>'  non greedy search for opening XML tag
            // '</\1 >'   matching the closing XML tag
            // '[\d\D]*'  matching anything including \n\r
            var XMLBlocks = Regex.Matches(textBlock, @"<(.+?)>[\d\D]*</\1>");
            foreach (Match XMLblock in XMLBlocks)
            {
                try
                {
                    doc.Root.Add(XDocument.Parse(XMLblock.ToString()).Root);
                }
                catch(Exception)
                {
                    result.Error = true;
                    result.ErrorDetials = $"Invailid XML block: {XMLblock.ToString()}";
                    _logger.LogWarning(result.ErrorDetials);
                    return result;
                }
            }
            result.Error = false;
            result.ExpenseData = doc;
            return result;
        }
        private bool ClosingTagIsMissing(string textBlock, ref Result result)
        {
            // '<(B[^/].+?)>' Find all the opening tags 
            var openingTags = Regex.Matches(textBlock, @"<([^/].+?)>");
            foreach (Match tag in openingTags)
            {
                if (!textBlock.Contains($"</{tag.Groups[1].ToString()}>"))
                {
                    result.Error = true;
                    result.ErrorDetials = $"Misssing closing tag for {tag.ToString()}";
                    _logger.LogWarning(result.ErrorDetials);
                    return true;
                }
            }
            return false;
        }
    }
}
