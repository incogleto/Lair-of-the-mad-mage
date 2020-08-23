using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skellyEnemy : MonoBehaviour
{
    public ParticleSystem blood;
    public GameObject player;
    public Animator anim;
    public Rigidbody2D rb;
    public AudioSource audioSource;

    public bool dead = false;
    public float speed = 10.0f;
    public bool aggro = false;
    public bool isActive = false;
    public GameObject meat;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(!dead && aggro && isActive)
        {
            //transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime );
            //rb.MovePosition(Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime ));
            Vector2 dir = player.transform.position - transform.position;
            rb.velocity = dir.normalized * speed;
        }
    }

    public void OnHit()
    {
        dead = true;
        Vector2 dir = transform.position - player.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        blood.transform.rotation = rot;
        anim.SetTrigger("death");
        blood.Play();
        audioSource.Play();
        StartCoroutine(DestroyDelay());
    }

    private IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log(Random.Range(0,10));
        if(Random.Range(0,10) >= 8)
        {
            Instantiate(meat, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.collider.tag == "Player" && !dead)
        {
            other.gameObject.BroadcastMessage("OnHit");
        }
    }

    public void Activate()
    {
        isActive = true;
    }
}
