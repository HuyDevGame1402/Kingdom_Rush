using System.Collections;
using UnityEngine;

public class LogicSpawnMeteorite : MonoBehaviour, IHasLogicOption
{
    [SerializeField] private int meteoriteLevel = 0;
    [SerializeField] private MeteorSkillSO meteorSkillSO;
    [SerializeField] private GameObject meteoritePrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnRadius = 0.5f; // Bán kính random xung quanh điểm click chuột
    [SerializeField] private float delayBetweenMeteors = 0.2f; // Thời gian delay giữa mỗi quả rơi xuống cho đẹp mắt

    [Header("Camera Out of Bounds Settings")]
    [SerializeField] private float cameraOffsetUp = 10f;    // Độ cao đẩy lên trên so với Target để ra ngoài camera
    [SerializeField] private float cameraOffsetLeft = -5f;  // Độ lệch sang trái (hoặc phải) tạo góc rơi chéo

    private MeteorLevelData levelData;
    private int totalMeteors;

    [SerializeField] private OptionUI optionUI;

    [SerializeField] private ReduceUITime reduceUITime;
    [SerializeField] private OptionClick optionClick;

    [Header("Sound")]
    [SerializeField] private AudioClip rainOfFireStartClip;
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        if (optionUI == null)
        {
            optionUI = GetComponent<OptionUI>();
        }
    }

    public void Execute(Vector3 pos)
    {
        if (meteorSkillSO == null || meteoritePrefab == null)
        {
            Debug.LogWarning("Chưa gán MeteorSkillSO hoặc MeteoritePrefab!");
            return;
        }

        if(SoundInGameManager.Instance != null && SoundInGameManager.Instance.CheckSoundEnabled())
        {
            audioSource.PlayOneShot(rainOfFireStartClip);
        }

        // Lấy dữ liệu của level hiện tại
        levelData = meteorSkillSO.GetLevelData(meteoriteLevel);
        totalMeteors = levelData.numberOfMeteors;

        // Dùng Coroutine để spawn các quả thiên thạch cách nhau một chút, nhìn sẽ tự nhiên hơn bay cùng lúc
        StartCoroutine(SpawnMeteorSequence(pos, totalMeteors, levelData));

        optionUI.UpdateSpriteNormal();
        optionClick.SetOnClick(false);
        reduceUITime.StartCountdown((int)levelData.cooldown);
    }

    private IEnumerator SpawnMeteorSequence(Vector3 centerPos, int count, MeteorLevelData levelData)
    {
        for (int i = 0; i < count; i++)
        {
            // 1. Tính toán vị trí Target ngẫu nhiên xung quanh điểm click
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 finalTargetPos = new Vector3(centerPos.x + randomCircle.x, centerPos.y + randomCircle.y, centerPos.z);

            // --- ĐOẠN SỬA LẠI: TỰ ĐỘNG RANDOM HƯỚNG RƠI ---
            // Chọn ngẫu nhiên độ lệch trục X: 
            // - Âm (ví dụ -5): rơi chéo từ trái sang
            // - Bằng 0: rơi thẳng đứng từ trên xuống
            // - Dương (ví dụ 5): rơi chéo từ phải sang
            float randomOffsetLeft = Random.Range(-Mathf.Abs(cameraOffsetLeft), Mathf.Abs(cameraOffsetLeft));

            // Nếu bạn muốn thỉnh thoảng có quả rơi THẲNG ĐỨNG (Tỷ lệ khoảng 20%)
            if (Random.value < 0.2f)
            {
                randomOffsetLeft = 0f;
            }

            // 2. Tính toán điểm xuất phát ở trên cao ngoài màn hình dựa trên độ lệch vừa random
            Vector3 spawnPosition = finalTargetPos + new Vector3(randomOffsetLeft, cameraOffsetUp, 0f);
            // ----------------------------------------------

            // 3. Tiến hành Sinh (Spawn) thiên thạch
            GameObject meteorGo = Instantiate(meteoritePrefab, spawnPosition, Quaternion.identity);

            // 4. Lấy Component MeteoriteProjectile để truyền TargetPos và Damage vào
            if (meteorGo.TryGetComponent<MeteoriteProjectile>(out var projectile))
            {
                float damage = Random.Range(levelData.meteorDamage.x, levelData.meteorDamage.y);
                projectile.Initialize(finalTargetPos, damage);
            }

            if (delayBetweenMeteors > 0)
            {
                yield return new WaitForSeconds(delayBetweenMeteors);
            }
        }
    }
}