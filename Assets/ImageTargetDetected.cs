using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageTargetDetected : DefaultTrackableEventHandler
{

    #region PROTECTED_METHODS

    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.y, 0));
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.y, 0));
    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();

    }

    #endregion // PROTECTED_METHODS
}