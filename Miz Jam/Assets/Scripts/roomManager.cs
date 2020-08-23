using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class roomManager : MonoBehaviour
{
    public List<GameObject> enemies;
    public List<door> doors;
    private bool enemiesPresent;
    private bool doorsOpen = true;
    public BoxCollider2D enterTrigger;

    public Direction connections;

    public int size_x;
    public int size_y;

    public GameObject door_N_Spawn;
    public GameObject door_W_Spawn;
    public GameObject door_E_Spawn;
    public GameObject door_S_Spawn;

    public AudioSource audioSource;
    public AudioClip openSfx;
    public AudioClip closeSfx;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enemiesPresent = false;
        foreach (var item in enemies)
        {
            
            if(item != null)
            {
                enemiesPresent = true;
            }
        }
        if(!enemiesPresent)
        {
            if(!doorsOpen)
            {
                foreach (var item in doors)
                {
                    item.SendMessage("Open");
                }
                doorsOpen = true;
                audioSource.PlayOneShot(openSfx);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            foreach (var item in enemies)
            {
                item.BroadcastMessage("Activate");
            }
            if(doorsOpen && enemiesPresent)
            {
                foreach (var item in doors)
                {
                        item.SendMessage("Close");
                }
                doorsOpen = false;
                audioSource.PlayOneShot(closeSfx);
            }
            Destroy(enterTrigger);
        }
    }
}
