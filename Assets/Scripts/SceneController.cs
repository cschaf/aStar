using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController: MonoBehaviour {

    public void LoadMainScene() {
        SceneManager.LoadScene("ARScene");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
    }
}
