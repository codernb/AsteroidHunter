using UnityEngine;
using XInputDotNetPure;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {

    public static Color[] PLAYER_COLOR = { Color.red, Color.blue, Color.green, Color.yellow };

    public GameObject player;
    public int playerNr;
    public GameObject greenBar;
    public GameObject whiteBar;
    public float spawnTime = 0f;
    public Light playerHalo;
    public int pointsSum = 0;
    public int currentPoints = 0;
    public int pointsMax = 0;
    public Transform orb;
    public Transform arrow;
    public Transform center;
    public string playerName;

    private GameObject myPlayer;
    private Player myPlayerScript;
    private bool gameOver = true;

	void Start () {
        playerHalo.color = PLAYER_COLOR[playerNr];
	}

    public void Go()
    {
        gameOver = false;
        pointsSum = 0;
        StartCoroutine(InitSpawn());
    }

    private IEnumerator InitSpawn()
    {
        yield return new WaitForSeconds(.01f);
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        currentPoints = 0;
        myPlayer = (GameObject)Instantiate(player, transform.position, transform.rotation);
        myPlayerScript = myPlayer.GetComponent<Player>();
        myPlayer.transform.LookAt(orb.position);
        myPlayerScript.playerNr = playerNr;
        myPlayerScript.playerSpawner = this;
        myPlayerScript.center = center;
    }

    void FixedUpdate()
    {
        if (myPlayer == null)
            return;
        transform.position = myPlayer.transform.position;
        arrow.rotation = myPlayer.transform.rotation;
    }
	
	void Update () {
        if (myPlayer == null)
            return;
        Player p = myPlayer.GetComponent<Player>();

        Vector3 greenScale = greenBar.transform.localScale;
        greenScale.x = p.currentLife / p.maxLife;
        greenScale.x = Mathf.Max(0f, greenScale.x);
        greenBar.transform.localScale = greenScale;

        Vector3 whiteScale = whiteBar.transform.localScale;
        whiteScale.x = p.currentBoost / p.boostTime;
        whiteScale.x = Mathf.Clamp(whiteScale.x, 0f, p.boostTime);
        whiteBar.transform.localScale = whiteScale;

        if (p.currentLife <= 0)
        {
            if (myPlayer.GetComponent<Player>().orb != null)
                myPlayer.GetComponent<Player>().orb.transform.parent = null;
            GamePad.SetVibration(myPlayer.GetComponent<Player>().index, 0, 0);
            playerDied();
            Destroy(myPlayer);
        }
	}

    private IEnumerator DeathVibrate()
    {
        GamePad.SetVibration((PlayerIndex)playerNr, .8f, .8f);
        yield return new WaitForSeconds(1.3f);
        GamePad.SetVibration((PlayerIndex)playerNr, 0, 0);
    }

    public void playerDied()
    {
        pointsMax = Mathf.Max(pointsMax, currentPoints);
        StartCoroutine(DeathVibrate());
        StartCoroutine(Respawn());
    }

    public void GivePoints(int points)
    {
        currentPoints += points;
        pointsSum += points;
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(spawnTime);
        if (!gameOver)
            SpawnPlayer();
    }

    void OnDestroy()
    {
        GamePad.SetVibration((PlayerIndex)playerNr, 0f, 0f);
    }

    public void Stop()
    {
        if (myPlayerScript != null)
            myPlayerScript.looseOrb();
        orb.GetComponent<Orb>().loosePlayer();
        Destroy(myPlayer);
    }

}
