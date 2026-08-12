using UnityEngine;
using TMPro;

public class Note1 : MonoBehaviour
{
    [Header("Journal UI")]
    public GameObject journalUI;
    public TMP_Text journalText;

    [Header("Letter")]
    [TextArea(5, 20)]
    public string letterContent;

    [Header("Page Buttons")]
    public GameObject nextButton;
    public GameObject previousButton;

    private void Start()
    {
        journalUI.SetActive(false);

        if (nextButton != null)
            nextButton.SetActive(false);

        if (previousButton != null)
            previousButton.SetActive(false);
    }

    public void OpenLetter()
    {
        journalUI.SetActive(true);

        journalText.text = letterContent;

        journalText.ForceMeshUpdate();

        journalText.pageToDisplay = 1;

        UpdateButtons();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseLetter()
    {
        journalUI.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void NextPage()
    {
        journalText.ForceMeshUpdate();

        if (journalText.pageToDisplay < journalText.textInfo.pageCount)
        {
            journalText.pageToDisplay++;
            UpdateButtons();
        }
    }

    public void PreviousPage()
    {
        if (journalText.pageToDisplay > 1)
        {
            journalText.pageToDisplay--;
            UpdateButtons();
        }
    }

    private void UpdateButtons()
    {
        journalText.ForceMeshUpdate();

        int currentPage = journalText.pageToDisplay;
        int totalPages = journalText.textInfo.pageCount;

        Debug.Log(
            gameObject.name +
            " | Current Page: " + currentPage +
            " | Total Pages: " + totalPages
        );

        if (previousButton != null)
        {
            previousButton.SetActive(
                totalPages > 1 && currentPage > 1
            );
        }

        if (nextButton != null)
        {
            nextButton.SetActive(
                totalPages > 1 && currentPage < totalPages
            );
        }
    }
}
