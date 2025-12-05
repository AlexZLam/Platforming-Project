using UnityEngine;
using UnityEngine.SceneManagement;

public class nxtLevel : MonoBehaviour
{
    int currentSceneIndex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(currentSceneIndex == 4)
            {
                SceneManager.LoadScene(0);
            }
            currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }
}
