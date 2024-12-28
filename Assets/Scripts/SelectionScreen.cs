using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectionScreen : MonoBehaviour
{
    [Header("UI Elements")]
    public ToggleGroup operationToggleGroup; // Group of toggles for operations
    public Dropdown difficultyDropdown;      // Dropdown for difficulty levels
    public Slider quantitySlider;            // Slider for quantity of equations
    public Text quantityText;                // Text to display current slider value

    void Start()
    {
        // Update the slider text in real-time
        quantitySlider.onValueChanged.AddListener((value) => UpdateQuantityText(value));
        UpdateQuantityText(quantitySlider.value);
    }

    void UpdateQuantityText(float value)
    {
        quantityText.text = $"{Mathf.RoundToInt(value)}";
    }

    public void StartGame()
    {
    // Get the selected operation
    string selectedOperation = "";
    foreach (Toggle toggle in operationToggleGroup.ActiveToggles())
    {
        selectedOperation = toggle.name; // The name of the toggle represents the operation
    }

    // Get the selected difficulty and quantity
    string selectedDifficulty = difficultyDropdown.options[difficultyDropdown.value].text;
    int selectedQuantity = Mathf.RoundToInt(quantitySlider.value);

    // Save the values using PlayerPrefs
    PlayerPrefs.SetString("SelectedOperation", selectedOperation);
    PlayerPrefs.SetString("SelectedDifficulty", selectedDifficulty);
    PlayerPrefs.SetInt("SelectedQuantity", selectedQuantity);


    // Load the determined scene
    UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

}
