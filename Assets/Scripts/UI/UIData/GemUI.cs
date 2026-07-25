using System;
using UnityEngine;
using TMPro;

public class GemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gemsVictory;
    [SerializeField] private TextMeshProUGUI gemsDefeat;

    private void Start()
    {
        if(GemManager.Instance != null)
        {
            GemManager.Instance.OnEndGameGemReward += GemManager_OnEndGameGemReward;
        }
    }

    private void GemManager_OnEndGameGemReward(int gems)
    {
        gemsVictory.text = gems.ToString();
        gemsDefeat.text = gems.ToString() + "GEMS";
    }
}
