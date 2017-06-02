using UnityEngine;
using System.Collections;

namespace Neutral
{
    public interface IEnemyState
    {
        void UpdateState();

        void OnTriggerEnter(Collider other);

        void ToPatrolState();

        void ToAlertState();

        void ToChaseState();

        void ToCombatState();

    }
}

