using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandController : MonoBehaviour
{
    public int Speed;
    private Rigidbody2D _rigidBody;
    private int _mostLeft;
    //private bool _isLasted;
    public bool IsSteped { get; private set; }
    private GemController _gem;

    // Use this for initialization
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.velocity = Vector2.left * Speed;
        _mostLeft = -12;
        //_isLasted = false;
        //_gem = GetComponentInChildren<GemController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var astronaut = other.gameObject.GetComponentInParent<astronautController>();
        if (other.CompareTag("Head"))
        {
        }
        else if (other.CompareTag("Foot"))
        {
            astronaut.Bounce();
            Step();
        }
    }

    private void Step()
    {
        if (IsSteped) return;
        this.IsSteped = true;
        MainGameController.UpdateTotalIslandPassed();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.transform.position.x < _mostLeft)
        {
            Destroy(gameObject);
            Step();
            if (!MainGameController.TutorialDone)
            {
                FinishTutorial();
            }
        }
    }

    private void FinishTutorial()
    {
        MainGameController.TutorialDone = true;
    }
}