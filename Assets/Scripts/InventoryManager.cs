using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("UI References")]
    public GameObject InventoryMenu;
    public GameObject toolbarUI;

    [Header("Item Slots")]
    public ItemSlot[] toolbarSlots;    // 3 slot ở toolbar
    public ItemSlot[] inventorySlots;  // Nhiều slot ở Inventory

    private bool menuActivated;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // InventoryCanvas là gốc nên được giữ lại toàn bộ
        DontDestroyOnLoad(gameObject);
    }


    void Update()
    {
        // Nhấn E để mở/tắt Inventory
        if (Input.GetKeyDown(KeyCode.E))
        {
            menuActivated = !menuActivated;
            Time.timeScale = menuActivated ? 0 : 1;

            if (InventoryMenu != null)
                InventoryMenu.SetActive(menuActivated);

            // Nếu muốn ẩn toolbar khi mở Inventory:
            // if (toolbarUI != null)
            //     toolbarUI.SetActive(!menuActivated);
        }

        // Dùng item trong Toolbar bằng phím số
        if (Input.GetKeyDown(KeyCode.Alpha1)) TryUseToolbarSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) TryUseToolbarSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) TryUseToolbarSlot(2);
    }

    private void TryUseToolbarSlot(int index)
    {
        if (index < toolbarSlots.Length && toolbarSlots[index].quantity > 0)
        {
            toolbarSlots[index].UseItem();
        }
    }

    /// <summary>
    /// Thêm item vào kho đồ. Ưu tiên thêm vào Toolbar trước, nếu đầy thì thêm vào Inventory.
    /// </summary>
    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        // Thêm vào Toolbar
        for (int i = 0; i < toolbarSlots.Length; i++)
        {
            if (!toolbarSlots[i].isFull &&
               (toolbarSlots[i].itemName == itemName || toolbarSlots[i].quantity == 0))
            {
                int leftOver = toolbarSlots[i].AddItem(itemName, quantity, itemSprite, itemDescription);
                if (leftOver <= 0) return 0;
                quantity = leftOver;
            }
        }

        // Nếu còn dư, thêm vào Inventory
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (!inventorySlots[i].isFull &&
               (inventorySlots[i].itemName == itemName || inventorySlots[i].quantity == 0))
            {
                int leftOver = inventorySlots[i].AddItem(itemName, quantity, itemSprite, itemDescription);
                if (leftOver <= 0) return 0;
                quantity = leftOver;
            }
        }

        return quantity; // Nếu không còn chỗ thì trả lại số item dư
    }

    public void DeselectAllSlots()
    {
        foreach (var slot in toolbarSlots)
            slot.thisItemSelected = false;

        foreach (var slot in inventorySlots)
            slot.thisItemSelected = false;
    }
}
