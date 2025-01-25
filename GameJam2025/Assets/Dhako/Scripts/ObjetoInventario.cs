using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjetoInventario : MonoBehaviour
{
    public Image Image;
    public string NombreObjeto;
    public int Cantidad;
    public TextMeshProUGUI CantidadUI;
    public int ObjectID= 0;
    public int totalObjs;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CantidadUI.text = Cantidad + " / " + totalObjs;
    }
}
