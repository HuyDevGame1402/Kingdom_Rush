public interface ITowerState
{
    void EnterState(TowerStateMachine tower); // Chạy 1 lần duy nhất khi bước vào trạng thái
    void UpdateState(TowerStateMachine tower); // Chạy liên tục mỗi Frame (Thay cho Update gốc)
    void ExitState(TowerStateMachine tower);  // Chạy 1 lần duy nhất khi thoát khỏi trạng thái
}