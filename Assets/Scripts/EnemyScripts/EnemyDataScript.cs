using UnityEngine;

public class EnemyDataScript : MonoBehaviour
{
    public Transform centerEnemy;
    public float pivotEnemy;
    public int health;
    public int damage;
    public float armor;

    public float finalArmor;

    private EnemyController enemyController;

    private void Awake()
    {
        if(enemyController == null) enemyController = GetComponent<EnemyController>();
    }

    private void Start()
    {
        InitData();
    }

    private void InitData()
    {
        armor = enemyController.unitData.armor;
        finalArmor = armor;
    }
}
