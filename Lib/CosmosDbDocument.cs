using System;
using Newtonsoft.Json;

namespace Lib
{
    public class CosmosDbDocument : ICosmosDbDocument
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        public string PartitionKey { get; set; }

        [JsonProperty(PropertyName = "ttl")]
        public int? TimeToLive { get; set; }

        public DateTime LastModificationDate { get; set; }

        public override string ToString()
        {
            return $"Id = '{Id}' PartitionKey = '{PartitionKey}'";
        }
    }
}