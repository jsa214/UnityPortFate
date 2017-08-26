﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI    ;

public class PlayerArmorController : MonoBehaviour {

    public GameObject playerArmor;
    Player player;
    CharacterStats characterStats;
    InventoryController inventoryController;


    void Awake ()
    {
        player = GetComponent<Player>();
        characterStats = GetComponent<Player>().characterStats;
        inventoryController = GetComponent<InventoryController>();
    }

    public void EquipArmor(Item itemToEquip)
    {
        SoundDatabase.PlaySound(0);
        foreach (Transform item in playerArmor.transform)
        {
            Item checkItem = item.GetComponent<ItemHolder>().item;
            if (checkItem.ItemType == Item.ItemTypes.Weapon)
            {
                UnequipArmor(checkItem);
                Destroy(item.gameObject);
            }
            else if (checkItem.ItemType == Item.ItemTypes.Armor)
            {
                if (checkItem.ArmorType == itemToEquip.ArmorType)
                {
                    UnequipArmor(checkItem);
                    Destroy(item.gameObject);
                }
            }
        }
        GameObject armor = new GameObject();
        armor.AddComponent<ItemHolder>();
        armor.GetComponent<ItemHolder>().item = itemToEquip;
        armor.AddComponent<SpriteRenderer>();
        armor.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Icons/PlayerEquips/" + itemToEquip.ItemName);
        armor.GetComponent<SpriteRenderer>().sortingOrder = 10;
        armor.name = itemToEquip.ItemName;
        armor.transform.SetParent(playerArmor.transform);
        armor.transform.localPosition = new Vector3(0, 0, 0);
        armor.transform.localScale = new Vector3(1, 1, 1);
        //
        characterStats.AddStatBonus(itemToEquip.Stats);
        //equippedWeapon = EquippedWeapon.GetComponent<IArmor>();
        itemToEquip.Stats = itemToEquip.Stats;
        UIEventHandler.ItemEquipped(itemToEquip);
        UIEventHandler.StatsChanged();
    }

    public void UnequipArmor(Item item)
    {
        SoundDatabase.PlaySound(0);

        characterStats.RemoveStatBonus(item.Stats);
        inventoryController.GiveItem(item);
        Destroy(playerArmor.transform.FindChild(item.ItemName).gameObject);
        UIEventHandler.ItemUnequipped(item);
        UIEventHandler.StatsChanged();
        SoundDatabase.PlaySound(0);
    }


}