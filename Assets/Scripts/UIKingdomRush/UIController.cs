using System.Collections.Generic;
using UnityEngine;
using System;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}