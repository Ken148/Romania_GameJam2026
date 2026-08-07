using UnityEngine;

public class PuzzlePlate : MonoBehaviour
{
    [SerializeField] private Puzzle puzzle;
    [SerializeField] private GlassGroup glassGroup;
    [SerializeField] private Boat boat;

    public void Interact()
    {
        puzzle.gameObject.SetActive(true);
    }


    public void PuzzleSolved()
    {
        glassGroup.SetDebrisEnabled(true);
        boat.Release();
    }
}