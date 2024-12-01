using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour {

   // private astronautController _astronaut;
    public int Speed;
    public float RotateSpeed;
    private Rigidbody2D _rigidBody2D;
    private int _mostLeft;
    // Use this for initialization
    void Start ()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _rigidBody2D.angularVelocity = RotateSpeed;
        _rigidBody2D.velocity = Vector2.left * Speed;
        _mostLeft = -12;
       // _astronaut = GetComponent<astronautController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Head"))
        {
            MainGameController.EndGame();
        }
        else if (other.CompareTag("Foot"))
        {
            //_astronaut.Bounce();
        }
    }

	// Update is called once per frame
	void Update ()
    {
        if (this.gameObject.transform.position.y < _mostLeft)
        {
            Destroy(gameObject);


        }
    }
}
