﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Item, IHasHitbox
{
    public Attributes UserStats { get; set; }
    public Animator Animator { get; set; }
    //public List<BaseStat> Stats { get; set; }
    //public PlayerSkillController playerSkillController { get; set; }
    public float Knockback { get; set; }
    public float StunDuration { get; set; }

    // IHasHitbox
    public List<Entity> EntitiesHit { get; set; } = new List<Entity>();
    public int Pierce { get; set; }
    public int Damage { get { return (int)(UserStats.Physical * Animator.GetFloat("DamageMultiplier")); } }
    public int HitChance { get { return UserStats.Hit; } }

    protected List<DamagingHitbox> Hitboxes { get; set; } = new List<DamagingHitbox>();

    int collideSoundID = -1;

    public enum WeaponTypes
    {
        Sword,
        Axe,
        Dagger,
        Wand,
        Staff
    }

    protected override void Awake()
    {
        base.Awake();
        Animator = GetComponent<Animator>();
        Pierce = 2;
        ResetEntitiesHit();
    }

    public virtual void SetAttackSpeed(float speed)
    {
        Animator.SetFloat("AttackSpeed", speed);
    }

    public virtual void SetDamageMultiplier(float dmg)
    {
        Animator.SetFloat("DamageMultiplier", dmg);
    }

    public virtual void PerformAttack()
    {
        if (!Animator.GetBool("IsLastAnimation"))
        {
            //CurrentDamage = damage;
            //Debug.Log("damage dealt: " + damage);
            Animator.SetTrigger("Basic Attack");
        }
    }

    public virtual void PerformDashAttack()
    {
        if (!Animator.GetBool("IsLastAnimation"))
        {
            Animator.SetTrigger("Dash Attack");
        }
    }

    public virtual void PerformSkillAnimation()
    {
        if (!Animator.GetBool("IsLastAnimation"))
        {
            Animator.SetTrigger("Skill");
        }
    }

    public virtual void PerformChannelAnimation(Skill skill)
    {
        if (!Animator.GetBool("IsLastAnimation"))
        {
            ChannelBarController.Instance.MakeChannelBar(skill.skillName, skill.skillChannelDuration);
            Animator.SetTrigger("Channel");
            Animator.SetFloat("ChannelTime", 1 / skill.skillChannelDuration);
        }
    }

    public virtual void ActivateHitbox(bool yes)
    {
        Hitboxes.ForEach(hitbox => hitbox.gameObject.SetActive(yes));
    }

    // Animation Events
    public virtual void IsLastAnimation()
    {
        Animator.SetBool("IsLastAnimation", true);
    }

    public virtual void EndLastAnimation()
    {
        Animator.SetBool("IsLastAnimation", false);
    }

    public virtual void PlayerCantMove()
    {
        PlayerMovement.cantMove = true;
    }

    public virtual void PlayerCanMove()
    {
        PlayerMovement.cantMove = false;
    }

    public virtual void ActivateSkill()
    {
        //playerSkillController.ActivateSkill(playerSkillController.UsingSkill);
    }

    public void SetCollideSound(int id)
    {
        collideSoundID = id;
    }

    public virtual void OnHitEffects(Damage dmg)
    {
        if (dmg.DidCrit)
            SoundDatabase.PlaySound(11);
        else
            SoundDatabase.PlaySound(collideSoundID);
    }

    public void ResetEntitiesHit()
    {
        EntitiesHit.Clear();
    }

}
