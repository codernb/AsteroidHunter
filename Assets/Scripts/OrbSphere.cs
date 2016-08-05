using UnityEngine;
using System.Collections;

public class OrbSphere : MonoBehaviour {

    public Orb orb;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Gate")
        {
            other.GetComponent<Gate>().Score(orb.player.player);
        }
    }

}
