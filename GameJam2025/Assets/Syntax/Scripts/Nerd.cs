using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nerd : MonoBehaviour
{
    [SerializeField] private float vida;
    [SerializeField] private GameObject efectoMuerte;
    
    public void RecibirDaño(float daño)
    {
        vida -= daño;
        if (vida <= 0)
        {
            Morir();
        }
    }
    
    private void Morir()
    {
        Destroy(gameObject);
    }

// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
