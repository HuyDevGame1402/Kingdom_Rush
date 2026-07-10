using UnityEngine;

[CreateAssetMenu(menuName = "Wave Actions/Open Panel")]
public class EventWavePanel : EventWaveScript
{
    [SerializeField] private string panelTag; 

    public override void Execute()
    {
        GameObject panel = GameObject.FindWithTag(panelTag);
        if (panel != null)
        {
            panel.transform.GetComponent<Animator>().SetTrigger("Open");
            Time.timeScale = 0f;
        }
    }
}
