using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class buttonManager : MonoBehaviour
{
    int currentSceneIndex;
    int counter = 1;
    public GameObject sliders;
    public void startBtn()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
    public void settingBtn()
    {
        if(counter == 0)
        {
            sliders.SetActive(false);
            counter = 1;
        }
        else if(counter == 1)
        {
            sliders.SetActive(true);
            counter = 0;
        }
        
    }
    public void QuitBtn()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
