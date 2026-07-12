using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite selectedSprite;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public virtual void UpdateSpriteNormal()
    {
        image.sprite = normalSprite;
    }

    public virtual void UpdateSpriteSelected()
    {
       image.sprite = selectedSprite;
    }
}
