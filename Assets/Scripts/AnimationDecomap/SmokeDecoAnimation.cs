using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeDecoAnimation : MonoBehaviour
{
    [SerializeField] private List<GameObject> smokeGameObject = new List<GameObject>();
    private void OnEnable()
    {
        for (int i = 0; i < smokeGameObject.Count; i++)
        {
            smokeGameObject[i].SetActive(true);
            MapDecoAnimationManager.Instance.PlayAnimation("mapDeco_smoke", smokeGameObject[i], 24f, true);
        }
    }
    private void OnDisable()
    {
        for (int i = 0; i < smokeGameObject.Count; i++)
        { 
            if(smokeGameObject[i] != null)
            {
                smokeGameObject[i].SetActive(false);
                MapDecoAnimationManager.Instance.StopAnimation(smokeGameObject[i]);
            }
        }
    }
}