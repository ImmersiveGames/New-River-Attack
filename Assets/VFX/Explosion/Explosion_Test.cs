using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion_Test : MonoBehaviour {

    public GameObject explosionEffect;

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.H))
        {
            Explode();
        }
	}

    private void Explode()
    {
        GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(gameObject);


    }
}
