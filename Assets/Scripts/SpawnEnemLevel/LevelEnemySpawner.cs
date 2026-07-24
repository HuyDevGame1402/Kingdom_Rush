using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Unity.Jobs;

public class LevelEnemySpawner : MonoBehaviour
{
    public static LevelEnemySpawner Instance { get; private set; }
    [Header("--- Spawn & Path Settings ---")]
    [SerializeField] private List<Transform> positionSpawns = new List<Transform>();
    // Mỗi Transform trong này chứa các con (Child) là các Waypoint của đường đó
    [SerializeField] private List<Transform> roadList = new List<Transform>();

    [Header("--- Level Data ---")]
    [SerializeField] private LevelData currentLevelData;

    [Header("--- Runtime Tracking ---")]
    private int currentWaveIndex = 0;
    private int activeEnemiesCount = 0; // Đếm số lượng quái đang còn sống trên map
    private bool isSpawningWave = false;
    [SerializeField] private int timeWaveNext = 0;
    private int timeBonus;

    // Danh sách enemy của wave hiện tại
    private List<EnemyController> currentWaveEnemies = new List<EnemyController>();
    public event Action<int> EventTimeNextWave;

    public List<Transform> enemyInGame = new List<Transform>();

    private void Awake()
    {
        Instance = this;
    }

    // Coroutine chính điều khiển toàn bộ các Wave trong Level
    private IEnumerator PlayLevelCoroutine()
    {
        for (int i = 0; i < currentLevelData.waves.Count; i++)
        {
            WaveData wave = currentLevelData.waves[i];
            currentWaveIndex = wave.waveIndex;

            Debug.Log($"<color=yellow>--- BẮT ĐẦU WAVE {currentWaveIndex} ---</color>");

            // 1. Chờ thời gian delay của Wave trước khi quái tràn ra (Giống thời gian đếm ngược trong Kingdom Rush)
            yield return new WaitForSeconds(wave.waveDelay);

            // 2. Gọi Coroutine spawn toàn bộ các nhóm quái trong Wave này
            isSpawningWave = true;
            StartCoroutine(SpawnWaveCoroutine(wave));

            // 3. Chờ cho đến khi tất cả quái trong Wave này bị tiêu diệt hoàn toàn mới qua Wave tiếp theo
            // (Hoặc bạn có thể cho bấm nút "Call Early" để bỏ qua dòng check này)
            while (isSpawningWave || activeEnemiesCount > 0)
            {
                yield return null; // Chờ frame tiếp theo rồi check lại
            }

            // 4. Cộng tiền thưởng hoàn thành Wave
            GrantWaveGold(wave.goldIncome);
            Debug.Log($"<color=green>--- HOÀN THÀNH WAVE {currentWaveIndex}! Được thưởng: {wave.goldIncome} Vàng ---</color>");
        }

        Debug.Log("<color=cyan><b>CHIẾN THẮNG LEVEL! BẠN ĐÃ VƯỢT QUA TẤT CẢ CÁC WAVE!</b></color>");
    }

    // Coroutine xử lý việc spawn từng Group trong một Wave
    private IEnumerator SpawnWaveCoroutine(WaveData wave)
    {
        // Duyệt qua từng nhóm quái cấu hình trong danh sách
        foreach (EnemyGroup group in wave.enemyGroups)
        {
            // Chờ một khoảng thời gian trước khi nhóm này xuất hiện (Group Delay)
            if (group.groupDelay > 0)
            {
                yield return new WaitForSeconds(group.groupDelay);
            }

            // Tiến hành spawn từng con quái trong nhóm
            for (int i = 0; i < group.count; i++)
            {
                SpawnEnemy(group.enemyPrefab);

                // Chờ khoảng cách giữa các con quái (Spawn Delay)
                if (group.spawnDelay > 0 && i < group.count - 1)
                {
                    yield return new WaitForSeconds(group.spawnDelay);
                }
            }
        }

        isSpawningWave = false; // Đã spawn hết quái của Wave này (nhưng quái trên map có thể vẫn còn sống)
    }

    // Hàm đảm nhận việc khởi tạo quái và setup đường đi
    //private void SpawnEnemy(GameObject enemyPrefab)
    //{
    //    if (enemyPrefab == null) return;

    //    // 1. Lấy ngẫu nhiên một điểm Spawn ban đầu
    //    Transform randomSpawnPoint = transform; // Mặc định lấy chính vị trí Spawner nếu list trống
    //    if (positionSpawns.Count > 0)
    //    {
    //        randomSpawnPoint = positionSpawns[Random.Range(0, positionSpawns.Count)];
    //    }

    //    // 2. Khởi tạo Enemy tại vị trí Spawn
    //    GameObject spawnedEnemy = Instantiate(enemyPrefab, randomSpawnPoint.position, Quaternion.identity);
    //    activeEnemiesCount++; // Tăng số lượng quái đang hoạt động

    //    // 3. Lấy ngẫu nhiên một con đường (Road) trong RoadList và phân tích các Waypoints
    //    Transform waypointsForEnemy = GetRandomRoadWaypoints();

    //    // 4. Truyền Waypoints vào EnemyController
    //    // Giả sử script trên quái của bạn tên là EnemyController
    //    EnemyController enemyCtrl = spawnedEnemy.GetComponent<EnemyController>();
    //    if (enemyCtrl != null)
    //    {
    //        // Gọi hàm setup của bạn và truyền list đường đi vào
    //        enemyCtrl.SetupWayPoints(waypointsForEnemy);

