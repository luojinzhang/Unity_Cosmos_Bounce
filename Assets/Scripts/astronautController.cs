using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class astronautController : MonoBehaviour
{
    public Animator AstronautAnimator;
    public Slider EnergyBar;
    public int Thrust = 1;
    public int MaxVelocity;
    private Rigidbody2D _rigidbody2D;
    //private Collider2D _footCollider2D;
    //private Collider2D _headCollider2D;
    private int _deadPoint;
    private int _sqrMaxVelociy;
    private int _relativeBounceForce = 18;
    public AudioSource AudioFly;
    public AudioSource AudioJump;
    public AudioSource AudioGems;
    



    private enum State
    {
        IsIdle = 0,
        IsThrust = 1,
        IsFall = 2,
        IsBounce = 3,
    }

    private int _currentState = (int)State.IsIdle;

    private void Start()
    {
        this.EnergyBar.maxValue = 80;
        this.EnergyBar.value = this.EnergyBar.maxValue;
        this._rigidbody2D = this.GetComponent<Rigidbody2D>();
        _currentState = (int)State.IsFall;
        //        AstronautAnimator.SetInteger("animState", 1);
        //this._footCollider2D = this.GetComponent<Collider2D>();
        // this._headCollider2D = this.GetComponentInChildren<Collider2D>();
        this._sqrMaxVelociy = this.MaxVelocity * this.MaxVelocity;
        _deadPoint = -7;


    }

    public void Bounce()
    {
        this.EnergyBar.value = this.EnergyBar.maxValue;
        this._rigidbody2D.velocity = Vector2.zero;
        this._rigidbody2D.AddRelativeForce(Vector2.up * Thrust * _relativeBounceForce);
        this._currentState = (int)State.IsBounce;
        AudioJump.Play();
    }

    public bool IsThrust()
    {
        if (this._currentState == (int)State.IsThrust)
        {
            return true;
        }

        return false;
    }

    private void SwitchAnimation()
    {
        switch (_currentState)
        {
            case (int)State.IsIdle:
            case (int)State.IsFall:
            case (int)State.IsBounce:
                AstronautAnimator.SetInteger("animState", 1);
                break;
            case (int)State.IsThrust:
                AstronautAnimator.SetInteger("animState", 0);
                break;
        }
    }

    private void FixedUpdate()
    {
        if (this._rigidbody2D.velocity.sqrMagnitude > this._sqrMaxVelociy)
        {
            this._rigidbody2D.velocity = this._rigidbody2D.velocity.normalized * MaxVelocity;
        }

        switch (_currentState)
        {
            case (int)State.IsThrust:
                {
                    this.EnergyBar.value--;
                    this._rigidbody2D.AddForce(Vector2.up * Thrust);
                    break;
                }
            case (int)State.IsFall:
                {
                    //this.EnergyBar.value++;
                    break;
                }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Gems"))
        {

            AudioGems.Play();
        }
        //else if (other.CompareTag("Asteroids"))
        //{



        //}
        else if (other.CompareTag("Laser"))
        {
            Bounce();
        }

    }

    

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this._currentState = (int)State.IsThrust;
            AudioFly.Play();

        }
        else if (Input.GetMouseButtonUp(0) || this.EnergyBar.value <= 0)
        {
            this._currentState = (int)State.IsFall;
            AudioFly.Stop();


        }

        this.SwitchAnimation();

        if (transform.position.y <= _deadPoint)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        MainGameController.EndGame();
    }
}