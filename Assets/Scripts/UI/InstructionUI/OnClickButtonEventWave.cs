using UnityEngine;
using UnityEngine.UI;

public class OnClickButtonEventWave : MonoBehaviour
{
    private Button buttonOnClick;

    [SerializeField] private GameObject logicAfterEventWave;

    private void Awake()
    {
        buttonOnClick = GetComponent<Button>();
    }
    private void Start()
    {
        buttonOnClick.onClick.AddListener(OnClickButtonLogic);
    }

    private void OnClickButtonLogic()
    {
        Time.timeScale = 1f;
        if (transform.parent.GetComponent<Animator>() != null)
        {
            transform.parent.GetComponent<Animator>().SetTrigger("Close");
            //Animator animator = transform.parent.GetComponent<Animator>();
            //Debug.Log(animator.gameObject.name);
            //animator.Play("Close");
        }
        if (logicAfterEventWave != null)
        {
            logicAfterEventWave.GetComponent<IHasLogicAfterEventWave>().Execute();
            Debug.LogWarning(gameObject.name);
        }
    }
}
