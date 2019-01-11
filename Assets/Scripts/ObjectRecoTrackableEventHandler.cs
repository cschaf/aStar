using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ObjectRecoTrackableEventHandler : DefaultTrackableEventHandler
{

    public GameObject Target;

    #region PROTECTED_METHODS

    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        Target.transform.position = transform.position;

    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();

    }

    #endregion // PROTECTED_METHODS
}