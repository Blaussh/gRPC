using Calculator;
using Dummy;
using Greet;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    class Program
    {
        const string target = "127.0.0.1:50051";

        static async Task Main(string[] args)
        {
            Channel channel = new Channel(target, ChannelCredentials.Insecure);

            await channel.ConnectAsync().ContinueWith((task) =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                    Console.WriteLine("The client connected successfully");
            });

            //var client = new DummyService.DummyServiceClient(channel);
            var greetingClient = new GreetingService.GreetingServiceClient(channel);

            var greeting = new Greeting()
            {
                FirstName = "Shai",
                LastName = "Blaustien"
            };

            var request = new GreetingRequest() { Greeting = greeting };

            var response = greetingClient.Greet(request);
            Console.WriteLine(response.Result);


            var calculatorService = new CalculatorService.CalculatorServiceClient(channel);
            var sumRequest = new CalRequest { A = 15, B = 28 };
            var mulRequest = new CalRequest { A = 5, B = 3 };

            var sumResponse = calculatorService.Sum(sumRequest);
            Console.WriteLine(sumResponse.Result);

            var mulResponse = calculatorService.Mul(mulRequest);
            Console.WriteLine(mulResponse.Result);

            //var requestForMultiGreet = new GreetManyTimesRequest() { Greeting = greeting };
            //var manyGreetingsResponse = greetingClient.GreetManyTimes(requestForMultiGreet);
            //while(await manyGreetingsResponse.ResponseStream.MoveNext())
            //{
            //    Console.WriteLine(manyGreetingsResponse.ResponseStream.Current.Result);
            //    await Task.Delay(200);
            //}

            var requestForPrimeNumbers = new PrimeRequest() { Number = 135 };
            var PrimeResponse = calculatorService.Prime(requestForPrimeNumbers);

            while (await PrimeResponse.ResponseStream.MoveNext())
            {
                Console.WriteLine(PrimeResponse.ResponseStream.Current.Result);
                await Task.Delay(1500);
            }
            channel.ShutdownAsync().Wait();
            Console.ReadKey();
        }
    }
}
