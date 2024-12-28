using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AnswerSpawner : MonoBehaviour
{
    public List<Button> answerButtons;        // Preset buttons in the scene
    public EquationGenerator equationGenerator; // Reference to the equation generator
    public Text equationText;                // UI Text to display the current equation

    private int currentEquationIndex = 0;    // Tracks the current equation index

    public void AssignAnswers(EquationGenerator.EquationData currentEquation)
    {
        // Update the equation text
        equationText.text = currentEquation.equation;

        // Generate answer choices
        List<int> answers = GenerateAnswerChoices(currentEquation.answer, currentEquation.equation);

        // Shuffle the answers
        answers = ShuffleList(answers);

        // Assign answers to the buttons
        for (int i = 0; i < answerButtons.Count; i++)
        {
            int answer = answers[i];
            var buttonText = answerButtons[i].GetComponentInChildren<Text>();
            buttonText.text = answer.ToString();

            // Clear previous listeners and add a new one
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(answer));
        }
    }

    private List<int> GenerateAnswerChoices(int correctAnswer, string equation)
    {
        List<int> answers = new List<int> { correctAnswer }; // Add the correct answer

        // Generate wrong answers
        while (answers.Count < answerButtons.Count)
        {
            int wrongAnswer = 0;

            // Different strategies based on operation
            if (equation.Contains("+") || equation.Contains("-"))
            {
                // Close numbers for addition/subtraction
                wrongAnswer = Random.Range(correctAnswer - 2, correctAnswer + 3);
            }
            else if (equation.Contains("×") || equation.Contains("÷"))
            {
                // Multiples for multiplication/division
                wrongAnswer = correctAnswer * Random.Range(1, 3);
            }

            // Ensure unique and valid answers
            if (!answers.Contains(wrongAnswer) && wrongAnswer > 0)
            {
                answers.Add(wrongAnswer);
            }
        }

        return answers;
    }

    private List<T> ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(0, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }

    private void OnAnswerSelected(int selectedAnswer)
    {
        Debug.Log($"Answer selected: {selectedAnswer}");
        var currentEquation = equationGenerator.GetEquationAt(currentEquationIndex);

        if (selectedAnswer == currentEquation.answer)
        {
            Debug.Log("Correct Answer!");
        }
        else
        {
            Debug.Log("Wrong Answer!");
        }

        // Move to the next equation
        currentEquationIndex++;
        if (currentEquationIndex < equationGenerator.equations.Count)
        {
            AssignAnswers(equationGenerator.GetEquationAt(currentEquationIndex));
        }
        else
        {
            Debug.Log("All equations answered!");
        }
    }
}
