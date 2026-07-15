using System.Collections;
using UnityEngine;

public class LoigcChillWand : MonoBehaviour, IHasLogicOption
{
    private int timeIce = 15;

    [SerializeField] private Transform backgroundIceUI;
    [SerializeField] private BagOptionUI bagOptionUI;

    public void Execute(Vector3 pos)
    {
        if (ItemManager.Instance != null)
        {
            ItemManager.Instance.RemoveItem(ItemType.ChillWand, 1);
        }
        backgroundIceUI.gameObject.SetActive(true);
        backgroundIceUI.GetComponent<IceUI>().StartCoroutineDisable(timeIce);
        if(IceChillWandDecoMap.Instance != null)
        {
            IceChillWandDecoMap.Instance.ShowIce();
            IceChillWandDecoMap.Instance.StartCoroutineDisable(timeIce);
        }
        if(LevelEnemySpawner.Instance != null && LevelEnemySpawner.Instance.enemyInGame.Count > 0)
        {
            for(int i = 0; i < LevelEnemySpawner.Instance.enemyInGame.Count; i++)
            {
                if(LevelEnemySpawner.Instance.enemyInGame[i].TryGetComponent(out CharacterFreezing
                    characterFreezing))
                {
                    characterFreezing.StartFreezeStatus(timeIce);
                }
            }
        }
        bagOptionUI.UpdateSpriteNormal();
    }
}
