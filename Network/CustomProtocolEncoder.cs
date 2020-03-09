using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyDemo.network {
    class CustomProtocolEncoder : MessageToByteEncoder<CustomProtocol> {
        public Encoding Charset { get; set; }

        public CustomProtocolEncoder(Encoding encoding) {
            Charset = encoding;
        }
        protected override void Encode(IChannelHandlerContext context, CustomProtocol message, IByteBuffer output) {
            string jsonStr = JsonConvert.SerializeObject(message);
            output.WriteBytes(Charset.GetBytes(jsonStr));
        }
    }
}
