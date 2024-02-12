using EventChannelUnit.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace EventChannelUnit.Samples.Example.Runtime
{
    [RequireComponent(typeof(Image))]
    public class ToggleImage : MonoBehaviour
    {
        [SerializeField] private BoolChannel toggleChannel;
        private Image _image;
        private void OnEnable()
        {
            _image = GetComponent<Image>();
            toggleChannel.OnEventRaised += OnToggled;
        }
        
        private void OnDisable()
        {
            toggleChannel.OnEventRaised -= OnToggled;
        }

        private void OnToggled(bool show)
        {
            _image.enabled = show;
        }

    }
}
