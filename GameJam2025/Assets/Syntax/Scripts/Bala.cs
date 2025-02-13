using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float force = 8f;
    [SerializeField] private float daño = 20;
    private readonly float angle = Mathf.PI/4; // rad for 60grad
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        float x = Mathf.Cos(angle) * force;
        float y = Mathf.Sin(angle) * force;
        rb.AddForce(new Vector3(x, y, 0), ForceMode.Impulse);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Nerd"))
        {
            other.GetComponent<EnemyController>().TakeDamage(daño);
            Destroy(gameObject);
        }

        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
