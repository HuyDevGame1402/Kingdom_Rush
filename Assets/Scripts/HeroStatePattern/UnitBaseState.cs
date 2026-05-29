public abstract class UnitBaseState
{
    protected BaseUnitStateMachine unit;

    public UnitBaseState(BaseUnitStateMachine unitStateMachine)
    {
        this.unit = unitStateMachine;
    }

    public abstract void Enter();  // Chạy 1 lần duy nhất khi vào State
    public abstract void Update(); // Chạy liên tục mỗi Frame (Thay cho Update của MonoBehaviour)
    public abstract void Exit();   // Chạy 1 lần duy nhất trước khi chuyển sang State khác
}