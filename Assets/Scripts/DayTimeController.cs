using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayTimeController : MonoBehaviour
{
    const float secondsInDay = 86400f;

    [SerializeField] private Color nightColor;
    [SerializeField] private AnimationCurve nightTimeCurve;
    [SerializeField] private Color dayColor = Color.white;

    float time;

}
