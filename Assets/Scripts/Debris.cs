using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Debris : MonoBehaviour
{
    [SerializeField] private int numberOfHits = 1;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float hitForce = 2f;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    public void Hit(Vector3 direction)
    {
        rb.isKinematic = false;
        rb.AddForce(direction * hitForce, ForceMode.Impulse);

        if (--numberOfHits <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Destroyed " + gameObject.name);
        }
            
    }
}
