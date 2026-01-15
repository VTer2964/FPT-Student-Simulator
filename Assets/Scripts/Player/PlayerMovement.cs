using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gravity = -9.81f;

    CharacterController controller;
    Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Lấy input từ bàn phím (WASD)
        float x = Input.GetAxis("Horizontal");   // A/D
        float z = Input.GetAxis("Vertical");     // W/S

        // Hướng di chuyển theo local space
        Vector3 move = transform.right * x + transform.forward * z;

        // Gọi Move để di chuyển
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Áp dụng gravity đơn giản
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // ép dính mặt đất
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
