public interface ICommand
{
    public void Execute();
    public void Undo();

    public SerializedCommand Serialize();

    // public abstract SerializedCommand Serialize();
}