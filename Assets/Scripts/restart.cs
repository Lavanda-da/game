using UnityEngine;
using UnityEngine.SceneManagement;  // Важно добавить!

public class restart : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
