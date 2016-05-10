using UnityEngine;
using System.Collections;

public class ParticleEnabler : MonoBehaviour {
    ParticleController pc;
    public GameObject particleSystem;
	// Use this for initialization
	void Start () {
        pc = GetComponent<ParticleController>();
	}

    // Update is called once per frame
    void Update()
    {
        particleSystem.SetActive(pc.pushed);
    }
}
