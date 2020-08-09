namespace App.Options
{
    public interface ICommandOption
    {
        bool IsValid();
        string GetValue();
    }
}
