using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using SharedKernel.Message;

namespace SharedKernel.Extension
{
    public static class SendSimpleMessageExtension
    {
        public static void SendSimpleMessage(this IActorRef actor, string message) => actor.Tell(new SimpleMessage {Message = message});
    }
}
