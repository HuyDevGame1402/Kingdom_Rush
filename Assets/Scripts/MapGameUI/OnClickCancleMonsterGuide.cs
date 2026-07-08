using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class OnClickCancleMonsterGuide : MonoBehaviour, IPointerClickHandler
{
    public event Action OnClickCancleMonsterGuideUI;
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickCancleMonsterGuideUI?.Invoke();
    }
}
