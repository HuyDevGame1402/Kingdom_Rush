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

    public void UpdateSpriteNormal()
    {
        image.sprite = normalSprite;
    }

    public void UpdateSpriteSelected()
    {
       image.sprite = selectedSprite;
    }
}
