using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class faceEnemy : MonoBehaviour
{

    public GameObject projectile;
    public GameObject player;
    public Animator anim;
    public GameObject projectileSpawner;

    public float timeToShoot;
    private float elapsedTime;

    public float moveSpeed = 4f;
    public Vector3 moveDir = Vector3.right;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        switch (Random.Range(0,4))
        {
            case 0: 
                moveDir = Vector3.right;
                break;
            case 1:
                moveDir = Vector3.up;
                break;
            case 2:
                moveDir = Vector3.left;
                break;
            case 3:
                moveDir = Vector3.down;
                break;
            default:
                break;
        }

        timeToShoot = Random.Range(2f,4f);
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime > timeToShoot)
        {
            anim.SetTrigger("fire");
            
            elapsedTime = 0;
            timeToShoot = Random.Range(2f,4f);
            StartCoroutine(ShootDelay());
        }
        projectileSpawner.transform.LookAt(player.transform);

        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }

    private void Shoot()
    {
        Debug.Log("shoot");
        Vector2 dir = transform.position - player.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        Rigidbody2D fireball = Instantiate(projectile, projectileSpawner.transform.position, rot).GetComponent<Rigidbody2D>();
        fireball.velocity = -dir.normalized * 5;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.collider.tag == "walls")
        {
            moveDir.x = -moveDir.x;
            moveDir.y = -moveDir.y;
        }
    }

    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(0.25f);

        Shoot();
        
    }
}
