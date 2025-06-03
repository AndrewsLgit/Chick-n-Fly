using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Runtime
{
    public class UIManager : BigBrother
    {
        #region Private Variables

        private bool _isPaused;

        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private GameObject _gameOverMenu;
        [SerializeField] private GameObject _victoryMenu;

        #endregion

        #region Unity API

        private void Awake()
        {
            // pause game on awake (optional, can put inside Start() method
            _isPaused = false;
            
            // some UI setup
        }

        private void Update()
        {
            // don't execute logic if the game is paused
            if (_isPaused) return;
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
            SceneManager.LoadSceneAsync(1);
        }

        public void GameOver()
        {
            // stop game time
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

        #endregion
    }
}