    //        // Đăng ký sự kiện khi quái chết hoặc lọt lưới để trừ activeEnemiesCount
    //        // (Bạn nên viết một callback/event trong EnemyController để khi quái hủy thì gọi hàm OnEnemyDestroyed)
    //        enemyCtrl.OnEnemyDestroyed += EnemyDiedHandler;
    //    }
    //    else
    //    {
    //        Debug.LogWarning($"Prefab {enemyPrefab.name} thiếu Component EnemyController!");
    //    }
    //}

    private EnemyController SpawnEnemy(GameObject enemyPrefab)
    {
        if (enemyPrefab == null)
            return null;

        Transform randomSpawnPoint = transform;

        if (positionSpawns.Count > 0)
            randomSpawnPoint = positionSpawns[UnityEngine.Random.Range(0, positionSpawns.Count)];

        GameObject spawnedEnemy = Instantiate(enemyPrefab,
            randomSpawnPoint.position,
            Quaternion.identity);

        activeEnemiesCount++;

        Transform waypointsForEnemy = GetRandomRoadWaypoints();

        EnemyController enemyCtrl = spawnedEnemy.GetComponent<EnemyController>();
        enemyInGame.Add(spawnedEnemy.transform);
        if (enemyCtrl != null)
        {
            enemyCtrl.SetupWayPoints(waypointsForEnemy);
            enemyCtrl.OnEnemyDestroyed += EnemyDiedHandler;
        }
        else
        {
            Debug.LogWarning($"Prefab {enemyPrefab.name} thiếu EnemyController!");
        }

        return enemyCtrl;
    }

    // Hàm bổ trợ lấy các điểm con (Child) từ một con đường ngẫu nhiên
    private Transform GetRandomRoadWaypoints()
    {
        Transform randomRoad = roadList[UnityEngine.Random.Range(0, roadList.Count)];
        return randomRoad;
    }

    // Hàm callback được gọi khi một con quái chết hoặc đi tới điểm cuối (Lọt lưới)
    private void EnemyDiedHandler()
    {
        activeEnemiesCount--;
        if (activeEnemiesCount < 0) activeEnemiesCount = 0;
    }

    private void GrantWaveGold(int amount)
    {
        // Thực hiện logic cộng tiền cho người chơi tại đây
        // Ví dụ: GoldManager.Instance.AddGold(amount);
    }

    public List<EnemyGroup> GetCurrentEnemyGroup()
    {
        return currentLevelData.waves[currentWaveIndex].enemyGroups;
    }

    public void SpawnCurrentWave()
    {
        if (currentWaveIndex >= currentLevelData.waves.Count)
        {
            Debug.Log("Đã hết Wave!");
            return;
        }

        WaveData wave = currentLevelData.waves[currentWaveIndex];
        Debug.Log($"<color=yellow>Spawn Wave {wave.waveIndex}</color>");
        if (wave.waveEvent != null)
        {
            wave.waveEvent.Execute();
        }
        currentWaveEnemies.Clear();
        if (currentWaveIndex + 1 < currentLevelData.waves.Count)
        {
            timeWaveNext = (int)currentLevelData.waves[currentWaveIndex + 1].waveDelay;
            timeBonus = currentLevelData.waves[currentWaveIndex].timeBonus;
        }    
        else
            timeWaveNext = 0;
        // Bắt đầu gọi Coroutine để spawn quái từ từ
        StartCoroutine(SpawnWaveRoutine(wave));
        currentWaveIndex++;

        if (WaveManager.Instance != null) WaveManager.Instance.AddWave();
    }
    private IEnumerator SpawnWaveRoutine(WaveData wave)
    {
        foreach (EnemyGroup group in wave.enemyGroups)
        {
            // 1. Chờ một khoảng thời gian TRƯỚC KHI nhóm này bắt đầu spawn (groupDelay)
            if (group.groupDelay > 0)
            {
                yield return new WaitForSeconds(group.groupDelay);
            }

            // 2. Spawn từng con quái trong nhóm
            for (int i = 0; i < group.count; i++)
            {
                EnemyController enemy = SpawnEnemy(group.enemyPrefab);
                if (enemy != null)
                {
                    currentWaveEnemies.Add(enemy);
                }

                // 3. Chờ một khoảng thời gian giữa mỗi con quái (spawnDelay)
                // Không cần chờ ở con quái cuối cùng của group nếu bạn muốn tối ưu
                if (i < group.count - 1 && group.spawnDelay > 0)
                {
                    yield return new WaitForSeconds(group.spawnDelay);
                }
            }
        }
        yield return new WaitForSeconds(timeBonus);
        LastEnemySpawner_EventLastEnemyInGame();
    }


    private void LastEnemySpawner_EventLastEnemyInGame()
    {
        EventTimeNextWave?.Invoke(timeWaveNext);
    }

    public void RemoveEnemy(Transform enemy)
    {
        if (enemyInGame.Contains(enemy))
        {
            enemyInGame.Remove(enemy);  
        }
    }

    public void CalculatorGem(Vector3 positionSpawn)
    {
        if(UnityEngine.Random.Range(0f,1f) < currentLevelData.dropGemPercent / 100f)
        {
            int gem = currentLevelData.gems[UnityEngine.Random.Range(0, currentLevelData.gems.Count)];
            
            if(GemManager.Instance != null)
            {
                GemManager.Instance.AddGem(gem);
            }

            if (GemVisualManager.Instance != null)
            {
                GemVisualManager.Instance.SpawnGemVisual(gem, positionSpawn);
            }
        }
    }
}