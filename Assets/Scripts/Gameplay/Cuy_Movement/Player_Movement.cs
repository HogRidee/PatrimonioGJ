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
    private SpriteRenderer _spriteRenderer;
    [Header ("Movement")] 
    [SerializeField] private float Speed;
    [SerializeField] private AudioClip WalkingSound_00;
    [SerializeField] private AudioClip WalkingSound_01;
    private bool _num_Steps;

    [Header("Health")]
    [SerializeField] private int _health;
    [SerializeField] private UIHealthDisplay _uiHealth;

    [Header("Power Ups")]
    private bool _isIntangible = false;
    private bool _isFaster = false;
    [SerializeField] private float _timerPowerUp;
    [SerializeField] private float _multiplySpeedPowerUp;
    private bool _hasPowerUp = false;

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

    private static readonly int DieHash = Animator.StringToHash("isDead");

    public Animator Animator { get => _animator; set => _animator = value; }
    public Rigidbody2D Rigidbody2D { get => _rigidbody2D; set => _rigidbody2D = value; }
    public AudioSource AudioSource { get => _audioSource; set => _audioSource = value; }
    public float Speed1 { get => Speed; set => Speed = value; }
    public AudioClip WalkingSound_001 { get => WalkingSound_00; set => WalkingSound_00 = value; }
    public AudioClip WalkingSound_011 { get => WalkingSound_01; set => WalkingSound_01 = value; }
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
    public bool HasPowerUp { get => _hasPowerUp; set => _hasPowerUp = value; }
    public bool IsIntangible { get => _isIntangible; set => _isIntangible = value; }
    public bool IsFaster { get => _isFaster; set => _isFaster = value; }

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _num_Steps = false;
        //MakeFaster();
        //MakeIntangible();
    }

    // Update is called once per frame
    void Update()
    {
        //Horizontal Movement
        _animator.SetBool("isRunning", _horizontalMove != 0f || _verticalMove != 0f);
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Projectile projectile = collider.GetComponent<Projectile>();
        if (projectile != null)
        {
            if (!_isIntangible)
            {
                TakeDamage(1);
                collider.gameObject.SetActive(false);
            }
        }
    }


    public void TakeDamage(int damage) {
        if (_isIntangible) return;
        _health -= damage;
        _uiHealth.SetLives(_health);
        Debug.Log("Vida: " + _health);
        if (_health <= 0)
        { 
            Death(5);
        }
    }
    private void Death(float Time_to_Death)
    {
        Player_Movement movementScript = GetComponent<Player_Movement>();
        if (movementScript != null)
        {
            movementScript.enabled = false;
        }

        _rigidbody2D.linearVelocity = Vector2.zero;

        GetComponent<Collider2D>().enabled = false;

        _animator.SetTrigger(DieHash);

        //StartCoroutine(DestroyAfterDelay(Time_to_Death));

       
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); 
        Destroy(gameObject); 
    }

    public int GetHealth()
    {
        return _health;
    }
    #endregion

    #region PowerUps

    public void MakePowerfull() {
        if (_hasPowerUp) return;
        int randomIndex = Random.Range(0, 2);
        switch (randomIndex) 
        { 
            case 0:
                _hasPowerUp = true;
                AddPointsScore(25);
                MakeIntangible();
                break;
            case 1:
                _hasPowerUp = true;
                AddPointsScore(50);
                MakeFaster();
                break;
            default:
                break;

        }
        
    }

    protected virtual void AddPointsScore(int n) { 
        
    }
    private void MakeIntangible() {
        
        //if (_hasPowerUp) return;
        StartCoroutine(IntangibleCoroutine(_timerPowerUp));
        
    }

    private IEnumerator IntangibleCoroutine(float timer)
    {
        _isIntangible =true;
        Color initialColor = _spriteRenderer.color;
        Color targetColor = new Color(0f, 0f, 1f, initialColor.a);
        _spriteRenderer.color = targetColor;
        Debug.Log("I´m Intangible");
        yield return new WaitForSeconds(timer);
        
        if (_isIntangible)
        {
            _spriteRenderer.color = initialColor;
            _isIntangible = false;
            _hasPowerUp=false;
            Debug.Log("I´m not Intangible anymore");
        }

    }

    private void MakeFaster() {
        
        //if (_hasPowerUp) return;
        StartCoroutine(FasterCoroutine(_timerPowerUp));
        
    }
    private IEnumerator FasterCoroutine(float timer)
    {
        _isFaster = true;
        float initialSpeed = _runSpeedHorizontal;
        Color initialColor = _spriteRenderer.color;
        Color targetColor = new Color(0f, 1f, 0f, initialColor.a);
        _runSpeedHorizontal = _runSpeedHorizontal * _multiplySpeedPowerUp;
        _spriteRenderer.color = targetColor;
        Debug.Log("I´m faster");
        yield return new WaitForSeconds(timer);
        
        _spriteRenderer.color = initialColor;
        _runSpeedHorizontal = initialSpeed;
        _isFaster=false;
        _hasPowerUp = false;
        Debug.Log("I´m not faster anymore");

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

    private void OnDestroy()
    {
        if (this.gameObject != null)
        {
            this.gameObject.SetActive(false);
        }
    }

}
