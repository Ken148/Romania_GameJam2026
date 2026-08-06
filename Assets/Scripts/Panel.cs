using UnityEngine;

public class Panel : MonoBehaviour
{
    [SerializeField] private GameObject cup;
    private Rigidbody cupRb;
    [SerializeField] private Puzzle puzzle;


    private Screw[] screws;

    private void Awake()
    {
        screws = GetComponentsInChildren<Screw>();

        cupRb = cup.GetComponent<Rigidbody>();
        cupRb.isKinematic = true;
        cupRb.useGravity = false;
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

        DropCup();
        puzzle.gameObject.SetActive(true);
        enabled = false;
    }

    private void DropCup()
    {
        cup.transform.SetParent(null);

        Rigidbody rb = cup.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;

        
    }
}