using UnityEngine;

public class Utils
{

    public static Vector3 GetRandomDirection()
    {
        return new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f)).normalized;
    }

    public static Vector3 GetRandomXZPositonInRect(Rect rect, float yValue)
    {
        return new Vector3(
            UnityEngine.Random.Range(rect.xMin, rect.xMax),
            yValue,
            UnityEngine.Random.Range(rect.yMin, rect.yMax)
        );
    }
}
