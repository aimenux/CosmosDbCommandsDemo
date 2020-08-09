using System;

namespace App.Options
{
    public class FromCommandOption : DateTimeCommandOption
    {
        public FromCommandOption() : base("--from", "From [date]")
        {
        }

        public override string GetValue()
        {
            if (!IsValid())
            {
                throw new ArgumentException("Invalid --from option");
            }

            return Value();
        }
    }
}