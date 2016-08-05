using UnityEngine;
using System.Collections;

public class Gate : MonoBehaviour {

    public PlayerSpawner player;
    public Transform orb;
    public float catapultSpeed = 50f;

    public void Score(Player carrier)
    {

        if (player == carrier.playerSpawner)
        {
            player.GivePoints(30);
            carrier.looseOrb();
            orb.GetComponent<Rigidbody>().isKinematic = false;
            orb.position -= orb.position.normalized * 10f;
            orb.GetComponent<Rigidbody>().velocity -= (orb.position.normalized + Random.insideUnitSphere) * catapultSpeed;
        }

    }

}
