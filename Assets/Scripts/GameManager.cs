using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text equationText;
    public Text timerText;
    public Text scoreText;
    public Button[] answerButtons;

    private string difficulty;
    private int quantity;
    private int currentEquationIndex = 0;
    private int score = 0;

    void Start()
    {
        // Retrieve settings
        difficulty = PlayerPrefs.GetString("Difficulty", "Easy"); // Default to "Easy" if not set
        quantity = PlayerPrefs.GetInt("Quantity", 10); // Default to 10 if not set

        // Initialize game
        Debug.Log($"Difficulty: {difficulty}, Quantity: {quantity}");
    }

}
