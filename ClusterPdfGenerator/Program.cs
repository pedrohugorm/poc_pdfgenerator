using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Routing;
using SharedKernel.Message;

namespace ClusterPdfGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var system = ActorSystem.Create("pdfsystem");

            var api = system.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "pdfgen");
            var printer = system.ActorOf<PrinterActor>();

            Console.WriteLine("Press any key to START...");
            Console.ReadKey();

            //TODO Obtain pending list here and send messages to create PDF
            system.Scheduler.Advanced.ScheduleRepeatedly(TimeSpan.FromSeconds(2), TimeSpan.FromMilliseconds(200), () =>
            //system.Scheduler.Advanced.ScheduleOnce(TimeSpan.FromSeconds(2), () =>
            {
                if (!api.Ask<Routees>(new GetRoutees()).Result.Members.Any())
                {
                    Console.WriteLine("NO ROUTEES");
                    throw new Exception();
                }

                Console.WriteLine("SENDING...");
                api.Tell(new GeneratePdf(), printer);
            });

            system.WhenTerminated.Wait();
        }
    }

    class PrinterActor : ReceiveActor
    {
        public PrinterActor()
        {
            ReceiveAny(o =>
            {
                Console.WriteLine($"GOT A RESPONSE: {o}");

                if (o is SimpleMessage)
                    Console.WriteLine(((SimpleMessage) o).Message);
            });
        }
    }
}
