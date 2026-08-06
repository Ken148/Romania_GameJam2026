using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDoor : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void Enter()
    {
        Debug.Log("Loading: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}