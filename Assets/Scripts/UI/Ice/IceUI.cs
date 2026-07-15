using UnityEngine;
using System.Collections;

public class IceUI : MonoBehaviour
{
    public void StartCoroutineDisable(int time)
    {
        StopAllCoroutines();
        StartCoroutine(CoroutineDisableIce(time));
    }

    private IEnumerator CoroutineDisableIce(int time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
