using UnityEngine;

public static class DirectionUtility
{
    public static void GetDirection(
        Vector2 dir,
        out bool faceLeft,
        out VerticalAnimation vertical)
    {
        faceLeft = dir.x < 0;

        vertical = dir.y > 0
            ? VerticalAnimation.Up
            : VerticalAnimation.Down;
    }
}