using UnityEngine;
using UnityEngine.UI;
using Scripts.UI;

namespace Scripts
{
    public class TimeController : MonoBehaviour
    {
        [SerializeField]
        private int _timeSpeed = 1;

        [SerializeField]
        private int _initialGameSpeedMultiplier = 1;

        [SerializeField]
        private Text _gameTime = null;

        [SerializeField]
        private GameSpeedButtonManager _gameSpeedButtons = null;

        private int _gameSpeedMultiplier = 0;
        private bool _paused = false;
        private float _gameTimer = 0;
        private float _dt = 0;

        public void Pause()
        {
            _paused = true;
        }

        public void Continue()
        {
            _paused = false;
        }

        public bool IsPaused()
        {
            return _paused;
        }

        public float GetGameTime()
        {
            return _gameTimer;
        }

        public float GetDt()
        {
            return _dt;
        }

        public float GetGameTimerDt()
        {
            return _dt / _timeSpeed;
        }

        private void Start()
        {
            _gameSpeedMultiplier = _initialGameSpeedMultiplier;

            _gameSpeedButtons.GameSpeedSelectedEvent += OnGameSpeedChanged;
        }

        private void Update()
        {
            if (!_paused)
            {
                _dt = Time.deltaTime * _gameSpeedMultiplier;
                _gameTimer += _dt;
                _dt *= _timeSpeed;
                UpdateGameTime();
            }
        }

        private void UpdateGameTime()
        {
            var min = (int)(_gameTimer / 60f);
            var sec = (int)(_gameTimer - 60f * min);
            _gameTime.text = min.ToString("00") + ":" + sec.ToString("00");
        }

        private void OnGameSpeedChanged(int speed)
        {
            _gameSpeedMultiplier = speed;
        }
    }
}
