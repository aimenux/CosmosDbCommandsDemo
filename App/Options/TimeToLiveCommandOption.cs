using System;
using McMaster.Extensions.CommandLineUtils;

namespace App.Options
{
    public class TimeToLiveCommandOption : CommandOption, ICommandOption
    {
        public TimeToLiveCommandOption() : base("--ttl", CommandOptionType.SingleValue)
        {
            Description = "Set TTL in seconds for cosmos db documents";
        }

        public bool IsValid()
        {
            return HasValue() && int.TryParse(Value(), out _);
        }

        public string GetValue()
        {
            if (!IsValid())
            {
                throw new ArgumentException("Invalid --ttl option");
            }

            return Value();
        }
    }
}