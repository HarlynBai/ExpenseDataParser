using Microsoft.Extensions.DependencyInjection;
using System;
using Serko;

namespace Serko.ExpenseData.DI.Config
{
    public static class DIConfigExtension
    {
        public static void AddExpenseDataParser(this IServiceCollection services)
        {
            services.AddTransient<ExpenseDataParser.Abstractions.IExpenseDataParser, ExpenseDataParser.ExpenseDataParser>();
            services.AddTransient<ExpenseDataParser.Abstractions.IResultDecorator>(x => new ExpenseDataParser.TotalNodeDecorator(new ExpenseDataParser.CostCentreNodeDecorator(null), new GSTRate.GSTRateProvider()));
        }
    }
}
