using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Countdown : MonoBehaviour
{
    public float timeLeft;

    public Text countdown;
    public Text looser;

    void Update()
    {
        if(timeLeft > 0) {
            timeLeft -= Time.deltaTime;
            countdown.text = "" + Mathf.Round(timeLeft);
            if (timeLeft <= 0)
            {
                looser.text = "Verloren";
            }
        }
    }
}