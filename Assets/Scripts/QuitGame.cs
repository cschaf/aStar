using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Die Klasse QuitGame.
/// Sie ist für das Beenden der Applikation zuständig
/// </summary>
public class QuitGame : MonoBehaviour {
    public void DoExit()
    {
        Application.Quit();
    }
}
