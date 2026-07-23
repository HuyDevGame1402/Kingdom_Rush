using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

// Định nghĩa các loại chế độ chơi
public enum LevelType
{
    Campaign,
    Heroic,
    Iron
}

// Định nghĩa cấu hình cho từng loại quái trong một Wave
[Serializable]
public struct EnemyGroup
{
    [Header("Enemy Settings")]
    public string enemyName;          // Tên hiển thị của quái (ví dụ: Goblin, Orc)
    public GameObject enemyPrefab;    // Prefab của quái để instantiate vào game

    [Header("Spawn Logic")]
    public int count;                 // Số lượng quái loại này trong group
    public float spawnDelay;          // Khoảng thời gian cách nhau giữa mỗi CON quái trong group này
    public float groupDelay;          // Thời gian chờ TRƯỚC KHI nhóm này bắt đầu spawn (để tạo khoảng cách với nhóm trước trong cùng 1 wave)
}

// Định nghĩa cấu trúc của một Wave (Đợt quái)
[Serializable]
public struct WaveData
{
    public int waveIndex;                   // Số thứ tự Wave (Wave 1, Wave 2...)
    public List<EnemyGroup> enemyGroups;    // Danh sách các nhóm quái sẽ xuất hiện trong Wave này
    public int goldIncome;                  // Tiền thưởng khi hoàn thành hoặc sống sót qua Wave này
    public float waveDelay;
    public int timeBonus; // thời gian đợi sau khi spawn hết quái r ms chuyển sang time wave tiếp

    public EventWaveScript waveEvent;
}

// Tạo Menu trong Unity Editor để dễ dàng bấm chuột phải tạo File dữ liệu
[CreateAssetMenu(fileName = "NewLevelData", menuName = "KingdomRush/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("--- Level Info ---")]
    public int levelId;
    public string levelName;
    public LevelType levelType;
    [TextArea(3, 5)] public string description;

    [Header("--- Set-Up (Initial Stats) ---")]
    public int startingGold = 265;
    public int live;
    public int wave;
    public int strategicPointsCount = 8;    // Số lượng vị trí xây tháp (X8)
    public int sheepCount = 8;             // Số lượng cừu trang trí trên map (X8)
    public int startingLives = 20;         // Số mạng ban đầu (thường là 20)

    [Header("--- Tower Restrictions ---")]
    [Tooltip("Danh sách các loại trụ được phép xây dựng trong map này")]
    public List<CastleData> allowedTowers; // TowerData là ScriptableObject quản lý thông tin từng trụ
    public List<BaseTowerSO> allowedBaseTowers;

    [Header("--- Wave Composition ---")]
    public List<WaveData> waves;

    [Header("Gems")]
    public int dropGemPercent;
    public int gemDefaut;
    public List<int> gems;

    public Sprite spriteIcon;
}