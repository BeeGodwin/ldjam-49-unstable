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

        private GameObject _ui;
        private Text _panelText;
        private Text _distanceText;
        private float _remainingDistance;

        public float distance;
        
        public void Start()
        {
            _ui = GameObject.Find("UIMain");
            _panelText = GameObject.Find("PanelText").GetComponent<Text>();
            _distanceText = GameObject.Find("DistanceText").GetComponent<Text>();
            
            _systems = new List<IGameSystem>
            {
                GameObject.Find("Raft").GetComponent<RaftController>(),
                GameObject.Find("Ocean").GetComponent<OceanController>(),
                GameObject.Find("Player").GetComponent<PlayerController>(),
                GameObject.Find("Weather").GetComponent<WeatherController>(),
                GameObject.Find("Sky").GetComponent<SkyController>(),
                GameObject.Find("Cargo").GetComponent<CargoController>()
            };
            _remainingDistance = distance;
        }

        public void Update()
        {
            if (_gameOver && Input.GetButton("Submit") || !_gameRunning && Input.GetButton("Submit"))
            {
                _gameRunning = true;
                Debug.Log("Starting");
                _ui.SetActive(false);
                _systems.ForEach(system => system.PlayGame());
                if (_gameOver) _gameOver = false;
            }
            
            if (_gameRunning && Input.GetButton("Cancel"))
            {
                _gameRunning = false;
                Debug.Log("Pausing");
                _ui.SetActive(true);
                _panelText.text = "Paused. Press Enter / Start";
                _systems.ForEach(system => system.PauseGame());
            }

            if (_gameRunning)
            {
                _remainingDistance -= Time.deltaTime;
                _distanceText.text = $"Distance Remaining: {(int)Mathf.Max(_remainingDistance / 10, 0f)}";
                if (_remainingDistance < 0)
                {
                    SetGameWon();
                }
                if (Input.GetKeyUp(KeyCode.L))
                {
                    SetGameLost();
                }
            }
            

            // DEBUG


            if (_gameRunning && Input.GetKeyUp(KeyCode.L))
            {
                SetGameLost();
            }

        }

        public void SetGameLost()
        {
            SetGameOver(true);
            _ui.gameObject.SetActive(true);
            _panelText.text = "Awwww crap, you drowned. \nYes, it's harsh, NGL.\nDidn't have time to make a swimming feature tho.\nPress Enter to restart";
        }

        public void SetGameWon()
        {
            SetGameOver(false);
            _ui.gameObject.SetActive(true);
            _panelText.text = "Congratulations, you made it! \nPress Enter to restart";
        }

        private void SetGameOver(bool won)
        {
            _gameOver = true;
            _gameRunning = false;
            _systems.ForEach(system => system.PauseGame());
            _systems.ForEach(system => system.ResetGame());
            _remainingDistance = distance;
        }
    }

    public interface IGameSystem
    {
        public void PlayGame();
        public void PauseGame();
        public void ResetGame();
    }
}

