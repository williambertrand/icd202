using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;

namespace UI
{
    /**
     * Each canvas is a different menu screen.
     */
    public enum CanvasType
    {
        MainMenu,
        GameUI,
        Pause,
        Settings,
        GameOver,
    }

    /**
     * Manage which canvas is currently active.
     */
    public class CanvasManager : MonoBehaviour
    {

        private Dictionary<CanvasType, CanvasController> _canvasControllers;

        private CanvasType? _previousCanvas;
    
        private void Awake()
        {
            _canvasControllers = GetComponentsInChildren<CanvasController>(true).ToDict(x => x.canvasType, x => x);

            SetActiveCanvas(CanvasType.MainMenu);
        }

        private void SetActiveCanvas(CanvasType? type)
        {
            Debug.Log($"Switching to canvas: {type}");
            _canvasControllers.ForEach((k, v) =>
            {
                var isActive = v.gameObject.activeSelf;
                var isNewCanvas = k.Equals(type);
                if (isActive && !isNewCanvas) _previousCanvas = k;
                v.gameObject.SetActive(isNewCanvas);
            });
        }

        public void SwitchCanvas(CanvasType type) => SetActiveCanvas(type);

        public void GoBackToLastCanvas()
        {
            Debug.Log($"prewv: {_previousCanvas}");
            if (_previousCanvas == null) return;
        
            SetActiveCanvas(_previousCanvas ?? CanvasType.MainMenu);
        }
    }
}