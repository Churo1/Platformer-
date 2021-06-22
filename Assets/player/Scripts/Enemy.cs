using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
public float health = 200f;

    public static RipplePostProcessor camRipple;
    public Animator deadanimation;
    public AudioSource deadaudio;
    public AudioSource hitAudio;
    public bool alive = true;
    private Shake shake;
    public GameObject effect;
    public SpriteRenderer[] bodyparts;
    public Color hurtColor;

    public  void TakeDamage(float damage)
    {


        health -= damage;
        //hitAudio.Play();
        

        if (health <= 0 && alive)
        {
            Instantiate(effect, transform.position, Quaternion.identity);
            alive = false;
            camRipple.RippleEffect();
            deadaudio.pitch = (float)(0.5 + (Random.value * 10) / 7);
            
            deadaudio.Play(0);
            deadanimation.SetTrigger("Dead");
            StartCoroutine(Flash());

            Destroy(gameObject,2.8f);

        }
        
        



    }
    IEnumerator Flash()
    {
        for (int i = 0; i < bodyparts.Length; i++)
        {
            bodyparts[i].color = hurtColor;
        }
        yield return new WaitForSeconds(0.05f);
        for (int i = 0; i < bodyparts.Length; i++)
        {
            bodyparts[i].color = Color.white;
        }
    }

    void Start()
    {
        hitAudio = GetComponent<AudioSource>();
        deadaudio = GetComponent<AudioSource>();
        shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<Shake>();
        deadanimation = GetComponent<Animator>();
        camRipple = Camera.main.GetComponent<RipplePostProcessor>();
    }

    void Update()
    {
        


        
    }
     
}
