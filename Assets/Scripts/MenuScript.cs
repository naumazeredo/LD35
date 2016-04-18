using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {
	void Update () {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) {
            SceneManager.LoadScene("MainScene");
        }
	}
}
