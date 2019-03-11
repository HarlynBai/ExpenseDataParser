using System;
using Xunit;

namespace Serko.ExpenseDataParser.xUnitTests
{
    public class ExpenseDataParserTests
    {
        //list of test cases:
        //1. When a text block is passed in, then an object that includes all the XML content is returned.
        //1.1 When a text block is passed in, then an XDocument object is returned.
        //1.2 When a text block is passed in, 
        //    then the returned XDocument object has a root Node 'SerKo.ExpenseData'.
        //1.3 When a text block is passed in, 
        //    then the returned XDocument object should include all the XML content from the text block.
        //2. When there is a missing closing XML tag in the text block, then an error is returned.
        //3. When there is invalid XML in the text block, then an error is returned.
        //4. When ‘cost_centre’ node is missing, then a ‘cost_centre’ node is added with value ‘UNKNOWN’.
        //5. When there is no "total" node in the XML, then an error is returned.
        //6. When 'total' node found, then ‘GST' and ‘total excluding GST’ node is added into the return.
    }
}
