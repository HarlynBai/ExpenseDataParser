using System;
using System.Collections.Generic;
using System.Text;

namespace Serko.GSTRate.Abstractions
{
    public interface IGSTRateProvider
    {
        decimal getGSTRate();
    }
}
