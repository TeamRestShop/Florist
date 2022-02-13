using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeManager : Singleton<TreeManager>
{
    private float maxHealth = 100f;
    public float health {get;set;} //현재체력

    private Slider healthBar;

    private void Start()
    {
        health = 50f;
        healthBar = GameObject.FindGameObjectWithTag("TreeHealthBar").GetComponent<Slider>();
        healthBar.value = (float)health/maxHealth;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void ChangeHealth(float value)
    {
        health = (float)Mathf.Clamp(health + value, 0, 100);
        healthBar.value = (float)health/maxHealth;
    }
}
