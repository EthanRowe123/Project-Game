using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    
    public int currentScore = 0;
    public List<string> collectedKeys;
    public float globalGravity = 0f;

    public GameObject[] coins;
    public float tester = 0.00f;
    private NetworkManager nm;
    private static GameManager _instance;

    public static GameManager Instance {  get { return _instance; } }
    // Start is called before the first frame update
    private int player =  1;
    public bool resetCoins = false;

    private void Awake()
    {

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        collectedKeys = new List<string>();
        nm = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)){
            foreach(GameObject coin in coins)
            {
                coin.SetActive(true);
            }
            resetCoins = true;
        }
    }

    public string GetPlayerName()
    {
        if(player == 1){
            player++;
            return ("Player1");
        }
        else if(player == 2)
        {
            player = 1;
            return ("Player2");
        }
        return("Jeff");
    }


}
