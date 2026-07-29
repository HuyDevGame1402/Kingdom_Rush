using UnityEngine;

/// Gắn script này vào cùng GameObject với ArrowKingdomRush.
/// Nó sẽ in ra Console mọi thông số thực tế khi mũi tên được bắn.
/// XÓA sau khi debug xong.
public class ArrowDebugger : MonoBehaviour
{
    private ArrowKingdomRush arrow;

    private void Awake()
    {
        arrow = GetComponent<ArrowKingdomRush>();
    }

    private void Update()
    {
        // Chỉ log frame đầu tiên khi đang bay
        if (arrow == null || !arrow.isFlying) return;

        // Đọc các field qua reflection để không cần sửa ArrowKingdomRush
        var t = arrow.GetType();
        var fA = t.GetField("pointA", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var fS = t.GetField("arrowSpeed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var fFT = t.GetField("flightTime", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var fEl = t.GetField("elapsed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (fA == null || fFT == null) return;

        float elapsed = (float)fEl.GetValue(arrow);
        if (elapsed > Time.deltaTime * 2f) return; // chỉ log frame đầu

        Vector3 pA = (Vector3)fA.GetValue(arrow);
        float spd = (float)fS.GetValue(arrow);
        float flightTime = (float)fFT.GetValue(arrow);
        float dist = Vector3.Distance(pA, arrow.transform.position);

        //Debug.Log($"<color=orange>[ArrowDebug]</color>\n" +
        //          $"  arrowSpeed  = {spd}\n" +
        //          $"  distance    = {dist:F2} units  ← khoảng cách thực tế\n" +
        //          $"  flightTime  = {flightTime:F3}s  ← thời gian bay thực\n" +
        //          $"  → Để bay 0.2s cần arrowSpeed = {dist / 0.2f:F0}\n" +
        //          $"  → Để bay 0.3s cần arrowSpeed = {dist / 0.3f:F0}");
    }
}