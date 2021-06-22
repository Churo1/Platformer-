
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;




[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {
    public float WalkSpeed;
    public float JumpForce;
    public AnimationClip _walk, _jump;
    public Animation _Legs;
    public Transform _Blade, _GroundCast;
    public Camera cam;
    public bool mirror;
    public AudioSource jumpsound;
    public AudioSource footsound;
    [Header("Dashing")]
    public bool canDash = true;
    public float dashTime;
    public float dashSpeed;
    public float TimeBTWDashes;
    public float dashJumpIncrease;
    public AudioSource dashSound;
    public PostProcessVolume Volume;
    private ChromaticAberration chroma;
    public GameObject effect;
    public GameObject dashEffect;
    public Animator dashanim;






    private bool _canJump, _canWalk;
    public bool _isWalk, _isJump;
    private float rot, _startScale;
    private Rigidbody2D rig;
    private Vector2 _inputAxis;
    private RaycastHit2D _hit;
    private Shake shake;
    public bool isJump => _isJump;
    



     void Start()
    {

        dashanim = GetComponent<Animator>();
       
        AudioSource[] soundEffects = GetComponents<AudioSource>();
        dashSound = soundEffects[0];
        jumpsound = soundEffects[1];

        //footsound = GetComponent<AudioSource>();  MUST BE COMPLETED :D
        
        rig = gameObject.GetComponent<Rigidbody2D>();
        _startScale = transform.localScale.x;
        shake = GameObject.FindGameObjectWithTag("camerShake").GetComponent<Shake>();
        //camRipple = Camera.main.GetComponent<RipplePostProcessor>();
    }

    void Update()
    {
        if (_hit = Physics2D.Linecast(new Vector2(_GroundCast.position.x, _GroundCast.position.y + 0.2f), _GroundCast.position))
        {


            if (!_hit.transform.CompareTag("Player"))
            {




                _canJump = true;
                _canWalk = true;
                

            }

        }
        else _canJump = false;

        _inputAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (_inputAxis.y > 0 && _canJump)
        {

            jumpsound.pitch = (float)(0.5 + (Random.value * 10) / 7);
            Instantiate(effect, transform.position, Quaternion.identity);


            jumpsound.Play(0);
            _canWalk = false;
            _isJump = true;

        }




    }

    void FixedUpdate()
    {
        if (_canWalk)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                DashAbility();
                dashSound.pitch = (float)(0.5 + (Random.value * 20) / 9);
                dashSound.Play();
                //shake.CamShake();
                dashanim.SetTrigger("dashanim");
                Enemy.camRipple.RippleEffect();
                





            }
        }

        Vector3 dir = cam.ScreenToWorldPoint(Input.mousePosition) - _Blade.transform.position;
        dir.Normalize();

        if (cam.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x + 0.2f)
            mirror = false;
        if (cam.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x - 0.2f)
            mirror = true;

        if (!mirror)
        {
            rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.localScale = new Vector3(_startScale, _startScale, 1);
            _Blade.transform.rotation = Quaternion.AngleAxis(rot, Vector3.forward);
        }
        if (mirror)
        {
            rot = Mathf.Atan2(-dir.y, -dir.x) * Mathf.Rad2Deg;
            transform.localScale = new Vector3(-_startScale, _startScale, 1);
            _Blade.transform.rotation = Quaternion.AngleAxis(rot, Vector3.forward);
        }

        if (_inputAxis.x != 0)
        {
            rig.velocity = new Vector2(_inputAxis.x * WalkSpeed * Time.deltaTime, rig.velocity.y);

            if (_canWalk)
            {
                _Legs.clip = _walk;
                _Legs.Play();
            }
        }

        else
        {
            rig.velocity = new Vector2(0, rig.velocity.y);
        }

        if (_isJump)
        {
            rig.AddForce(new Vector2(0, JumpForce));
            _Legs.clip = _jump;
            _Legs.Play();
            _canJump = false;
            _isJump = false;
        }
    }

    public bool IsMirror()
    {
        return mirror;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, _GroundCast.position);
    }

    void DashAbility()
    {
        StartCoroutine(Dash());
    }

    IEnumerator Dash()
    {
        canDash = false;
        WalkSpeed = dashSpeed;
        JumpForce = dashJumpIncrease;
         Instantiate(dashEffect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(dashTime);
        WalkSpeed = 500;
        JumpForce = 500f;
        yield return new WaitForSeconds(TimeBTWDashes);
        canDash = true;


    }


}
