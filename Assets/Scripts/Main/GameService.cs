using System.Collections;
using System.Collections.Generic;
using TowerDefense.Events;
using TowerDefense.Map;
using UnityEngine;

public class GameService : MonoBehaviour
{
    private EventService eventService;
    private MapService mapService;

    [SerializeField] private MapSO mapScriptableObject;

    private void Start()
    {
        InitializeServices();
        InjectDependencies();
    }

    private void InitializeServices()
    {
        eventService = new EventService();
        mapService = new MapService(mapScriptableObject);
       
    }

    private void InjectDependencies()
    {
        mapService.Init(eventService);
       
    }

}
