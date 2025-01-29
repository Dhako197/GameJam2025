using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FollowPlayer : MonoBehaviour
{
    private Transform player; 
    public Vector3 offset; 
    public float smoothSpeed = 5f;
    private Camera mainCamera; // set this via inspector
    private float shake = 0;
    private readonly float shakeAmount = 0.025f;
    private readonly float decreaseFactor = 1.5f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        mainCamera = gameObject.GetComponent<Camera>();
    }

    private void Update()
    {
        CheckShake();
    }

    void LateUpdate()
    {
        if (player != null)
        {
            
            Vector3 desiredPosition = player.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
    }

     void CheckShake()
    {
        if (shake > 0)
        {
            Vector3 randomize = Random.insideUnitSphere * shakeAmount;
            mainCamera.transform.Translate(new Vector3(randomize.x, randomize.y, 0), Space.Self);
            shake -= Time.deltaTime * decreaseFactor;
            return;
        }
        
        shake = 0.0f;
    }

    public void ChangeTransform(Transform transform)
    {
        player = transform;
    }

    public void ResetTransform()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Shake()
    {

        shake = 0.1f;
    }
}
