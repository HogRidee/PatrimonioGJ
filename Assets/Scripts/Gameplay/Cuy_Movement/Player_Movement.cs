using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Player_Movement : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;

    [Header ("Movement")] 
    [SerializeField] private float Speed;
    [SerializeField] private AudioClip WalkingSound_00;
    [SerializeField] private AudioClip WalkingSound_01;
    [SerializeField] private AudioClip HitSound_00;
    private bool _num_Steps;

    [Header("Health")]
    [SerializeField] private int _health;
    
    [Header("Dash")]
    [SerializeField] private AudioClip _dashSound;
    [SerializeField] private float _dashingTime;
    [SerializeField] private float _dashVelocity;
    private float _lastDash;
    private bool _canMove = true;
    private bool _canDash = true;

    [Header("Joystic Config")]
    private float _horizontalMove = 0f;
    private float _verticalMove = 0f;
    [SerializeField]private float _runSpeedHorizontal = 1f;
    private Vector2 _lastInputDirection = Vector2.right;
    public TrailRenderer TrailRenderer;
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _num_Steps = false;

    }

    // Update is called once per frame
    void Update()
    {
        //Horizontal Movement
        /* Animator.SetBool("Running", _horizontalMove != 0.0f);
        if (_horizontalMove != 0.0f && Grounded)
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                if (!Num_Steps)
                    GetComponent<AudioSource>().PlayOneShot(WalkingSound_00);
                else
                    GetComponent<AudioSource>().PlayOneShot(WalkingSound_01);
                Num_Steps = !Num_Steps;
            }
        }
        */
        if (_horizontalMove < 0.0f) transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (_horizontalMove > 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        //Code to Dash
        if ((Input.GetMouseButtonDown(1) && _canDash) && Time.time > _lastDash + 0.00f)
        {
            //GetComponent<AudioSource>().PlayOneShot(_dashSound);
            StartCoroutine(Dash());
            _lastDash = Time.time;
        }
    }

    #region Dash

    public void callDash()
    {
        if (_canDash && Time.time > _lastDash + 3.00f)
        {
            GetComponent<AudioSource>().PlayOneShot(_dashSound);
            StartCoroutine(Dash());
            _lastDash = Time.time;
        }
    }
    //Player Dash
    private IEnumerator Dash()
    {
        _canMove = false;
        _canDash = false;

        Vector2 _dashDirection = _lastInputDirection.normalized;
        _rigidbody2D.linearVelocity = _dashDirection * _dashVelocity;
        TrailRenderer.emitting = true;
        yield return new WaitForSeconds(_dashingTime);

        _rigidbody2D.linearVelocity = Vector2.zero;
        TrailRenderer.emitting = false;
        _canMove = true;
        _canDash = true;
    }

    #endregion

    #region Health
    private void Death(float Time_to_Death)
    {
        //Para que el jugador no se pueda mover al morir
        Player_Movement movementScript = GetComponent<Player_Movement>();
        if (movementScript != null)
        {
            movementScript.enabled = false;
        }
        //
        _rigidbody2D.linearVelocity = Vector2.zero;  // Detener cualquier movimiento
        _rigidbody2D.gravityScale = 0;  // Evitar que la gravedad lo afecte
        GetComponent<Collider2D>().enabled = false; // Ignorar colisiones



        //_animator.SetBool("Death", true);
    }

    public int GetHealth()
    {
        return _health;
    }
    #endregion


    private void FixedUpdate()
    {
        this._horizontalMove = Input.GetAxisRaw("Horizontal") * _runSpeedHorizontal;    
        this._verticalMove = Input.GetAxisRaw("Vertical") * _runSpeedHorizontal;
        Vector2 _movementDirection = new Vector2(_horizontalMove, _verticalMove).normalized;
        if (_movementDirection != Vector2.zero) { 
            _lastInputDirection = _movementDirection;
        }
        if (_canMove)
            transform.position += new Vector3(_horizontalMove, _verticalMove, 0.0f) * Time.fixedDeltaTime * _runSpeedHorizontal;
    }




}
