using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomManager : MonoBehaviour
{
    public List<GameObject> enemies;
    public List<door> doors;
    private bool enemiesPresent;
    public BoxCollider2D enterTrigger;

    public bool left;
    public bool right;
    public bool up;
    public bool down;

    public int size_x;
    public int size_y;

    public GameObject door_N_Spawn;
    public GameObject door_W_Spawn;
    public GameObject door_E_Spawn;
    public GameObject door_S_Spawn;


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
            foreach (var item in doors)
            {
                item.SendMessage("Open");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            foreach (var item in enemies)
            {
                item.BroadcastMessage("Activate");
            }
            foreach (var item in doors)
            {
                item.SendMessage("Close");
            }
            Destroy(enterTrigger);
        }
    }
}
