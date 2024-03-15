using System;
using EventChannelUnit.Runtime;
using UnityEngine;

namespace EventChannelUnit.Samples.Example.Runtime.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupEventListener : MonoBehaviour
    {
        [SerializeField] private BoolChannel visibilityChannel;
        [SerializeField] private float minAlpha;
        [SerializeField] private bool defaultIsActive = false;
        private CanvasGroup _canvasGroup;
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            SetVisibility(defaultIsActive);
        }
        private void OnEnable()
        {
            visibilityChannel.OnEventRaised += SetVisibility;
        }

        private void OnDisable()
        {
            visibilityChannel.OnEventRaised -= SetVisibility;
        }

        private void SetVisibility(bool show)
        {
            _canvasGroup.alpha = show ? 1f : minAlpha;
            _canvasGroup.blocksRaycasts = show;
            _canvasGroup.interactable = show;
        }
    }
}
