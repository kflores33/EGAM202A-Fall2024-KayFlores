using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// code from here: https://discussions.unity.com/t/how-do-i-check-a-rotation-relative-to-another-objects-rotation/147501/2
public static class Utility
{
    public static float RelativeAngle(Vector3 fwd, Vector3 targetDir, Vector3 upDir)
    {
        var angle = Vector3.Angle(fwd, targetDir);

        if (Utility.AngleDirection(fwd, targetDir, upDir) == -1)
            return -angle;
        else
            return angle;
    }

    public static float RelativeAngle(Vector2 fwd, Vector2 targetDir, Vector3 upDir)
    {
        var angle = Vector2.Angle(fwd, targetDir);

        if (Utility.AngleDirection(fwd, targetDir, upDir) == -1)
            return -angle;
        else
            return angle;
    }

    public static float AngleDirection(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0F)
            return 1F;
        else if (dir < 0F)
            return -1F;
        else
            return 0F;
    }

    public static float AngleDirection(Vector2 fwd, Vector2 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(new Vector3(fwd.x, 0f, fwd.y),
                                     new Vector3(targetDir.x, 0f, targetDir.y));
        float dir = Vector3.Dot(perp, up);

        if (dir > 0F)
            return 1F;
        else if (dir < 0F)
            return -1F;
        else
            return 0F;
    }
}
