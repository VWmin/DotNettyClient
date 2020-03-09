using DotNetty.Transport.Channels;
using NettyDemo.log;
using NettyDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyDemo.network.resolver {
    class ChatMessageResolver : Resolver {
        private static MessageType supportMessageType = MessageType.Chat;
        public MainWindowViewModel Model { get; set; }

        public ChatMessageResolver(MainWindowViewModel model) {
            Model = model;
        }
        public void Resolve(IChannelHandlerContext ctx, CustomProtocol msg) {
            //Log.Info($"收到来自{msg.Id}的聊天 >>> {msg.Content}");
            Model.ReceiveText = msg.Content;
        }

        public bool Support(CustomProtocol msg) {
            return supportMessageType == msg.Type;
        }
    }
}
