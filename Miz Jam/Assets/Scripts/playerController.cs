using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 8.0f;
    public Animator anim;
    public ParticleSystem dust;
    public PlayerInput m_PlayerInput;
    public SpriteRenderer sprite;
    public AudioSource audioSource;
    public AudioClip[] swordClips;
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
    private bool isDead = false;

    private bool slashing = false;

    public int maxHP = 3;
    public int currentHP = 3;

    private float slashTime = 0.2f;

    private void Start ()
    {
        m_MoveAction = m_PlayerInput.actions["Move"];
    }

    private void Update ()
    {
        if(!rolling && !isDead)
        {
            speed = 8.0f;
            m_move = m_MoveAction.ReadValue<Vector2> ();
            if(slashing)
            {
                m_move = Vector2.zero;
            }
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
        if(currentHP <= 0)
        {
            anim.SetTrigger("death");
            isDead = true;
            rb.velocity = Vector2.zero;
            dust.Stop();
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
        if(!rolling && !isDead)
        {
            audioSource.PlayOneShot(swordClips[Random.Range(0, swordClips.Length)], Random.Range(0.7f, 1f));
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
    }

    public void OnRestart(InputValue value)
    {
        if(isDead)
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    public IEnumerator SwingSwordLeft()
    {
        sword.SetActive(false);
        slashLeft.SetActive(true);
        slashing = true;

        yield return new WaitForSeconds(slashTime);

        sword.SetActive(true);
        slashLeft.SetActive(false);
        slashing = false;
    }

    public IEnumerator SwingSwordRight()
    {
        sword.SetActive(false);
        slashRight.SetActive(true);
        slashing = true;

        yield return new WaitForSeconds(slashTime);

        sword.SetActive(true);
        slashRight.SetActive(false);
        slashing = false;
    }

    public IEnumerator SwingSwordUp()
    {
        sword.SetActive(false);
        slashUp.SetActive(true);
        slashing = true;

        yield return new WaitForSeconds(slashTime);

        sword.SetActive(true);
        slashUp.SetActive(false);
        slashing = false;
    }

    public IEnumerator SwingSwordDown()
    {
        sword.SetActive(false);
        slashDown.SetActive(true);
        slashing = true;

        yield return new WaitForSeconds(slashTime);

        sword.SetActive(true);
        slashDown.SetActive(false);
        slashing = false;
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
        if(!rolling && !isInvulnerable && !isDead)
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