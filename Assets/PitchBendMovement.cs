using UnityEngine;
using System.Collections;

public class PitchBendMovement : MonoBehaviour {
    Rigidbody rigidBody;
    PitchBendController pbc;

    [Range(0, 1)]
    public float scale = 1f;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        pbc = GetComponent<PitchBendController>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (pbc.selector)
        {
            rigidBody.AddForce(transform.forward * 50.0f);
        }

        rigidBody.AddForce(transform.forward * pbc.pressure);

        transform.Rotate( - Vector3.up, pbc.right * scale);
        transform.Rotate(Vector3.up, pbc.left * scale);
    }
}
