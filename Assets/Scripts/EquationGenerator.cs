using UnityEngine;
using System.Collections.Generic;

public class EquationGenerator : MonoBehaviour
{
    public enum Difficulty { Easy, Medium, Hard }
    public Difficulty difficulty = Difficulty.Easy;

    public int minValue = 1; // Minimum value for numbers
    public int maxValue = 10; // Maximum value for numbers

    [System.Serializable]
    public struct EquationData
    {
        public string equation;
        public int answer;

        public EquationData(string eq, int ans)
        {
            equation = eq;
            answer = ans;
        }
    }

    public List<EquationData> equations = new List<EquationData>(); // Stores equations and answers

    public void SetDifficulty(Difficulty newDifficulty)
    {
        difficulty = newDifficulty;

        // Adjust ranges based on difficulty
        switch (difficulty)
        {
            case Difficulty.Easy:
                minValue = 1;
                maxValue = 10;
                break;

            case Difficulty.Medium:
                minValue = 10;
                maxValue = 50;
                break;

            case Difficulty.Hard:
                minValue = 50;
                maxValue = 100;
                break;
        }
    }

    public void GenerateEquations(string operation, int quantity)
    {
        equations.Clear(); // Clear any existing equations

        for (int i = 0; i < quantity; i++)
        {
            var equationData = GenerateSingleEquation(operation);
            equations.Add(equationData);
        }
    }

    private EquationData GenerateSingleEquation(string operation)
    {
        int num1 = Random.Range(minValue, maxValue + 1);
        int num2 = Random.Range(minValue, maxValue + 1);

        string equation = "";
        int answer = 0;

        switch (operation)
        {
            case "Addition":
                equation = $"{num1} + {num2} = ?";
                answer = num1 + num2;
                break;

            case "Subtraction":
                if (num1 < num2) (num1, num2) = (num2, num1); // Ensure no negative answers
                equation = $"{num1} - {num2} = ?";
                answer = num1 - num2;
                break;

            case "Multiplication":
                equation = $"{num1} × {num2} = ?";
                answer = num1 * num2;
                break;

            case "Division":
                num1 *= num2; // Ensure clean division (no remainders)
                equation = $"{num1} ÷ {num2} = ?";
                answer = num1 / num2;
                break;

            default:
                Debug.LogError("Unsupported operation!");
                break;
        }

        return new EquationData(equation, answer);
    }

    public EquationData GetEquationAt(int index)
    {
        if (index >= 0 && index < equations.Count)
        {
            return equations[index];
        }
        else
        {
            Debug.LogError("Index out of range!");
            return default;
        }
    }
}
