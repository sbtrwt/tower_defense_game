using System.Collections.Generic;
using TowerDefense.Events;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TowerDefense.Map
{
    public class MapService
    {
        private MapSO mapScriptableObject;
        private SpriteRenderer tileOverlay;
        private MapData currentMapData;
        private Grid currentGrid;
        private Tilemap currentTileMap;

        private EventService eventService;
        public MapService(MapSO mapScriptableObject)
        {
            this.mapScriptableObject = mapScriptableObject;
            tileOverlay = Object.Instantiate(mapScriptableObject.TileOverlay).GetComponent<SpriteRenderer>();
            ResetTileOverlay();
            LoadMap(1);
        }
        public void Init(EventService eventService)
        {
            this.eventService = eventService;
            SubscribeToEvents();
        }
        private void SubscribeToEvents() => eventService.OnMapSelected.AddListener(LoadMap);
        private void LoadMap(int mapId)
        {
            currentMapData = mapScriptableObject.MapDatas.Find(mapData => mapData.MapID == mapId);
            currentGrid = Object.Instantiate(currentMapData.MapPrefab);
            currentTileMap = currentGrid.GetComponentInChildren<Tilemap>();
        }
        private void ResetTileOverlay() => SetTileOverlayColor(TileOverlayColor.TRANSPARENT);

        private void SetTileOverlayColor(TileOverlayColor colorToSet)
        {
            switch (colorToSet)
            {
                case TileOverlayColor.TRANSPARENT:
                    tileOverlay.color = mapScriptableObject.DefaultTileColor;
                    break;
                case TileOverlayColor.SPAWNABLE:
                    tileOverlay.color = mapScriptableObject.SpawnableTileColor;
                    break;
                case TileOverlayColor.NON_SPAWNABLE:
                    tileOverlay.color = mapScriptableObject.NonSpawnableTileColor;
                    break;
            }
        }
        public Vector3 GetEnemySpawnPositionForCurrentMap() => currentMapData.SpawningPoint;
        public List<Vector3> GetWayPointsForCurrentMap() => currentMapData.WayPoints;
        private enum TileOverlayColor
        {
            TRANSPARENT,
            SPAWNABLE,
            NON_SPAWNABLE
        }
    }
}