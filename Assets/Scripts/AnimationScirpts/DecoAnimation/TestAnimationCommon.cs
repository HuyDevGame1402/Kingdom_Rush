using UnityEngine;

public class TestAnimationCommon : MonoBehaviour
{
    [Header("Cấu Hình ID Nhóm Decor")]
    public string commonID = "gui_common";

    [Header("Cấu Hình Animation")]
    public GameObject spriteGameObject;
    public float frameRate = 0.1f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayAimationByFrame(0, 18);
        }    

        if (Input.GetKeyDown(KeyCode.Alpha2))
            PlayAimationByFrame(0, 37);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            PlayAimationByFrame(0, 53);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            Play("effect_powerbuy");

        if (Input.GetKeyDown(KeyCode.Alpha5))
            Play("error_feedback");

        if (Input.GetKeyDown(KeyCode.Alpha6))
            Play("heart");

        if (Input.GetKeyDown(KeyCode.Alpha7))
            Play("hud_bonusFx");

        if (Input.GetKeyDown(KeyCode.Alpha8))
            Play("main_icons");

        if (Input.GetKeyDown(KeyCode.Alpha9))
            Play("nextwave_coin");

        if (Input.GetKeyDown(KeyCode.Alpha0))
            Play("rally_feedback");

        if (Input.GetKeyDown(KeyCode.Q))
            Play("power_portrait_glow");

        if (Input.GetKeyDown(KeyCode.W))
            Play("power_portrait_fireball");

        if (Input.GetKeyDown(KeyCode.E))
            Play("power_portrait_reinforcement");

        if (Input.GetKeyDown(KeyCode.R))
            Play("power_portrait_backpack");

        if (Input.GetKeyDown(KeyCode.T))
            Play("power_portrait_doors");

        if (Input.GetKeyDown(KeyCode.Y))
            Play("special_icons");

        if (Input.GetKeyDown(KeyCode.U))
            Play("sub_icons");

        if (Input.GetKeyDown(KeyCode.I))
            Play("tooltip_icons");

        if (Input.GetKeyDown(KeyCode.O))
            Play("victoryStars");

        if (Input.GetKeyDown(KeyCode.P))
            Play("waveReward");
    }

    void Play(string animationName)
    {
        Debug.Log("Play Animation : " + animationName);

        DecorSpriteAnimator.Instance.PlayAnimation(
            spriteGameObject,
            commonID,
            animationName,
            frameRate);
    }
    
    private void PlayAimationByFrame(int startFrame, int endFrame)
    {
        //DecorSpriteAnimator.Instance.PlayAnimation(
        //    spriteGameObject,
        //    commonID,
        //    "victoryStars",
        //    startFrame,
        //    endFrame
        //);
    }
}