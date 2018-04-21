using HelloWorld.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using System;
using System.Threading.Tasks;

namespace ActorClient
{
    class Program
    {
        static void Main(string[] args)
        {
            IHelloWorld actor = ActorProxy.Create<IHelloWorld>(ActorId.CreateRandom(), new Uri("fabric:/MyApplication/HelloWorldActorService"));
            Task<string> retval = actor.GetHelloWorldAsync();
            Console.Write(retval.Result);
            //Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
