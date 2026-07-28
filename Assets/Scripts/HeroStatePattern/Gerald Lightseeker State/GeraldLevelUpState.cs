using UnityEngine;

public class GeraldLevelUpState : UnitBaseState
{
    private HeroGeraldController hero;
    private bool isLevelUpFinished;

    public GeraldLevelUpState(HeroGeraldController stateMachine) : base(stateMachine)
    {
        this.hero = stateMachine;
    }

    public override void Enter()
    {
        isLevelUpFinished = false;

        // Phát Animation Mừng Lên Cấp (Level Up)
        if (hero.baseUnitAnimationHandler is CharacterAnimationHandler customHandler)
        {
            customHandler.PlayLevelUpAnimation(
                hero.unitData.animations,
                hero.GeraldData.heroAnimations,
                hero.spriteObject,
                onComplete: () =>
                {
                    // Khi diễn xong Animation -> Đánh dấu hoàn thành
                    isLevelUpFinished = true;
                }
            );
        }
        else
        {
            // Dự phòng nếu không tìm thấy AnimationHandler
            isLevelUpFinished = true;
        }
    }

    public override void Update()
    {
        // Chỉ khi chạy xong Animation mới chuyển sang Trạng thái tiếp theo
        if (isLevelUpFinished)
        {
            if (hero.currentTarget != null && hero.IsTargetInAttackRange())
            {
                hero.TransitionToState(hero.AttackState);
            }
            else
            {
                hero.TransitionToState(hero.IdleState);
            }
        }
    }
    public override void FixedUpdate()
    {

    }
    public override void Exit()
    {
        isLevelUpFinished = false;
    }
}