using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DescriptionName : MonoBehaviour
{
    private RectTransform descriptor;

    private float counter = 0;
    private float rotation = 0.15f;
    private readonly float smoothness = 0.3f;
    
    void Start()
    {
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        descriptor = GameObject.FindGameObjectWithTag("Descriptor").GetComponent<RectTransform>();
    }

    private void Update()
    {
        counter += Time.deltaTime;
        if (counter > smoothness)
        {
            rotation *= -1;
            Quaternion rotationQuaternion = descriptor.rotation;
            descriptor.rotation = new Quaternion(rotationQuaternion.x, rotationQuaternion.y, rotation, rotationQuaternion.w);
            counter = 0;
            return;
        }
    }
}
