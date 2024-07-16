using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using System;
using System.Threading.Tasks;

namespace GrpcClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new CarService.CarServiceClient(channel);

            var reply = await client.GetAllCarsAsync(new Empty());
            foreach (var car in reply.Cars)
            {
                Console.WriteLine($"ID: {car.Id}, Date: {car.DateOfCreation}, VinNo: {car.Vinnumber}, reason: {car.Reason}");
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
