using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

/// <summary>
/// Überschriebener DefaultHandler, der greift, wenn Vuforia ein eingescanntes Objekt erkennt. 
/// Das Objekt wird dann einfach auf der Y-Achse auf 0 gesetzt,
/// damit es auf der gleichen ebene bleibt wie unser Ground
/// </summary>
public class ObjectRecoTrackableEventHandler : DefaultTrackableEventHandler
{
    #region PROTECTED_METHODS

    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();
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