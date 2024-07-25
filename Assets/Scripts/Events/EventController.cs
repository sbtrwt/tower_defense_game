﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense.Events
{
    public class EventController
    {
        public event Action BaseEvent;
        public void InvokeEvent() => BaseEvent?.Invoke();
        public void AddListener(Action listener) => BaseEvent += listener;
        public void RemoveListener(Action listener) => BaseEvent -= listener;
    }

    public class EventController<T>
    {
        public event Action<T> BaseEvent;
        public void InvokeEvent(T type) => BaseEvent?.Invoke(type);
        public void AddListener(Action<T> listener) => BaseEvent += listener;
        public void RemoveListener(Action<T> listener) => BaseEvent -= listener;
    }
}
