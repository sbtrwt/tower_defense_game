using System.Collections;
using System.Collections.Generic;
using TowerDefense.Events;
using TowerDefense.Map;
using TowerDefense.Player;
using TowerDefense.Wave;
using UnityEngine;

public class GameService : MonoBehaviour
{
    private EventService eventService;
    private MapService mapService;
    private WaveService waveService;
    private PlayerService playerService;

    [SerializeField] private MapSO mapScriptableObject;
    [SerializeField] private WaveSO waveScriptableObject;
    [SerializeField] private PlayerSO playerScriptableObject;
    private void Start()
    {
        InitializeServices();
        InjectDependencies();
    }

    private void InitializeServices()
    {
        eventService = new EventService();
        mapService = new MapService(mapScriptableObject);
        waveService = new WaveService(waveScriptableObject);
        playerService = new PlayerService(playerScriptableObject);
    }

    private void InjectDependencies()
    {
        mapService.Init(eventService);
        waveService.Init(eventService, mapService);
        //playerService.Init(mapService, uiService, soundService);
    }

}
