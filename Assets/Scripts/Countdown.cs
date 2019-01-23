using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Die Countdown Klasse.
/// Sie repräsentiert den Spiel-Countdown am oberen Rand der Anwendung.
/// Zusätzlich verwaltet sie den Text (Gewonnen/Verloren) der nach Spielende angezeigt wird.
/// </summary>
public class Countdown : MonoBehaviour
{
    public float timeLeft;

    public Text countdown;
    public Text winner;

    void Update()
    {
		if(winner.text == "Gewonnen"){
			countdown.text = "";
		}
		else{
      // Zähle die Zeit runter bis 0
			if(timeLeft > 0) {
				timeLeft -= Time.deltaTime;
				countdown.text = "" + Mathf.Round(timeLeft);
				if (timeLeft <= 0)
				{
					winner.text = "Verloren";
				}
			}
		}
    }
}