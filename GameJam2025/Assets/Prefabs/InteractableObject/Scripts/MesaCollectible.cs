using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MesaCollectible : MonoBehaviour, IInteractable
{

    private int count = 1;
    private float cooldown= 5;
    private bool startCooldown = false;
    private int id;

    [SerializeField] private GameObject PicableObjPrefab;
    // Start is called before the first frame update
    void Start()
    {
        id = System.Guid.NewGuid().GetHashCode();
    }

    void Update()
    {
        if (startCooldown) cooldown -= 1 * Time.deltaTime;
        if (cooldown < 0) startCooldown = false;
    }

    public string GetAction()
    {
        if (count >= 1)
        {
            string action = "Tomar";
            return action;
        }
        if (cooldown <= 0)
        {
            string action2 = "Inspeccionar";
            return action2;
        }
        return null;
       
    }

    public GameObject GetObject()
    {
        if (count >=1)
        {
            count--;
            startCooldown = true;
            return PicableObjPrefab;
        }
        else return null;
    }

    public int GetId()
    {
        return id;
    }

    public Description GetDescription()
    {
        return new Description("Mesa", transform, 1);
    }
}
