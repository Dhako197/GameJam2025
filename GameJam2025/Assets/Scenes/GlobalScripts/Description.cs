using UnityEngine;

public class Description
{
    public string description;
    public Transform transform;
    public float offsetY;
    public float offsetX;

    public Description(string description, Transform transform)
    {
        this.description = description;
        this.transform = transform;
        offsetY = 0;
        offsetX = 0;
    }

    public Description(string description, Transform transform, float offsetY)
    {
        this.description = description;
        this.transform = transform;
        this.offsetY = offsetY;
        offsetX = 0;
    }

    public Description(string description, Transform transform, float offsetY, float offsetX)
    {
        this.description = description;
        this.transform = transform;
        this.offsetY = offsetY;
        this.offsetX = offsetX;
    }
}