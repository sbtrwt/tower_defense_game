using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TowerDefense.UI
{
    public class TowerImageHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
    {
        private RectTransform rectTransform;
        private Image towerImage;
        private TowerCellController owner;

        private Sprite spriteToSet;
        private Vector2 originalAnchoredPosition;
        private Vector3 originalPosition;

        public void ConfigureImageHandler(Sprite spriteToSet, TowerCellController owner)
        {
            this.spriteToSet = spriteToSet;
            this.owner = owner;
        }

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            towerImage = GetComponent<Image>();
            towerImage.sprite = spriteToSet;
            originalPosition = rectTransform.localPosition;
            originalAnchoredPosition = rectTransform.anchoredPosition;
        }

        public void OnPointerDown(PointerEventData eventData) => towerImage.color = new Color(1, 1, 1, 0.6f);

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta;
            owner.TowerDraggedAt(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ResetTowerImage();
            owner.TowerDroppedAt(eventData.position);
        }

        private void ResetTowerImage()
        {
            towerImage.color = new Color(1, 1, 1, 1f);
            rectTransform.anchoredPosition = originalAnchoredPosition;
            rectTransform.localPosition = originalPosition;
            GetComponent<LayoutElement>().enabled = false;
            GetComponent<LayoutElement>().enabled = true;
        }
    }
}
