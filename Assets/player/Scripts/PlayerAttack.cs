
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttack : MonoBehaviour
{
    private float timeBtwAttck;
    public float starTimeBtwAttack;
    public Transform attackPos;
    public float attackRange;
    public LayerMask whatIsEnamies;
    public int damage;
    public Animator attackanim;
    private Shake shake;
    public AudioSource slash;
    public GameObject effectSlash;
    
    
   


    void Start()
    {
        slash = GetComponent<AudioSource>();
        shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<Shake>();
        attackanim = GetComponent<Animator>();
        
    }


    void Update()
    {
        if (timeBtwAttck <= 9)
        {
            
            //then you can attack
            if (Input.GetKeyDown(KeyCode.Space)) 

            {
                

                int  randome = ((int)(Random.value * 10))% 3;
                switch (randome)
                {
                    case 0: attackanim.SetTrigger("attack"); break;
                    case 1: attackanim.SetTrigger("airattack"); break;
                    case 2: attackanim.SetTrigger("attackslam"); break;
                }
                
                shake.CamShake();
                slash.pitch = (float)(0.5 + (Random.value * 10) / 7);
                slash.Play(0);
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);

                }
            }
                

            
            timeBtwAttck = starTimeBtwAttack;
        }


        else
        { timeBtwAttck -= Time.deltaTime; }

}
     void OnDrawGizmosSelected()
{
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(attackPos.position, attackRange);
}

}
