using UnityEngine;
using System.Collections.Generic;

public class InstructionUIGame : MonoBehaviour
{
    [SerializeField] private List<Transform> instructionsList = new List<Transform>();
    private int instructionIndex;


    public void OnClickNextInstruction()
    {
        instructionsList[instructionIndex].gameObject.SetActive(false);
        instructionIndex += 1;
        instructionsList[instructionIndex].gameObject.SetActive(true);
        if(SoundInGameManager.Instance != null)
        {
            SoundInGameManager.Instance.PlayClickNextInstruction();
        }
    } 

    public void OnClickSkipInstruction()
    {
        instructionsList[instructionIndex].gameObject.SetActive(false);
        if (SoundInGameManager.Instance != null)
        {
            SoundInGameManager.Instance.PlayLevelUp();
        }
    }
} 
