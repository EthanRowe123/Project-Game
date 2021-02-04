using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
public class GravityChanger : NetworkBehaviour
{
    public Rigidbody2D rb;
    public float gravChange = 0.01f;

    public float maxGravity;
    public float minGravity;

    public GravityBar gravityBar;

    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("_GM").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        gravityBar.SetGravityValues(minGravity, maxGravity);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            float grav = 0;
            if (Input.GetButton("Fire1"))
            {
                grav = gravChange * -1;
            }
            if (Input.GetButton("Fire2"))
            {
                grav = gravChange;
            }
            CmdChangeGrav(grav);
        }        
    }
    
    [Command]
    public void CmdChangeGrav(float num)
    {
        if (isServer)
        {
            gm.globalGravity += num;
            gm.globalGravity = Mathf.Clamp(gm.globalGravity, minGravity, maxGravity);

            RpcSyncVarWithClients(gm.globalGravity);
        }
    }

    [ClientRpc]
    void RpcSyncVarWithClients(float num)
    {
        if (isLocalPlayer)
        {
            gm.globalGravity = num;            
            rb.gravityScale = gm.globalGravity;
            gravityBar.setGravity(gm.globalGravity);      
        }
    }
}
