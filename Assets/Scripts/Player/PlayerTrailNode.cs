using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailConstants
{
    public static float EXP_TIME = 1500.0f;
}


public class TrailNode
{
    Sprite sprite;
    float dropTime;

    public TrailNode(Sprite sprite)
    {
        dropTime = Time.time;
        this.sprite = sprite;
    }
}
