using DotNetty.Transport.Channels;
using NettyDemo.network;
using NettyDemo.network.resolver;
using NettyDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyDemo.Network.resolver {
    class LoginMessageResolver : Resolver {
        private static MessageType supportMessageType = MessageType.Login;

        public MainWindowViewModel Model { get; set; }

        public LoginMessageResolver(MainWindowViewModel model) {
            Model = model;
        }
        public void Resolve(IChannelHandlerContext ctx, CustomProtocol msg) {
            string[] onlineIds = msg.Content.Split(';');
            ObservableCollection<Models.Online> collection = new ObservableCollection<Models.Online>();

            foreach (string one in onlineIds) {
                collection.Add(new Models.Online(one));
            }

            Model.Onlines = collection;
        }

        public bool Support(CustomProtocol msg) {
            return msg.Type == supportMessageType;
        }
    }
}
