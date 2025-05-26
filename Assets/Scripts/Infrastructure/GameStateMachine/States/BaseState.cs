using System;
using System.Threading.Tasks;

public abstract class BaseState: IGameState
{
    public Action<Type> OnStateEnd { get; set; }

    public abstract void EnterState();

    public abstract void ExitState();

    protected void EndState()
    {
        OnStateEnd?.Invoke(this.GetType());
    }
}
