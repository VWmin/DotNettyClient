using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using NettyDemo.log;
using NettyDemo.network.resolver;
using NettyDemo.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyDemo.network {
    class CustomProtocolHandler : SimpleChannelInboundHandler<CustomProtocol> {
        private static MessageResolverFactory resolverFactory = MessageResolverFactory.GetFactory();

        private static CustomProtocol heartbeat = CustomProtocol.Heartbeat();
        public override bool IsSharable => true;

        private ChatMessageResolver chatResolver;
        private HeartbeatMessageResolver heartbeatResolver;

        public CustomProtocolHandler(MainWindowViewModel model) {
            chatResolver = new ChatMessageResolver(model);
            heartbeatResolver = new HeartbeatMessageResolver();
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, CustomProtocol msg) {
            Resolver resolver = resolverFactory.GetResolver(msg);
            resolver.Resolve(ctx, msg);
        }

        public override void UserEventTriggered(IChannelHandlerContext context, object evt) {
            if (evt is IdleStateEvent) {
                IdleStateEvent idleStateEvent = evt as IdleStateEvent;

                if (idleStateEvent.State == IdleState.WriterIdle) {
                    //Log.Info("正在发送心跳包");
                    //向服务端发送消息
                    
                    //context.Channel.WriteAndFlushAsync(heartbeat);
                    //context.Channel.WriteAndFlushAsync(CustomProtocol.Msg(heartbeat.Id, "嘤嘤嘤"));
                }
            }
        }

        public override void ChannelRegistered(IChannelHandlerContext context) {
            resolverFactory.RegisterResolver(heartbeatResolver);
            resolverFactory.RegisterResolver(chatResolver);
            base.ChannelRegistered(context);
        }


    }
}
