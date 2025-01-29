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
    public TextMeshProUGUI CantidadUI;
    public int Cantidad;
    public int ObjectID= 0;
    public int totalObjs;

    void Update()
    {
        CantidadUI.text = Cantidad + " / " + totalObjs;
    }
}
