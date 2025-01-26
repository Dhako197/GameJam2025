using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventarioController : MonoBehaviour
{
    // Start is called before the first frame update
    public static InventarioController Instance { get; private set; }
    [SerializeField] public ObjetoInventario[] _inventario;

    [Header("InfoCards")] 
    [SerializeField] private GameObject[] infocard;

    private bool isUiOpen = false;

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

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (isUiOpen)
            {
               Resume();
            }
            
        }
    }

    private void Resume()
    {
        Debug.Log("se cierra");
        foreach (var go in infocard)
        {
            go.SetActive(false);
        }

        isUiOpen = false;
        Time.timeScale = 1;
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
               CheckInfo(obj.Cantidad,picableTest._id);
               break;
           }
       }
    }

    public void SetObByID(int Id)
    {
        foreach (var obj in _inventario)
        {
            if (Id == obj.ObjectID)
            {
                obj.gameObject.SetActive(true);
                obj.Cantidad++;
                CheckInfo(obj.Cantidad, Id);
                break;
            }
        }
    }

    private void CheckInfo(int catidad, int id)
    {
        if (catidad == 1)
        { 
            switch (id)
            {
                case 1:
                    infocard[0].SetActive(true);
                    break;
                case 2:
                    infocard[1].SetActive(true);
                    break;
                case 3:
                    infocard[2].SetActive(true);
                    break;
            }

            Time.timeScale = 0;
            StartCoroutine("Cooldown");

        }
    }

    public void NoCollectionableInfo(int id)
    {
        if (isUiOpen == false)
        {
            switch (id)
            {
                case 4:
                    infocard[3].SetActive(true);
                    break;
                case 5:
                    infocard[4].SetActive(true);
                    break;
                case 6:
                    infocard[5].SetActive(true);
                    break;
            }
            Debug.Log("Entro a la info");
            Time.timeScale = 0;
            StartCoroutine("Cooldown");
        }
       
       
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSecondsRealtime(1f);
        isUiOpen = true;
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
