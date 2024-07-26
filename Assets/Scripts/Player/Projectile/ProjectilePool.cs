using System.Collections.Generic;
using TowerDefense.Utilities;
using UnityEngine;

namespace TowerDefense.Player
{
    public class ProjectilePool : GenericObjectPool<ProjectileController>
    {
        private PlayerService playerService;
        private ProjectileView projectilePrefab;
        private List<ProjectileSO> projectileScriptableObjects;
        private Transform projectileContainer;

        public ProjectilePool(PlayerService playerService, ProjectileView projectilePrefab, List<ProjectileSO> projectileScriptableObjects)
        {
            this.playerService = playerService;
            this.projectilePrefab = projectilePrefab;
            this.projectileScriptableObjects = projectileScriptableObjects;
            this.projectileContainer = new GameObject("Projectile Container").transform;
        }

        public ProjectileController GetProjectile(ProjectileType projectileType)
        {
            ProjectileController projectile = GetItem();
            ProjectileSO scriptableObjectToUse = projectileScriptableObjects.Find(so => so.Type == projectileType);
            projectile.Init(scriptableObjectToUse);
            return projectile;
        }

        protected override ProjectileController CreateItem() => new ProjectileController(playerService, projectilePrefab, projectileContainer);
    }
}
