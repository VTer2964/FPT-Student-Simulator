using UnityEngine;

public enum MedalType { None = 0, Bronze = 1, Silver = 2, Gold = 3 }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentDay = 1;
    public int maxDay = 7;

    public int gold;
    public int silver;
    public int bronze;
    public int totalScore;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // giữ qua các scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddMedal(MedalType type)
    {
        switch (type)
        {
            case MedalType.Gold:
                gold++;
                totalScore += 3;
                break;
            case MedalType.Silver:
                silver++;
                totalScore += 2;
                break;
            case MedalType.Bronze:
                bronze++;
                totalScore += 1;
                break;
            case MedalType.None:
                // No medal awarded, no points added
                break;
        }
    }

    public void NextDay()
    {
        currentDay++;

        if (currentDay > maxDay)
        {
            Debug.Log("Show ending here!");
        }
    }
}
