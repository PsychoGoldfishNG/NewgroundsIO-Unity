using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestSceneBehaviour : MonoBehaviour
{
    const string FirstSceneName = "LoaderScene";
    
    [SerializeField] Button BackButton;

    void Start()
    {
        BackButton.onClick.AddListener(this.GoBack);
    }

    void GoBack()
    {
        SceneManager.LoadScene(FirstSceneName, LoadSceneMode.Single);
    }

    void Update()
    {
        // Keep the session from expiring while we play
        StartCoroutine(NGIO.KeepSessionAlive());
    }
}
