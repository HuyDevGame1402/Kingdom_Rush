using UnityEngine;
using System.Collections.Generic;

public class LogicSpawnReinforce : MonoBehaviour, IHasLogicOption
{

    [SerializeField] private List<ReinforceType> reinforceTypes = new List<ReinforceType>();

    private float radius = 0.5f;
    private int heroSpawnCount = 2;
    private Vector2 offset;
    [SerializeField] private OptionUI optionUI;

    private int coolDownTime = 10;

    [SerializeField] private ReduceUITime reduceUITime;
    [SerializeField] private OptionClick optionClick;

    private void Start()
    {
        if(optionUI == null)
        {
            optionUI = GetComponent<OptionUI>();
        }
    }

    public void Execute(Vector3 pos)
    {
        for (int i = 0; i <  heroSpawnCount; i++)
        {
            if(ReinforceSpawnHero.Instance != null)
            {
                offset = Random.insideUnitCircle * radius;
                ReinforceSpawnHero.Instance.GetFromPool(
                    reinforceTypes[Random.Range(0, reinforceTypes.Count)],
                    pos + new Vector3(offset.x, offset.y, 0f));
            }
        }
        optionUI.UpdateSpriteNormal();
        optionClick.SetOnClick(false);
        reduceUITime.StartCountdown(coolDownTime);
    }
}
