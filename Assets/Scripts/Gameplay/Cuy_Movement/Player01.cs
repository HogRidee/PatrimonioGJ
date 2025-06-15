using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player01 : Player_Movement
{
    [SerializeField] private Score01 score01;
    private Vector2 _movementInput;
    private bool _dash;

    public void OnMove(InputValue value)
    {
        _movementInput = value.Get<Vector2>();
    }

    public void OnDash(InputValue value)
    {
        if (value.isPressed)
            _dash = true;
    }

    protected override void callDash()
    {
        if (_dash && this.CanDash && Time.time > this.LastDash + 0.00f)
        {
            GetComponent<AudioSource>().PlayOneShot(this.DashSound);
            StartCoroutine(Dash());
            this.LastDash = Time.time;
        }
        _dash = false;
    }
    protected override void Movement() {
        this.HorizontalMove = _movementInput.x * this.RunSpeedHorizontal;
        this.VerticalMove = _movementInput.y * this.RunSpeedHorizontal;
        Vector2 _movementDirection = new Vector2(this.HorizontalMove, this.VerticalMove).normalized;
        if (_movementDirection != Vector2.zero)
        {
            this.LastInputDirection = _movementDirection;
        }
        if (this.CanMove)
            transform.position += new Vector3(this.HorizontalMove, this.VerticalMove, 0.0f) * Time.fixedDeltaTime * this.RunSpeedHorizontal;
    }

    protected override void AddPointsScore(int n)
    {
        if (score01 != null)
            score01.AddPoints(n);
    }
}
