using UnityEngine;
using System.Collections;
using System;

public class IceUI : MonoBehaviour
{
    public event Action OnDisableGameObjectIce;
    public void StartCoroutineDisable(int time)
    {
        StopAllCoroutines();
        StartCoroutine(CoroutineDisableIce(time));
    }

    private IEnumerator CoroutineDisableIce(int time)
    {
        yield return new WaitForSeconds(time);
        OnDisableGameObjectIce?.Invoke();
        gameObject.SetActive(false);
    }
}
