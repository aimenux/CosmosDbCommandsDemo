using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lib;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace App.Commands
{
    public class InitializeCommand : CommandLineApplication, ICommand
    {
        private readonly ICosmosDbClient<CosmosDbDocument> _cosmosDbClient;
        private readonly ILogger _logger;

        public InitializeCommand(ICosmosDbClient<CosmosDbDocument> cosmosDbClient, ILogger logger)
        {
            Name = "Initialize";
            Description = "Inserting documents in cosmos db";
            OnExecuteAsync(ExecuteAsync);

            _cosmosDbClient = cosmosDbClient;
            _logger = logger;
        }

        public async Task<int> ExecuteAsync(CancellationToken cancellationToken)
        {
            const int neverExpire = -1;
            const int nbrDocuments = 5;

            var documents = Enumerable.Repeat(0, nbrDocuments)
                .Select(x => new CosmosDbDocument
                {
                    Id = Guid.NewGuid().ToString(),
                    PartitionKey = Guid.NewGuid().ToString(),
                    LastModificationDate = DateTime.Now,
                    TimeToLive = neverExpire
                }).ToArray();

            await _cosmosDbClient.UpsertAsync(documents);

            _logger.LogInformation($"Inserting '{nbrDocuments}' documents");

            return 0;
        }
    }
}