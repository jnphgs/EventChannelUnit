using UnityEngine;
using Random = UnityEngine.Random;

namespace EventChannelUnit.Samples.Example.Runtime
{
    [RequireComponent(typeof(MeshRenderer))]
    public class RandomColorMesh : MonoBehaviour
    {
        private void Awake()
        {
            var meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material.color = Random.ColorHSV();
        }
    }
}
