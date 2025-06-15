using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class Player02 : Player_Movement
{
    [SerializeField] private Score02 score02;
    protected override void callDash()
    {
        if (Input.GetButtonDown("Dash02") && this.CanDash && Time.time > this.LastDash + 0.00f)
        {
            GetComponent<AudioSource>().PlayOneShot(this.DashSound);
            StartCoroutine(Dash());
            this.LastDash = Time.time;
        }
    }
    protected override void Movement()
    {
        this.HorizontalMove = Input.GetAxisRaw("Horizontal02") * this.RunSpeedHorizontal;
        this.VerticalMove = Input.GetAxisRaw("Vertical02") * this.RunSpeedHorizontal;
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
        if (score02 != null)
            score02.AddPoints(n);
    }
}
