using UnityEngine;

public class SettingMenuSceneGameManager : MonoBehaviour
{
    public static SettingMenuSceneGameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
