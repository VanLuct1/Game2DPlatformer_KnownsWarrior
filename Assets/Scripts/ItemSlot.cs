using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    ///
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    public string itemDescription;
    public Sprite emptySprite;
    [SerializeField]
    private int maxNumberOfItems; 


    //====ITEM SLOT====//
    [SerializeField]
    private TMP_Text quantityText;
    [SerializeField]
    private Image itemImage; // Ensure this is UnityEngine.UI.Image
    //===ITEM DESCRIPTION SLOT===//
    public Image itemDescriptionImage;
    public TMP_Text ItemDescriptionNameText;
    public TMP_Text ItemDescriptionText;


    public bool thisItemSelected;

    private InventoryManager inventoryManager;
    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();

    }
    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        if (isFull)
            return quantity;

        this.itemName = itemName;
        // Check if the item already exists in the slot
        this.itemSprite = itemSprite;
        itemImage.sprite = itemSprite;
        // If the item already exists, just update the quantity
        this.itemDescription = itemDescription;
        this.quantity += quantity;
        if(this.quantity >= maxNumberOfItems)
        {
            quantityText.text = quantity.ToString();
            quantityText.enabled = true;
            isFull = true; // Mark the slot as full if it reaches max capacity
            int extraItems = this.quantity - maxNumberOfItems;
            this.quantity = maxNumberOfItems;
            return extraItems;
        }
        //
        quantityText.text = this.quantity.ToString();
        quantityText.enabled = true;
        return 0; // Return 0 if no extra items are left over
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }
    public void OnLeftClick()
    {
        inventoryManager.DeselectAllSlots();
        thisItemSelected = true;

        if (ItemDescriptionNameText != null)
            ItemDescriptionNameText.text = itemName;

        if (ItemDescriptionText != null)
            ItemDescriptionText.text = itemDescription;

        if (itemDescriptionImage != null)
        {
            itemDescriptionImage.sprite = itemSprite != null ? itemSprite : emptySprite;
        }
    }

    public void OnRightClick()
    {
        thisItemSelected = false;
   
    }
    public void UseItem()
    {
        if (itemName == "Medicine HP")
        {
            // Tìm Damageable của player để hồi máu
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Damageable damageable = player.GetComponent<Damageable>();
            if (damageable != null)
            {
                int amountToHeal = damageable.MaxHealth - damageable.Health; // hồi 100% lượng máu đã mất
                damageable.Heal(amountToHeal);
                Debug.Log("Đã hồi máu: " + amountToHeal);
            }
        }
        else if (itemName == "Medicine Mana")
        {
            PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            if (player != null)
            {
                int missingMana = player.maxMana - player.currentMana;
                player.RestoreMana(missingMana);
            }
        }

        // Trừ số lượng item
        quantity--;
        quantityText.text = quantity.ToString();

        if (quantity <= 0)
        {
            ClearSlot();
        }
    }
    public void ClearSlot()
    {
        itemName = "";
        itemDescription = "";
        itemSprite = emptySprite;
        itemImage.sprite = emptySprite;
        quantity = 0;
        quantityText.text = "";
        quantityText.enabled = false;
        isFull = false;
    }

}
