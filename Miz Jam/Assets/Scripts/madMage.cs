using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class madMage : MonoBehaviour
{
    public GameObject projectile;
    public GameObject blast;
    public ParticleSystem blood;

    public Animator anim;
    public GameObject player;

    public Rigidbody2D attackPattern1;
    public GameObject[] attack1Spawners;
    public GameObject attack2Spawners;
    public GameObject shield;
    public AudioSource audioSource;
    public AudioClip hurt;

    private bool shielded = false;
    public int health = 3;
    public bool isActive = false;
    public uiManager ui;

    // Start is called before the first frame update
    void Start()
    {
        attackPattern1.angularVelocity = 70;
        player = GameObject.FindWithTag("Player");
        ui = GameObject.FindWithTag("UI").GetComponent<uiManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator Attack1()
    {
        Coroutine attack = StartCoroutine(Attack1Fire());
        yield return new WaitForSeconds(3f);
        StopCoroutine(attack);
        shield.SetActive(false);
        shielded = false;
        StartCoroutine(BecomeVulnerable());
    }

    IEnumerator Attack1Fire()
    {
        while (true)
        {
            foreach (var spawner in attack1Spawners)
            {
                Rigidbody2D fireball = Instantiate(projectile, spawner.transform).GetComponent<Rigidbody2D>();
                fireball.velocity = -spawner.transform.up * 5;
            }
            yield return new WaitForSeconds(0.15f);
        }
    }

    IEnumerator Attack2()
    {
        Coroutine attack = StartCoroutine(Attack2Fire());
        yield return new WaitForSeconds(4f);
        StopCoroutine(attack);
        shield.SetActive(false);
        shielded = false;
        StartCoroutine(BecomeVulnerable());
    }

    IEnumerator Attack2Fire()
    {
        while(true)
        {
            Vector2 dir = transform.position - player.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
            Rigidbody2D fireball = Instantiate(blast, attack2Spawners.transform).GetComponent<Rigidbody2D>();
            fireball.velocity = -dir.normalized * 10;
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator Attack3()
    {
        Coroutine attack1 = StartCoroutine(Attack1Fire());
        Coroutine attack2 = StartCoroutine(Attack2Fire());
        yield return new WaitForSeconds(4f);
        StopCoroutine(attack1);
        StopCoroutine(attack2);
        shield.SetActive(false);
        shielded = false;
        StartCoroutine(BecomeVulnerable());
    }

    public void OnHit()
    {
        if(!shielded){
            health--;
            audioSource.PlayOneShot(hurt);
            shield.SetActive(true);
            shielded = true;
        }
        if(health <= 0)
        {
            anim.SetTrigger("death");
            Vector2 dir = transform.position - player.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
            blood.transform.rotation = rot;
            blood.Play();
            shield.SetActive(false);
            shielded = false;
            ui.SendMessage("OnVictory");
            player.SendMessage("Victory");
        }
    }

    IEnumerator BecomeVulnerable()
    {
        yield return new WaitForSeconds(3f);
        if(health > 0)
        {
            shield.SetActive(true);
            shielded = true;
            yield return new WaitForSeconds(1f);
            switch (health)
            {
                case 3: StartCoroutine(Attack1()); break;
                case 2: StartCoroutine(Attack2()); break;
                case 1: StartCoroutine(Attack3()); break;
                default: break;
            }
        }
    }

    public void Activate()
    {
        if(!isActive)
        {
            isActive = true;
            StartCoroutine(Attack1());
        }   
    }
}
