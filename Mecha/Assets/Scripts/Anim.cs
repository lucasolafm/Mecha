
using UnityEngine;

public static class Anim
{
    public static float EaseOutSine(float x)
    {
        return Mathf.Sin(x * Mathf.PI / 2);
    }
}