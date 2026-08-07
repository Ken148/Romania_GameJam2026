using UnityEngine;
using UnityEngine.Events;

public class WirePuzzleGroup : MonoBehaviour
{
    [SerializeField] private int puzzlesRequired = 3;
    [SerializeField] private UnityEvent onAllPuzzlesSolved;

    private int solvedCount;

    public void PuzzleSolved()
    {
        solvedCount++;

        if (solvedCount >= puzzlesRequired)
            onAllPuzzlesSolved.Invoke();
    }
}