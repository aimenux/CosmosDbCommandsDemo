using System.Threading;
using System.Threading.Tasks;
using App.Options;
using Lib;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace App.Commands
{
    public class TimeToLiveCommand : CommandLineApplication, ICommand
    {
        private readonly ICosmosDbClient<CosmosDbDocument> _cosmosDbClient;
        private readonly TimeToLiveCommandOption _ttlOption;
        private readonly FromCommandOption _fromOption;
        private readonly ToCommandOption _toOption;
        private readonly ILogger _logger;

        public TimeToLiveCommand(
            ICosmosDbClient<CosmosDbDocument> cosmosDbClient,
            TimeToLiveCommandOption ttlOption,
            FromCommandOption fromOption,
            ToCommandOption toOption,
            ILogger logger)
        {
            Name = "Set";
            Description = "Set TTL between [from] and [to] dates to [ttl] seconds";
            Options.AddRange(new CommandOption[] {ttlOption, fromOption, toOption});
            OnExecuteAsync(ExecuteAsync);

            _ttlOption = ttlOption;
            _fromOption = fromOption;
            _toOption = toOption;
            _logger = logger;
            _cosmosDbClient = cosmosDbClient;
        }

        private int TimeToLive => int.Parse(_ttlOption.GetValue());
        private string Query => $"SELECT * from c where c.LastModificationDate >= \"{_fromOption.GetValue()}\" and c.LastModificationDate <= \"{_toOption.GetValue()}\"";

        public async Task<int> ExecuteAsync(CancellationToken cancellationToken)
        {
            var documents = await _cosmosDbClient.GetAsync(Query);

            foreach (var document in documents)
            {
                await _cosmosDbClient.SetTimeToLiveAsync(document.Id, document.PartitionKey, TimeToLive);
                _logger.LogInformation($"TTL '{TimeToLive}' seconds was set for document [{document}]");
            }
            
            return 0;
        }
    }
}
