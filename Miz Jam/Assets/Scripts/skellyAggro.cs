using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skellyAggro : MonoBehaviour
{

    public skellyEnemy skelly;
    public Animator anim;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
        {
            anim.SetTrigger("aggro");
            skelly.aggro = true;
        }
    }

}
