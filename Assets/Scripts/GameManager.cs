using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject continuteButton;
    [SerializeField] private GameObject restartButton;

    private void Awake()
    {
        Instance = this;
    }
    public void OnWin()
    {
        continuteButton.SetActive(true);
    }

    public void OnLose()
    {
        restartButton.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Player.Instance.gameObject)
        {
            Player.Instance.GetDamage(10);
        }
    }

    public void DestroyEntity(GameObject entity)
    {
        StartCoroutine(DestroyCoroutine(entity));
    }

    private IEnumerator DestroyCoroutine(GameObject entity)
    {
        yield return new WaitForSeconds(0.7f);
        Destroy(entity);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Continue()
    {
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
