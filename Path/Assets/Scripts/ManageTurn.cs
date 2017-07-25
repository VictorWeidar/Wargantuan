using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageTurn : MonoBehaviour
{
    void Awake()
    {
        GameEventSignals.OnCurrentTurnEnd += OnCurrentTurnEnd;
    }

    void OnDestroy()
    {
        GameEventSignals.OnCurrentTurnEnd -= OnCurrentTurnEnd;
    }

    void OnCurrentTurnEnd()
    {
        EndCurrentTurn();
    }

    void EndCurrentTurn()
    {
        Character.activeFactionIndex = (Character.activeFactionIndex + 1) % Character.allFactions.Count;
        for (int i = Character.allFactions.Count - 1; i >= 0; --i) {
            if (i != Character.activeFactionIndex)
                foreach (var elem in Character.allFactions[i])
                    elem.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            else
                foreach (var elem in Character.allFactions[i])
                    elem.gameObject.layer = LayerMask.NameToLayer("Default");
        }

        if (Character.selectedCharacter != null)
            Character.selectedCharacter.Selected = false;
        Tile.UnhighlightAll();
    }
}
