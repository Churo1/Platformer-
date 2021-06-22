using UnityEngine;
using System.Collections;

public class EnamyScript : MonoBehaviour
{
    public float speed;
    private Animator animvar;
    private Transform target;
    public GameObject effect;
    public float distance;
    private bool movingRight = true;
    public Transform groundedDetection;
    public float moveInput;
    public bool facingRight = true;

    void Start()
    {
        Flip();
        animvar = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(target.position, transform.position) < 20)
        {
            //transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, transform.position.y, transform.position.z), speed * Time.deltaTime);
            if (target.position.x > transform.position.x && !facingRight) //if the target is to the right of enemy and the enemy is not facing right
                Flip();
            if (target.position.x < transform.position.x && facingRight)
                Flip();
        }
        else
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        RaycastHit2D groundInfo = Physics2D.Raycast(groundedDetection.position, Vector2.down, distance);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("danger"))
        {
            Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if (col.gameObject.tag.Equals("Player"))
        {
            Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        facingRight = !facingRight;
    }
}