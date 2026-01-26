using UnityEngine;
using UnityEngine.SceneManagement;

public class ChessPuzzleManager : MonoBehaviour
{
    [Header("Settings")]
    public LayerMask chessPieceLayer; // Layer đặt cho các quân cờ
    public LayerMask boardTileLayer;  // Layer đặt cho các ô bàn cờ

    private GameObject selectedPiece;
    private Vector3 originalPosition;

    void Update()
    {
        // Xử lý Input chuột trái để chọn hoặc di chuyển
        if (Input.GetMouseButtonDown(0))
        {
            HandleInput();
        }

        // Thoát game về menu chính (Ví dụ phím ESC)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Campus_Main"); // Tên scene chính của bạn
        }
    }

    void HandleInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            // Nếu click vào quân cờ (kiểm tra theo Layer)
            if (((1 << hit.collider.gameObject.layer) & chessPieceLayer) != 0)
            {
                SelectPiece(hit.collider.gameObject);
            }
            // Nếu click vào ô bàn cờ và đã chọn quân
            else if (((1 << hit.collider.gameObject.layer) & boardTileLayer) != 0 && selectedPiece != null)
            {
                MovePiece(hit.collider.gameObject.transform.position);
            }
        }
    }

    void SelectPiece(GameObject piece)
    {
        selectedPiece = piece;
        originalPosition = piece.transform.position;
        Debug.Log("Đã chọn quân: " + piece.name);

        // Gợi ý: Có thể thêm code đổi màu quân cờ hoặc nhấc nhẹ lên y để biết đang chọn
    }

    void MovePiece(Vector3 targetPos)
    {
        // Ở đây bạn có thể thêm logic kiểm tra nước đi hợp lệ của cờ vua
        // Để đơn giản cho tutorial, ta cho phép di chuyển đến vị trí click
        // Giữ nguyên độ cao Y của quân cờ để không bị chìm xuống đất
        Vector3 newPos = new Vector3(targetPos.x, selectedPiece.transform.position.y, targetPos.z);

        selectedPiece.transform.position = newPos;

        // Kiểm tra điều kiện thắng ngay sau khi đi
        CheckWinCondition();

        selectedPiece = null; // Bỏ chọn sau khi đi
    }

    void CheckWinCondition()
    {
        // Ví dụ: Kiểm tra nếu quân "King" đến vị trí đích (bạn có thể đặt 1 object trigger vô hình ở đích)
        // Đây là logic giả định, bạn cần tùy chỉnh theo thế cờ cụ thể
        Debug.Log("Kiểm tra nước cờ...");

        // if (thắng) {
        //     Debug.Log("Win Chess!");
        //     GameManager.Instance.AddMedal(MedalType.Gold);
        //     SceneManager.LoadScene("Campus_Main");
        // }
    }
}