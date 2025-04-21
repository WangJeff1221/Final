using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndUI : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI efficiencyText;
    public TextMeshProUGUI accuracyText;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        int score = GameManager.FinalScore;
        int totalShots = GameManager.FinalShots;
        int hits = GameManager.FinalHits;
        float accuracy = 0f;
        if (totalShots > 0)
            accuracy = (float)hits / totalShots * 100f;
        finalScoreText.text = "Final Score: " + score;
        efficiencyText.text = $"Hits/Total Shots: {hits}/{totalShots}";
        accuracyText.text = "Hit Rate: " + accuracy.ToString("F1") + "%";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("StartScene");
    }
}
