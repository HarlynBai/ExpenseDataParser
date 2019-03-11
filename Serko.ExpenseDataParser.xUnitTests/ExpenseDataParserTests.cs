using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Xml.Linq;
using Xunit;

namespace Serko.ExpenseDataParser.xUnitTests
{
    public class ExpenseDataParserTests
    {
        //list of test case:
        //3. When there is invalid XML in the text block, then an error is returned.
        //4. When ‘cost_centre’ node is missing, then a ‘cost_centre’ node is added with value ‘UNKNOWN’.
        //5. When there is no "total" node in the XML, then an error is returned.
        //6. When 'total' node found, then ‘GST' and ‘total excluding GST’ node is added into the return.
        [Fact]
        public void WhenATextBlockIsPastInThenAXDocumentIsReturned()
        {
            string textBlock = "could be anything";
            var mockLogger = new Mock<ILogger<ExpenseDataParser>>();
            var dataParser = new ExpenseDataParser(mockLogger.Object);
            // Action
            var ret = dataParser.Parse(textBlock);
            // Expectation
            Assert.IsType<XDocument>(ret.ExpenseData);
        }

        [Fact]
        public void WhenATextBlockIsPastInThenTheReturnedXDocumentContainsARootNodeNamedSerKoDotExpenseData()
        {
            string textBlock = "could be andything";
            var mockLogger = new Mock<ILogger<ExpenseDataParser>>();
            var dataParser = new ExpenseDataParser(mockLogger.Object);
            // Action
            var ret = dataParser.Parse(textBlock);
            // Expectation
            Assert.Equal("SerKo.ExpenseData", ret.ExpenseData.Root.Name);
        }

        [Fact]
        public void WhenATextBlockIsPastInThenTheReturnedXMLDocumentContainsAllTheXMLComponentFromTheTextBlock()
        {
            string textBlock = @"this is a test block that includes two XML components.
                                the first part:
                                <XMLComponent1>
                                    <foo>dog</foo>
                                </XMLComponent1>
                                the second part:
                                <XMLComponent2><bar>cat</bar></XMLComponent2>";

            var mockLogger = new Mock<ILogger<ExpenseDataParser>>();
            var dataParser = new ExpenseDataParser(mockLogger.Object);
            // Action
            var ret = dataParser.Parse(textBlock);
            // Expectation
            var nodeDog = ret.ExpenseData.Root.Element("XMLComponent1").Element("foo").Value;
            var nodeCat = ret.ExpenseData.Root.Element("XMLComponent2").Element("bar").Value;
            Assert.Equal("dog", nodeDog);
            Assert.Equal("cat", nodeCat);
        }

        [Fact]
        public void WhenAClosingXMLTagIsMissingThenAErrorIsReturned()
        {
            string textBlock = @"This is a test block that has a missing closing XML tag
                                <XMLtag1>
                                    <XMLTagMissing>
                                    <XMLTag2>
                                    </XMLTag2>
                                </XMLTag1>";
            var mockLogger = new Mock<ILogger<ExpenseDataParser>>();
            var dataParser = new ExpenseDataParser(mockLogger.Object);
            // Action
            var ret = dataParser.Parse(textBlock);
            // Expectation
            Assert.True(ret.Error);
        }
    }
}
