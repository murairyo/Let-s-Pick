using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRotation : MonoBehaviour
{
    public float rotationSpeed = 60f;
   

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed, 0 * Time.deltaTime);
    }
}
