using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TowerDefense.Wave;
using TowerDefense.Player;
using TowerDefense.Events;

namespace TowerDefense.UI
{
    public class UIService : MonoBehaviour
    {
        private WaveService waveService;
        private EventService eventService;

        [Header("Gameplay Panel")]
        [SerializeField] private GameObject gameplayPanel;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI waveProgressText;
        [SerializeField] private TextMeshProUGUI currentMapText;
        [SerializeField] private Button nextWaveButton;

        [Header("Level Selection Panel")]
        [SerializeField] private GameObject levelSelectionPanel;
        [SerializeField] private List<MapButton> mapButtons;

        [Header("Tower Selection UI")]
        private TowerSelectionUIController towerSelectionController;
        [SerializeField] private GameObject TowerSelectionPanel;
        [SerializeField] private Transform cellContainer;
        [SerializeField] private TowerCellView towerCellPrefab;
        [SerializeField] private List<TowerCellSO> towerCellScriptableObjects;

        [Header("Game End Panel")]
        [SerializeField] private GameObject gameEndPanel;
        [SerializeField] private TextMeshProUGUI gameEndText;
        [SerializeField] private Button playAgainButton;
        [SerializeField] private Button quitButton;

        private void Start()
        {
            gameplayPanel.SetActive(false);
            gameEndPanel.SetActive(false);

            nextWaveButton.onClick.AddListener(OnNextWaveButton);
            quitButton.onClick.AddListener(OnQuitButtonClicked);
            playAgainButton.onClick.AddListener(OnPlayAgainButtonClicked);
        }

        public void Init(WaveService waveService, PlayerService playerService, EventService eventService)
        {
            this.waveService = waveService;
            this.eventService = eventService;

            InitializeMapSelectionUI(eventService);
            InitializeTowerSelectionUI(playerService);
            SubscribeToEvents();
        }

        private void InitializeMapSelectionUI(EventService eventService)
        {
            levelSelectionPanel.SetActive(true);
            foreach (MapButton mapButton in mapButtons)
            {
                mapButton.Init(eventService);
            }
        }

        private void InitializeTowerSelectionUI(PlayerService playerService)
        {
            towerSelectionController = new TowerSelectionUIController( cellContainer, towerCellPrefab, towerCellScriptableObjects, playerService);
            TowerSelectionPanel.SetActive(false);
            towerSelectionController.SetActive(false);
        }

        private void SubscribeToEvents() => eventService.OnMapSelected.AddListener(OnMapSelected);

        public void OnMapSelected(int mapID)
        {
            levelSelectionPanel.SetActive(false);
            gameplayPanel.SetActive(true);
            TowerSelectionPanel.SetActive(true);
            towerSelectionController.SetActive(true);
            currentMapText.SetText("Map: " + mapID);
        }

        private void OnNextWaveButton()
        {
            waveService.StarNextWave();
            SetNextWaveButton(false);
        }

        private void OnQuitButtonClicked() => Application.Quit();

        private void OnPlayAgainButtonClicked() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        public void SetNextWaveButton(bool setInteractable) => nextWaveButton.interactable = setInteractable;

        public void UpdateHealthUI(int healthToDisplay) => healthText.SetText(healthToDisplay.ToString());

        public void UpdateMoneyUI(int moneyToDisplay) => moneyText.SetText(moneyToDisplay.ToString());

        public void UpdateWaveProgressUI(int waveCompleted, int totalWaves) => waveProgressText.SetText(waveCompleted.ToString() + "/" + totalWaves.ToString());

        public void UpdateGameEndUI(bool hasWon)
        {
            gameplayPanel.SetActive(false);
            levelSelectionPanel.SetActive(false);
            gameEndPanel.SetActive(true);

            if (hasWon)
                gameEndText.SetText("You Won");
            else
                gameEndText.SetText("Game Over");
        }

    }
}
