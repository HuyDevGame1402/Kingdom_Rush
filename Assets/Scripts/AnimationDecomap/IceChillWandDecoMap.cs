using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class IceChillWandDecoMap : MonoBehaviour
{

    public static IceChillWandDecoMap Instance { get; private set; }

    [SerializeField] private List<Transform> wayPointsStart = new();
    [SerializeField] private List<Transform> wayPointsMiddle = new();
    [SerializeField] private List<Transform> wayPointsEnd = new();

    [SerializeField] private List<Transform> iceSheet = new();

    [SerializeField] private int iceCount = 7;

    [SerializeField] private float randomRadius = 0.8f;

    private void Awake()
    {
        Instance = this;
        InitIceSheet();
    }

    public void ShowIce()
    {
        DisableIce();

        if (iceSheet.Count == 0)
            return;

        List<Transform> availableIce = new List<Transform>(iceSheet);

        int startCount = 0;
        int middleCount = 0;
        int endCount = 0;

        if (iceCount >= 3)
        {
            startCount = 1;
            middleCount = 1;
            endCount = 1;

            int remain = iceCount - 3;

            for (int i = 0; i < remain; i++)
            {
                switch (Random.Range(0, 3))
                {
                    case 0:
                        startCount++;
                        break;
                    case 1:
                        middleCount++;
                        break;
                    case 2:
                        endCount++;
                        break;
                }
            }
        }
        else
        {
            for (int i = 0; i < iceCount; i++)
            {
                switch (Random.Range(0, 3))
                {
                    case 0:
                        startCount++;
                        break;
                    case 1:
                        middleCount++;
                        break;
                    case 2:
                        endCount++;
                        break;
                }
            }
        }

        SpawnIceInSection(wayPointsStart, startCount, availableIce);
        SpawnIceInSection(wayPointsMiddle, middleCount, availableIce);
        SpawnIceInSection(wayPointsEnd, endCount, availableIce);
    }

    private void SpawnIceInSection(List<Transform> wayPoints, int count, List<Transform> availableIce)
    {
        if (count <= 0 || wayPoints.Count == 0)
            return;

        List<Transform> availableWayPoints = new List<Transform>(wayPoints);

        count = Mathf.Min(count, availableWayPoints.Count, availableIce.Count);

        for (int i = 0; i < count; i++)
        {
            int wpIndex = Random.Range(0, availableWayPoints.Count);
            Transform wp = availableWayPoints[wpIndex];
            availableWayPoints.RemoveAt(wpIndex);

            int iceIndex = Random.Range(0, availableIce.Count);
            Transform ice = availableIce[iceIndex];
            availableIce.RemoveAt(iceIndex);

            Vector3 pos = wp.position;

            pos.x += Random.Range(-randomRadius, randomRadius);
            pos.y += Random.Range(-randomRadius, randomRadius);

            ice.position = pos;
            ice.gameObject.SetActive(true);
        }
    }

    public void StartCoroutineDisable(int time)
    {
        StopAllCoroutines();
        StartCoroutine(CoroutineDisableIce(time));
    }

    private IEnumerator CoroutineDisableIce(int time)
    {
        yield return new WaitForSeconds(time);
        DisableIce();
    }

    private void InitIceSheet()
    {
        iceSheet.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            iceSheet.Add(transform.GetChild(i));
        }
    }

    private void DisableIce()
    {
        foreach (Transform ice in iceSheet)
        {
            ice.gameObject.SetActive(false);
        }
    }
}