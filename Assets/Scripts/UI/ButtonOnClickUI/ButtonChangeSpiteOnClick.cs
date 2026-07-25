using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class ButtonChangeSpiteOnClick : MonoBehaviour
{
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite clickSprite;

    private Image image;
    private float timeDelay = 0.1f;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void OnClickChangeSprite()
    {
        image.sprite = clickSprite;
        StartCoroutine(CoroutineChangeSprite());
    }

    private IEnumerator CoroutineChangeSprite()
    {
        yield return new WaitForSeconds(timeDelay);
        image.sprite = normalSprite;
    }
} 
