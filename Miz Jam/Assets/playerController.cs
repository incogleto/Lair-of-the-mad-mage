using System.Collections;
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

    [SerializeField]
    private Vector2 m_move;

    private bool rolling = false;
    private bool flipped = false;

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
            if (rb.velocity.x > 0 && flipped == false)
            {
                anim.transform.Rotate (0, 180, 0);
                flipped = true;
            }
            else if (rb.velocity.x < 0 && flipped == true)
            {
                anim.transform.Rotate (0, 180, 0);
                flipped = false;
            }
            anim.SetTrigger ("rolling");
            rolling = true;
            ParticleSystem.EmissionModule em = dust.emission;
            em.enabled = false;
            StartCoroutine (rollingTimer ());
        }

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
}