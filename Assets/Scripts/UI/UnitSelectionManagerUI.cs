using System;
using UnityEngine;

public class UnitSelectionManagerUI : MonoBehaviour
{
    [SerializeField] private RectTransform selectionAreaRectTransform;
    [SerializeField] private Canvas canvas;


    private void Start()
    {
        UnitSelectionManager.Instance.OnSelectionAreaStart += UnitSlelectionManager_OnSelectionAreaStart;
        UnitSelectionManager.Instance.OnSelectionAreaEnd += UnitSelectionManager_OnSelectionAreaEnd;

        selectionAreaRectTransform.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (selectionAreaRectTransform.gameObject.activeSelf)
        {
            UpdateVisual();
        }
    }
    private void UnitSlelectionManager_OnSelectionAreaStart(object sender, EventArgs e)
    {
        selectionAreaRectTransform.gameObject.SetActive(true);
        UpdateVisual();
    }
    private void UnitSelectionManager_OnSelectionAreaEnd(object sender, EventArgs e)
    {
        selectionAreaRectTransform.gameObject.SetActive(false);
    }

    private void UpdateVisual()
    {
        Rect selectionAreaRect = UnitSelectionManager.Instance.GetSelectionAreaRect();
        float scaleFactor = canvas.scaleFactor;
        selectionAreaRectTransform.anchoredPosition = selectionAreaRect.position / scaleFactor;
        selectionAreaRectTransform.sizeDelta = selectionAreaRect.size / scaleFactor;
    }


}
