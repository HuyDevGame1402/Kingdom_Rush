using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLayer : MonoBehaviour
{
    public float speed = 5f; // Tốc độ di chuyển
    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Lấy Rigidbody2D từ object
        rb.gravityScale = 0; // Tắt trọng lực để không bị rơi
    }

    void Update()
    {
        // Kiểm tra từng phím riêng lẻ
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.J)) moveX = -1; // J để sang trái
        if (Input.GetKey(KeyCode.L)) moveX = 1;  // L để sang phải
        if (Input.GetKey(KeyCode.I)) moveY = 1;  // I để đi lên
        if (Input.GetKey(KeyCode.K)) moveY = -1; // K để đi xuống

        moveInput = new Vector2(moveX, moveY).normalized; // Chuẩn hóa để tốc độ không bị tăng khi di chuyển chéo
    }

    void FixedUpdate()
    {
        rb.velocity = moveInput * speed; // Cập nhật vận tốc cho Rigidbody2D
    }


}
