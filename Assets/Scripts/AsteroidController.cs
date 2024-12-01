using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour {

    public int Speed;
    private Rigidbody _rigidBody;
    private int _mostLeft;
    public float tumble;
    public GameObject PlayerExplosion;

    // Use this for initialization
    void Start ()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.angularVelocity = Random.insideUnitSphere * tumble;
        _rigidBody.velocity = new Vector3(-2, -1, 0) * Speed;
        _mostLeft = -7;
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Body"))
        {
            var astronaut = other.GetComponentInParent<astronautController>();
            
            var particle = Instantiate(PlayerExplosion, other.transform.position, other.transform.rotation);
            var particleController = particle.GetComponent<ParticleController>();
            particleController.SetCallBack(EndGame);
            Destroy(astronaut.gameObject);
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

    private void EndGame()
    {
        MainGameController.EndGame();
    }
}


