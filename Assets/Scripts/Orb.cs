using UnityEngine;
using System.Collections;

public class Orb : MonoBehaviour {

    public PlayerCollider player;
    public float catchSpeed = .05f;
    public float pointsSpeed = 3f;
    public int points =  1;
    public float changeWait = .25f;
    public GameObject beacon;
    public Camera cam;

    private float lastPoint = float.MinValue;
    private float nextChange = float.MinValue;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && Time.time > nextChange)
        {
            if (player != null)
                player.looseOrb();
            player = other.GetComponent<PlayerCollider>();
            player.giveOrb(gameObject);
            GetComponent<ParticleSystem>().Emit(100);
            nextChange = Time.time + changeWait;
        }
    }

    void OnTriggerStay(Collider other)
    {
       if (other.tag == "Catcher" && transform.parent == null)
            GetComponent<Rigidbody>().velocity -= (transform.position - other.transform.position).normalized;
    }

    void Update()
    {
        if (player != null && Time.time > lastPoint + pointsSpeed)
        {
            player.givePoints(points);
            lastPoint = Time.time;
        }

        if (IsVisibleFrom(GetComponent<Renderer>(), cam))
            beacon.SetActive(false);
        else
            beacon.SetActive(true);
    }


    private bool IsVisibleFrom(Renderer renderer, Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }

    public void loosePlayer()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        player = null;
    }

}
