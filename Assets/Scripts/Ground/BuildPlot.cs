using UnityEngine;
using System;


public class BuildPlot : MonoBehaviour
{
    public bool isOccupied = false;
    public event Action<Transform,bool> OnClickBuildTower;

    private void OnMouseDown()
    {
        OnClickBuildTower?.Invoke(transform,isOccupied);
    }
}
