using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VenetStudio
{
    public interface IDamageable
    {
        public void Damage(int amount);
    }

    public interface IInteractible
    {
        public void Register();
        public void Interact(Action onInteractionComplete);
    }
}