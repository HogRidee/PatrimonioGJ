using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player01 : Player_Movement
{
    [SerializeField] private Score01 score01;

    private enum ControlMethod { Keyboard, Joystick }
    private ControlMethod currentControlMethod = ControlMethod.Keyboard;

    protected override void callDash()
    {
        bool dashPressed = false;

        if (currentControlMethod == ControlMethod.Keyboard)
            dashPressed = Input.GetButtonDown("Dash01");
        else if (currentControlMethod == ControlMethod.Joystick)
            dashPressed = Input.GetButtonDown("Dash01_Joystic");

        if (dashPressed && this.CanDash && Time.time > this.LastDash)
        {
            GetComponent<AudioSource>().PlayOneShot(this.DashSound);
            StartCoroutine(Dash());
            this.LastDash = Time.time;
        }
    }

    protected override void Movement()
    {
        float hKey = Input.GetAxisRaw("Horizontal01_Keyboard");
        float vKey = Input.GetAxisRaw("Vertical01_Keyboard");
        float hJoy = Input.GetAxisRaw("Horizontal01_Joystick");
        float vJoy = Input.GetAxisRaw("Vertical01_Joystick");

        // Detectar qué se usó recientemente
        if (Mathf.Abs(hKey) > 0.1f || Mathf.Abs(vKey) > 0.1f)
            currentControlMethod = ControlMethod.Keyboard;
        else if (Mathf.Abs(hJoy) > 0.1f || Mathf.Abs(vJoy) > 0.1f)
            currentControlMethod = ControlMethod.Joystick;

        // Zona muerta para joystick
        if (Mathf.Abs(hJoy) < 0.2f) hJoy = 0;
        if (Mathf.Abs(vJoy) < 0.2f) vJoy = 0;
        vJoy *= -1; // invertir eje vertical del joystick

        float h = 0f, v = 0f;

        if (currentControlMethod == ControlMethod.Keyboard)
        {
            h = hKey;
            v = vKey;
        }
        else if (currentControlMethod == ControlMethod.Joystick)
        {
            h = hJoy;
            v = vJoy;
        }

        this.HorizontalMove = Mathf.Clamp(h, -1f, 1f) * this.RunSpeedHorizontal;
        this.VerticalMove = Mathf.Clamp(v, -1f, 1f) * this.RunSpeedHorizontal;

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
