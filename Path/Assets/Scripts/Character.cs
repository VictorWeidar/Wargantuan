using System.Collections.Generic;
using UnityEngine;
using CCKit;

public class Character : MonoBehaviour
{
    public enum EnumFactionType { firefolk, treefolk, undead }

    public static LinkedList<Character> all = new LinkedList<Character>();
    [HideInInspector] public Tile attachedTile;

    public EnumFactionType faction;
    public float speed = 1.0f;
    public int movementRange = 7;
    public float selectedAlpha = 0.4f;
    public float deselectedAlpha = 1.0f;

    public Vector3 offset = Vector3.up;
    public Stack<Tile> path = new Stack<Tile>();
    Vector3 current = Vector3.zero, next = Vector3.zero;

    MeshRenderer[] meshRenderer;

    bool moving;
    public bool Moving {
        get { return moving; }
        set {
            if (moving = value)
                movingCharacter = this;
            else
                movingCharacter = null;
        }
    }

    bool selected;
    public bool Selected {
        get { return selected; }
        set {
            if (selected = value) {
                foreach (var elem in all)
                    if (elem != this)
                        elem.Selected = false;
                GameEventSignals.DoCharacterSelected(selectedCharacter = this, true);
            }
            else {
                GameEventSignals.DoCharacterSelected(this, false);
                selectedCharacter = null;
            }
        }
    }

    public static Character selectedCharacter = null;
    public static Character movingCharacter = null;

    public static Queue<Character> firefolkFaction = new Queue<Character>();
    public static Queue<Character> treefolkFaction = new Queue<Character>();
    public static Queue<Character> undeadFaction = new Queue<Character>();
    public static List<Queue<Character>> allFactions = new List<Queue<Character>>();
    public static int activeFactionIndex = 0;

    void Awake()
    {
        GameEventSignals.OnCharacterSelected += OnCharacterSelected;

        all.AddLast(this);
        Moving = false;
        AddToFaction();

        meshRenderer = GetComponentsInChildren<MeshRenderer>();
    }

    void Update()
    {
        Move();
    }

    void OnCharacterSelected(Character _character, bool _selected)
    {
        if (this == _character) {
            if (_selected == true)
                foreach (var elem in meshRenderer)
                    Utility.ModifyAlpha(elem, selectedAlpha);
            else
                foreach (var elem in meshRenderer)
                    Utility.ModifyAlpha(elem, deselectedAlpha);
        }
    }

    void OnDestroy()
    {
        GameEventSignals.OnCharacterSelected -= OnCharacterSelected;
    }

    void Move()
    {
        if (Moving) {
            if (current == next) {
                current = transform.position;
                if (path.Count == 0) {
                    Moving = false;
                    return;
                }
                next = path.Pop().transform.position + offset;
                transform.LookAt(next);
            }
            else {
                transform.position += transform.forward * (speed * Time.deltaTime);
                if (Vector3.Distance(current, next) < Vector3.Distance(current, transform.position))
                    transform.position = current = next;
            }
        }
    }

    public bool GetSelected()
    {
        if (!Moving) {
            Tile.UnhighlightAll();
            Selected = true;

            ManageTerrain.Ins.graph.BFSearch(attachedTile.node, movementRange, (AdjacencyListNode<Tile> _node) => {
                _node.mVal.GetHighlighted(_node.mVal.selectedAlpha);
            }, (AdjacencyListNode<Tile> _node) => {
                _node.mVal.GetHighlighted(_node.mVal.highlightedAlpha);
            });

            return true;
        }
        return false;
    }

    public void MoveTo(Tile _target)
    {
        if (_target.Highlighted && !Moving) {
            _target.node.mOccupied = true;
            attachedTile.node.mOccupied = false;
            attachedTile = _target;

            var currentNode = _target.node;
            while (currentNode.mDistance != 0) {
                path.Push(currentNode.mVal);
                currentNode = currentNode.mPredecessor;
            }
            Moving = true;
        }
    }

    public static Character GetSelectedInstance()
    {
        foreach (var elem in all)
            if (elem.selected)
                return elem;
        return null;
    }

    void AddToFaction()
    {
        var currentFaction = Utility.GetFaction(this);
        if (currentFaction.Count == 0)
            allFactions.Add(currentFaction);
        currentFaction.Enqueue(this);
    }
}
