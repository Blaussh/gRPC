using Calculator;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Calculator.CalculatorService;

namespace server
{
    public class CalculatorServiceImpl : CalculatorServiceBase
    {
        public override Task<CalResponse> Sum(CalRequest request, ServerCallContext context)
        {
            int result = request.A + request.B;
            string resultMessage = String.Format("{0} + {1} = {2}", request.A, request.B, result);
            return Task.FromResult(new CalResponse() { Result = resultMessage });
        }

        public override Task<CalResponse> Mul(CalRequest request, ServerCallContext context)
        {
            int result = request.A * request.B;
            string resultMessage = String.Format("{0} * {1} = {2}", request.A, request.B, result);
            return Task.FromResult(new CalResponse() { Result = resultMessage });
        }

        public override async Task Prime(PrimeRequest request, IServerStreamWriter<PrimeResponse> responseStream, ServerCallContext context)
        {
            Console.WriteLine("The Server Recieved the request: ");
            Console.WriteLine(request.ToString());
            int result;
            int k = 2;
            int N = request.Number;
            while (N > 1)
            {
                if (N % k == 0)
                {
                    result = k;
                    await responseStream.WriteAsync(new PrimeResponse() { Result = result });
                    N /= k;
                }
                else
                {
                    k += 1;
                }
            }
        }
    }
}
