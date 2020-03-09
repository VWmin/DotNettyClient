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
    class CustomProtocolDecoder : ByteToMessageDecoder {
        public Encoding Charset { get; set; }

        public CustomProtocolDecoder(Encoding charset) {
            Charset = charset;
        }
        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output) {
            byte[] bytes = new byte[input.ReadableBytes];
            input.ReadBytes(bytes);
            string jsonStr = Charset.GetString(bytes);

            CustomProtocol msg = JsonConvert.DeserializeObject<CustomProtocol>(jsonStr);
            output.Add(msg);
        }
    }
}
