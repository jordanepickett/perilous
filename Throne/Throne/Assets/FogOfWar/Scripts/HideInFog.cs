﻿using UnityEngine;
using UnityEngine.UI;

namespace FoW
{
    [AddComponentMenu("FogOfWar/HideInFog")]
    public class HideInFog : MonoBehaviour
    {
        public int team = 0;

        [Range(0.0f, 1.0f)]
        public float minFogStrength = 0.2f;

        GameObject localPlayer;
        FogOfWar fow;
        
        Transform _transform;
        Renderer _renderer;
        Renderer[] childrenRenderes;
        Graphic _graphic;
        Canvas _canvas;

        void Start()
        {
            _transform = transform;
            _renderer = GetComponent<Renderer>();
            _graphic = GetComponent<Graphic>();
            _canvas = GetComponentInChildren<Canvas>();
            childrenRenderes = GetComponentsInChildren<Renderer>();
        }

        void Update()
        {
            FogOfWar fow = null;
            var players = PlayersContainer.GetPlayers();
            foreach (var player in players)
            {
                if (player.GetComponent<PlayerController>().isLocalPlayer)
                {
                    fow = player.GetComponentInChildren<FoW.FogOfWar>();
                    localPlayer = player;
                }
            }

            //GameObject localPlayer = CustomNetworkManager.singleton.GetPlayers();
            //FogOfWar fow = FogOfWar.GetFogOfWarTeam(team);
            if (fow == null)
            {
                Debug.LogWarning("There is no Fog Of War team for team #" + team.ToString());
                return;
            }
            bool visible = !fow.IsInFog(_transform.position, minFogStrength);

            if (_renderer != null)
            {
                if(childrenRenderes.Length > 0)
                {
                    foreach(var rend in childrenRenderes)
                    {
                        if(rend.GetComponent<GatherVisualization>())
                        {
                            break;
                        }
                        rend.enabled = visible;
                    }
                }
                _renderer.enabled = visible;
            }
            if (_graphic != null)
                _graphic.enabled = visible;
            if (_canvas != null)
                _canvas.enabled = visible;
        }
    }
}
