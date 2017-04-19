﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Status {
    public string statusName;
    public int statusID = -1;
    //public Sprite statusIMG;
    public int statusChance;

    public Status(string name, int id)
    {
        statusName = name;
        //statusIMG = Resources.Load<Sprite>("StatusEffects/" + name);
        statusID = id;
        statusChance = 0;
    }

    public Status()
    {
        statusID = -1;
    }
}