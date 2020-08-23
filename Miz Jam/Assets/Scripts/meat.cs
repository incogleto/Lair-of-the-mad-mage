using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meat : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
        {
            other.gameObject.BroadcastMessage("OnHeal");
            Destroy(gameObject);
        }
    }
}
