using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyDemo.network.resolver {
    interface Resolver {
        bool Support(CustomProtocol msg);
        void Resolve(IChannelHandlerContext ctx, CustomProtocol msg);
    }
}
