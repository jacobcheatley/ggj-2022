public interface ICommand
{
    public void Execute();
    public void Undo();

    public CommandData ToData();

    // public abstract SerializedCommand Serialize();
}