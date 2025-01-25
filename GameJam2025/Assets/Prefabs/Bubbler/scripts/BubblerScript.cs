using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblerScript : MonoBehaviour
{
    [SerializeField] public float speed = 10f;
    private bool crouching = false;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
        Crouch();
    }

    void Inspect()
    {

    }

    void Walk()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        if (horizontalMovement != 0 || verticalMovement != 0)
        {
            Vector3 movement = new Vector3(horizontalMovement, 0, verticalMovement) * speed * Time.deltaTime;
            transform.Translate(movement, Space.Self);
        }
    }

    void Crouch()
    {
        crouching = Input.GetButton("Crouch");
    }
}
