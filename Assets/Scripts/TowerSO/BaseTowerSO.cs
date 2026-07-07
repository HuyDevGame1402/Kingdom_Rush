using UnityEngine;

public class BaseTowerSO : ScriptableObject
{
    public int towerID;
    public string towerName;
    public string towerDescription;
    public Sprite towerIcon;
    public Sprite towerIconGray;
    public int priceTower;
    public int priceBuyTower;

    // value
    public Sprite iconAttack;
    public int minAttack;
    public int maxAttack;

    public Vector3 offsetPositionSpawnTower;

    public GameObject towerPrefab;
}
