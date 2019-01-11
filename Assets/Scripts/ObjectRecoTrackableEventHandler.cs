using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ObjectRecoTrackableEventHandler : DefaultTrackableEventHandler
{
    #region PROTECTED_METHODS

    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();
        Debug.Log("" + transform.position);
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();

    }

    #endregion // PROTECTED_METHODS
}