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
        public void Start()
        {
            _systems = new List<IGameSystem>();
            _systems.Add(GameObject.Find("Raft").GetComponent<RaftController>());
            _systems.Add(GameObject.Find("Ocean").GetComponent<OceanController>());
            _systems.Add(GameObject.Find("Player").GetComponent<PlayerController>());
            _systems.Add(GameObject.Find("Weather").GetComponent<WeatherController>());
            _systems.Add(GameObject.Find("Sky").GetComponent<SkyController>());
            _systems.Add(GameObject.Find("Cargo").GetComponent<CargoController>());
        }
    }

    public interface IGameSystem
    {
        public void PlayGame();
        public void PauseGame();
        public void StartGame();
    }

}

