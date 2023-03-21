using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Scripts.Tasks;
using Scripts.UI;
using Scripts.UI.Dialogs;
using Scripts.Utils;

namespace Scripts
{
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private Camera GameCamera = null;
        [SerializeField]
        private Ship PlayerShip = null;
        [SerializeField]
        private List<Star> StarPath = null;
        [SerializeField]
        private float TimeAccuracy = 0.001f;
        [SerializeField]
        private TrajectoryRenderer CurrentTrajectory = null;
        [SerializeField]
        private TrajectoryRenderer PridictionTrajectory = null;
        [SerializeField]
        private float TrajectoryPredictionTime = 5f;
        [SerializeField]
        private float TrajectoryPointsMinSqrDistance = 0.01f;
        [SerializeField]
        private ScrollControl ScrollControl = null;
        [SerializeField]
        private StarsBackground2 Background = null;
        [SerializeField]
        private Vector3 _playerShipInitialSpeed = Vector3.zero;
        [SerializeField]
        private Button _pauseButton = null;
        [SerializeField]
        private DialogManager _dialogManager = null;
        [SerializeField]
        private TimeController _timeController = null;
        [SerializeField]
        private TaskManager _taskManager = null;
        [SerializeField]
        private string _nextLevelSceneName = "";

        private float _cameraSize = 1;
        private Vector3 _playerShipSpeed = Vector3.zero;
        private Vector3 _playerShipAcc = Vector3.zero;
        private List<Vector3> _trajectoryPoints = new List<Vector3>(1024);

        private void Start()
        {
            //_playerShipSpeed
            var dR = StarPath[0].transform.position - PlayerShip.transform.position;
            var dir = dR.normalized;
            var playerShipDir = new Vector3(-dir.y, dir.x, 0);
            var speed = Math.Sqrt(StarPath[0].GetMass() / dR.magnitude);

            _playerShipSpeed = playerShipDir * (float)speed;

            Debug.Log(_playerShipSpeed.y);

            _playerShipSpeed = _playerShipInitialSpeed;

            PlayerShip.AccelerationChangingEvent += OnPlayerShipAccelerationChanging;
            PlayerShip.AccelerationChangedEvent += OnPlayerShipAccelerationChanged;

            ScrollControl.ScrollEvent += OnScrolled;

            _pauseButton.onClick.AddListener(() => _dialogManager.ShowDialog(DialogEnum.Pause));

            _cameraSize = (float)Math.Log(GameCamera.orthographicSize, 2);
            OnScrolled(0);

            _taskManager.OnLevelCompleted += LevelCompleted;
            _taskManager.OnLevelFailed += LevelFailed;
            _taskManager.OnTaskCompleted += TaskCompleted;
            _taskManager.OnTaskFailed += TaskFailed;
            _taskManager.OnTaskProgressChanged += TaskProgressChanged;

            _dialogManager.OnGameShouldBePaused += OnPause;
            _dialogManager.OnGameShouldBeResumed += OnContinue;
            _dialogManager.OnRestartLevelEvent += OnRestart;
            _dialogManager.OnGoToSceneEvent += OnReturn;
            _dialogManager.OnGoToNextSceneEvent += OnNext;
            _dialogManager.OnGameExitEvent += OnExit;
            _dialogManager.ShowDialog(DialogEnum.LevelStart);
        }

        private void Update()
        {
            if (!_timeController.IsPaused())
            {
                Vector3 playerPos = PlayerShip.transform.position;
                var dt = _timeController.GetDt();

                while (dt > TimeAccuracy)
                {
                    var result = GravityUtils.CalculateGravity(StarPath, playerPos, _playerShipSpeed, _playerShipAcc, TimeAccuracy);
                    playerPos += result.Key;
                    _playerShipSpeed += result.Value;

                    dt -= TimeAccuracy;
                }

                var endRresult = GravityUtils.CalculateGravity(StarPath, playerPos, _playerShipSpeed, _playerShipAcc, dt);
                playerPos += endRresult.Key;
                _playerShipSpeed += endRresult.Value;

                PlayerShip.transform.position = playerPos;

                UpdateTrajectory(CurrentTrajectory, Vector3.zero);

                var cameraPos = playerPos;
                cameraPos.z = GameCamera.transform.position.z;
                GameCamera.transform.position = cameraPos;

                if (Background != null)
                {
                    Background.transform.position = new Vector3(GameCamera.transform.position.x, GameCamera.transform.position.y, Background.transform.position.z);
                    Background.Tick();
                }
            }
        }

