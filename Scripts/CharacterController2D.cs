using UnityEngine;
using UnityEngine.Events;
using Mirror;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterController2D : NetworkBehaviour
{
    [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
   

    const float k_GroundedRadius =  0.2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;

    public Camera camera;
    public Camera playerCam;
    public Animator animator;

    public Image key1;
    public Image key2;
    public Image key3;

    private GameManager gm;
    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;
    public Vector3 SpawnPoint;

    private Rigidbody2D rb;
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public int score = 0;

    private void Awake()
    {
        
        gm = GameObject.Find("_GM").GetComponent<GameManager>();
        name = gm.GetPlayerName();
        key1.GetComponent<Image>().color = new Color32(255, 255, 255, 100);
        key2.GetComponent<Image>().color = new Color32(255, 255, 255, 100);
        key3.GetComponent<Image>().color = new Color32(255, 255, 255, 100);
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        SpawnPoint = transform.position;
        //transform.position = SpawnPoint;
        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        rb = GetComponent<Rigidbody2D>();


    }

    private void FixedUpdate()
    {
        if (Input.GetButton("Cancel")){
            NetworkManager nm = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
            NetworkManagerHUD nmh = GameObject.Find("NetworkManager").GetComponent<NetworkManagerHUD>();
            nm.StopClient();
            Destroy(nmh);
            Destroy(nm);
            SceneManager.LoadScene("MainMenu");
        }
        GetComponentInChildren<Text>().text = "Score " + score;

        //sets the position of the players second camera towards the other play. Sets the camera to a static player if the player does not exist
        if (this.isLocalPlayer)
        {
            if (this.name == "Player1")
            {
                if (GameObject.Find("Player2") != null)
                {
                    GameObject.Find("Player2").GetComponent<Rigidbody2D>().isKinematic = true;
                    playerCam.transform.position = GameObject.Find("Player2").transform.position;
                }
                else
                {
                    playerCam.transform.position = new Vector3(-30.4552956f, -13.4052019f, -74.4514771f);
                }

            }
            else if (this.name == "Player2")
            {
                if (GameObject.Find("Player2") != null)
                {
                    GameObject player1 = GameObject.Find("Player1");
                    player1.GetComponent<Rigidbody2D>().isKinematic = true;
                    playerCam.transform.position = player1.transform.position;
                }
                else
                {
                    playerCam.transform.position = new Vector3(-30.4552956f, -13.4052019f, -74.4514771f);
                }

            }
            if(gm.resetCoins == true)
            {
                score = 0;
                gm.resetCoins = false;
            }
        }
           bool wasGrounded = m_Grounded;
            m_Grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.

            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    m_Grounded = true;
                    if (!wasGrounded)
                        OnLandEvent.Invoke();
                }
            }
            updateKey();
        
    }


    public void Move(float move, bool jump)
    {
        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {           
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        
        // If the player should jump...
        if (m_Grounded && jump)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        camera.transform.localScale = new Vector3(camera.transform.localScale.x * -1, camera.transform.localScale.y, camera.transform.localScale.z);
        playerCam.transform.localScale = new Vector3(playerCam.transform.localScale.x*-1, playerCam.transform.localScale.y, playerCam.transform.localScale.z);

        //transform.Rotate(0f, 180f, 0f);
    }

    private void Spawn()
    {
        transform.position = SpawnPoint;
        animator.SetBool("IsDead", false);
        m_Rigidbody2D.rotation = 0f;
        m_Rigidbody2D.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        
    }

    public void die(){
        animator.SetBool("IsDead", true);
        m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Death Zone")
        {
            die();
        }
    }

    public void updateKey()
    {
        switch (gm.collectedKeys.Count)
        {
            case 1:
                key1.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                break;
            case 2:
                key2.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                break;
            case 3:
                key3.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                break;
            case 0:
                key1.GetComponent<Image>().color = new Color32(255, 255, 255, 100);
                key2.GetComponent<Image>().color = new Color32(255, 255, 255, 100);
                key3.GetComponent<Image>().color = new Color32(255, 255, 255, 100);
                break;
        }
    }
}
