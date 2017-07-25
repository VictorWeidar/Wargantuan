using System.Collections.Generic;
using UnityEngine;
using CCKit;

public class Tile : MonoBehaviour
{
    public enum EnumMarkType { walkable, nonWalkable }

    public float unhighlightedAlpha = 1.0f;
    public float highlightedAlpha = 0.2f;
    public float selectedAlpha = 0.5f;

    public AdjacencyListNode<Tile> node;
    public EnumMarkType Mark { get; set; }
    public bool Highlighted { get; private set; }

    public static bool selectedTileChanged;
    bool selected;
    public bool Selected {
        get { return selected; }
        set
        {
            if (selected = value) {
                selectedTileChanged = false;
                if (prevSelectedTile == null || prevSelectedTile != this) {
                    selectedTileChanged = true;
                    if (prevSelectedTile != null)
                        prevSelectedTile.Selected = false;
                    GameEventSignals.DoTileSelected(prevSelectedTile, false);
                    GameEventSignals.DoTileSelected(this, true);
                }
                prevSelectedTile = this;
            }
        }
    }

    public static Stack<Tile> highlightedTiles = new Stack<Tile>();
    public static Tile prevSelectedTile = null;

    void Awake()
    {
        Selected = Highlighted = false;
        GameEventSignals.OnTileSelected += OnTileSelected;
    }

    void OnTileSelected(Tile _tile, bool _selected)
    {
        if (this == _tile) {
            if (_selected) {
                GetHighlighted(selectedAlpha);
            }
            else {
                GetHighlighted(highlightedAlpha);
            }
        }
    }

    void OnDestroy()
    {
        GameEventSignals.OnTileSelected -= OnTileSelected;
    }

    public void GetHighlighted(float _alpha)
    {
        var tileRenderer = GetComponent<MeshRenderer>();
        Utility.ModifyAlpha(tileRenderer, _alpha);
        Highlighted = true;
        highlightedTiles.Push(this);
    }

    public static void UnhighlightAll()
    {
        while (highlightedTiles.Count > 0) {
            var highlightedTile = highlightedTiles.Pop();
            highlightedTile.Highlighted = false;
            Utility.ModifyAlpha(highlightedTile.GetComponent<MeshRenderer>(), highlightedTile.unhighlightedAlpha);
        }
    }
}
