using DotNetty.Transport.Channels;
using NettyDemo.log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyDemo.network.resolver {
    class HeartbeatMessageResolver : Resolver {
        private static MessageType supportMessageType = MessageType.Heartbeat;
        public void Resolve(IChannelHandlerContext ctx, CustomProtocol msg) {
            Log.Info("收到服务器的心跳响应");
        }

        public bool Support(CustomProtocol msg) {
            return msg.Type == supportMessageType;
        }
    }
}
