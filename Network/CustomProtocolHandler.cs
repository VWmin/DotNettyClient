using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using NettyDemo.log;
using NettyDemo.network.resolver;
using NettyDemo.Network.resolver;
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
        private static CustomProtocol login = CustomProtocol.Login();
        public override bool IsSharable => true;

        private ChatMessageResolver chatResolver;
        private HeartbeatMessageResolver heartbeatResolver;
        private LoginMessageResolver loginResolver;

        public CustomProtocolHandler(MainWindowViewModel model) {
            chatResolver = new ChatMessageResolver(model);
            heartbeatResolver = new HeartbeatMessageResolver();
            loginResolver = new LoginMessageResolver(model);
        }

        /// <summary>
        /// 收到消息，根据消息类型选择解析
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="msg"></param>
        protected override void ChannelRead0(IChannelHandlerContext ctx, CustomProtocol msg) {
            Resolver resolver = resolverFactory.GetResolver(msg);
            resolver.Resolve(ctx, msg);
        }

        /// <summary>
        /// 心跳事件
        /// </summary>
        /// <param name="context"></param>
        /// <param name="evt"></param>
        public override void UserEventTriggered(IChannelHandlerContext context, object evt) {
            if (evt is IdleStateEvent) {
                IdleStateEvent idleStateEvent = evt as IdleStateEvent;

                if (idleStateEvent.State == IdleState.WriterIdle) {
                    //Log.Info("正在发送心跳包");
                    //向服务端发送消息
                    
                    context.Channel.WriteAndFlushAsync(heartbeat);
                    //context.Channel.WriteAndFlushAsync(CustomProtocol.Msg(heartbeat.Id, "嘤嘤嘤"));
                }
            }
        }

        /// <summary>
        /// 注册消息类型解析
        /// </summary>
        /// <param name="context"></param>
        public override void ChannelRegistered(IChannelHandlerContext context) {
            resolverFactory.RegisterResolver(heartbeatResolver);
            resolverFactory.RegisterResolver(chatResolver);
            resolverFactory.RegisterResolver(loginResolver);
            base.ChannelRegistered(context);
        }

        /// <summary>
        /// 连接建立成功
        /// </summary>
        /// <param name="context"></param>
        public override void ChannelActive(IChannelHandlerContext context) {
            base.ChannelActive(context);
            context.Channel.WriteAndFlushAsync(login);
        }

        /// <summary>
        /// 连接断开
        /// </summary>
        /// <param name="context"></param>
        public override void ChannelInactive(IChannelHandlerContext context) {
            //todo: 如何判断主动断开和异常重连
            base.ChannelInactive(context);
        }

        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception) {
            //异常时断开连接
            context.CloseAsync();
            //base.ExceptionCaught(context, exception);
        }

    }
}
