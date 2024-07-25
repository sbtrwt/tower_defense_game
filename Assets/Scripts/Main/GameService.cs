using System.Collections;
using System.Collections.Generic;
using TowerDefense.Events;
using TowerDefense.Map;
using TowerDefense.Wave;
using UnityEngine;

public class GameService : MonoBehaviour
{
    private EventService eventService;
    private MapService mapService;
    private WaveService waveService;

    [SerializeField] private MapSO mapScriptableObject;
    [SerializeField] private WaveSO waveScriptableObject;
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
    }

    private void InjectDependencies()
    {
        mapService.Init(eventService);
        waveService.Init(eventService, mapService);
    }

}
