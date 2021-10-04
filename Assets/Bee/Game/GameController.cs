using System.Collections.Generic;
using Bee.Cargo;
using Bee.Ocean;
using Bee.Player;
using Bee.Raft;
using Bee.Sky;
using Bee.Weather;
using UnityEngine;
using UnityEngine.UI;

namespace Bee.Game
{
    public class GameController : MonoBehaviour
    {
        private List<IGameSystem> _systems;
        private bool _gameRunning;
        private bool _gameOver;

        private Canvas _ui;
        private Text _panelText;
        
        public void Start()
        {
            _ui = GameObject.Find("Canvas").GetComponent<Canvas>();
            _panelText = GameObject.Find("PanelText").GetComponent<Text>();
            
            _systems = new List<IGameSystem>
            {
                GameObject.Find("Raft").GetComponent<RaftController>(),
                GameObject.Find("Ocean").GetComponent<OceanController>(),
                GameObject.Find("Player").GetComponent<PlayerController>(),
                GameObject.Find("Weather").GetComponent<WeatherController>(),
                GameObject.Find("Sky").GetComponent<SkyController>(),
                GameObject.Find("Cargo").GetComponent<CargoController>()
            };
        }

        public void Update()
        {
            if (_gameOver && Input.GetButton("Submit") || !_gameRunning && Input.GetButton("Submit"))
            {
                _gameRunning = true;
                Debug.Log("Starting");
                _ui.gameObject.SetActive(false);
                _systems.ForEach(system => system.PlayGame());
                if (_gameOver) _gameOver = false;
            }
            
            if (_gameRunning && Input.GetButton("Cancel"))
            {
                _gameRunning = false;
                Debug.Log("Pausing");
                _ui.gameObject.SetActive(true);
                _panelText.text = "Paused. Press Enter / Start";
                _systems.ForEach(system => system.PauseGame());
            }
            
            // DEBUG
            if (_gameRunning && Input.GetKeyUp(KeyCode.W))
            {
                SetGameWon();
            }

            if (_gameRunning && Input.GetKeyUp(KeyCode.L))
            {
                SetGameLost();
            }

        }

        public void SetGameLost()
        {
            SetGameOver(true);
            _ui.gameObject.SetActive(true);
            _panelText.text = "LOSER. Press Enter to restart";
        }

        public void SetGameWon()
        {
            SetGameOver(false);
            _ui.gameObject.SetActive(true);
            _panelText.text = "WINNER. Press Enter to restart";
        }

        private void SetGameOver(bool won)
        {
            _gameOver = true;
            _gameRunning = false;
            _systems.ForEach(system => system.PauseGame());
            _systems.ForEach(system => system.ResetGame());
        }
    }

    public interface IGameSystem
    {
        public void PlayGame();
        public void PauseGame();
        public void ResetGame();
    }

}

