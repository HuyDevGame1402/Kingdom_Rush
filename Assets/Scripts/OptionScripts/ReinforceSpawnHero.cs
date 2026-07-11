using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct PoolConfig
{
    public ReinforceType type;
    public GameObject prefab;
    public int initialSize;
}

public class ReinforceSpawnHero : MonoBehaviour
{
    public static ReinforceSpawnHero Instance { get; private set; }
    [SerializeField] private int levelReinforce;
    [SerializeField] private ReinforceSO reinforceSO;

    [SerializeField] private List<PoolConfig> poolConfigs = new List<PoolConfig>();

    // Lưu trữ pool tách biệt theo từng loại để check và lấy cho nhanh
    private Dictionary<ReinforceType, Queue<GameObject>> _poolDictionary;
    private Dictionary<ReinforceType, GameObject> _prefabDictionary;

    private void Awake()
    {
        Instance = this;
        _poolDictionary = new Dictionary<ReinforceType, Queue<GameObject>>();
        _prefabDictionary = new Dictionary<ReinforceType, GameObject>();
    }

    private void Start()
    {
        InitPoolConfigs();
        InitializePool();
    }

    private void InitPoolConfigs()
    {
        for(int i = 0; i <  reinforceSO.reinforceHeros.Count; i++)
        {
            if(levelReinforce == reinforceSO.reinforceHeros[i].level)
            {
                poolConfigs = reinforceSO.reinforceHeros[i].heros;
                return;
            }
        }
    }
    private void InitializePool()
    {
        foreach (var config in poolConfigs)
        {
            _prefabDictionary[config.type] = config.prefab;
            _poolDictionary[config.type] = new Queue<GameObject>();

            for (int i = 0; i < config.initialSize; i++)
            {
                GameObject obj = Instantiate(config.prefab, transform);
                obj.SetActive(false);
                _poolDictionary[config.type].Enqueue(obj);
            }
        }
    }
    public GameObject GetFromPool(ReinforceType type, Vector3 position)
    {
        if (!_poolDictionary.ContainsKey(type))
        {
            Debug.LogError($"Pool không tồn tại loại: {type}");
            return null;
        }
        GameObject obj = null;
        if (_poolDictionary[type].Count > 0)
        {
            obj = _poolDictionary[type].Dequeue();
            obj.GetComponent<HealthHero>().ResetHealth();
        }
        else
        {
            obj = Instantiate(_prefabDictionary[type], transform);
        }

        obj.transform.position = position;
        obj.SetActive(true);
        
        if(obj.TryGetComponent(out HealthHero healthHero))
        {
            healthHero.StartLife();
        }

        return obj;
    }

    // Hàm trả Object về Pool khi chết/biến mất
    public void ReturnToPool(ReinforceType type, GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        _poolDictionary[type].Enqueue(obj);
    }

}
