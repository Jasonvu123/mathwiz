using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public EquationGenerator equationGenerator; // Reference to the EquationGenerator
    public AnswerSpawner answerSpawner;        // Reference to the AnswerSpawner
    public Text timerText;                     // UI Text to display the timer
    public Text scoreText;                     // UI Text to display the score
    public GameObject summaryPanel;            // Panel to display the summary
    public Text summaryText;                   // Text inside the summary panel

    public Button home;

    private int currentEquationIndex = 0;      // Tracks the current equation index
    private float timeElapsed = 0f;            // Tracks the elapsed time
    private int score = 0;                     // Tracks the player's score

    private int correctAns = 0;
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

        // Debug the selected and correct answers
        Debug.Log($"Selected: {selectedAnswer}, Correct: {currentEquation.answer}");

        if (selectedAnswer == currentEquation.answer)
        {
            score += 10; // Correct answer
            correctAns += 1;
            Debug.Log("Correct Answer! Score updated.");
        }
        else
        {
            score -= 5; // Wrong answer
            Debug.Log("Wrong Answer! Score updated.");
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

        Debug.Log("Game Over! Displaying Summary.");

        // Stop updating the timer
        timeElapsed = Mathf.Round(timeElapsed * 10) / 10; // Round to 1 decimal place

        // Calculate correct and wrong answers

        // Show the summary panel
        summaryPanel.SetActive(true);
        summaryText.text = $"Game Over!\n" +
                           $"Time: {timeElapsed:F1} seconds\n" +
                           $"Score: {score}\n" +
                           $"Correct: {correctAns}\n" +
                           $"Wrong: {currentEquationIndex - correctAns}";

        Debug.Log($"Summary:\nTime: {timeElapsed}\nScore: {score}\nCorrect: {correctAns}\nWrong: {currentEquationIndex - correctAns}");

        // Reveal the Home button and attach functionality
        home.gameObject.SetActive(true);
        home.onClick.RemoveAllListeners(); // Prevent duplicate listeners
        home.onClick.AddListener(() => GoBackToHome());
 
    }

    private void GoBackToHome()
    {
        SceneManager.LoadScene("Selection"); // Replace "SelectionScreen" with your main menu scene name
    }
}
