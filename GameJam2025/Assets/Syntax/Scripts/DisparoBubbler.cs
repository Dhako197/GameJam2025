using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoBubbler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform controladorDisparo;
    [SerializeField] private GameObject prefabBala;
    [SerializeField] private int cantidadBalas;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            Debug.Log("Disparando...");
            if (cantidadBalas > 0)
            {
                Disparar();
                cantidadBalas--;
            }
            else
            {
                Debug.Log("No tienes balas...");
            }
           
        }
    }
    
    public void Disparar()
    {
        Instantiate(prefabBala, controladorDisparo.position, controladorDisparo.rotation);
    }
}
