using System;

namespace App.Options
{
    public class ToCommandOption : DateTimeCommandOption
    {
        public ToCommandOption() : base("--to", "To [date]")
        {
        }

        public override string GetValue()
        {
            if (!IsValid())
            {
                throw new ArgumentException("Invalid --to option");
            }

            return Value();
        }
    }
}