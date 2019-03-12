using Serko.ExpenseDataParser.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Serko.ExpenseDataParser
{
    public class ResultDecorator : IResultDecorator
    {
        private IResultDecorator _resultDecorator;
        public ResultDecorator(IResultDecorator resultDecorator)
        {
            _resultDecorator = resultDecorator;
        }
        public virtual void Process(ref Result result)
        {
            if (_resultDecorator != null)
            {
                _resultDecorator.Process(ref result);
            }
        }
    }
}
