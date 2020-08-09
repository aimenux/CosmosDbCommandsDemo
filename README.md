![.NET Core](https://github.com/aimenux/CosmosDbCommandsDemo/workflows/.NET%20Core/badge.svg)
# CosmosDbCommandsDemo
Playing crud commands on cosmos db

In this demo, i m playing with `CommandLineUtils` library & `TimeToLive` feature in CosmosDb. I m providing commands/options to :
> - Insert documents in cosmos db ( `.\App.exe Initialize`)
> - Remove documents in cosmos db (`.\App.exe Set --ttl 30 --from 2020-08-08 --to 2020-08-10`)

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