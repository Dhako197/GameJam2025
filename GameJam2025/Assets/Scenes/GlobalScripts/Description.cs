using UnityEngine;

public class Description
{
    public string description;
    public Transform transform;
    public float offset;

    public Description(string d, Transform t)
    {
        description = d;
        transform = t;
        offset = 0;
    }

    public Description(string d, Transform t, float o)
    {
        description = d;
        transform = t;
        offset = 0;
    }
}