using System;
using Bee.Game;
using UnityEngine;

namespace Bee.Ocean
{
    public class OceanNodeController : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            var go = other.gameObject;
            if (go.name == "PlayerBody")
            {
                var control = GameObject.Find("GameControl").GetComponent<GameController>();
                control.SetGameLost();
            }
        }
    }
}

