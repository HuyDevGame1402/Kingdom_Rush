using UnityEngine;

public static class UpdateFacingGameobject
{
    public static void UpdateFacing(ref GameObject ob, bool faceLeft)
    {
        Vector3 scale = ob.transform.localScale;

        scale.x = faceLeft ? -1 : 1;

        ob.transform.localScale = scale;
    }
}
