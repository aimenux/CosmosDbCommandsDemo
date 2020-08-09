![.NET Core](https://github.com/aimenux/CosmosDbCommandsDemo/workflows/.NET%20Core/badge.svg)
# CosmosDbCommandsDemo
Playing crud commands on cosmos db

In this demo, i m playing with CommandLineUtils library & TimeToLive feature in CosmosDb.
> - I m using an ioc fashion to set up commands/options using CommandLineUtils library
> - I m providing two commands :
>> - `InitializeCommand` : inserting some documents in cosmos db (.\App.exe Initialize)
>> - `TimeToLiveCommand` : setting time to live to some value for documents matching with from/to dates (.\App.exe Initialize)

```
❯ .\App.exe
Run commands on CosmosDb container

Usage: CosmosDbCommandsDemo [command] [options]

Options:
  -?|-h|--help  Show help information

Commands:
  Initialize    Inserting documents in cosmos db
  Set           Set TTL between [from] and [to] dates to [ttl] seconds

Run 'CosmosDbCommandsDemo [command] -?|-h|--help' for more information about a command.
```