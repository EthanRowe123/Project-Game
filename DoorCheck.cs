using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCheck : MonoBehaviour
{
    public string key_name;
    public Animator topAnimator;
    public Animator botAnimator;

    public Collider2D collider;
    private GameManager gm;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gm = GameObject.Find("_GM").GetComponent<GameManager>();
        if (collision.tag == "Player")
        {
            if (gm.collectedKeys.Contains(key_name))
            {
                topAnimator.SetBool("IsOpen", true);
                botAnimator.SetBool("IsOpen", true);
                gm.collectedKeys.Remove(key_name);
                collider.enabled = false;
            }
        }
    }
}
