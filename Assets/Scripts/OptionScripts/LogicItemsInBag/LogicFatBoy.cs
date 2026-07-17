using UnityEngine;
using System.Collections.Generic;

public class LogicFatBoy : MonoBehaviour, IHasLogicOption
{
    [SerializeField] private Transform pointTarget;

    [SerializeField] private GameObject hotAirBalloon;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    [SerializeField] private Transform parentHotAirBalloon;
    [SerializeField] private List<GameObject> hotAirBalloonPool = new List<GameObject>();
    private int countPool = 3;
    private GameObject hotAirCreate;
    [SerializeField] private BagOptionUI bagOptionUI;

    private void Start()
    {
        InitPool();
    }

    private void InitPool()
    {
        for(int i = 0; i < countPool; i++)
        {
            hotAirCreate = Instantiate(hotAirBalloon, startPoint.position, Quaternion.identity, parentHotAirBalloon);
            if(hotAirCreate.TryGetComponent(out HotAirBalloon hotAirBalloonScript))
            {
                hotAirBalloonScript.Constructor(pointTarget, startPoint, endPoint);
            }
            hotAirBalloonPool.Add(hotAirCreate);
            hotAirCreate.SetActive(false);
        }
        hotAirCreate = null;    
    }

    public void Execute(Vector3 pos)
    {
        if (ItemManager.Instance != null)
        {
            ItemManager.Instance.RemoveItem(ItemType.FatBoy, 1);
        }
        hotAirCreate = GetHotAirBalloon();
        hotAirCreate.SetActive(true);
        if (hotAirCreate != null && hotAirCreate.TryGetComponent(out HotAirBalloon hotAirBalloonScript))
        {
            if(hotAirBalloonScript.CheckConstructor())
            {
                hotAirBalloonScript.StartFly();
            }
            else
            {
                hotAirBalloonScript.Constructor(pointTarget, startPoint, endPoint);
                hotAirBalloonPool.Add(hotAirCreate);
            }
        }
        bagOptionUI.UpdateSpriteNormal();
    }

    private GameObject GetHotAirBalloon()
    {
        for(int i = 0; i <  hotAirBalloonPool.Count; i++)
        {
            if(hotAirBalloonPool[i].activeSelf == false) return hotAirBalloonPool[i];
        }

        return Instantiate(hotAirBalloon, startPoint.position, Quaternion.identity, parentHotAirBalloon);
    }

}
