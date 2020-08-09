using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace Lib
{
    public sealed class CosmosDbClient<TDocument> : ICosmosDbClient<TDocument>, IDisposable where TDocument : ICosmosDbDocument
    {
        private readonly CosmosClient _client;
        private readonly Container _container;

        public CosmosDbClient(
            string endpointUrl,
            string authKey,
            string databaseName,
            string containerName)
        {
            _client = new CosmosClient(endpointUrl, authKey);
            _container = _client.GetContainer(databaseName, containerName);
            var response = _container.ReadContainerAsync().GetAwaiter().GetResult();
            if (response.Resource.DefaultTimeToLive == null)
            {
                throw new Exception($"TimeToLive should be set for container '{_container.Id}'");
            }
        }

        public async Task<ICollection<TDocument>> GetAsync(string query)
        {
            var documentQuery = _container.GetItemQueryIterator<TDocument>(query);

            var documents = new List<TDocument>();
            while (documentQuery.HasMoreResults)
            {
                documents.AddRange(await documentQuery.ReadNextAsync());
            }

            return documents;
        }

        public Task UpsertAsync(params TDocument[] documents)
        {
            if (documents?.Any() != true) return Task.CompletedTask;
            var tasks = documents.Select(x => _container.UpsertItemAsync(x));
            return Task.WhenAll(tasks);
        }

        public async Task SetTimeToLiveAsync(string id, string partitionKey, int timeToLive)
        {
            var pk = new PartitionKey(partitionKey);
            var response = await _container.ReadItemAsync<TDocument>(id, pk);
            var document = response.Resource;
            if (document == null)
            {
                throw new Exception($"Unfound document with id '{id}' and partition key '{partitionKey}'");
            }

            document.TimeToLive = timeToLive;
            document.LastModificationDate = DateTime.Now;
            await _container.ReplaceItemAsync(document, id, pk);
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
