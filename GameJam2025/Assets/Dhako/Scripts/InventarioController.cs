using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventarioController : MonoBehaviour
{
    public static InventarioController Instance { get; private set; }

    [SerializeField] private GameObject textCounter;
    [SerializeField] private GameObject textAdition;
    [SerializeField] private int totalGums;

    [Header("InfoCards")]
    [SerializeField] private GameObject gumnInfoCard;
    [SerializeField] private GameObject toyKeynInfoCard;
    [SerializeField] private GameObject realKeynInfoCard;
    [SerializeField] private GameObject origaminInfoCard;
    [SerializeField] private GameObject coralInfoCard;
    [SerializeField] private GameObject strawInfoCard;

    [Header("InventorySprites")]
    [SerializeField] private Sprite gumImage;
    [SerializeField] private Sprite strawImage;
    [SerializeField] private Sprite realKeysImage;
    [SerializeField] private Sprite toyKeysImage;

    private readonly List<ObjetoInventario> uiItemBox = new List<ObjetoInventario>();
    private readonly List<FoundItem> foundItems = new List<FoundItem>();
    private TextMeshProUGUI textField;
    private TextMeshProUGUI textAditionField;
    private GameObject currentInfoCard;
    private bool isUiOpen = false;
    private bool hasRealKeys = false;
    private bool needsAddition = false;

    private readonly string llaves = "llaves";
    private readonly string chicle = "chicle";
    private readonly string pitillo = "pitillo";

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
        textField = textCounter.GetComponent<TextMeshProUGUI>();
        textAditionField = textAdition.GetComponent<TextMeshProUGUI>();
        textField.enabled = false;
        currentInfoCard = gumnInfoCard;
        foreach (ObjetoInventario box in gameObject.GetComponentsInChildren<ObjetoInventario>())
        {
            uiItemBox.Add(box);
            box.gameObject.SetActive(false);
            box.uiBox.SetActive(false);
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

        CheckAddition();
    }

    private void Resume()
    {
        currentInfoCard.SetActive(false);
        isUiOpen = false;
        Time.timeScale = 1;
    }

    Sprite GetImage(string item)
    {
        if (item == llaves) { return toyKeysImage; }
        if (item == chicle) { return gumImage; }
        if (item == pitillo) { return strawImage; }
        return null;
    }

    void UpdateInventoryUI()
    {
        UpdateInventoryUI(0);
    }

    void UpdateInventoryUI(int increased)
    {
        List<FoundItem> newItems = new List<FoundItem>();
        List<int> boxInUse = new List<int>();

        for (int i = 0; i < foundItems.Count; i++)
        {
            FoundItem foundItem = foundItems[i];
            int assignedItemBox = uiItemBox.FindIndex(box => box.NombreObjeto == foundItem.itemName);

            // if item already has a box we only update amount
            if (assignedItemBox != -1)
            {
                boxInUse.Add(assignedItemBox);
                bool isGum = foundItem.itemName == chicle;

                if(isGum)
                {
                    
                    textField.text = foundItem.amount + "/" + totalGums;

                    if (increased > 0)
                    {
                        needsAddition = true;
                        Vector3 position = textCounter.transform.localPosition;
                        textAdition.transform.localPosition = new Vector3(position.x + 100, position.y + 300, position.z);
                        textAditionField.enabled = true;
                        textAditionField.text = "+" + increased;
                    }
                }
                continue;
            }

            // save new item for check
            newItems.Add(foundItems[i]);
        }

        for (int i = 0; i < uiItemBox.Count; i++)
        {
            // find empty box
            if (!boxInUse.Contains(i))
            {
                ObjetoInventario itembox = uiItemBox[i];
                
                // no new item, delete data
                if (newItems.Count == 0)
                {
                    itembox.NombreObjeto = "";
                    itembox.gameObject.SetActive(false);
                    itembox.uiBox.SetActive(false);
                    continue;
                }

                // new item in queue, assig to box
                itembox.gameObject.SetActive(true);
                itembox.uiBox.SetActive(true);

                FoundItem newItem = newItems[0];

                bool isGum = newItem.itemName == chicle;
                RectTransform boxTransform = itembox.gameObject.GetComponent<RectTransform>();
                
                itembox.NombreObjeto = newItem.itemName;
                itembox.gameObject.GetComponent<Image>().sprite = GetImage(newItem.itemName);
                boxTransform.sizeDelta = new Vector2(100, isGum ? 50 : 100);

                if (isGum)
                {
                    textField.enabled = true;
                    RectTransform rt = textCounter.GetComponent<RectTransform>();
                    Vector3 position = rt.position;
                    rt.position = new Vector3(boxTransform.position.x, position.y, position.z);
                    textField.text = newItem.amount + "/" + totalGums;
                }

                CheckInfo(newItem.itemName);
                newItems.RemoveAt(0);
            }
        }
    }

    private void CheckInfo(string itemName)
    {
        if (itemName == chicle) { currentInfoCard = gumnInfoCard; }
        if (itemName == pitillo) { currentInfoCard = strawInfoCard; }
        if (itemName == llaves) { currentInfoCard = hasRealKeys ? realKeynInfoCard : toyKeynInfoCard; }

        currentInfoCard.SetActive(true);

        StartCoroutine("Cooldown");
    }

    public void NoCollectionableInfo(InteractuableInfo interactableInfo)
    {
        int id = interactableInfo.InfoId;

        // needs change
        if (isUiOpen == false)
        {
            if (id == 4) { currentInfoCard = origaminInfoCard; }
            if (id == 5) { currentInfoCard = coralInfoCard; }

            currentInfoCard.SetActive(true);
            StartCoroutine("Cooldown");
        }
    }

    public void CheckAddition()
    {
        if (!needsAddition) { return;  }
        //Debug.Log("needsAddition: " + needsAddition);
        Vector3 diff = textAditionField.gameObject.transform.localPosition - textCounter.transform.localPosition;
        
        if (Math.Abs(diff.x) < 10 && Math.Abs(diff.y) < 10)
        {
            needsAddition = false;
            textAditionField.enabled = false;
            return;
        }

        textAdition.transform.localPosition = Vector3.MoveTowards(textAdition.transform.localPosition, textCounter.transform.localPosition, Time.deltaTime * 600f);
    }

    private IEnumerator Cooldown()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1;
        isUiOpen = true;
    }

    public bool HasDoorKeys()
    {
        return hasRealKeys;
    }

    public bool HasReplacementKeys()
    {
        return foundItems.Find(item => item.itemName == llaves) != null;
    }

    public void ReplaceKeys()
    {
        ObjetoInventario itembox = uiItemBox.Find(item => item.NombreObjeto == llaves);
        itembox.gameObject.GetComponent<Image>().sprite = realKeysImage;
        hasRealKeys = true;
        CheckInfo(llaves);
    }

    public int GetGumAmount()
    {
        FoundItem gums = foundItems.Find(item => item.itemName == chicle);
        if (gums == null) { return 0; }

        return gums.amount;
    }

    public bool HasStraw()
    {
        return foundItems.Find(item => item.itemName == pitillo) != null;
    }

    public void UseBullet()
    {
        int gumsBoxIndex = foundItems.FindIndex(item => item.itemName == chicle);
        if (gumsBoxIndex == -1) { return; }
        foundItems[gumsBoxIndex].amount -= 1;
        UpdateInventoryUI();
    }

    public void AddItem(FoundItem foundItem)
    {
        int itemIndex = foundItems.FindIndex(item => item.itemName == foundItem.itemName);
        if (itemIndex == -1)
        {
            foundItems.Add(foundItem);
            UpdateInventoryUI();
            return;
        }

        foundItems[itemIndex].amount += foundItem.amount;
        UpdateInventoryUI(foundItem.amount);
    }

    public void RemoveItem(FoundItem foundItem)
    {
        RemoveItem(foundItem, false);
    }

    public void RemoveItem(FoundItem foundItem, bool isReplace)
    {
        foundItems.Remove(foundItem);

        if (!isReplace)
        {
            UpdateInventoryUI();
            return;
        }
    }
}
