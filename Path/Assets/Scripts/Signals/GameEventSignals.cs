using UnityEngine;

public static class GameEventSignals
{
    /// <summary>
    /// pending
    /// </summary>
    public static event TileSelectedHandler OnTileSelected;
    public delegate void TileSelectedHandler(Tile _tile, bool _selected);
    public static void DoTileSelected(Tile _tile, bool _selected) { if (OnTileSelected != null) OnTileSelected(_tile, _selected); }

    /// <summary>
    /// pending
    /// </summary>
    public static event TerrainGeneratedHandler OnTerrainGenerated;
    public delegate void TerrainGeneratedHandler();
    public static void DoTerrainGenerated() { if (OnTerrainGenerated != null) OnTerrainGenerated(); }

    /// <summary>
    /// pending
    /// </summary>
    public static event CharacterSelectedHandler OnCharacterSelected;
    public delegate void CharacterSelectedHandler(Character _character, bool _selected);
    public static void DoCharacterSelected(Character _character, bool _selected) { if (OnCharacterSelected != null) OnCharacterSelected(_character, _selected); }

    /// <summary>
    /// pending
    /// </summary>
    public static event CurrentTurnEndHandler OnCurrentTurnEnd;
    public delegate void CurrentTurnEndHandler();
    public static void DoCurrentTurnEnd() { if (OnCurrentTurnEnd != null) OnCurrentTurnEnd(); }
}
