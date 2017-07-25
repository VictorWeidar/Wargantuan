using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CCKit;

public class ManageTerrain : MonoBehaviour
{
    public static ManageTerrain Ins { get; private set; }

    public GameObject prefabTile;
    public GameObject prefabTile2;

    public AdjacencyList<Tile> graph = new AdjacencyList<Tile>();
    List<List<AdjacencyListNode<Tile>>> nodes = new List<List<AdjacencyListNode<Tile>>>();

    public int defaultRange = 5;
    public float gridYPos = 0.0f;
    [Tooltip("the separation between adjacent tiles")]
    [SerializeField] float separation;

    string levelFileDirPath;
    int rows = 0, cols = 0;
    [HideInInspector] public float rowLength;
    [HideInInspector] public float ColLength;
    delegate void SetEdgeDel(AdjacencyListNode<Tile> _head, AdjacencyListNode<Tile> _tail);

    void Awake()
    {
        Ins = this;
        SceneManager.sceneLoaded += OnSceneLoaded;

        levelFileDirPath = System.IO.Path.GetDirectoryName(Application.dataPath);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        int levelIndex = _scene.buildIndex;
        GenerateTerrain(levelIndex);
        GameEventSignals.DoTerrainGenerated();
    }

    void GenerateTerrain(int _levelIndex)
    {
        string[] lines = System.IO.File.ReadAllLines(string.Format("{0}/Level Files/level_{1}.csv", Application.streamingAssetsPath, _levelIndex));
        rows = lines.Length;
        for (int row = 0, cols0 = 0; row < rows; ++row, cols0 = 0) {
            nodes.Add(new List<AdjacencyListNode<Tile>>());

            var stringReader = new System.IO.StringReader(lines[row]);
            int currentChar, mark = 0;
            GameObject currentPrefabTile = null;
            while ((currentChar = stringReader.Peek()) != -1) {
                stringReader.Read();
                if ((char)currentChar == ',') {
                    if (mark == 0) {
                        currentPrefabTile = prefabTile;
                        goto Proc0;
                    }
                    mark = 0;
                    continue;
                }
                else if ((char)currentChar == 'X') {
                    if (mark == 0) {
                        mark = 1;
                        currentPrefabTile = prefabTile2;
                    }
                }
                else {
                    if (mark == 0) {
                        mark = 1;
                        currentPrefabTile = prefabTile;
                    }
                }
            Proc0:
                var tile = Instantiate(currentPrefabTile, new Vector3(separation * cols0++, gridYPos, -separation * row), prefabTile.transform.rotation).GetComponent<Tile>();
                tile.Mark = Utility.GetTileMark((char)currentChar);
                nodes[row].Add(tile.node = graph.AddVertex(tile));
            }// while ((currentChar = stringReader.Peek()) != -1)

            if (cols < cols0)
                cols = cols0;
        }
        
        for (int row = 0; row < rows; ++row) {
            for (int col = nodes[row].Count; col < cols; ++col) {
                var tile = Instantiate(prefabTile2, new Vector3(separation * col, gridYPos, -separation * row), prefabTile.transform.rotation).GetComponent<Tile>();
                tile.Mark = Tile.EnumMarkType.nonWalkable;
                nodes[row].Add(tile.node = graph.AddVertex(tile));
            }
        }

        for (int i = 0; i < rows; ++i) {
            for (int j = 0; j < cols; ++j) {
                SetEdgeDel SetEdge = (AdjacencyListNode<Tile> _head, AdjacencyListNode<Tile> _tail) => {
                    graph.SetEdge(_head, _tail, Utility.GetTileWeight(_tail.mVal.Mark));
                };
                if (i != 0)
                    SetEdge(nodes[i][j], nodes[i - 1][j]);
                if (i != rows - 1)
                    SetEdge(nodes[i][j], nodes[i + 1][j]);
                if (j != 0)
                    SetEdge(nodes[i][j], nodes[i][j - 1]);
                if (j != cols - 1)
                    SetEdge(nodes[i][j], nodes[i][j + 1]);
            }
        }

        var tileRenderer = prefabTile.GetComponent<MeshRenderer>();
        rowLength = (cols - 1) * (separation - tileRenderer.bounds.extents.x) + cols * tileRenderer.bounds.extents.x;
        ColLength = (rows - 1) * (separation - tileRenderer.bounds.extents.z) + rows * tileRenderer.bounds.extents.z;
    }
}
