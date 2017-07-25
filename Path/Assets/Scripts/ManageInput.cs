using System.Collections.Generic;
using UnityEngine;
using CCKit;

public class ManageInput : MonoBehaviour
{
    public static ManageInput Ins { get; private set; }

    public GameObject prefabCharacter;
    public GameObject prefabCharacter2;
    public GameObject prefabCharacter3;

    public delegate void AxisFire1DownDel();
    public AxisFire1DownDel axisFire1Down;

    void Awake()
    {
        Ins = this;
        InputSignals.OnAxisFire1Down += OnAxisFire1Down;
        axisFire1Down = ManageCharacter;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            ShowRange();
    }

    void OnAxisFire1Down()
    {
        axisFire1Down();
    }

    void OnDestroy()
    {
        InputSignals.OnAxisFire1Down -= OnAxisFire1Down;
    }

    public void ManageCharacter()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit)) {
            var target = raycastHit.collider.gameObject;
            if (target.CompareTag("Character")) {
                var character = target.GetComponent<Character>();
                character.GetSelected();
            }
            else if (target.CompareTag("Tile")) {
                if (Character.selectedCharacter == null || Character.movingCharacter != null) return;
                
                var tile = target.GetComponent<Tile>();
                if (!tile.Highlighted) {
                    if (Character.selectedCharacter != null)
                        Character.selectedCharacter.Selected = false;
                    Tile.UnhighlightAll();
                    return;
                }

                tile.Selected = true;
                if (Tile.selectedTileChanged) return;
                
                var selectedCharacter = Character.GetSelectedInstance();
                if (selectedCharacter != null)
                    selectedCharacter.MoveTo(tile);
                Tile.UnhighlightAll();
                GameEventSignals.DoTileSelected(tile, true);
            }
        }// if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit))
    }

    void ShowRange()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit)) {
            var target = raycastHit.collider.gameObject;
            if (target.CompareTag("Tile")) {
                if (Character.selectedCharacter != null)
                    Character.selectedCharacter.Selected = false;
                Tile.UnhighlightAll();

                var tile = target.GetComponent<Tile>();
                if (tile.Mark == Tile.EnumMarkType.walkable) {
                    ManageTerrain.Ins.graph.BFSearch(tile.node, ManageTerrain.Ins.defaultRange, (AdjacencyListNode<Tile> _node) => {
                        _node.mVal.GetHighlighted(tile.selectedAlpha);
                    }, (AdjacencyListNode<Tile> _node) => {
                        _node.mVal.GetHighlighted(tile.highlightedAlpha);
                    });
                }
            }
        }
    }

    public void SpawnCharacter()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit)) { 
            var target = raycastHit.collider.gameObject;
            if (target.CompareTag("Tile")) {
                var tile = target.GetComponent<Tile>();
                if (tile.Mark == Tile.EnumMarkType.walkable) {
                    var character = Instantiate(prefabCharacter).GetComponent<Character>();

                    tile.node.mOccupied = true;
                    character.attachedTile = tile.node.mVal;
                    character.transform.position = character.attachedTile.transform.position + character.offset;
                }
            }
        }
    }

    public void SpawnCharacter2()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit))
        {
            var target = raycastHit.collider.gameObject;
            if (target.CompareTag("Tile"))
            {
                var tile = target.GetComponent<Tile>();
                if (tile.Mark == Tile.EnumMarkType.walkable)
                {
                    var character = Instantiate(prefabCharacter2).GetComponent<Character>();

                    tile.node.mOccupied = true;
                    character.attachedTile = tile.node.mVal;
                    character.transform.position = character.attachedTile.transform.position + character.offset;
                }
            }
        }
    }

    public void SpawnCharacter3()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit))
        {
            var target = raycastHit.collider.gameObject;
            if (target.CompareTag("Tile"))
            {
                var tile = target.GetComponent<Tile>();
                if (tile.Mark == Tile.EnumMarkType.walkable)
                {
                    var character = Instantiate(prefabCharacter3).GetComponent<Character>();

                    tile.node.mOccupied = true;
                    character.attachedTile = tile.node.mVal;
                    character.transform.position = character.attachedTile.transform.position + character.offset;
                }
            }
        }
    }
}
