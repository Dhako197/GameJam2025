using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveTo : MonoBehaviour
{
    private float limit = 15;
    private float counter = 0;
    private bool moved = false;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0f, 1.9f, -11.3f);
        transform.rotation = Quaternion.Euler(12.5f,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (!moved && counter > limit)
        {
            moved = true;
            move();
        }
    }

    void move()
    {
        //transform.position = new Vector3(2.1f, 1.9f, -11.9f);
        //transform.Rotate(new Vector3(-12.5f, -41f, 0), Space.Self);
        transform.rotation = Quaternion.Euler(0, -41f, 0);
    }
}
