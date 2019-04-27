using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour {
    ConnectionManager cManager;
    MessageQueue msgQueue;
    public List<GameObject> players = new List<GameObject>();
    private GameObject player;

    bool player1Ready = false;
    bool player2Ready = false;

    void Awake() {
        //DontDestroyOnLoad(gameObject);
        NetworkRequestTable.init();
        NetworkResponseTable.init();
	}
	
	// Use this for initialization
	void Start () {

        
        cManager = gameObject.GetComponent<ConnectionManager>();
        msgQueue = gameObject.GetComponent<MessageQueue>();
        msgQueue.AddCallback(Constants.SMSG_AUTH, ResponseCreate);
        msgQueue.AddCallback(Constants.SMSG_MOVE, ResponseMove);
        msgQueue.AddCallback(Constants.SMSG_READY, ResponseReady);
        msgQueue.AddCallback(Constants.SMSG_START, ResponseStart);
        msgQueue.AddCallback(Constants.SMSG_UNREADY, ResponseUnready);
        //msgQueue.AddCallback(Constants.SMSG_TEST, responseTest);

        Debug.Log("Starting Coroutine");
		StartCoroutine(RequestHeartbeat(1f));
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Function to be called after processing a message.
    // Function callbacks used to show what is received or what happens to the client
    // after the request is done processing from the connection manager. 
    public void ResponseCreate(ExtendedEventArgs eventArgs)
    {
        ResponseCreateEventArgs argID = eventArgs as ResponseCreateEventArgs;
        if (players.Count >= 2)
        {
           foreach(GameObject player in players)
            {
                Destroy(player);
            }
            players = new List<GameObject>();

        }
        player = spawnHere(eventArgs);
        player.tag = argID.user_id.ToString();
        players.Add(player);
            
        if (players.Count == 1)
        {
            // Activates the ready screen for player
            GameObject readyScreen = player.transform.GetChild(1).gameObject;
            readyScreen.transform.GetChild(3).gameObject.SetActive(true);
            readyScreen.transform.GetChild(4).gameObject.SetActive(true);

            Debug.Log("Added first player in list.");
        }
        
        // Turning off components and game objects for the other player. 
        if(players.Count > 1)
        {
            Debug.Log("Adding subsequent players that join.");

            // Player Object is a child of the Player Spawn Game Object.
            GameObject playerObject = player.transform.GetChild(0).gameObject;

            // Turn off canvas of other player
            player.transform.GetChild(1).gameObject.SetActive(false);

            // Turn off all children associated with the new player object that joins.
            for (int i = 0; i < playerObject.transform.childCount; i++)
            {
                // The rest of the children is within this player. 
                GameObject child = playerObject.transform.GetChild(i).gameObject;

                // Turning off children of player. 
                if (child != null)
                    child.SetActive(false);
            }

        
            // Turn off all movement and camera objects so that one input doesn't move both player objects.
            playerObject.GetComponent<FPMovement>().enabled = false;
            playerObject.GetComponent<CharacterController>().enabled = false;
            playerObject.GetComponent<MouseLook>().enabled = false;
            

        }
        Debug.Log("Successfully created player in callback function.");
    }

    // Spawn players in different areas of the room based on their tag.
    public GameObject spawnHere(ExtendedEventArgs eventArgs)
    {
        GameObject spawn = null;
        // if eventArgs.playertag == 1 or eventArgs.playertag == 2 to tell them to spawn in different areas
        ResponseCreateEventArgs argID = eventArgs as ResponseCreateEventArgs;
        if (argID.user_id == 1)
        {
            spawn = Instantiate(Resources.Load<GameObject>("Prefabs/Player1Spawn"));
            
        }
        if (argID.user_id == 2)
        {
            spawn = Instantiate(Resources.Load<GameObject>("Prefabs/Player2Spawn"));
        }
        return spawn;
    }

    public void ResponseMove(ExtendedEventArgs eventArgs)
    {
        
        ResponseMoveEventArgs argTag = eventArgs as ResponseMoveEventArgs;

        foreach (GameObject eachPlayer in players)
        {
            if(eachPlayer.tag == argTag.clientTag.ToString())
            {
                // Previous player position
                Transform previous = eachPlayer.transform;

               // Lerp for smoother player movement from the server.
                eachPlayer.transform.position = Vector3.Lerp(previous.position, 
                                                eachPlayer.transform.position = new Vector3(argTag.posX, 2, argTag.posZ), 
                                                Time.deltaTime * 12);
            }
        }

        Debug.Log("Call back for moving.");
       
    }

    
    public void RequestReady()
    {
        player = players[0];
        int readyPlayer = int.Parse(player.tag);
        RequestReady ready = new RequestReady();
        ready.send(readyPlayer);
        cManager.send(ready);
        Debug.Log("Sent ready request");
    }

    public void ResponseReady(ExtendedEventArgs eventArgs)
    {
        ResponseReadyEventArgs args = eventArgs as ResponseReadyEventArgs;
        // player[0] represents the the current client
        player = players[0];
        // Ready screen 
        GameObject readyScreen = player.transform.GetChild(1).gameObject;

        if (args.readyPlayer == 1)
        {
            Debug.Log("Activating player 1 ready button");
            readyScreen.transform.GetChild(5).gameObject.GetComponent<Toggle>().isOn = true;
            player1Ready = true;
            // If both players ready then make start button interactive for player 1.
        }
        else if(args.readyPlayer == 2)
        {
            Debug.Log("Activating player 2 ready button");
            readyScreen.transform.GetChild(6).gameObject.GetComponent<Toggle>().isOn = true;
            player2Ready = true;
        }

        if (player1Ready == true && player2Ready == true)
        {
            readyScreen.transform.GetChild(2).gameObject.GetComponent<Button>().interactable = true;
        }

    }

    public void RequestUnready()
    {
        player = players[0];
        int unreadyPlayer = int.Parse(player.tag);
        RequestUnready unready = new RequestUnready();
        unready.send(unreadyPlayer);
        cManager.send(unready);
        Debug.Log("Sent unready request");
    }

    public void ResponseUnready(ExtendedEventArgs eventArgs)
    {
        ResponseUnreadyEventArgs args = eventArgs as ResponseUnreadyEventArgs;
        // player[0] represents the the current client
        player = players[0];
        // Ready screen 
        GameObject readyScreen = player.transform.GetChild(1).gameObject;

        if (args.unreadyPlayer == 1)
        {
            Debug.Log("Deactivating player 1 ready button");
            readyScreen.transform.GetChild(5).gameObject.GetComponent<Toggle>().isOn = false;
            player1Ready = false;
            
        }
        else if (args.unreadyPlayer == 2)
        {
            Debug.Log("Deactivating player 2 ready button");
            readyScreen.transform.GetChild(6).gameObject.GetComponent<Toggle>().isOn = false;
            player2Ready = false;
        }

        if (player1Ready == false || player2Ready == false)
        {
            readyScreen.transform.GetChild(2).gameObject.GetComponent<Button>().interactable = false;
        }

    }

    public void RequestStart ()
    {
        player = players[0];
        RequestStart start = new RequestStart();
        start.send(1);
        cManager.send(start);
        Debug.Log("Sent start request");
    }
    public void ResponseStart(ExtendedEventArgs eventArgs)
    {
        // Used for movement to to begin for both players. 
        // if eventargs returns 1
        player = players[0];
        GameObject playerObject = player.transform.GetChild(0).gameObject;
        playerObject.GetComponent<StartPlayerComponents>().gameStarted();
        Debug.Log("Players Activated");
    }
    public IEnumerator RequestHeartbeat(float time) {

        Debug.Log("In Coroutine");
		ConnectionManager cManager = gameObject.GetComponent<ConnectionManager>();

		
			RequestHeartbeat request = new RequestHeartbeat();
			request.send();
		
			cManager.send(request);

        yield return new WaitForSeconds(time);
        StartCoroutine(RequestHeartbeat(1f));
	}
}
