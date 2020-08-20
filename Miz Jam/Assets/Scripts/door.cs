using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class door : MonoBehaviour
{
    public GameObject closed_L;
    public GameObject closed_R;

    private bool isOpen = false;


    // Start is called before the first frame update
    void Start()
    {
        Open(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.oKey.wasPressedThisFrame)
        {
            if(isOpen)
            {
                Close();
                isOpen = false;
            }
            else
            {
                Open();
                isOpen = true;
            }
        }
    }

    public void Open()
    {
        closed_L.SetActive(false);
        closed_R.SetActive(false);
    }

    public void Close()
    {
        closed_L.SetActive(true);
        closed_R.SetActive(true);
    }
}
