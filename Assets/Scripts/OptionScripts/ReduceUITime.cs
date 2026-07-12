using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ReduceUITime : MonoBehaviour
{
    [SerializeField] private Image image;

    private float totalTime;   
    private float currentTime;

    public event Action OnReduceFinish;

    public void StartCountdown(int time)
    {
        StopAllCoroutines();

        // Khởi tạo giá trị ban đầu
        totalTime = time;
        currentTime = time;
        image.enabled = true;
        image.fillAmount = 1f;
        StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        // Vòng lặp chạy khi thời gian hiện tại vẫn lớn hơn 0
        while (currentTime > 0)
        {
            // Trừ dần theo thời gian thực của mỗi khung hình
            currentTime -= Time.deltaTime;

            // Cập nhật thanh UI dựa trên thời gian thực tế còn lại
            image.fillAmount = Mathf.Clamp01(currentTime / totalTime);

            yield return null;
        }

        image.fillAmount = 0f;
        image.enabled = false;
        Debug.Log("Hết giờ!");
        OnReduceFinish?.Invoke();
    }

    public void ReduceCurrentTime(float amount)
    {
        if (currentTime > 0)
        {
            currentTime -= amount;
            if (currentTime < 0)
            {
                currentTime = 0;
            }
            image.fillAmount = Mathf.Clamp01(currentTime / totalTime);

            Debug.Log($"Đã giảm {amount}s. Thời gian còn lại: {currentTime}s");
        }
    }
}