﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitbox : MonoBehaviour {

    Weapon parentWeapon;
    public float DamageMultiplier { get; set; }

    void Start()
    {
        parentWeapon = transform.parent.GetComponent<Weapon>();
    }


    
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            if (!parentWeapon.EnemiesHit.Exists(aGameObject => aGameObject == col.gameObject)
                && parentWeapon.EnemiesHit.Count < parentWeapon.pierce)
            {
                parentWeapon.EnemiesHit.Add(col.gameObject);
                Damage damage = new Damage((int)(parentWeapon.CharacterStats.Physical * parentWeapon.Animator.GetFloat("DamageMultiplier") * DamageMultiplier));

                parentWeapon.OnHit(damage);
                col.GetComponentInChildren<Enemy>().TakeDamage(damage);

                col.GetComponentInChildren<Enemy>().EnemyMovement.knockable.AddXKnockback(parentWeapon.knockback, parentWeapon.transform.parent);
                col.GetComponentInChildren<Enemy>().EnemyMovement.stun.StunnedDuration += parentWeapon.stunDuration;
            }
        }
    }
}
