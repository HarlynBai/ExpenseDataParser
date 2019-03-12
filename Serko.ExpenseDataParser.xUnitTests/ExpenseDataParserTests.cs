using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using Serko.ExpenseDataParser.Common;
using Serko.ExpenseDataParser.Abstractions;
using Serko.GSTRate;

namespace Serko.ExpenseDataParser.xUnitTests
{
    public class ExpenseDataParserTests
    {
        [Fact]
        public void WhenATextBlockIsPastInThenAXDocumentIsReturned()
        {
            string textBlock = "could be anything";
            var mockLogger = new Mock<ILogger<ExpenseDataParser>>();
            var mockResultDecorator = new Mock<IResultDecorator>();
            var dataParser = new ExpenseDataParser(mockLogger.Object, mockResultDecorator.Object);
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
            var mockResultDecorator = new Mock<IResultDecorator>();
            var dataParser = new ExpenseDataParser(mockLogger.Object, mockResultDecorator.Object);
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
            var mockResultDecorator = new Mock<IResultDecorator>();
            var dataParser = new ExpenseDataParser(mockLogger.Object, mockResultDecorator.Object);
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
                                <XMLTag1>
                                    <XMLTagMissing>
                                    <XMLTag2>
                                    </XMLTag2>
                                </XMLTag1>";
            var mockLogger = new Mock<ILogger<ExpenseDataParser>>();
            var mockResultDecorator = new Mock<IResultDecorator>();
            var dataParser = new ExpenseDataParser(mockLogger.Object, mockResultDecorator.Object);
            // Action
            var ret = dataParser.Parse(textBlock);
            // Expectation
            Assert.True(ret.Error);
        }

        [Fact]
        public void WhenAInvalidXMLBlockIsFoundThenReturnAnError()
        {
            string textBlock = @"This is a invalid XML block that has a invailid XML characters
                                <XMLTag1>
                                    <XMLTag2>
                                    '&>
                                    </XMLTag2>
                                </XMLTag1>";
            var mockLogger = new Mock<ILogger<ExpenseDataParser>>();
            var mockResultDecorator = new Mock<IResultDecorator>();
            var dataParser = new ExpenseDataParser(mockLogger.Object, mockResultDecorator.Object);
            // Action
            var ret = dataParser.Parse(textBlock);
            // Expectation
            Assert.True(ret.Error);
        }

        [Fact]
        public void WhenThereIsNoCost_centreNodeThenACost_centreNodeIsAddedWithValueUnknown()
        {
            string textBlock = @"This is a XML block without Cost_centre Node
                                <XMLTag1>
                                    <XMLTag2>
                                    </XMLTag2>
                                </XMLTag1>";
            var mockLogger = new Mock<ILogger<ExpenseDataParser>>();
            var dataParser = new ExpenseDataParser(mockLogger.Object, new CostCentreNodeDecorator(null));
            // Action
            var ret = dataParser.Parse(textBlock);
            // Expectation
            XElement xElement;
            ret.ExpenseData.FindFirstXElementByName("Cost_centre", out xElement);
            Assert.Equal("UNKNOWN", xElement.Value);
        }

        [Fact]
        public void WhenThereIsNoTotalNodeThenAErrorIsReturned()
        {
            string textBlock = @"This is a XML block without Total Node
                                <XMLTag1>
                                    <XMLTag2>
                                    </XMLTag2>
                                </XMLTag1>";
            var mockLogger = new Mock<ILogger<ExpenseDataParser>>();
            var dataParser = new ExpenseDataParser(mockLogger.Object, new TotalNodeDecorator(null, new GSTRateProvider()));
            // Action
            var ret = dataParser.Parse(textBlock);
            // Expectation
            Assert.True(ret.Error);
        }
        [Fact]
        public void WhenTotalNodeFoundThenAddGSTAndTotalExcludingGSTNode()
        {
            string textBlock = @"Hi Yvaine,
Please create an expense claim for the below. Relevant details are marked up as requested…
<expense><cost_centre>DEV002</cost_centre> <total>1024.01</total><payment_method>personal card</payment_method> </expense>
From: Ivan Castle Sent: Friday, 16 February 2018 10:32 AM To: Antoine Lloyd Antoine.Lloyd@example.com Subject: test
Hi Antoine,
Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our <description>development team’s project end celebration dinner</description> on <date>Tuesday 27 April 2017</date>. We expect to arrive around 7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
Regards,
Ivan";
            var mockLogger = new Mock<ILogger<ExpenseDataParser>>();
            var dataParser = new ExpenseDataParser(mockLogger.Object, new TotalNodeDecorator(new CostCentreNodeDecorator(null), new GSTRateProvider()));
            // Action
            var ret = dataParser.Parse(textBlock);
            // Expectation
            Assert.True(ret.ExpenseData.XElementExists("GST"));
            Assert.True(ret.ExpenseData.XElementExists("TotalExcludingGST"));
        }
    }
}
