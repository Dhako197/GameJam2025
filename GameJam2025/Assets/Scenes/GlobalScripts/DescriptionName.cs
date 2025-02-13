using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DescriptionName : MonoBehaviour
{
    private RectTransform descriptor;
    private TextMeshProUGUI descriptorText;

    private float counter = 0;
    private float rotation = 0.15f;
    private readonly float smoothness = 0.3f;
    
    void Start()
    {
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        descriptor = GameObject.FindGameObjectWithTag("Descriptor").GetComponent<RectTransform>();
        descriptorText = GameObject.FindGameObjectWithTag("Descriptor").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        counter += Time.deltaTime;
        if (counter > smoothness)
        {
            rotation *= -1;
            //descriptorText.color = rotation < 0 ? Color.red : Color.black;
            Quaternion rotationQuaternion = descriptor.rotation;
            descriptor.rotation = new Quaternion(rotationQuaternion.x, rotationQuaternion.y, rotation, rotationQuaternion.w);
            counter = 0;
            return;
        }
    }
}
