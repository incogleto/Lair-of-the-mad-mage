using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trap : MonoBehaviour
{

    public GameObject projectile;
    public GameObject projectileSpawner;

    public float timeToShoot;
    private float elapsedTime;
    private bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime > timeToShoot)
            {            
                elapsedTime = 0;
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        Rigidbody2D fireball = Instantiate(projectile, projectileSpawner.transform).GetComponent<Rigidbody2D>();
        fireball.velocity = -transform.up    * 5;
    }

    public void Activate()
    {
        isActive = true;
        Shoot();
        elapsedTime = 0;
    }

    private void Deactivate()
    {
        isActive = false;
    }


}
