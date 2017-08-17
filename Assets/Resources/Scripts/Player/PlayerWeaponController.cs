﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour {

    public GameObject playerHand;
    public GameObject EquippedWeapon { get; set; }

    Transform spawnProjectile;
    Item currentlyEquippedItem;
    IWeapon equippedWeapon;
    CharacterStats characterStats;
    InventoryController inventoryController;
    PlayerSkillController playerSkillController;

    void Start()
    {
        spawnProjectile = transform.FindChild("ProjectileSpawn");
        characterStats = GetComponent<Player>().characterStats;
        inventoryController = GetComponent<InventoryController>();
        playerSkillController = GetComponent<PlayerSkillController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!EquippedWeapon.GetComponent<Animator>().GetBool("IsLastAnimation"))
            {
                PanelSkill panel = playerSkillController.skillPanel.transform.GetChild(0).GetComponent<PanelSkill>();
                if (!(panel.cooldownRemain > 0))
                {
                    playerSkillController.usingSkillName = panel.skill.skillName;
                    panel.SkillUsed();
                    PerformSkill();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.X) && EquippedWeapon != null)
        {
            PerformWeaponAttack();
        }
    }

    public void EquipWeapon(Item itemToEquip)
    {
        UnequipWeapon();
        EquippedWeapon = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/Items/Weapons/" + itemToEquip.ItemName), playerHand.transform);
        if (EquippedWeapon.GetComponent<IProjectileWeapon>() != null)
        {
            EquippedWeapon.GetComponent<IProjectileWeapon>().ProjectileSpawn = spawnProjectile;
        }
        characterStats.AddStatBonus(itemToEquip.Stats);
        equippedWeapon = EquippedWeapon.GetComponent<IWeapon>();
        equippedWeapon.Stats = itemToEquip.Stats;
        currentlyEquippedItem = itemToEquip;
        EquippedWeapon.transform.SetParent(playerHand.transform);
        equippedWeapon.playerSkillController = playerSkillController;
        SoundDatabase.PlaySound(0);
        UIEventHandler.ItemEquipped(itemToEquip);
        UIEventHandler.StatsChanged();
    }

    public void UnequipWeapon()
    {
        if (EquippedWeapon != null)
        {
            SoundDatabase.PlaySound(0);
            characterStats.RemoveStatBonus(equippedWeapon.Stats);
            inventoryController.GiveItem(currentlyEquippedItem.ItemName);
            Destroy(playerHand.transform.GetChild(0).gameObject);
            UIEventHandler.StatsChanged();
        }
    }

    public void PerformWeaponAttack()
    {
        equippedWeapon.PerformAttack(CalculateDamage());
    }
    
    public void PerformSkill()
    {
        equippedWeapon.PerformSkillAnimation();
    }

    private int CalculateDamage()
    {
        int damageToDeal = 0;
        damageToDeal = ((characterStats.GetStat(BaseStat.BaseStatType.Physical).GetCalcStatValue() * 2) +
            Random.Range(2,8));
        damageToDeal += CalculateCrit(damageToDeal);
        return damageToDeal;
    }

    private int CalculateCrit(int damage)
    {
        if (Random.value <= .1f)
        {
            int critDamage = (int)(damage * Random.Range(.5f, .75f));
            return critDamage;
        }
        return 0;
    }
}