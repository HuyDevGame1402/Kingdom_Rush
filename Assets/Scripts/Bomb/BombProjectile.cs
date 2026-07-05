using UnityEngine;
using System;
using System.Collections;

public class BombProjectile : BaseProjectile
{
    [Header("Components")]
    [SerializeField] private GameObject bombSpriteObj;
    [SerializeField] private GameObject explosionObj;

    [Header("Explosion Animation")]
    [SerializeField] private string explosionAnimName;
    [SerializeField] private int explosionStartFrame;
    [SerializeField] private int explosionEndFrame;

    [Header("Flight")]
    [SerializeField] private float flightDuration = 1.2f;
    [SerializeField] private float arcHeight = 3f;

    [Header("Prediction")]
    [SerializeField] private bool predictTargetMovement = true;

    [Header("Rotation")]
    [SerializeField] private float spinSpeed = 360f;
    [SerializeField] private bool spinClockwise = true;

    private Vector3 fixedTargetPos;

    private Vector3 velocity;
    private float gravity;

    private float elapsedTime;
    private float currentSpin;

    private bool hasLanded;

    [SerializeField, Range(0f, 3f)]
    private float predictionFactor = 0.75f;

    public event Action OnHitEvent;



    public void LaunchWithArc(Transform enemy, float bombSpeed, float extraArcHeight
        , int damage = 1)
    {
        hasLanded = false;
        elapsedTime = 0f;
        currentSpin = 0f;

        float finalArcHeight = arcHeight + extraArcHeight;

        if (enemy != null)
        {
            if (predictTargetMovement &&
                enemy.parent.TryGetComponent(out EnemyController enemyCtrl))
            {
                // Dự đoán theo waypoint
                fixedTargetPos = enemyCtrl.GetFuturePosition(flightDuration * predictionFactor);
            }
            else
            {
                fixedTargetPos = enemy.position;
            }
        }
        else
        {
            fixedTargetPos = transform.position;
        }

        base.Launch(enemy, bombSpeed, damage);

        //--------------------------------------------------
        // Ballistic calculation
        //--------------------------------------------------

        Vector3 delta = fixedTargetPos - startPosition;

        gravity = (8f * finalArcHeight) /
                  (flightDuration * flightDuration);

        velocity = new Vector3(
            delta.x / flightDuration,
            (delta.y +
             0.5f * gravity *
             flightDuration *
             flightDuration) / flightDuration,
            delta.z / flightDuration);

        //--------------------------------------------------

        if (bombSpriteObj != null)
        {
            bombSpriteObj.SetActive(true);
            bombSpriteObj.transform.rotation = Quaternion.identity;
        }

        if (explosionObj != null)
            explosionObj.SetActive(false);

        transform.position = startPosition;

        Debug.DrawLine(enemy.position, fixedTargetPos, Color.red, 5f);
        Debug.Log($"Current = {enemy.position}  Future = {fixedTargetPos}");
    }

    protected override void MoveLogic()
    {
        if (hasLanded)
            return;

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= flightDuration)
        {
            transform.position = fixedTargetPos;
            OnHitTarget();
            return;
        }

        //--------------------------------------------------
        // Position
        //--------------------------------------------------

        Vector3 pos;

        pos.x = startPosition.x + velocity.x * elapsedTime;

        pos.y = startPosition.y
              + velocity.y * elapsedTime
              - 0.5f * gravity * elapsedTime * elapsedTime;

        pos.z = startPosition.z + velocity.z * elapsedTime;

        transform.position = pos;

        //--------------------------------------------------
        // Rotation
        //--------------------------------------------------

        Vector2 moveVelocity = new Vector2(
            velocity.x,
            velocity.y - gravity * elapsedTime);

        float travelAngle =
            Mathf.Atan2(moveVelocity.y, moveVelocity.x) * Mathf.Rad2Deg;

        currentSpin +=
            (spinClockwise ? -1f : 1f)
            * spinSpeed
            * Time.deltaTime;

        if (bombSpriteObj != null)
        {
            bombSpriteObj.transform.rotation =
                Quaternion.Euler(
                    0f,
                    0f,
                    travelAngle + currentSpin);
        }
    }

    protected override void OnHitTarget()
    {
        hasLanded = true;
        isFlying = false;

        if (bombSpriteObj != null)
            bombSpriteObj.SetActive(false);
        OnHitEvent?.Invoke();

        if (explosionObj != null)
        {
            explosionObj.SetActive(true);

            SpriteSheetAnimator.Instance.PlayAnimation(
                target: explosionObj,
                animPrefix: explosionAnimName,
                startFrame: explosionStartFrame,
                endFrame: explosionEndFrame,
                frameRate: -1,
                eventFrame: -1,
                onEventTrigger: null,
                onComplete: () =>
                {
                    explosionObj.SetActive(false);
                    //StartCoroutine(CoroutineDisableGameObject());
                });
        }
        else
        {
            HideGameObject();
        }
    }

    public void HideGameObject()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator CoroutineDisableGameObject()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}