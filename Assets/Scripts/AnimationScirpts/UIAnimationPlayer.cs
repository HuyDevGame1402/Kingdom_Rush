using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimationPlayer : MonoBehaviour
{
    private Image uiImage;
    private List<Sprite> spritesToPlay;
    private float frameRate;
    private bool isLoop;
    private Action onAnimationComplete; // Lưu trữ hành động khi kết thúc anim
    private Coroutine animCoroutine;

    private void Awake()
    {
        uiImage = GetComponent<Image>();
    }

    public void StartAnimation(List<Sprite> sprites, float fps, bool loop, Action onComplete)
    {
        this.spritesToPlay = sprites;
        this.frameRate = fps;
        this.isLoop = loop;
        this.onAnimationComplete = onComplete; // Gán callback

        StopAnimation();

        if (spritesToPlay != null && spritesToPlay.Count > 0)
        {
            animCoroutine = StartCoroutine(PlayRoutine());
        }
    }

    public void StopAnimation()
    {
        if (animCoroutine != null)
        {
            StopCoroutine(animCoroutine);
            animCoroutine = null;
        }
    }

    private IEnumerator PlayRoutine()
    {
        float timePerFrame = 1f / frameRate;
        int currentFrame = 0;

        while (true)
        {
            if (uiImage == null) break;

            uiImage.sprite = spritesToPlay[currentFrame];

            yield return new WaitForSeconds(timePerFrame);

            currentFrame++;

            if (currentFrame >= spritesToPlay.Count)
            {
                if (isLoop)
                {
                    currentFrame = 0;
                }
                else
                {
                    // Nếu KHÔNG lặp (loop = false), kết thúc Coroutine và gọi Action Complete
                    animCoroutine = null;
                    onAnimationComplete?.Invoke(); // Gọi hàm callback an toàn
                    break;
                }
            }
        }
    }

    private void OnDisable()
    {
        StopAnimation();
    }
}