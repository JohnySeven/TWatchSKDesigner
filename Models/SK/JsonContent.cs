using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TWatchSKDesigner.Models.SK
{
    public class JsonContent : HttpContent
    {
        public byte[] JsonBytes { get; private set; }

        public JsonContent(string json)
        {
            JsonBytes = Encoding.UTF8.GetBytes(json);
            Headers.Add("Content-Type", "application/json");
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return stream.WriteAsync(JsonBytes, 0, JsonBytes.Length);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = JsonBytes.Length;
            return true;
        }
    }
}
