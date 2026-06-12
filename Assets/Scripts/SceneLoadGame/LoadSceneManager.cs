using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager Instance { get; private set; }

    public const string MAIN_MENU_SCENE = "MenuSceneGame";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void LoadMenuSceneGame()
    {
        SceneManager.LoadScene(MAIN_MENU_SCENE);
    }
}
