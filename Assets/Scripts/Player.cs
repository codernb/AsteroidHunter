using UnityEngine;
using XInputDotNetPure;
using System.Collections;

public class Player : MonoBehaviour {

    public float sidewaysFactor = .5f;
    public float backwardsFactor = .2f;
    public float turnFactor = .7f;
    public int playerNr;
    public GameObject shot;
    public float triggerSensitivity = .1f;
    public GameObject shotSpawn;
    public float weaponSpeed = .5f;
    public float maxLife = 30f;
    public float currentLife;
    public int points = 10;
    public float boostReload = 10f;
    public float boostSpeed = 10f;
    public float boostTime = .5f;
    public float currentBoost;
    public ParticleSystem flare;
    public ParticleSystem core;
    public Transform engine;
    public PlayerSpawner playerSpawner;
    public GameObject orb;
    public PlayerIndex index;
    public Transform center;

    private float lastShot = float.MinValue;
    private float lastBoost = float.MinValue;
    private bool boost;
    private bool vibrating = false;
    private bool outOfBounds = false;

    void Start()
    {
        index = (PlayerIndex)playerNr;
        currentLife = maxLife;
        currentBoost = boostTime;
    }	

	void FixedUpdate () {
        GamePadThumbSticks sticks = GamePad.GetState(index).ThumbSticks;
        float horizontal = sticks.Left.X * 4;
        float vertical = sticks.Left.Y * 4;
        float turn = sticks.Right.X * 3f / 2f;
        if (vertical < 0)
            vertical *= backwardsFactor;
        GetComponent<Rigidbody>().velocity += horizontal * transform.right + vertical * transform.forward;
        GetComponent<Rigidbody>().angularVelocity += new Vector3(0f, turn * turnFactor, 0f);

        if (boost && orb == null && currentBoost > 0f)
        {
            GetComponent<Rigidbody>().velocity += transform.forward * boostSpeed;
            currentBoost -= .1f;
            lastBoost = Time.time;
        }
        else if (Time.time > lastBoost + boostTime && currentBoost < boostTime)
        {
            currentBoost += .01f;
        }

        float factor = Mathf.Sqrt(GetComponent<Rigidbody>().velocity.magnitude);

        core.startSize = factor / 2f;
        flare.startSize = factor / 2f;

        engine.transform.localPosition = new Vector3(0f, 0f, -factor / 10f + .1f);

        if (outOfBounds)
        {
            Vector3 vel = GetComponent<Rigidbody>().velocity;
            vel += (center.transform.position - transform.position) / 50f;
            GetComponent<Rigidbody>().velocity = vel;
        }
        bool fire = GamePad.GetState(index).Triggers.Right > triggerSensitivity;
        boost = GamePad.GetState(index).Buttons.LeftShoulder == ButtonState.Pressed;
        if (fire && lastShot + weaponSpeed < Time.time) {
            Fire();
        }
	}

    void Update()
    {
    }

    private void Fire()
    {
        GameObject tempShot = (GameObject) Instantiate(shot, shotSpawn.transform.position, shotSpawn.transform.rotation);
        tempShot.GetComponent<Shot>().initSpeed = GetComponent<Rigidbody>().velocity;
        tempShot.GetComponent<Shot>().origin = this;
        lastShot = Time.time;
        if (!vibrating)
            StartCoroutine(Vibrate(.1f, .2f, .2f));
    }

    IEnumerator Vibrate(float duration, float left, float right)
    {
        GamePad.SetVibration(index, left, right);
        yield return new WaitForSeconds(duration);
        if (!vibrating)
            GamePad.SetVibration(index, 0, 0);
    }

    public void DealDamage(float damage, Player origin)
    {
        if (origin == this)
            return;
        if (origin != null && currentLife - damage <= 0)
            origin.givePoints(points);
        DealDamage(damage);
    }

    public void DealDamage(float damage)
    {
        currentLife -= damage;
        StartCoroutine(Vibrate(.5f, .5f, .5f));
    }

    public void giveOrb(GameObject orb)
    {
        orb.transform.SetParent(transform);
        orb.transform.position = transform.position + transform.forward * -transform.localScale.z * 2;
        orb.GetComponent<Orb>().enabled = true;
        orb.GetComponent<Rigidbody>().isKinematic = true;
        this.orb = orb;
    }

    public void looseOrb()
    {
        if (orb == null)
            return;
        orb.transform.SetParent(null);
        orb.GetComponent<Orb>().loosePlayer();
        orb = null;
    }

    public void givePoints(int points)
    {
        playerSpawner.GivePoints(points);
        if (points >= 10)
            StartCoroutine(VibrateN(2, .3f, 4f, .4f));
    }

    private IEnumerator VibrateN(int n, float duration, float left, float right)
    {
        vibrating = true;
        for (int i = 0; i < n; i++)
        {
            if (i % 2 == 0)
                GamePad.SetVibration(index, left, right);
            else
                GamePad.SetVibration(index, right, left);
            yield return new WaitForSeconds(duration);
            GamePad.SetVibration(index, 0f, 0f);
            yield return new WaitForSeconds(.2f);
        }
        vibrating = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerBoundary")
        {
            outOfBounds = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "PlayerBoundary")
        {
            outOfBounds = true;
        }
    }

    void OnDestroy()
    {
        if (orb != null)
        {
            orb.GetComponent<Rigidbody>().isKinematic = false;
            orb.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
        }
    }

}
