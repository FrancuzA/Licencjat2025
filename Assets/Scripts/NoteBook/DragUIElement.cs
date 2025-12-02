using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragUIElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Header("Drag Settings")]
    [Range(0, 1)] public float alphaThreshold = 0.1f;

    [Header("Visual Feedback")]
    [SerializeField] private float dragScaleFactor = 1.05f;
    [SerializeField] private float scaleSpeed = 10f;

    [Header("ItemInfo")]
    public ItemInfo itemInfo;

    //[Header("Audio")]
    //private AudioManager _audioManager;

    private Transform originalParent;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector3 originalPosition;
    private Vector3 originalScale;
    private Vector3 targetScale;
    private float originalZPosition;
    private bool isDragging = false;
    private Image image;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        originalScale = rectTransform.localScale;
        targetScale = originalScale;
        originalZPosition = rectTransform.position.z;
        canvasGroup = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();
        image.sprite = itemInfo.ItemUI;
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        try
        {
            image.alphaHitTestMinimumThreshold = alphaThreshold;
        }
        catch (System.InvalidOperationException)
        {
            Debug.LogWarning("Alpha hit test disabled - texture not readable", this);
        }
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            

            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay )
            {
                originalParent = gameObject.transform.parent;
                Dependencies.Instance.UnregisterDependency<DragUIElement>();
                Dependencies.Instance.RegisterDependency<DragUIElement>(this);
                originalZPosition = rectTransform.position.z;
                RectTransformUtility.ScreenPointToWorldPointInRectangle(
                    rectTransform,
                    eventData.position,
                    eventData.pressEventCamera,
                    out Vector3 worldPoint);

                isDragging = true;
                targetScale = originalScale * dragScaleFactor;
                gameObject.transform.SetParent(canvas.transform);

                if (canvasGroup != null)
                {
                    canvasGroup.blocksRaycasts = false;
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        targetScale = originalScale;
        gameObject.transform.SetParent(originalParent.transform);
        gameObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging && canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                canvas.GetComponent<RectTransform>(),
                eventData.position,
                eventData.pressEventCamera,
                out Vector3 worldPoint);

            worldPoint.z = originalZPosition;
            rectTransform.position = worldPoint;
        }
    }

    private void Update()
    {
        if (rectTransform.localScale != targetScale)
        {
            rectTransform.localScale = Vector3.Lerp(
                rectTransform.localScale,
                targetScale,
                Time.deltaTime * scaleSpeed);
        }
    }
}
