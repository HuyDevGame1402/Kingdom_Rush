using System.Collections.Generic;
using UnityEngine;

public class DecoAnimationClick : MonoBehaviour
{
    public static DecoAnimationClick Instance { get; private set; }

    private Vector3 positionActive;
    [SerializeField] private float offsetYErrorFeedback = -0.3f;
    [SerializeField] private float offsetYFlag;

    [SerializeField] private GameObject errorFeedbackGameObject;
    [SerializeField] private GameObject flagGameObject;

    public int currentFrame = 0;
    public List<EnemyAnimConfig> manualPivotOffsets = new List<EnemyAnimConfig>();

    private void Awake()
    {
        Instance = this;
    }

    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //        PlayAnimationErrorFeedback(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    //}

    private void ShowSingleFrame(int frame)
    {
        DecorSpriteAnimator.Instance.ShowFrame(
            flagGameObject,
            "gui_common",
            "rally_feedback",
            frame,
            manualPivotOffsets);
    }

    public void PlayAnimationErrorFeedback(Vector3 position)
    {
        errorFeedbackGameObject.SetActive(true);
        positionActive = position;
        positionActive.y += offsetYErrorFeedback;
        positionActive.z = 0;
        errorFeedbackGameObject.transform.position = positionActive;
        DecorSpriteAnimator.Instance.PlayAnimation(
            errorFeedbackGameObject,
            "gui_common",
            "error_feedback",
        1,
        15,
            0.05f,
            () =>
            {
                errorFeedbackGameObject.SetActive(false);
            });
    }

    public void PlayAnimationFlag(Vector3 position)
    {
        flagGameObject.SetActive(true);
        positionActive = position;
        positionActive.y += offsetYFlag;
        positionActive.z = 0;
        flagGameObject.transform.position = positionActive;
        DecorSpriteAnimator.Instance.PlayAnimation(
            flagGameObject,
            "gui_common",
            "rally_feedback",
            1,                      
            0,                      
            30,                    
            0.05f,        
            () =>
            {
                flagGameObject.SetActive(false);
            },
            manualPivotOffsets
        );
    }

}
