using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class faceEnemy : MonoBehaviour
{

    public GameObject projectile;
    public GameObject player;
    public Animator anim;
    public GameObject projectileSpawner;
    public ParticleSystem blood;
    public Rigidbody2D rb;
    public AudioSource audioSource;

    public float timeToShoot;
    private float elapsedTime;

    public float moveSpeed = 4f;
    public Vector3 moveDir = Vector3.right;

    private bool dead = false;
    private bool isActive = false;
    private bool wallCollide = true;
    public GameObject meat;



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
        if(!dead && isActive)
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

            rb.velocity = moveDir * moveSpeed;
        }
    }

    private void Shoot()
    {
        Vector2 dir = transform.position - player.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        Rigidbody2D fireball = Instantiate(projectile, projectileSpawner.transform.position, rot).GetComponent<Rigidbody2D>();
        fireball.velocity = -dir.normalized * 5;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.collider.tag == "walls" && wallCollide)
        {
            StartCoroutine(NoCollide());
            moveSpeed = -moveSpeed;
        }
        else if(other.collider.tag == "enemies" && wallCollide)
        {
            StartCoroutine(NoCollide());
            moveSpeed = -moveSpeed;
        }
        else if(other.collider.tag == "Player" && !dead)
        {
            other.gameObject.BroadcastMessage("OnHit");
        }
    }

    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(0.50f);

        Shoot();
        
    }

    public void OnHit()
    {
        dead = true;
        Vector2 dir = transform.position - player.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        blood.transform.rotation = rot;
        blood.Play();
        anim.SetTrigger("death");
        audioSource.Play();
        StartCoroutine(DestroyDelay());
    }

    private IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
        if(Random.Range(0,10) >= 8)
        {
            Instantiate(meat, transform);
        }
    }

    public void Activate()
    {
        isActive = true;
    }

    //fix for getting stuck when colliding with multiple walls
    IEnumerator NoCollide()
    {
        wallCollide = false;
        yield return new WaitForSeconds(0.01f);
        wallCollide = true;
    }

}