        private void UpdateTrajectory(TrajectoryRenderer trajectory, Vector3 acc)
        {
            _trajectoryPoints.Clear();
            var dt = TrajectoryPredictionTime;
            Vector3 playerPos = PlayerShip.transform.position;
            Vector3 playerSpeed = _playerShipSpeed;
            var size = GameCamera.orthographicSize > 200 ? ((float)Math.Pow(1.5, _cameraSize) / 5) : (GameCamera.orthographicSize / 50);

            while (dt > TimeAccuracy)
            {
                var result = GravityUtils.CalculateGravity(StarPath, playerPos, playerSpeed, acc, TimeAccuracy);

                playerPos += result.Key;
                playerSpeed += result.Value;

                if (_trajectoryPoints.Count == 0 || (playerPos - _trajectoryPoints[_trajectoryPoints.Count - 1]).sqrMagnitude > TrajectoryPointsMinSqrDistance * size)
                {
                    _trajectoryPoints.Add(playerPos);
                }

                dt -= TimeAccuracy;
            }

            var endResult = GravityUtils.CalculateGravity(StarPath, playerPos, playerSpeed, acc, dt);
            playerPos += endResult.Key;

            if ((playerPos - _trajectoryPoints[_trajectoryPoints.Count - 1]).sqrMagnitude > TrajectoryPointsMinSqrDistance * size)
            {
                _trajectoryPoints.Add(playerPos);
            }

            trajectory.SetTrajectory(_trajectoryPoints, size);
        }

        private void OnPlayerShipAccelerationChanging(Vector3 acc)
        {
            _playerShipAcc = acc;
            UpdateTrajectory(PridictionTrajectory, acc);
        }

        private void OnPlayerShipAccelerationChanged(Vector3 acc)
        {
            _playerShipAcc = Vector3.zero;
            PridictionTrajectory.Clear();
        }

        private void OnScrolled(float delta)
        {
            if (!_timeController.IsPaused())
            {
                _cameraSize += delta;
                if (_cameraSize < 0.1f)
                {
                    _cameraSize = 0.1f;
                }

                GameCamera.orthographicSize = (float)Math.Pow(2, _cameraSize) - 1;

                if (Background != null)
                {
                    Background.CameraOrtographicSizeChanged();
                }

                UpdateObjectsScale();
            }
        }

        private void UpdateObjectsScale()
        {
            if (GameCamera.orthographicSize > 200)
            {
                PlayerShip.transform.localScale = Vector3.one * 0.5f * (float)Math.Pow(1.3, _cameraSize);
            }
            else
            {
                PlayerShip.transform.localScale = Vector3.one * GameCamera.orthographicSize / 50;
            }

            foreach (var star in StarPath)
            {
                if (GameCamera.orthographicSize / star.GetSize() > 14)
                {
                    var camSize = Math.Log(star.GetSize() * 14 + 1, 2);
                    var coeff = (float)((Math.Pow(2, camSize) - 1) / Math.Pow(1.5, camSize));
                    star.transform.localScale = Vector3.one * coeff * (float)Math.Pow(1.5, _cameraSize) * 0.0014f * star.GetSize();
                }
                else
                {
                    star.transform.localScale = Vector3.one * star.GetSize();
                }
            }
        }

        private void OnPause()
        {
            _timeController.Pause();
        }

        private void OnContinue()
        {
            _timeController.Continue();
        }

        private void OnRestart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }

        private void OnReturn(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }

        private void OnNext()
        {
            SceneManager.LoadScene(_nextLevelSceneName, LoadSceneMode.Single);
        }

        private void OnExit()
        {
            Application.Quit();
        }

        private void LevelCompleted()
        {
            _dialogManager.ShowDialog(DialogEnum.LevelCompleted);
        }

        private void LevelFailed()
        {
            _dialogManager.ShowDialog(DialogEnum.LevelFailed);
        }

        private void TaskCompleted()
        {
            // TODO: update UI
        }

        private void TaskFailed()
        {
            // TODO: update UI
        }

        private void TaskProgressChanged()
        {
            // TODO: update UI
        }
    }
}