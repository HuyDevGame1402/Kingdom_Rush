using UnityEngine;
using UnityEngine.UI;
using System;

public class OnClickButtonShopGame : MonoBehaviour
{
    [SerializeField] private ItemAbstract itemGame;
    public Action<Transform,ItemAbstract> OnClickItem;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        OnClickItem?.Invoke(transform,itemGame);
    }
}
