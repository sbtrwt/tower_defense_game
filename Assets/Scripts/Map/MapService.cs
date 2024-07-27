using System.Collections.Generic;
using TowerDefense.Enemy;
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

        public void ValidateSpawnPosition(Vector3 cursorPosition)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(cursorPosition);
            Vector3Int cellPosition = GetCellPosition(mousePosition);
            Vector3 cellCenter = GetCenterOfCell(cellPosition);

            if (CanSpawnOnPosition(cellCenter, cellPosition))
            {
                tileOverlay.transform.position = cellCenter;
                SetTileOverlayColor(TileOverlayColor.SPAWNABLE);
            }
            else
            {
                tileOverlay.transform.position = cellCenter;
                SetTileOverlayColor(TileOverlayColor.NON_SPAWNABLE);
            }
        }
        private Vector3Int GetCellPosition(Vector3 mousePosition) => currentGrid.WorldToCell(new Vector3(mousePosition.x, mousePosition.y, 0));

        private Vector3 GetCenterOfCell(Vector3Int cellPosition) => currentGrid.GetCellCenterWorld(cellPosition);

        private bool CanSpawnOnPosition(Vector3 centerCell, Vector3Int cellPosition)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(centerCell, 0.1f);
            return InisdeTilemapBounds(cellPosition) && !HasClickedOnObstacle(colliders) && !IsOverLappingEnemy(colliders);
        }
        private bool HasClickedOnObstacle(Collider2D[] colliders)
        {
            foreach (Collider2D collider in colliders)
            {
                if (collider.GetComponent<TilemapCollider2D>() != null)
                    return true;
            }
            return false;
        }

        private bool IsOverLappingEnemy(Collider2D[] colliders)
        {
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.GetComponent<EnemyView>() != null && !collider.isTrigger)
                    return true;
            }
            return false;
        }
        private bool InisdeTilemapBounds(Vector3Int mouseToCell)
        {
            BoundsInt tilemapBounds = currentTileMap.cellBounds;
            return tilemapBounds.Contains(mouseToCell);
        }
        public bool TryGetTowerSpawnPosition(Vector3 cursorPosition, out Vector3 spawnPosition)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(cursorPosition);
            Vector3Int cellPosition = GetCellPosition(mousePosition);
            Vector3 cellCenter = GetCenterOfCell(cellPosition);

            ResetTileOverlay();

            if (CanSpawnOnPosition(cellCenter, cellPosition))
            {
                spawnPosition = cellCenter;
                return true;
            }
            else
            {
                spawnPosition = Vector3.zero;
                return false;
            }
        }
        private enum TileOverlayColor
        {
            TRANSPARENT,
            SPAWNABLE,
            NON_SPAWNABLE
        }
    }
}