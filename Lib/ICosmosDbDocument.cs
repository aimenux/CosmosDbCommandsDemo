using System;
using Newtonsoft.Json;

namespace Lib
{
    public interface ICosmosDbDocument
    {
        [JsonProperty("id")]
        string Id { get; set; }

        string PartitionKey { get; set; }

        [JsonProperty(PropertyName = "ttl")]
        int? TimeToLive { get; set; }

        DateTime LastModificationDate { get; set; }
    }
}