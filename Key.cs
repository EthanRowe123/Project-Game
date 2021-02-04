using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public string keyName;
    private GameManager gm;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        gm = GameObject.Find("_GM").GetComponent<GameManager>();
        if (collision.tag == "Player")
        {
            gm.collectedKeys.Add(keyName);
            Destroy(gameObject);
        }
    }
}
