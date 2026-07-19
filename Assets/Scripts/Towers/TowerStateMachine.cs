using System.Collections.Generic;
using UnityEngine;

public class TowerStateMachine : MonoBehaviour
{
    [SerializeField] private Transform currentTarget;
    public Transform CurrentTarget { get => currentTarget; set => currentTarget = value; }

    // Các trạng thái sẵn có của Trụ
    public TowerIdleState IdleState = new TowerIdleState();
    public TowerAttackState AttackState = new TowerAttackState();
    public TowerCooldownState CooldownState = new TowerCooldownState();

    // Biến lưu trạng thái đang chạy hiện tại
    private ITowerState currentState;

    [SerializeField] private List<Vector2> dirs = new List<Vector2>();

    [Header("Data Tower")]
    [SerializeField] private CastleData _archerDataTower;

    public bool isCreateTower = false;
    public ArcherTowerSetupAnimation archerTowerAnimation;

    [SerializeField] private TowerSoundBasic towerSoundBasic;

    private void Start()
    {
        // Khi game bắt đầu, đưa Trụ vào trạng thái đứng yên đợi quái
        TransitionToState(IdleState);
        //towerSoundBasic.PlayAudioTowerReady();
    }

    private void Update()
    {
        // Ủy quyền chạy Update cho Trạng thái hiện tại xử lý
        if (currentState != null)
        {
            currentState.UpdateState(this);
        }
    }

    /// <summary>
    /// Hàm cốt lõi dùng để đổi trạng thái của Trụ
    /// </summary>
    public void TransitionToState(ITowerState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }

        currentState = newState;
        currentState.EnterState(this);
    }

    public void SetCastleData(CastleData castleData)
    {
        _archerDataTower = castleData;
    }
    public CastleData GetDataTower()
    {
        return _archerDataTower;
    }

    public void SetTargetEnemy(Transform enemy)
    {
        currentTarget = enemy;
    }
    public ITowerState GetCurrentState()
    {
        return currentState;
    }
}