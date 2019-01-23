using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Die SceneController Klasse.
/// Sie ist für das Laden und das Zurücksetzen der Hauptszene zuständig
/// </summary>
public class SceneController: MonoBehaviour {

    public void LoadMainScene() {
        SceneManager.LoadScene("ARScene");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
    }
}
