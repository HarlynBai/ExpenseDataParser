using Serko.GSTRate.Abstractions;
using System;

namespace Serko.GSTRate
{
    public class GSTRateProvider : IGSTRateProvider
    {
        public decimal getGSTRate()
        {
            return 0.05M;
        }
    }
}
