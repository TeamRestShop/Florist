using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class TreeManager : Singleton<TreeManager>, IPunObservable 
{
    private float maxHealth = 100f;
    public float health {get;set;} //현재체력

    private Slider healthBar;

    private void Start()
    {
        health = maxHealth;
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
        
        if(health <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(health);
        }
        else
        {
            // Network player, receive data
            health = (float)stream.ReceiveNext();
            ChangeHealth(health);
        }
    }
}
