using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SudokuManager : MonoBehaviour
{
    [Header("Settings")]
    public GameObject cellPrefab;
    public float spacing = 1.1f;      // Khoảng cách giữa các ô thường
    public float blockGap = 0.5f;     // <--- MỚI: Khoảng cách giữa các khu vực 3x3
    public Transform gridOrigin;

    [Header("Effects")]
    public ParticleSystem winEffect;

    private List<SudokuCell> allCells = new List<SudokuCell>();
    private bool isGameActive = true;

    // MAP ĐỀ BÀI
    int[,] puzzle = {
        {5,3,0, 0,7,0, 0,0,0},
        {6,0,0, 1,9,5, 0,0,0},
        {0,9,8, 0,0,0, 0,6,0},
        {8,0,0, 0,6,0, 0,0,3},
        {4,0,0, 8,0,3, 0,0,1},
        {7,0,0, 0,2,0, 0,0,6},
        {0,6,0, 0,0,0, 2,8,0},
        {0,0,0, 4,1,9, 0,0,5},
        {0,0,0, 0,8,0, 0,7,9}
    };

    // ĐÁP ÁN
    int[,] solution = {
        {5,3,4, 6,7,8, 9,1,2},
        {6,7,2, 1,9,5, 3,4,8},
        {1,9,8, 3,4,2, 5,6,7},
        {8,5,9, 7,6,1, 4,2,3},
        {4,2,6, 8,5,3, 7,9,1},
        {7,1,3, 9,2,4, 8,5,6},
        {9,6,1, 5,3,7, 2,8,4},
        {2,8,7, 4,1,9, 6,3,5},
        {3,4,5, 2,8,6, 1,7,9}
    };

    void Start()
    {
        // --- ĐOẠN CODE MỚI THÊM VÀO ---
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // -----------------------------

        GenerateGrid();
        SetupCamera();
    }
    void GenerateGrid()
    {
        // Tính toán kích thước tổng thể để căn giữa (bao gồm cả khoảng hở mới)
        // 8 khoảng spacing + 2 khoảng blockGap
        float totalWidth = (8 * spacing) + (2 * blockGap);
        Vector3 centerOffset = new Vector3(totalWidth / 2f, 0, totalWidth / 2f);

        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                // 1. Tính vị trí cơ bản
                float xPos = col * spacing;
                float zPos = (8 - row) * spacing;

                // 2. Cộng thêm khoảng cách cho Block (Cứ mỗi 3 ô thì cộng thêm Gap)
                // (col / 3) sẽ trả về 0, 1 hoặc 2 tương ứng với Block thứ mấy
                xPos += (col / 3) * blockGap;
                zPos += ((8 - row) / 3) * blockGap;

                Vector3 pos = new Vector3(xPos, 0, zPos) - centerOffset;

                if (gridOrigin != null) pos += gridOrigin.position;

                GameObject obj = Instantiate(cellPrefab, pos, Quaternion.identity, transform);

                SudokuCell cell = obj.GetComponent<SudokuCell>();

                int num = puzzle[row, col];
                int ans = solution[row, col];
                cell.Setup(num, num != 0, ans);

                allCells.Add(cell);
            }
        }
    }

    void Update()
    {
        if (!isGameActive) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                SudokuCell cell = hit.collider.GetComponent<SudokuCell>();
                if (cell != null)
                {
                    cell.Interact();
                    CheckWin();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Campus_Main");
        }
    }

    void CheckWin()
    {
        foreach (var cell in allCells)
        {
            if (cell.currentNumber != cell.correctNumber) return;
        }
        WinGame();
    }

    void WinGame()
    {
        Debug.Log("YOU WIN!");
        isGameActive = false;
        if (winEffect != null) winEffect.Play();
        if (GameManager.Instance != null) GameManager.Instance.AddMedal(MedalType.Gold);
        Invoke("GoBack", 3f);
    }

    void GoBack()
    {
        SceneManager.LoadScene("Campus_Main");
    }

    void SetupCamera()
    {
        if (Camera.main != null)
        {
            // --- CẤU HÌNH CAMERA CẬN CẢNH (ZOOM IN) ---

            // 1. Vị trí (Position): 
            // x = 0: Giữ nguyên căn giữa
            // y = 11f: Hạ thấp xuống (Cũ là 16f) -> Giúp bàn cờ to hơn
            // z = -8.5f: Tiến lại gần hơn (Cũ là -12f)
            Camera.main.transform.position = new Vector3(0, 11f, -8.5f);

            // 2. Góc xoay (Rotation):
            // x = 60f: Chúi xuống thêm một chút để nhìn rõ các ô ở xa
            Camera.main.transform.rotation = Quaternion.Euler(60f, 0f, 0f);
        }
    }
}