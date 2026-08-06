using UnityEngine;

public class Airduct : MonoBehaviour
{
    [SerializeField] private GameObject closedModel;
    [SerializeField] private GameObject openedModel;


    private Screw[] screws;

    private void Awake()
    {
        screws = GetComponentsInChildren<Screw>();

        closedModel.SetActive(true);
        openedModel.SetActive(false);
    }

    private void Update()
    {
        CheckScrews();
    }

    private void CheckScrews()
    {
        foreach (Screw screw in screws)
        {
            if (screw.IsScrewedIn)
                return;
        }

        closedModel.SetActive(false);
        openedModel.SetActive(true);
        enabled = false;
    }
}
