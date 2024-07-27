using System.Collections;
using System.Collections.Generic;
using TowerDefense.Events;
using TowerDefense.Map;
using TowerDefense.Player;
using TowerDefense.Sound;
using TowerDefense.UI;
using TowerDefense.Wave;
using UnityEngine;

public class GameService : MonoBehaviour
{
    private EventService eventService;
    private MapService mapService;
    private WaveService waveService;
    private PlayerService playerService;
    private SoundService soundService;
    [SerializeField] private UIService uiService;

    [SerializeField] private MapSO mapScriptableObject;
    [SerializeField] private WaveSO waveScriptableObject;
    [SerializeField] private PlayerSO playerScriptableObject;
    [SerializeField] private SoundSO soundScriptableObject;

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgMusicSource;
    private void Start()
    {
        InitializeServices();
        InjectDependencies();
    }

    private void InitializeServices()
    {
        eventService = new EventService();
        soundService = new SoundService(soundScriptableObject, sfxSource, bgMusicSource);
        mapService = new MapService(mapScriptableObject);
        waveService = new WaveService(waveScriptableObject);
        playerService = new PlayerService(playerScriptableObject);
    }

    private void InjectDependencies()
    {
        mapService.Init(eventService);
        uiService.Init(waveService, playerService, eventService);
        playerService.Init(mapService, uiService, soundService);
        waveService.Init(uiService, mapService, playerService, soundService, eventService);
    }
    private void Update()
    {
        playerService.Update();
    }
}
