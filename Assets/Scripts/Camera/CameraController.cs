using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player; // Tham chiếu đến Transform của nhân vật
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f); // Khoảng cách giữa camera và player
    [SerializeField] private float smoothSpeed = 5f; // Tốc độ di chuyển mượt mà

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 desiredPosition = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        }
    }
}
