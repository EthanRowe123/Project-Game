using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Coin : MonoBehaviour
{
    private Text scoreText;

    public int scoreval = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {  
        if (collision.tag == "Player")
        {
            collision.GetComponent<CharacterController2D>().score += scoreval;
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }
}
