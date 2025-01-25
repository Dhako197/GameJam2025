using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MovimientoTest : MonoBehaviour
{
    public float speed = 5f; 
    private Rigidbody rb;
    [SerializeField] private Transform _transform;
    [SerializeField] private Animator _animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (moveHorizontal != 0 || moveVertical != 0)
        {
            Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
            rb.velocity = (movement * speed);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }

        if (moveHorizontal < 0)
        {
            _transform.localRotation = new Quaternion(0,180,0,1);
        }
        else if(moveHorizontal > 0)
        {
            _transform.localRotation = new Quaternion(0,0,0,0);
        }
        
        _animator.SetFloat("HorMove", moveHorizontal);

       
    }
    
}
