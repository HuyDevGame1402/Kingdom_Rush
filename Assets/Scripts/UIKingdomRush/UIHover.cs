using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite spriteHover;
    public Sprite spriteNormal;
    public Image image;
    public void OnPointerEnter(PointerEventData eventData)
    {
        image.sprite = spriteHover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.sprite = spriteNormal;
    }
}