using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Head") || other.CompareTag("Foot"))
        {
            this.gameObject.SetActive(false);
            MainGameController.UpdateTotalGems();
        }
    }

    public void Reset()
    {
        this.gameObject.SetActive(true);
    }
}