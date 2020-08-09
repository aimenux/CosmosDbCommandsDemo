using System;
using System.Globalization;
using McMaster.Extensions.CommandLineUtils;

namespace App.Options
{
    public abstract class DateTimeCommandOption : CommandOption, ICommandOption
    {
        private const string DefaultFormatDate = "yyyy-MM-dd";
        private const DateTimeStyles Style = DateTimeStyles.None;
        private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

        protected DateTimeCommandOption(string template, string description) : base(template, CommandOptionType.SingleValue)
        {
            Description = description;
        }

        public bool IsValid()
        {
            return DateTime.TryParseExact(Value(), DefaultFormatDate, Culture, Style, out _);
        }

        public abstract string GetValue();
    }
}