using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Scripts.UI;

namespace Scripts
{
    public class MainSceneController : MonoBehaviour
    {
        [SerializeField]
        private Button StartButton = null;

        [SerializeField]
        private Button SettingsButton = null;

        [SerializeField]
        private Button ExitButton = null;

        [SerializeField]
        private LevelsButtonManager LevelsButton = null;

        // Start is called before the first frame update
        private void Start()
        {
            StartButton.onClick.AddListener(OnStart);
            SettingsButton.onClick.AddListener(OnSettings);
            ExitButton.onClick.AddListener(OnExit);

            LevelsButton.gameObject.SetActive(false);
        }

        // Update is called once per frame
        private void Update()
        {

        }

        private void OnStart()
        {
            StartButton.gameObject.SetActive(false);
            SettingsButton.gameObject.SetActive(false);
            ExitButton.gameObject.SetActive(false);

            LevelsButton.gameObject.SetActive(true);
            LevelsButton.LevelChoosedEvent += OnLevelChoosed;
        }

        private void OnSettings()
        {
        }

        private void OnExit()
        {
            Application.Quit();
        }

        private void OnLevelChoosed(string levelName)
        {
            SceneManager.LoadScene(levelName, LoadSceneMode.Single);
        }
    }
}