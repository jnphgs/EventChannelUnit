# Event Channel Unit

[![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/jnphgs/EventChannelUnit/main.yml)](https://github.com/jnphgs/EventChannelUnit/actions)
[![openupm](https://img.shields.io/npm/v/com.jnphgs.event-channel-unit?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.jnphgs.event-channel-unit/)
[![GitHub Release](https://img.shields.io/github/v/release/jnphgs/EventChannelUnit)](https://github.com/jnphgs/EventChannelUnit/releases)


## Overview

This Package is designed to integrate the EventChannel pattern into Unity's visual scripting environment.

By using the Event Channel Unit, you can easily create, manage, and trigger events through visual scripting.
This makes your game or application architecture cleaner, more modular, and easier to maintain.

## Features

- EventChannel Pattern: Implements the EventChannel pattern to decouple event publishers and subscribers, enhancing code modularity and reusability.
- Visual Scripting Support: Seamlessly integrates with Unity's visual scripting tools, allowing for drag-and-drop event management.

## UI

### Visual Scripting Unit

![vsunits](https://github.com/jnphgs/EventChannelUnit/assets/37029324/67740051-354e-4359-8226-ae663432a19c)

By default, EventUnits are implemented for the following types of EventChannels.

- void
- bool
- int
- float
- string
- Vector2
- Vector3

Additionally, you can trigger an event by calling RaiseEvent() for each EventChannel.

### Inspector

![inspector](https://github.com/jnphgs/EventChannelUnit/assets/37029324/8166c971-3aed-44b9-a91a-44810d8f81bf)

When you select an EventChannel asset, these features are available in the inspector to facilitate development:

- **Object Listeners**: Objects currently subscribing to the selected EventChannel asset's event.
- **Unit Listeners**: Visual Scripting Units currently subscribing to the selected EventChannel asset's event.
- **Raise Event**: You can publish an event with the Test Value.
- **Log**: You can view the log of events published since this asset was opened in the inspector.

## Example Usage

### Visual Scripting

#### Transition with EventUnit

![ex1](https://github.com/jnphgs/EventChannelUnit/assets/37029324/cd95fcb3-0cf2-4b65-abd0-156e68a90443)

In this example, we subscribe to an event from VoidChannel and transition without arguments, but you can also control the transition by branching conditions based on arguments.

#### Raize Event with State Graph

![ex2](https://github.com/jnphgs/EventChannelUnit/assets/37029324/d40974f5-0aa1-4b11-bd15-4d5e65251879)

In this example, we raise BoolChannel events at the Enter and Exit of the state. 
These events control the On/Off state of a panel displayed during a specific state.

At the design stage of the State Management, you only need to think about abstract things like what to display, and you can implement how to actually fade in or fade out later.

### GameObject

#### Spawn GameObject with IntChannel

```cs
using EventChannelUnit.Runtime;
using UnityEngine;
public class Spawner : MonoBehaviour
{
    [SerializeField] private IntChannel spawnChannel;
    [SerializeField] private GameObject itemPrefab;
    
    private void OnEnable()
    {
        spawnChannel.OnEventRaised += Spawn;
    }
    private void OnDisable()
    {
        spawnChannel.OnEventRaised -= Spawn;
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
}
```

In this Spawner example, GameObjects are instantiated at random positions based on the number of arguments in an IntChannel event.

#### Toggle CanvasGroup Visibility with BoolChannel

```cs
using EventChannelUnit.Runtime;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupEventListener : MonoBehaviour
{
    [SerializeField] private BoolChannel visibilityChannel;
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
        _canvasGroup.alpha = show ? 1f : 0f;
        _canvasGroup.blocksRaycasts = show;
        _canvasGroup.interactable = show;
    }
}
```
In this example, we assume a UI panel with a CanvasGroup, where visibility can be toggled based on the arguments of a BoolChannel event. 
Additionally, fading effects can be achieved by using other Tween libraries with SetVisibility.

