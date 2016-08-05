using UnityEngine;
using System.Collections;

public class Compas : MonoBehaviour {

    public Transform cam;
    public Transform gate;
    public Transform arrow;

	void FixedUpdate () {

        transform.LookAt(gate.position);
        transform.localScale = new Vector3(1f, 1f, cam.position.y / 10f);

	}
}
