using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerstats_forinstance : MonoBehaviour
{

    public PlayerStats playerStats;
    public double damage;


    // Start is called before the first frame update
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();

    }

    // Update is called once per frame
    void Update()
    {
        damage = playerStats.playerDamage;


    }
}
