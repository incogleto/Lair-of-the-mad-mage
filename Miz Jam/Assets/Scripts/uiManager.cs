using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiManager : MonoBehaviour
{

    public GameObject fullHeart1;
    public GameObject fullHeart2;
    public GameObject fullHeart3;

    public playerController player;

    public Text gameOverText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player.currentHP <= 0)
        {
            gameOverText.gameObject.SetActive(true);
        }
        if(player.currentHP > 0)
        {
            fullHeart1.SetActive(true);
        }
        else
        {
            fullHeart1.SetActive(false);
        }
        if(player.currentHP > 1)
        {
            fullHeart2.SetActive(true);
        }
        else
        {
            fullHeart2.SetActive(false);
        }
        if(player.currentHP > 2)
        {
            fullHeart3.SetActive(true);
        }
        else
        {
            fullHeart3.SetActive(false);
        }
    }
}
