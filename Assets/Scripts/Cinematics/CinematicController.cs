using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Controller;


namespace RPG.Cinematics
{
    public class CinematicController : MonoBehaviour
    {

        GameObject Player;
        private void Start()
        {
            Player = GameObject.FindWithTag("Player");
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnabledControl;
        }


        void EnabledControl(PlayableDirector pd)
        {
            Player.GetComponent<PlayerController>().enabled = true;

        }
        void DisableControl(PlayableDirector pd)
        {
            Player.GetComponent<PlayerController>().enabled = false;
        }

    }
}

