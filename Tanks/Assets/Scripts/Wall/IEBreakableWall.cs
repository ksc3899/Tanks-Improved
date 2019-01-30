using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IEBreakableWall : MonoBehaviour
{
    public float strikeForce = 5f;

    private Rigidbody rb;

    //I can add in start, but don't for the sake of code efficiency.
    /*private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }*/

    public void WallBreakDown(Vector3 shellDirection)
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(strikeForce * shellDirection, ForceMode.Impulse);
    }
}
