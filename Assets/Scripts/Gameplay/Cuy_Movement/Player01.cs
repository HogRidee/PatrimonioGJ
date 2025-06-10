using System.Collections;
using UnityEngine;

public class Player01 : Player_Movement
{
    protected override void callDash()
    {
        if ((Input.GetButtonDown("Dash01") && this.CanDash && Time.time > this.LastDash + 0.00f))
        {
            GetComponent<AudioSource>().PlayOneShot(this.DashSound);
            StartCoroutine(Dash());
            this.LastDash = Time.time;
        }
    }
    protected override void Movement() {
        this.HorizontalMove = Input.GetAxisRaw("Horizontal01") * this.RunSpeedHorizontal;
        this.VerticalMove = Input.GetAxisRaw("Vertical01") * this.RunSpeedHorizontal;
        Vector2 _movementDirection = new Vector2(this.HorizontalMove, this.VerticalMove).normalized;
        if (_movementDirection != Vector2.zero)
        {
            this.LastInputDirection = _movementDirection;
        }
        if (this.CanMove)
            transform.position += new Vector3(this.HorizontalMove, this.VerticalMove, 0.0f) * Time.fixedDeltaTime * this.RunSpeedHorizontal;
    }
}
