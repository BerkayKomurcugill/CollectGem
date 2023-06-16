using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum GemType
{
    pink,
    green,
    yellow
}


public class GemScript : MonoBehaviour
{
    public GemType GemType;
    public int price;
}
