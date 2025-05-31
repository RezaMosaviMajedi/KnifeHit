using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataLevels")]
public class DataLevels : ScriptableObject
{
    public DataLevel[] dataLevels;
}

[System.Serializable]
public class DataLevel
{
    public int maxScore;

    [Header("Trunk Rotation")]
    public bool pingpong;
    public float durationTime = 2f;
    public float maxRotation = 2;
    public AnimationCurve rotationCurve;

    [Header("Trunk Design")]
    public int knive_count;
    public int apple_count;
}