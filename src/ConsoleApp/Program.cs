using System;
using Microsoft.Extensions.DependencyInjection;
using Application;
using Infrastructure;
using Application.Prices.Queries.PriceCalculate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {        
            var serviceProvider = new ServiceCollection()
                    .AddApplication()
                    .AddInfrastructure()
                    .BuildServiceProvider();

            var logger = serviceProvider.GetService<ILogger<Program>>();

            try
            {
                logger.LogInformation("Start Program.cs");

                var mediator = serviceProvider.GetService<IMediator>();

                var command = new CalulatePriceQuery();
                command.Products = args;
            
                var result = mediator.Send(command).Result;

                Console.WriteLine("SubTotal : " + $"{result.PriceInfo.SubTotal.ToCurrencyString()}");
                Console.WriteLine("DiscontMessage : " + $"{result.PriceInfo.DiscontMessage}");
                Console.WriteLine("Total : " + $"{result.PriceInfo.Total.ToCurrencyString()}");

                
                logger.LogInformation("End Program.cs");
            }
            catch (Exception ex)
            {
                logger.LogError("Error Program.cs : " , ex.Message);
                throw new ApplicationException("An application error occurred", ex);
            }
        }
    }
}
