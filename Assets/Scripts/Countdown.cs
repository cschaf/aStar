using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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