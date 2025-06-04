using SharedData.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Runtime
{
    public class UIManager : BigBrother
    {
        #region Public Variables

        public EventChannel m_OnPlayerDeath;

        #endregion
        #region Private Variables

        private bool _isPaused;

        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private GameObject _gameOverMenu;
        [SerializeField] private GameObject _victoryMenu;
        [SerializeField] private TextMeshProUGUI _timerTextNumberTMP;
        [SerializeField] private string _timerTextString = "Remaining time: ";
        [SerializeField] private float _timerStartValue;
        private CountdownTimer _gameTimer;

        #endregion

        #region Unity API

        private void Awake()
        {
            // pause game on awake (optional, can put inside Start() method
            _isPaused = false;
            
            // some UI setup
        }

        private void Start()
        {
            SetupTimers();
            _gameTimer.Start();
        }

        private void Update()
        {
            // don't execute logic if the game is paused
            if (_isPaused)
            {
                _gameTimer.Pause();
                return;
            }
            _gameTimer.Resume();
            _gameTimer.Tick(Time.deltaTime);
           _timerTextNumberTMP.text = $"{_timerTextString}{Mathf.FloorToInt(_gameTimer.GetTime()).ToString()}"; 
        }

        #endregion
        
        #region Main Methods
        
        // Set (invert) pause state
        public void PauseGame()
        {
            // inverse _isPaused value
            _isPaused = !_isPaused;
            // set timescale to pause value
            Time.timeScale = _isPaused ? 0 : 1;
            // then activate the pause menu UI
            _pauseMenu.SetActive(_isPaused);
        }
        public void ResumeGame()
        {
            // remove paused state
            _isPaused = false;
            // set timeScale to 1 (game resumed)
            Time.timeScale = 1;
            // disable the pause menu UI
            _pauseMenu.SetActive(_isPaused );
        }

        public void QuitGame()
        {
            // exit game
            Application.Quit();
        }

        public void RestartGame()
        {
            // load first scene in SceneManager
            // todo: declare scene inside Unity Build Settings
            SceneManager.LoadSceneAsync(0);
        }

        public void GameOver()
        {
            // stop game time
            // m_OnPlayerDeath?.Invoke(new Empty());
            Time.timeScale = 0;
            _gameOverMenu.SetActive(true);
        }

        public void Victory()
        {
            Time.timeScale = 0;
            _victoryMenu.SetActive(true);
        }
        
        /*
        private void SetScoreTexts()
        {
            // get text from ScriptableObject (the SO manages round value increase internally)
           _scoreText.text = $"Round {_roundSystemSO.GetCurrentRound()}";
           _highScoreText.text = $"HighScore: {_roundSystemSO.m_highScore}";           }
         */

        #endregion

        #region Utils

        private void SetPause()
        {
            _isPaused = !_isPaused;
        }
        
        private void SetupTimers()
        {
            _gameTimer = new CountdownTimer(_timerStartValue);
            // Disable arrow (direction indicator) when the player jumps
            _gameTimer.OnTimerStart += () => _timerTextNumberTMP.text = _timerTextString;
            _gameTimer.OnTimerStop += GameOver;

            //_timers.Add(_jumpTimer);
        }

        // private void HandleTimers()
        // {
        //     foreach (var timer in _timers)
        //     {
        //         timer.Tick(Time.deltaTime);
        //     }
        // }

        #endregion
    }
}