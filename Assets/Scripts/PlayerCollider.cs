using UnityEngine;
using XInputDotNetPure;
using System.Collections;

public class PlayerCollider : MonoBehaviour {

    public Player player;
    public float damageSpeed = 30f;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle") {
            StartCoroutine(Vibrate(.25f, .5f, .5f));
            float speed = player.GetComponent<Rigidbody>().velocity.magnitude;
            if (speed > damageSpeed)
                player.DealDamage(speed / 1.5f - damageSpeed);
        }
    }

    IEnumerator Vibrate(float duration, float left, float right)
    {
        GamePad.SetVibration((PlayerIndex)player.playerNr, left, right);
        yield return new WaitForSeconds(duration);
        GamePad.SetVibration((PlayerIndex)player.playerNr, 0, 0);
    }

    public void giveOrb(GameObject orb)
    { 
        player.giveOrb(orb);
        StartCoroutine(Vibrate(.25f, .4f, .4f));
    }

    public void looseOrb()
    {
        player.looseOrb();
    }

    public void givePoints(int points)
    {
        player.givePoints(points);
    }

    public void DealDamage(float damage, Player origin)
    {
        player.DealDamage(damage, origin);
    }

}
