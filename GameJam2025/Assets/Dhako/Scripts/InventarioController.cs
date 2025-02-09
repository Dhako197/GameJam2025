using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventarioController : MonoBehaviour
{
    public static InventarioController Instance { get; private set; }
    [SerializeField] private ObjetoInventario[] _inventario;
    private float shake = 0;
    private ObjetoInventario itemToShake = null;
    private readonly float shakeAmount = 0.025f;
    private readonly float decreaseFactor = 1.5f;

    [Header("InfoCards")] 
    [SerializeField] private GameObject[] infocard;

    private bool isUiOpen = false;
    private List<int> entered = new List<int>();

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
        if (Input.GetButtonDown("Interact") || Math.Abs(Input.GetAxis("Horizontal")) > 0.1 || Math.Abs(Input.GetAxis("Vertical")) > 0.1)
        {
            if (isUiOpen)
            {
               Resume();
            }
            
        }

        CheckShake();
    }

    private void Resume()
    {
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
               if (picableTest._id == 3)
               {
                   obj.Cantidad = 1;
               }
               else
               {    
                    obj.Cantidad++;
                    // shake
                    itemToShake = obj;
                    Shake();
               }
               
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
        if (catidad == 1 && !hasEntered(id))
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
                case 6:
                    infocard[5].SetActive(true);
                    Debug.Log("abre info pitillo");
                    break;
            }

            markEntered(id);
            Time.timeScale = 0;
            StartCoroutine("Cooldown");
        }
    }

    public void NoCollectionableInfo(InteractuableInfo interactableInfo)
    {
        int id = interactableInfo.InfoId;
        if (isUiOpen == false)
        {
            Debug.Log(isUiOpen);
            Debug.Log(id);
            switch (id)
            {
                case 4:
                    infocard[3].SetActive(true);
                    break;
                case 5:
                    infocard[4].SetActive(true);
                    break;
            }
            Debug.Log("Entro a la info");
            Time.timeScale = 0;
            StartCoroutine("Cooldown");
        }
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1;
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
    
    void markEntered(int id)
    {
        entered.Add(id);
    }

    bool hasEntered(int id)
    {
        return entered.Contains(id);
    }

    public bool HasDoorKeys()
    {
        return _inventario[2].Cantidad > 0;
    }

    public bool HasReplacementKeys()
    {
        return _inventario[1].Cantidad > 0;
    }

    public void ReplaceKeys()
    {
        UseItem(2);

        // add loader

        SetObByID(3);
    }

    void CheckShake()
    {
        if (shake > 0)
        {
            Vector3 randomize = UnityEngine.Random.insideUnitSphere * shakeAmount;
            itemToShake.transform.Translate(new Vector3(randomize.x, randomize.y, 0), Space.Self);
            shake -= Time.deltaTime * decreaseFactor;
            return;
        }

        shake = 0.0f;
    }

    public void Shake()
    {

        shake = 0.1f;
    }

    public int GetGumAmount()
    {
        return _inventario[0].Cantidad;
    }

    public int GetStrawAmount()
    {
        if (_inventario[3] != null)
        {
            return _inventario[3].Cantidad;
        }
        else return 0;

    }

    public void UseBullet()
    {
        _inventario[0].Cantidad -= 1;
    }
}
