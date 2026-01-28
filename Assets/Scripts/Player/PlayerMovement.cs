using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gravity = -9.81f;

    CharacterController controller;
    Vector3 velocity;

    // THÊM: Animator để điều khiển animation
    public Animator anim;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // TÌM Animator trong child model (hoặc kéo thủ công)
        if (anim == null)
            anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // Lấy input từ bàn phím (WASD)
        float x = Input.GetAxis("Horizontal");   // A/D
        float z = Input.GetAxis("Vertical");     // W/S

        // Hướng di chuyển theo local space
        Vector3 move = transform.right * x + transform.forward * z;

        // THÊM: Tính speed cho animation (0 = đứng yên, 1 = chạy max)
        float speed = move.magnitude;  // Độ dài vector di chuyển

        // THÊM: Gửi speed cho Animator
        if (anim != null)
            anim.SetFloat("Speed", speed, 0.1f, Time.deltaTime);

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
