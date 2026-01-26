using UnityEngine;

public class ChessPieceData : MonoBehaviour
{
    // Tọa độ hiện tại trên bàn cờ (Ví dụ: x=3, z=4)
    public int gridX;
    public int gridZ;

    // Có phải là quân của người chơi không? (Để chặn không cho click quân địch)
    public bool isPlayerPiece = true;

    public void MoveTo(int newX, int newZ, Vector3 worldPosition)
    {
        gridX = newX;
        gridZ = newZ;
        // Di chuyển vật lý đến vị trí mới
        transform.position = new Vector3(worldPosition.x, transform.position.y, worldPosition.z);
    }
}