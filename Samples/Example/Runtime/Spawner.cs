using System.Collections.Generic;
using EventChannelUnit.Runtime;
using UnityEngine;

namespace EventChannelUnit.Samples.Example.Runtime
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private IntChannel spawnChannel;
        [SerializeField] private VoidChannel resetChannel;
        [SerializeField] private GameObject itemPrefab;
        private readonly List<GameObject> _items = new List<GameObject>();
        private void OnEnable()
        {
            spawnChannel.OnEventRaised += Spawn;
            resetChannel.OnEventRaised += Reset;
        }
        private void OnDisable()
        {
            spawnChannel.OnEventRaised -= Spawn;
            resetChannel.OnEventRaised -= Reset;
        }
        private void Spawn(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var item = Instantiate(itemPrefab, transform);
                item.transform.localPosition = new ()
                {
                    x = Random.Range(-1f, 1f),
                    y = Random.Range(-1f, 1f),
                    z = Random.Range(-1f, 1f)
                };
                item.transform.rotation = Random.rotation;
                _items.Add(item);
            }
        }
        private void Reset()
        {
            foreach (var item in _items) Destroy(item);
            _items.Clear();
        }
    }
}
