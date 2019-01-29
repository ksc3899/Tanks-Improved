using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellModelRotator : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(new Vector3(0, 180, 0) * Time.deltaTime);
    }
}
