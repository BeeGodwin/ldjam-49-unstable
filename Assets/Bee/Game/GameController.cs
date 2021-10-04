using System.Collections.Generic;
using Bee.Cargo;
using Bee.Ocean;
using Bee.Player;
using Bee.Raft;
using Bee.Sky;
using Bee.Weather;
using UnityEngine;

namespace Bee.Game
{
    public class GameController : MonoBehaviour
    {
        private List<IGameSystem> _systems;
        private bool _gameRunning;
        private bool _gameOver;
        public void Start()
        {
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
            if (_gameOver || !_gameRunning && Input.GetButton("Submit"))
            {
                _gameRunning = true;
                Debug.Log("Starting");
                _systems.ForEach(system => system.PlayGame());
            }
            
            if (_gameRunning && Input.GetButton("Cancel"))
            {
                _gameRunning = false;
                Debug.Log("Pausing");
                _systems.ForEach(system => system.PauseGame());
            }
        }

        public void SetGameLost()
        {
            SetGameOver(true);
        }

        public void SetGameWon()
        {
            SetGameOver(false);
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

