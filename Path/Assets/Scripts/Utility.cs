using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static Color ModifyAlpha(Color _color, float _alpha)
    {
        _color.a = _alpha;
        return _color;
    }

    public static void ModifyAlpha(Renderer _renderer, float _alpha)
    {
        _renderer.material.color = ModifyAlpha(_renderer.material.color, _alpha);
    }

    public static Queue<Character> GetFaction(Character _character)
    {
        switch (_character.faction) {
            case Character.EnumFactionType.firefolk:
                return Character.firefolkFaction;
            case Character.EnumFactionType.treefolk:
                return Character.treefolkFaction;
            case Character.EnumFactionType.undead:
                return Character.undeadFaction;
            default:
                return null;
        }
    }

    public static Tile.EnumMarkType GetTileMark(char _mark)
    {
        switch (_mark) {
            case 'X':
                return Tile.EnumMarkType.nonWalkable;
            case ',':
                return Tile.EnumMarkType.walkable;
            default:
                return Tile.EnumMarkType.walkable;
        }
    }

    public static int GetTileWeight(Tile.EnumMarkType _mark)
    {
        switch (_mark) {
            case Tile.EnumMarkType.nonWalkable:
                return -1;
            default:
                return 1;
        }
    }
}
