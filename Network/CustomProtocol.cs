using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyDemo.network {
    class CustomProtocol {
        private static string UUID = Guid.NewGuid().ToString();

        private string id;

        public string Id {
            get { return id; }
            set { id = value; }
        }

        private string to;

        public string To {
            get { return to; }
            set { to = value; }
        }

        private MessageType type;

        public MessageType Type {
            get { return type; }
            set { type = value; }
        }

        private string content;

        public string Content {
            get { return content; }
            set { content = value; }
        }

        public CustomProtocol() { }


        private CustomProtocol(MessageType type, String content) {
            this.Id = UUID;
            this.Type = type;
            this.Content = content;
        }

        private CustomProtocol(MessageType type, string to, String content) {
            this.Id = UUID;
            this.To = to;
            this.Type = type;
            this.Content = content;
        }

        public static CustomProtocol Msg(string to, String content) {
            return new CustomProtocol(MessageType.Chat, to, content);
        }

        public static CustomProtocol Heartbeat(string content) {
            return new CustomProtocol(MessageType.Heartbeat, content);
        }

        public static CustomProtocol Heartbeat() {
            return Heartbeat("ping");
        }

        public static CustomProtocol Login() {
            return new CustomProtocol(MessageType.Login, "login");
        }
    }
}
