using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventarioController : MonoBehaviour
{
    // Start is called before the first frame update
    public static InventarioController Instance { get; private set; }
    [SerializeField] private ObjetoInventario[] _inventario;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        for (int i = 0; i < _inventario.Length; i++)
        {
            _inventario[i].gameObject.SetActive(false);
        }
    }

    public void SetObjectUI(PicableTest picableTest)
    {
        
       foreach (var obj in _inventario)
       {
           if (picableTest._id == obj.ObjectID)
           {
               obj.gameObject.SetActive(true);
               obj.Cantidad++;
               Destroy(picableTest.gameObject);
               break;
           }
       }
    }

    public void UseItem(int ID)
    {
        for (int i = 0; i < _inventario.Length; i++)
        {
            if (ID == _inventario[i].ObjectID)
            {
                _inventario[i].Cantidad--;
                if (_inventario[i].Cantidad <= 0)
                {
                    _inventario[i].ObjectID = 0;
                    _inventario[i].gameObject.SetActive(false);
                }
                break;
            }
        }
    }
    
}
