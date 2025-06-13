using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Player_Movement : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private AudioSource _audioSource;
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
    private float _stepDelay = 0.5f; // Tiempo mínimo entre pasos en segundos
    private float _nextStepTime = 0f;

    public TrailRenderer TrailRenderer;

    public Animator Animator { get => _animator; set => _animator = value; }
    public Rigidbody2D Rigidbody2D { get => _rigidbody2D; set => _rigidbody2D = value; }
    public AudioSource AudioSource { get => _audioSource; set => _audioSource = value; }
    public float Speed1 { get => Speed; set => Speed = value; }
    public AudioClip WalkingSound_001 { get => WalkingSound_00; set => WalkingSound_00 = value; }
    public AudioClip WalkingSound_011 { get => WalkingSound_01; set => WalkingSound_01 = value; }
    public AudioClip HitSound_001 { get => HitSound_00; set => HitSound_00 = value; }
    public bool Num_Steps { get => _num_Steps; set => _num_Steps = value; }
    public int Health { get => _health; set => _health = value; }
    public AudioClip DashSound { get => _dashSound; set => _dashSound = value; }
    public float DashingTime { get => _dashingTime; set => _dashingTime = value; }
    public float DashVelocity { get => _dashVelocity; set => _dashVelocity = value; }
    public float LastDash { get => _lastDash; set => _lastDash = value; }
    public bool CanMove { get => _canMove; set => _canMove = value; }
    public bool CanDash { get => _canDash; set => _canDash = value; }
    public float HorizontalMove { get => _horizontalMove; set => _horizontalMove = value; }
    public float VerticalMove { get => _verticalMove; set => _verticalMove = value; }
    public float RunSpeedHorizontal { get => _runSpeedHorizontal; set => _runSpeedHorizontal = value; }
    public Vector2 LastInputDirection { get => _lastInputDirection; set => _lastInputDirection = value; }
    public float StepDelay { get => _stepDelay; set => _stepDelay = value; }
    public float NextStepTime { get => _nextStepTime; set => _nextStepTime = value; }

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _num_Steps = false;

    }

    // Update is called once per frame
    void Update()
    {
        //Horizontal Movement
        // Animator.SetBool("Running", _horizontalMove != 0.0f);
        _stepDelay = Mathf.Clamp(1f / Mathf.Abs(_horizontalMove + _verticalMove), 0.1f, 0.5f);
        if (_horizontalMove != 0.0f || _verticalMove !=0.0f)
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                if (Time.time >= _nextStepTime) 
                {
                    
                    AudioClip stepSound = _num_Steps ? WalkingSound_01 : WalkingSound_00;
                    _audioSource.PlayOneShot(stepSound);

                    
                    _num_Steps = !_num_Steps;
                    _nextStepTime = Time.time + _stepDelay;
                }
            }
        }
        
        if (_horizontalMove < 0.0f) transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        else if (_horizontalMove > 0.0f) transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

        //Code to Dash
        callDash();
    }

    #region Dash

    protected virtual void callDash()
    {
        if ((Input.GetButtonDown("Dash") &&  _canDash && Time.time > _lastDash + 0.00f))
        {
            GetComponent<AudioSource>().PlayOneShot(_dashSound);
            StartCoroutine(Dash());
            _lastDash = Time.time;
        }
    }
    //Player Dash
    protected IEnumerator Dash()
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
        Movement();
    }

    protected virtual void Movement() {
        this._horizontalMove = Input.GetAxisRaw("Horizontal") * _runSpeedHorizontal;
        this._verticalMove = Input.GetAxisRaw("Vertical") * _runSpeedHorizontal;
        Vector2 _movementDirection = new Vector2(_horizontalMove, _verticalMove).normalized;
        if (_movementDirection != Vector2.zero)
        {
            _lastInputDirection = _movementDirection;
        }
        if (_canMove)
            transform.position += new Vector3(_horizontalMove, _verticalMove, 0.0f) * Time.fixedDeltaTime * _runSpeedHorizontal;
    }


}
