using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeSpriteLightButton : MonoBehaviour
{
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite clickSprite;
    private Image image;
    private Coroutine coroutineChangeSprite;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void ChangeSprite()
    {
        if (coroutineChangeSprite != null)
        {
            StopCoroutine(coroutineChangeSprite);
            coroutineChangeSprite = StartCoroutine(CoroutineChangeSprite());
        }
        else
        {
            coroutineChangeSprite = StartCoroutine(CoroutineChangeSprite());
        }
    }

    private IEnumerator CoroutineChangeSprite()
    {
        image.sprite = clickSprite;
        yield return new WaitForSeconds(0.1f);
        image.sprite = normalSprite;
    }
}
