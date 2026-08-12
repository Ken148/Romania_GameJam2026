using UnityEngine;
using TMPro;

public class Journal : MonoBehaviour
{
    [Header("Journal UI")]
    public GameObject journalUI;
    public TMP_Text journalText;

    [Header("Letter")]
    [TextArea(5, 20)]
    public string letterContent;

    private void Start()
    {
        journalUI.SetActive(false);
    }

    public void OpenLetter()
    {
        Debug.Log("Journal opened!");

        journalUI.SetActive(true);

        journalText.text = letterContent;

        journalText.ForceMeshUpdate();

        journalText.pageToDisplay = 1;

        Debug.Log("Number of pages: " + journalText.textInfo.pageCount);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseLetter()
    {
        Debug.Log("Journal closed!");

        journalUI.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void NextPage()
    {
        Debug.Log("Next button pressed!");

        journalText.ForceMeshUpdate();

        if (journalText.pageToDisplay < journalText.textInfo.pageCount)
        {
            journalText.pageToDisplay++;
            Debug.Log("Now on page: " + journalText.pageToDisplay);
        }
        else
        {
            Debug.Log("Already on the last page.");
        }
    }

    public void PreviousPage()
    {
        Debug.Log("Previous button pressed!");

        if (journalText.pageToDisplay > 1)
        {
            journalText.pageToDisplay--;
            Debug.Log("Now on page: " + journalText.pageToDisplay);
        }
        else
        {
            Debug.Log("Already on the first page.");
        }
    }
}