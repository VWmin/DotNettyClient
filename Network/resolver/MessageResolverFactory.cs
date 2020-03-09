using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyDemo.network.resolver {
    class MessageResolverFactory {
        private static MessageResolverFactory factory = new MessageResolverFactory();
        private static List<Resolver> resolvers = new List<Resolver>();

        private MessageResolverFactory() { }

        public static MessageResolverFactory GetFactory() { return factory; }

        public Resolver GetResolver(CustomProtocol msg) {
            foreach(Resolver resolver in resolvers) {
                if (resolver.Support(msg)) {
                    return resolver;
                }
            }

            throw (new ArgumentException($"cannot find resolver, message type: {msg.Type}"));
        }

        public void RegisterResolver(Resolver resolver) {
            resolvers.Add(resolver);
        }
      
    }
}
