using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

public class HotAirBalloon : MonoBehaviour
{
    [Header("atomicBomb_plane_wing_")]
    [SerializeField] private GameObject atomicBombPlaneWing;
    [SerializeField] private string animationNamePlaneWing = "atomicBomb_plane_wing_";
    [SerializeField] private int startFramePlaneWing;
    [SerializeField] private int endFranePlaneWing;
    [SerializeField] private float frameRatePlaneWing;
    [SerializeField] private List<EnemyAnimConfig> configsPlaneWing = new List<EnemyAnimConfig>();

    [Header("atomicBomb_plane_engine_")]
    [SerializeField] private GameObject atomicBombPlaneEngine;
    [SerializeField] private string animationNamePlaneEngine = "atomicBomb_plane_engine_";
    [SerializeField] private int startFramePlaneEngine;
    [SerializeField] private int endFranePlaneEngine;
    [SerializeField] private float frameRatePlaneEngine;
    [SerializeField] private List<EnemyAnimConfig> configsPlaneEngine = new List<EnemyAnimConfig>();

    [Header("Bomb Fat Boy")]
    [SerializeField] private Transform fatBoy;

    [Header("Point")]
    [SerializeField] private Transform pointStart;
    [SerializeField] private Transform pointEnd;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float bobAmplitude = 0.35f;
    [SerializeField] private float noiseSpeed = 0.8f;
    [SerializeField] private float tiltAngle = 5f;
    [SerializeField] private float tiltSmooth = 5f;

    [Header("Drop")]
    [SerializeField] private Transform pointTarget;
    [SerializeField] private float flightTime = 1.2f;
    [SerializeField] private float gravity = 18f;

    public float FlightTime => flightTime;

    private bool bombDropped;

    private void Start()
    {
        PlayAnimationPlaneEngine();
        PlayAnimationPlaneWing();
        StartCoroutine(FlyRoutine());
    }

    private void PlayAnimation(GameObject targetGameObject, string animationName, int startFrame,
        int endFrame, float frameRate, List<EnemyAnimConfig> configOffset, Action onComplete = null)
    {
        targetGameObject.SetActive(true);
        SpriteSheetAnimator.Instance.PlayAnimation(
            target: targetGameObject,
            animPrefix: animationName,
            startFrame: startFrame,
            endFrame: endFrame,
            eventFrame: -1,
            onEventTrigger: () => { },
            offsetConfigs: configOffset,
            frameRate: frameRate,
            onComplete: onComplete
        );
    }

    private void PlayAnimationPlaneWing()
    {
        PlayAnimation(atomicBombPlaneWing, animationNamePlaneWing, startFramePlaneWing,
            endFranePlaneWing, frameRatePlaneWing, configsPlaneWing);
    }
    private void PlayAnimationPlaneEngine()
    {
        PlayAnimation(atomicBombPlaneEngine, animationNamePlaneEngine, startFramePlaneEngine,
            endFranePlaneEngine, frameRatePlaneEngine, configsPlaneEngine);
    }
    private IEnumerator FlyRoutine()
    {
        Vector2 startPos = pointStart.position;
        Vector2 endPos = pointEnd.position;

        float z = transform.position.z;

        transform.position = new Vector3(startPos.x, startPos.y, z);

        float distance = Vector2.Distance(startPos, endPos);

        // Seed để mỗi máy bay có dao động khác nhau
        float noiseSeed = UnityEngine.Random.Range(0f, 1000f);

        float traveled = 0f;
        float previousOffset = 0f;

        while (traveled < distance)
        {
            float delta = moveSpeed * Time.deltaTime;
            traveled += delta;

            float t = Mathf.Clamp01(traveled / distance);

            // Di chuyển theo đường thẳng
            Vector2 pos = Vector2.Lerp(startPos, endPos, t);

            // Dao động mềm bằng Perlin Noise
            float noise = Mathf.PerlinNoise(noiseSeed, Time.time * noiseSpeed);

            // Chuyển từ [0,1] -> [-1,1]
            float offset = (noise - 0.5f) * 2f * bobAmplitude;

            pos.y += offset;

            transform.position = new Vector3(pos.x, pos.y, z);

            if (!bombDropped)
            {
                FatBoy bomb = fatBoy.GetComponent<FatBoy>();

                float dropDistance = moveSpeed * bomb.FlightTime;

                if (transform.position.x >= pointTarget.position.x - dropDistance)
                {
                    bombDropped = true;

                    fatBoy.SetParent(null, true);

                    bomb.Drop(pointTarget);
                }
            }

            // Nghiêng nhẹ theo chiều đang lên/xuống
            float deltaOffset = offset - previousOffset;
            float targetRotation = -deltaOffset * tiltAngle * 20f;

            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.Euler(0f, 0f, targetRotation),
                Time.deltaTime * tiltSmooth);

            previousOffset = offset;

            yield return null;
        }

        transform.position = new Vector3(endPos.x, endPos.y, z);

        // Trả lại góc ban đầu
        while (Quaternion.Angle(transform.rotation, Quaternion.identity) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.identity,
                Time.deltaTime * tiltSmooth);

            yield return null;
        }

        transform.rotation = Quaternion.identity;
    }
}
