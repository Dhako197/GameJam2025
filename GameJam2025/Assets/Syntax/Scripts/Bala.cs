using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float force = 5;
    [SerializeField] private float daño;
    [SerializeField] private float angle = 45;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        //Vector2 direction = new Vector2(force * Mathf.Cos(angle), force * Mathf.Sin(angle));
        //transform.Translate(direction * Time.deltaTime);
        rb.AddRelativeForce(Vector2.one.normalized * force, ForceMode.Impulse);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Nerd"))
        {
            other.GetComponent<Nerd>().RecibirDaño(daño);
            Destroy(gameObject);
        }

        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
