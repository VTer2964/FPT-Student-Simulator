using UnityEngine;

public class ChessSetup : MonoBehaviour
{
    [Header("Cần kéo Prefab vào đây")]
    public GameObject tilePrefab;   // Prefab ô đất
    public GameObject piecePrefab;  // Prefab quân cờ (Tạm thời dùng chung)

    [Header("Cần kéo Material vào đây")]
    public Material matWhite;       // Màu ô trắng
    public Material matBlack;       // Màu ô đen
    public Material matPlayer;      // Màu quân mình
    public Material matEnemy;       // Màu quân địch

    // Kích thước chuẩn cờ vua
    private int boardSize = 8;

    void Start()
    {
        GenerateLevel();
    }

    [ContextMenu("Tạo Bàn Cờ 8x8")]
    public void GenerateLevel()
    {
        // 1. Dọn dẹp bàn cờ cũ
        GameObject oldBoard = GameObject.Find("Board_Container");
        if (oldBoard != null) DestroyImmediate(oldBoard);

        // 2. Tạo Group chứa mới
        GameObject boardContainer = new GameObject("Board_Container");

        // 3. Vòng lặp tạo 8x8 ô
        for (int x = 0; x < boardSize; x++)
        {
            for (int z = 0; z < boardSize; z++)
            {
                // Tạo ô đất
                Vector3 pos = new Vector3(x, 0, z);
                GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity, boardContainer.transform);
                tile.name = $"Tile_{x}_{z}"; // Đặt tên theo tọa độ

                // Gán Layer (Bắt buộc để click chuột)
                tile.layer = LayerMask.NameToLayer("BoardTile");

                // Tô màu xen kẽ kiểu bàn cờ
                Renderer rend = tile.GetComponent<Renderer>();
                if ((x + z) % 2 == 0) rend.material = matBlack; // Ô sẫm màu
                else rend.material = matWhite;                  // Ô sáng màu
            }
        }

        // 4. Setup Camera tự động để nhìn thấy hết bàn 8x8
        SetupCamera();

        // 5. (Tùy chọn) Tạo thử 2 quân để test
        // Quân Xe trắng ở góc (0,0)
        SpawnPiece("Rook_White", 0, 0, true, matPlayer);
        // Vua đen ở góc đối diện (7,7)
        SpawnPiece("King_Black", 7, 7, false, matEnemy);

        // 6. Cập nhật GameController
        SetupGameController();

        Debug.Log("Đã tạo bàn cờ 8x8 hoàn tất!");
    }

    void SpawnPiece(string name, int x, int z, bool isPlayer, Material mat)
    {
        GameObject piece = Instantiate(piecePrefab, new Vector3(x, 0, z), Quaternion.identity);
        piece.name = name;
        piece.layer = LayerMask.NameToLayer("ChessPiece");

        piece.GetComponent<Renderer>().material = mat;

        ChessPieceData data = piece.GetComponent<ChessPieceData>();
        if (data == null) data = piece.AddComponent<ChessPieceData>();

        data.isPlayerPiece = isPlayer;
        data.gridX = x;
        data.gridZ = z;
    }

    void SetupCamera()
    {
        if (Camera.main != null)
        {
            // --- CẤU HÌNH GÓC NHÌN CHÉO ---

            // 1. Vị trí (Position): 
            // x = 3.5f (Vẫn giữ ở giữa chiều ngang bàn cờ)
            // y = 8.0f (Hạ thấp độ cao xuống một chút cho gần hơn)
            // z = -4.0f (Lùi ra phía sau bàn cờ để nhìn thấy quân mình ở gần nhất)
            Camera.main.transform.position = new Vector3(3.5f, 8.0f, -4.0f);

            // 2. Góc xoay (Rotation):
            // x = 55f (Ngẩng lên để nhìn chéo, thay vì 90 độ cắm mặt xuống đất)
            Camera.main.transform.rotation = Quaternion.Euler(55f, 0f, 0f);
        }
    }

    void SetupGameController()
    {
        ChessGameController ctrl = FindFirstObjectByType<ChessGameController>();
        if (ctrl == null)
        {
            GameObject obj = new GameObject("GameController");
            ctrl = obj.AddComponent<ChessGameController>();
        }

        ctrl.pieceLayer = 1 << LayerMask.NameToLayer("ChessPiece");
        ctrl.tileLayer = 1 << LayerMask.NameToLayer("BoardTile");

        // Tìm quân Xe trắng vừa tạo
        GameObject rook = GameObject.Find("Rook_White");
        if (rook != null) ctrl.correctPiece = rook.GetComponent<ChessPieceData>();

        // Đặt mục tiêu mới: Xe phải chạy đến góc kia (0, 7)
        ctrl.targetX = 0;
        ctrl.targetZ = 7;
    }
}