using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoTest : MonoBehaviour
{
    public float speed = 5f; 
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
       
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
        movement.Normalize();
        //transform.position += movement * speed * Time.deltaTime;
        rb.AddForce(movement * speed);
    }
    
}
