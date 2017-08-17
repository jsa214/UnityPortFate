﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class InventoryController : MonoBehaviour {

    public static InventoryController Instance { get; set; }
    public ConsumableController consumableController;
    public PlayerWeaponController playerWeaponController;
    public PlayerArmorController playerArmorController;
    public InventoryUIDetails inventoryDetailsPanel;

    public List<Item> playerItems = new List<Item>();

    void Start()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        consumableController = GetComponent<ConsumableController>();
        playerWeaponController = GetComponent<PlayerWeaponController>();
        playerArmorController = GetComponent<PlayerArmorController>();
        GiveItem("Test Sword");
        GiveItem("Log Potion");
        GiveItem("Longsword");
        GiveItem("Wooden Staff");
        GiveItem("Leather Hat");
        GiveItem("Leather Gloves");
        GiveItem("Strength Necklace");

    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.V))
    //    {
    //        playerWeaponController.EquipWeapon(new Item(new List<BaseStat>(), "staff"));
    //    }
    //}

    public void GiveItem(string itemName)
    {
        Item item = ItemDatabase.Instance.GetItem(itemName);
        playerItems.Add(item);
        UIEventHandler.ItemAddedToInventory(item);
    }

    public void GiveItem(Item item)
    {
        playerItems.Add(item);
        UIEventHandler.ItemAddedToInventory(item);

    }

    public void SetUnequipItemDetails(Item item, Button selectedButton, GameObject gameobj)
    {
        inventoryDetailsPanel.SetUnequipItem(item, selectedButton, gameobj);
    }

    public void SetItemDetails(Item item, Button selectedButton)
    {
        inventoryDetailsPanel.SetItem(item, selectedButton);
    }

    public void EquipWeapon(Item itemToEquip)
    {
        playerWeaponController.EquipWeapon(itemToEquip);
    }

    public void ConsumeItem(Item itemToConsume)
    {
        consumableController.ConsumeItem(itemToConsume);
    }

    public void EquipArmor(Item itemToEquip)
    {
        playerArmorController.EquipArmor(itemToEquip);
    }
}