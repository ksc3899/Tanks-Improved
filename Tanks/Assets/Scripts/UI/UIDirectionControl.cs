using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDirectionControl : MonoBehaviour
{
    public bool useRelativeRotation = true;

    Quaternion relativeRotation;

    private void Awake()
    {
        relativeRotation = transform.parent.localRotation;
    }

    private void FixedUpdate()
    {
        if (useRelativeRotation)
            transform.rotation = relativeRotation;
    }
}
