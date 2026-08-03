using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Debris : MonoBehaviour
{
    [SerializeField] private int numberOfHits = 3;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float hitTorque = 3f;
    [SerializeField] private float hitForce = 1.5f;
    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private float destroyDelay = 0.2f;
    private bool destroyed;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.angularDamping = 8f;
        rb.isKinematic = true;
    }

    public void Hit(Vector3 direction)
    {
        if (destroyed) return;

        rb.isKinematic = false;
        rb.AddForce(direction * hitForce, ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * hitTorque, ForceMode.Impulse);


        if (--numberOfHits <= 0)
        {
            if (destroyEffect != null)
                Instantiate(destroyEffect, transform.position, Quaternion.identity);
            destroyed = true;

            Destroy(gameObject, destroyDelay);
            Debug.Log("Destroyed " + gameObject.name);
        }
            
    }
}
