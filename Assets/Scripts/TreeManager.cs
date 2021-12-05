using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeManager : Singleton<TreeManager>
{
    private int maxHealth = 100;
    public int health {get;set;} //현재체력

    private Slider healthBar;

    private void Start()
    {
        health = 50;
        healthBar = GameObject.FindGameObjectWithTag("TreeHealthBar").GetComponent<Slider>();
    }

    // Update is called once per frame
    private void Update()
    {
        healthBar.value = (float)health/maxHealth;
    }
}
