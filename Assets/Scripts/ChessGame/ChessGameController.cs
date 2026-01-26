using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Nếu muốn hiển thị thông báo thắng

public class ChessGameController : MonoBehaviour
{
    [Header("Cấu hình Layer")]
    public LayerMask pieceLayer;
    public LayerMask tileLayer;

    [Header("Đáp án của thế cờ")]
    public ChessPieceData correctPiece; // Quân cờ cần di chuyển
    public int targetX;                 // Tọa độ X đích đến (ví dụ: 0-7)
    public int targetZ;                 // Tọa độ Z đích đến (ví dụ: 0-7)

    private ChessPieceData selectedPiece;


    void Start()
    {
        // --- ĐOẠN CODE MỚI THÊM VÀO ---
        // Mở khóa chuột để người chơi có thể click
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // -----------------------------

        // (Nếu bạn có code khác trong Start thì để nguyên ở dưới)
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }

        // Phím thoát
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Campus_Main");
        }
    }

    void HandleClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            // 1. Nếu click vào quân cờ
            if (((1 << hit.collider.gameObject.layer) & pieceLayer) != 0)
            {
                ChessPieceData piece = hit.collider.GetComponent<ChessPieceData>();
                if (piece != null && piece.isPlayerPiece)
                {
                    selectedPiece = piece;
                    Debug.Log("Đã chọn quân tại: " + piece.gridX + "," + piece.gridZ);
                    // Gợi ý: Thêm code đổi màu quân cờ để biết đang chọn
                }
            }
            // 2. Nếu click vào ô đất (Tile) VÀ đã chọn quân trước đó
            else if (((1 << hit.collider.gameObject.layer) & tileLayer) != 0 && selectedPiece != null)
            {
                // Lấy tọa độ từ vị trí của ô đất
                // Giả sử mỗi ô cách nhau 1 đơn vị và bắt đầu từ 0
                int clickedX = Mathf.RoundToInt(hit.collider.transform.position.x);
                int clickedZ = Mathf.RoundToInt(hit.collider.transform.position.z);

                MoveSelectedPiece(clickedX, clickedZ, hit.collider.transform.position);
            }
        }
    }

    void MoveSelectedPiece(int x, int z, Vector3 worldPos)
    {
        // Di chuyển quân cờ
        selectedPiece.MoveTo(x, z, worldPos);

        // Kiểm tra chiến thắng
        if (selectedPiece == correctPiece && x == targetX && z == targetZ)
        {
            Debug.Log("CHÚC MỪNG! BẠN ĐÃ GIẢI ĐƯỢC THẾ CỜ!");
            // Thưởng và quay về
            GameManager.Instance.AddMedal(MedalType.Gold);
            Invoke("ReturnToMain", 2f); // Đợi 2 giây rồi về
        }
        else
        {
            Debug.Log("Nước đi sai rồi! Hãy thử lại.");
            // Logic reset game nếu cần
        }

        selectedPiece = null; // Bỏ chọn
    }

    void ReturnToMain()
    {
        SceneManager.LoadScene("Campus_Main");
    }
}