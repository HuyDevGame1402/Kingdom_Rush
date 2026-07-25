using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveText;

    private void Awake()
    {
        waveText = transform.GetComponent<TextMeshProUGUI>();
        WaveManager.Instance.WaveChange += WaveManager_WaveChange;
    }

    private void WaveManager_WaveChange(int wave, int waveMax)
    {
        waveText.text = "WAVE " +  wave.ToString() + "/" + waveMax.ToString();
    }
}
