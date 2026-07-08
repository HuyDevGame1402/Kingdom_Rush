using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class MonsterGuideUI : MonoBehaviour
{
    [SerializeField] private OnClickMonsterGuide onClickMonsterGuide;

    [Header("UI")]
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject des;
    [SerializeField] private GameObject lightUI;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    private List<EnemyGroup> enemyGroups;

    [SerializeField] private bool isTop;

    [SerializeField] private Transform panelMonsterGuideUI;


    private void Start()
    {
        RegisterEventOnClick();
    }

    private void RegisterEventOnClick()
    {
        onClickMonsterGuide.OnClickMonsterGuideUI += OnClickMonsterGuide_OnClickMonsterGuideUI;
        onClickMonsterGuide.SpawnMonsterGuideUI += OnClickMonsterGuide_SpawnMonsterGuideUI;
        panelMonsterGuideUI.GetComponent<OnClickCancleMonsterGuide>().OnClickCancleMonsterGuideUI += CancleMonsterUI;
    }

    private void OnClickMonsterGuide_SpawnMonsterGuideUI()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.SetState(GameManager.GameState.Playing);
        }
        main.SetActive(false);
        DisableDes();
        StartCoroutine(TestCoroutine());
        panelMonsterGuideUI.gameObject.SetActive(false);
    }

    private IEnumerator TestCoroutine()
    {
        yield return new WaitForSeconds(5f);
        main.gameObject.SetActive(true);
    }

    private void OnClickMonsterGuide_OnClickMonsterGuideUI()
    {
        if (LevelEnemySpawner.Instance != null)
        {
            enemyGroups = LevelEnemySpawner.Instance.GetCurrentEnemyGroup();
        }
        textMeshProUGUI.text = "";

        for (int i = 0; i < enemyGroups.Count; i++)
        {
            textMeshProUGUI.text += enemyGroups[i].enemyName + " X " + enemyGroups[i].count + "\n";
        }
        textMeshProUGUI.gameObject.SetActive(true);
        des.SetActive(true);

        // cần tính toán vị trí des ở đây

        RectTransform mainRect = main.GetComponent<RectTransform>();
        RectTransform desRect = des.GetComponent<RectTransform>();

        float mainY = mainRect.anchoredPosition.y;
        float halfHeight = desRect.rect.height * 0.5f;

        Vector2 pos = desRect.anchoredPosition;

        pos.y = isTop
            ? mainY - halfHeight
            : mainY + halfHeight;

        desRect.anchoredPosition = pos;

        main.SetActive(true);
        lightUI.SetActive(true);
        panelMonsterGuideUI.gameObject.SetActive(true);
    }
    private void CancleMonsterUI()
    {
        DisableDes();
        onClickMonsterGuide.SetConfirmNextWave(false);
        panelMonsterGuideUI.gameObject.SetActive(false);
    }
    private void DisableDes()
    {
        textMeshProUGUI.gameObject.SetActive(false);
        des.SetActive(false);
        lightUI.SetActive(false);
    }
}
