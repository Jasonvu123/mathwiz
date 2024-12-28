using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public EquationGenerator equationGenerator; // Reference to the EquationGenerator
    public AnswerSpawner answerSpawner;        // Reference to the AnswerSpawner
    public Text timerText;                     // UI Text to display the timer
    public Text scoreText;                     // UI Text to display the score
    public GameObject summaryPanel;            // Panel to display the summary
    public Text summaryText;                   // Text inside the summary panel

    private int currentEquationIndex = 0;      // Tracks the current equation index
    private float timeElapsed = 0f;            // Tracks the elapsed time
    private int score = 0;                     // Tracks the player's score
    private bool isGameRunning = true;         // Tracks if the game is active

    void Start()
    {
        // Retrieve settings from PlayerPrefs
        string selectedOperation = PlayerPrefs.GetString("SelectedOperation", "Addition");
        string selectedDifficulty = PlayerPrefs.GetString("SelectedDifficulty", "Easy");
        int selectedQuantity = PlayerPrefs.GetInt("SelectedQuantity", 10);

        // Initialize the game
        InitializeGame(selectedOperation, selectedDifficulty, selectedQuantity);
    }

    void Update()
    {
        if (isGameRunning)
        {
            // Update the timer
            timeElapsed += Time.deltaTime;
            timerText.text = $"Time: {timeElapsed:F1}"; // Display time with 1 decimal place
        }
    }

    private void InitializeGame(string operation, string difficulty, int quantity)
    {
        // Set difficulty in the EquationGenerator
        EquationGenerator.Difficulty parsedDifficulty = EquationGenerator.Difficulty.Easy;
        switch (difficulty)
        {
            case "Medium":
                parsedDifficulty = EquationGenerator.Difficulty.Medium;
                break;
            case "Hard":
                parsedDifficulty = EquationGenerator.Difficulty.Hard;
                break;
        }
        equationGenerator.SetDifficulty(parsedDifficulty);

        // Generate equations
        equationGenerator.GenerateEquations(operation, quantity);

        // Initialize UI
        score = 0;
        scoreText.text = $"Score: {score}";
        timerText.text = "Time: 0.0";
        summaryPanel.SetActive(false); // Hide summary panel

        // Load the first question
        if (equationGenerator.equations.Count > 0)
        {
            LoadNextEquation();
        }
        else
        {
            EndGame(); // No equations to load
        }
    }

    public void OnAnswerSelected(int selectedAnswer)
    {
        if (!isGameRunning) return;

        var currentEquation = equationGenerator.GetEquationAt(currentEquationIndex);

        // Check if the answer is correct
        if (selectedAnswer == currentEquation.answer)
        {
            score += 10; // Correct answer
        }
        else
        {
            score -= 5; // Wrong answer
        }

        // Update the score UI
        scoreText.text = $"Score: {score}";

        // Move to the next equation
        currentEquationIndex++;
        if (currentEquationIndex < equationGenerator.equations.Count)
        {
            LoadNextEquation();
        }
        else
        {
            EndGame(); // No more equations
        }
    }

    private void LoadNextEquation()
    {
        var nextEquation = equationGenerator.GetEquationAt(currentEquationIndex);
        answerSpawner.AssignAnswers(nextEquation);
    }

    private void EndGame()
    {
        isGameRunning = false;

        // Show the summary panel
        summaryPanel.SetActive(true);
        summaryText.text = $"Game Over!\nTime: {timeElapsed:F1} seconds\nScore: {score}";
    }
}
