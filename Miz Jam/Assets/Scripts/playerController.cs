﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 8.0f;
    public Animator anim;
    public ParticleSystem dust;
    public PlayerInput m_PlayerInput;
    public SpriteRenderer sprite;
    private InputAction m_MoveAction;

    public GameObject slashLeft;
    public GameObject slashRight;
    public GameObject slashUp;
    public GameObject slashDown;
    public GameObject sword;

    [SerializeField]
    private Vector2 m_move;

    private bool rolling = false;
    private bool flipped = false;

    private bool isInvulnerable = false;

    private bool slashing = false;

    public int maxHP = 3;
    public int currentHP = 3;

    private void Start ()
    {
        m_MoveAction = m_PlayerInput.actions["Move"];
    }

    private void Update ()
    {

        if (rolling)
        {
            speed = 8.0f;
            rb.velocity = m_move * speed;

        }
        else
        {
            speed = 8.0f;
            m_move = m_MoveAction.ReadValue<Vector2> ();
            rb.velocity = m_move * speed;
            anim.SetBool ("moving", m_move.magnitude > 0.1f);

            if (m_move.magnitude > 0f)
            {
                //if(!dust.isPlaying) dust.Play();
                ParticleSystem.EmissionModule em = dust.emission;
                em.enabled = true;

            }
            else
            {
                //if(dust.isPlaying) dust.Stop();
                ParticleSystem.EmissionModule em = dust.emission;
                em.enabled = false;
            }

            if (rb.velocity.x > 0 && flipped == false)
            {
                //sprite.transform.Rotate (0, 180, 0);
                anim.transform.Rotate (0,180,0);
                flipped = true;
            }
            else if (rb.velocity.x < 0 && flipped == true)
            {
                //sprite.transform.Rotate (0, 180, 0);
                anim.transform.Rotate (0,180,0);
                flipped = false;
            }
        }

    }

    public void OnMove (InputValue value)
    {
        //m_move = value.Get<Vector2>() * speed;
        //anim.SetBool("moving", context.ReadValue<Vector2>() != Vector2.zero);
    }

    public void OnDodge (InputValue value)
    {
        if (!rolling && rb.velocity.magnitude > 0.1f)
        {
            anim.SetTrigger ("rolling");
            rolling = true;
            ParticleSystem.EmissionModule em = dust.emission;
            em.enabled = false;
            StartCoroutine (rollingTimer ());
        }
    }

    public void OnSwing(InputValue value)
    {
        Vector3 dir =  Mouse.current.position.ReadValue();
        dir = Camera.main.ScreenToWorldPoint(dir);
        dir = transform.position - dir;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if(!slashing){
            if(angle >= -45 && angle < 45)
                StartCoroutine(SwingSwordLeft());
            else if(angle >= 45 && angle < 135)
                StartCoroutine(SwingSwordDown());
            else if(angle >= 135 || angle < -135)
                StartCoroutine(SwingSwordRight());
            else if(angle >= -135 && angle < -45)
                StartCoroutine(SwingSwordUp());
        }
    }

    public IEnumerator SwingSwordLeft()
    {
        sword.SetActive(false);
        slashLeft.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        sword.SetActive(true);
        slashLeft.SetActive(false);
    }

    public IEnumerator SwingSwordRight()
    {
        sword.SetActive(false);
        slashRight.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        sword.SetActive(true);
        slashRight.SetActive(false);
    }

    public IEnumerator SwingSwordUp()
    {
        sword.SetActive(false);
        slashUp.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        sword.SetActive(true);
        slashUp.SetActive(false);
    }

    public IEnumerator SwingSwordDown()
    {
        sword.SetActive(false);
        slashDown.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        sword.SetActive(true);
        slashDown.SetActive(false);
    }

    IEnumerator rollingTimer ()
    {
        yield return new WaitForSeconds (.83f);

        rolling = false;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.collider.tag == "enemies")
        {
            Debug.Log("oof");
        }
    }

    public void OnHit()
    {
        if(!rolling && !isInvulnerable)
        {
            currentHP--;
            StartCoroutine(BecomeInvulnerable());
        }
    }

    IEnumerator BecomeInvulnerable()
    {
        isInvulnerable = true;

        for(int i = 0; i < 8; i++)
        {
            if (sprite.color == Color.white)
            {
                sprite.color = Color.clear;
            }
            else
            {
                sprite.color = Color.white;
            }
 
            yield return new WaitForSeconds(.10f);
        }

        //yield return new WaitForSeconds(1.5f);
        sprite.color = Color.white;

        isInvulnerable = false;
    }
}