using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NettyDemo.ViewModels;

namespace NettyDemo.network {
    public delegate void ReceiveMessageEvent(object message);


    public class ProtoBufSocket {
        public event ReceiveMessageEvent ReceiveMessage;

        private AutoResetEvent ChannelInitilizedEvent;
        private Bootstrap SocketBootstrap;
        private MultithreadEventLoopGroup WorkGroup;
        private volatile bool Connected = false;
        private IChannel Channel;
        private CustomProtocolHandler handler;

        public ProtoBufSocket(MainWindowViewModel model) {
            handler = new CustomProtocolHandler(model);
            ChannelInitilizedEvent = new AutoResetEvent(false);
            SocketBootstrap = new Bootstrap();
            WorkGroup =  new MultithreadEventLoopGroup();
            InitBootstrap();
        }

        private void InitBootstrap() {
           
            SocketBootstrap.Group(WorkGroup)
                .Channel<TcpSocketChannel>()
                .Option(ChannelOption.TcpNodelay, true)
                .Option(ChannelOption.SoKeepalive, true)
                .Option(ChannelOption.SoTimeout, 5)
                .Option(ChannelOption.ConnectTimeout, TimeSpan.FromSeconds(5))
                .Handler(new ActionChannelInitializer<ISocketChannel>(channel => {
                    IChannelPipeline pipeline = channel.Pipeline;
                    pipeline.AddLast("idleStateHandler", new IdleStateHandler(0, 5, 0));
                    pipeline.AddLast("lineBasedFrameDecoder", new LineBasedFrameDecoder(2048));
                    pipeline.AddLast("customEncoder", new CustomProtocolEncoder(Encoding.UTF8));
                    pipeline.AddLast("customDecoder", new CustomProtocolDecoder(Encoding.UTF8));
                    pipeline.AddLast("tcpHandler", handler);
                }));
        }


        public void Connect() {
            Connected = false;
            do {
                try {
                    Channel = AsyncHelpers.RunSync<IChannel>(() => SocketBootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8081)));
                    ChannelInitilizedEvent.Set();
                    Connected = true;
                } catch (Exception ce) {
                    Console.WriteLine(ce.StackTrace);
                    Console.WriteLine("Reconnect server after 5 seconds...");
                    Thread.Sleep(5000);
                }
            } while (!Connected);
        }
        public void Disconnect() {
            WorkGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
        }

        public void SendMessage(object message) {
            if (!Connected)
                //Connected = ChannelInitilizedEvent.WaitOne();
                Connect();
            Channel.WriteAndFlushAsync(message);
        }

        ~ProtoBufSocket() {
            Disconnect();
        }

    }
}
