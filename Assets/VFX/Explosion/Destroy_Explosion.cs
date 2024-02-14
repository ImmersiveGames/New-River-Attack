using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_Explosion : MonoBehaviour
{

    // Use this for initialization
    private void Start()
    {
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    private void Update()
    {

    }
}
