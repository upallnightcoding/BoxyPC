using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCntrl : MonoBehaviour
{
    public static Player player;

    private const int LEFT_MOUSE_BUTTON = 0;

    [SerializeField] private GameData gameData;
    [SerializeField] private GameObject pegPreFab;
    [SerializeField] private Transform cameraPos;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Material drawLineMaterial;
    [SerializeField] private GameObject player1PreFab;
    [SerializeField] private GameObject player2PreFab;

    private int boardSize;
    private Peg pegStart;

    private Color player1Color;
    private Color player2Color;

    private GameState state;

    private static Wall[,] wall;

    private List<GameObject> listGameObject;

    // Start is called before the first frame update
    void Start()
    {
        boardSize = 4;

        listGameObject = new List<GameObject>();

        state = GameState.SELECT_ANCHOR;

        player = Player.NO_PLAY;

        lineRenderer.positionCount = 2;

        player1Color = Color.white;
        player2Color = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        // Read user keyboard guestures
        //-----------------------------
        bool leftMouseButton = Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON);
        Vector3 mousePos = Input.mousePosition;

        Debug.Log(player);

        switch(player)
        {
            case Player.PLAYER_1:
                MakeMove(leftMouseButton, mousePos);
                break;
            case Player.PLAYER_2:
                MakeMove(leftMouseButton, mousePos);
                break;
            case Player.NO_PLAY:
                break;
            case Player.START_GAME:
                boardSize = gameData.boardSize;
                wall = new Wall[boardSize - 1, boardSize - 1];
                CreateBoard();
                player = Player.PLAYER_1;
                break;
            case Player.STOP_GAME:
                player = Player.NO_PLAY;

                foreach(GameObject go in listGameObject)
                {
                    Destroy(go);
                }

                break;
        }
    }

    public static void StartPlay()
    {
        player = Player.START_GAME;
    }

    public static void StopPlay()
    {
        player = Player.STOP_GAME;
    }

    private void MakeMove(bool leftMouseButton, Vector3 mousePos)
    {
        switch(state)
        {
            case GameState.SELECT_ANCHOR:
                state = SelectAnchorPeg(leftMouseButton, mousePos);
                break;
            case GameState.SELECT_PIN:
                state = SelectPinPeg(leftMouseButton, mousePos);

                if (state == GameState.SELECT_ANCHOR)
                {
                    TogglePlayer();
                }

                if (state == GameState.SELECT_CANCEL)
                {
                    state = GameState.SELECT_ANCHOR;
                }
                break;
        }
    }

    private GameState SelectAnchorPeg(bool leftMouseButton, Vector3 mousePos)
    {
        GameState state = GameState.SELECT_ANCHOR;

        if (leftMouseButton)
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            RaycastHit2D hits = Physics2D.GetRayIntersection(ray);

            if (hits.collider != null)
            {
                Peg peg = hits.collider.GetComponent<Peg>();

                if (peg.IsOpen())
                {
                    peg.Selected();

                    pegStart = peg;

                    state = GameState.SELECT_PIN;
                }
            }
        }

        return (state);
    }

    private GameState SelectPinPeg(bool leftMouseButton, Vector3 mousePos)
    {
        GameState state = GameState.SELECT_PIN;

        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (leftMouseButton)
        {
            RaycastHit2D hits = Physics2D.GetRayIntersection(ray);

            if (hits.collider != null)
            {
                Peg peg = hits.collider.GetComponent<Peg>();

                if (peg.IsOpen() && LegalMove(pegStart, peg))
                {
                    lineRenderer.SetPosition(0, pegStart.GetPosition);
                    lineRenderer.SetPosition(1, pegStart.GetPosition);

                    CreateRenderLine(pegStart, peg);
                    LinkPegs(pegStart, peg);
                    UpDateBoxSideCount(pegStart, peg);

                    pegStart.Reset();
                    peg.Reset();

                    pegStart.DisplayCount();
                    peg.DisplayCount();

                    state = GameState.SELECT_ANCHOR;

                    AudioManager.Instance.SoundLegalMove();
                } 
            } else
            {
                lineRenderer.SetPosition(0, pegStart.GetPosition);
                lineRenderer.SetPosition(1, pegStart.GetPosition);

                pegStart.Reset();

                state = GameState.SELECT_CANCEL;
            }
        } else
        {
            lineRenderer.SetPosition(0, pegStart.GetPosition);
            lineRenderer.SetPosition(1, ray.GetPoint(0.0f));

            RaycastHit2D hits = Physics2D.GetRayIntersection(ray);

            if (hits.collider != null)
            {
                Peg peg = hits.collider.GetComponent<Peg>();

                if (!LegalMove(pegStart, peg))
                {
                    peg.Illegal();
                    //AudioManager.Instance.SoundIllegalMove();
                }
            } 
        }

        return (state);
    }

    private void UpDateBoxSideCount(Peg pegStart, Peg pegEnd)
    {
        if (pegStart.Y == pegEnd.Y)
        {
            int col = Mathf.Min((int)pegStart.X, (int)pegEnd.X);
            int row = (int)pegStart.Y;

            AddOneToBoxSideCount(col, row);
            AddOneToBoxSideCount(col, row - 1);
        }

        if (pegStart.X == pegEnd.X)
        {
            int col = (int)pegStart.X;
            int row = Mathf.Min((int)pegStart.Y, (int)pegEnd.Y);

            AddOneToBoxSideCount(col, row);
            AddOneToBoxSideCount(col - 1, row);
        }
    }

    private void AddOneToBoxSideCount(int col, int row)
    {
        if ((col >= 0) && (row >= 0) && (col < boardSize - 1) && (row < boardSize - 1))
        {
            if(wall[col, row].Add())
            {
                Vector3 position = new Vector3(col + 0.5f, row + 0.5f, 0.0f);
                GameObject block = (player == Player.PLAYER_1) ? player1PreFab : player2PreFab;

                GameObject go = Instantiate(block, position, Quaternion.identity);

                listGameObject.Add(go);

                AudioManager.Instance.SoundCompleteBox();
            }

            print("Col: " + col + " Row: " + row + " Count: " + wall[col, row].Debug());
        }

    }

    private bool LegalMove(Peg pegStart, Peg pegEnd) 
    {
        float c1 = pegStart.X;
        float r1 = pegStart.Y;

        float c2 = pegEnd.X;
        float r2 = pegEnd.Y;

        bool rowMove = (r1 == r2) && (Mathf.Abs(c1 - c2) == 1);
        bool colMove = (c1 == c2) && (Mathf.Abs(r1 - r2) == 1);

        bool duplicate = false;

        if (rowMove)
        {
            if (c1 < c2)
            {
                duplicate = (pegStart.East && pegEnd.West);
            }
            else
            {
                duplicate = (pegStart.West && pegEnd.East);
            }
        }
        
        if (colMove)
        {
            if (r1 < r2)
            {
                duplicate = (pegStart.North && pegEnd.South);
            }
            else
            {
                duplicate = (pegStart.South && pegEnd.North);
            }
        }

        bool legalMove = rowMove || colMove;

        return (legalMove && !duplicate);
    }

    private Color GetPlayerColor()
    {
        return ((player == Player.PLAYER_1) ? player1Color : player2Color);
    }

    private void TogglePlayer()
    {
        player = (player == Player.PLAYER_1) ? Player.PLAYER_2 : Player.PLAYER_1;
    }

    private void CreateRenderLine(Peg pegStart, Peg pegEnd)
    {
        GameObject go = new GameObject();
        LineRenderer link = go.AddComponent<LineRenderer>();
        link.material = drawLineMaterial;
        link.startWidth = 0.089f;
        link.endWidth = 0.089f;
        link.useWorldSpace = true;
        link.positionCount = 2;
        link.SetPosition(0, pegStart.GetPosition);
        link.SetPosition(1, pegEnd.GetPosition);
        link.startColor = GetPlayerColor();
        link.endColor = GetPlayerColor();
        link.transform.parent = transform;

        listGameObject.Add(go);
    }

    private void LinkPegs(Peg p1, Peg p2)
    {
        float c1 = p1.X;
        float r1 = p1.Y;

        float c2 = p2.X;
        float r2 = p2.Y;

        if (r1 == r2)
        {
            if (c1 < c2)
            {
                p1.East = true;
                p2.West = true;
            } else
            {
                p2.East = true;
                p1.West = true;
            }
        } 
        
        if (c1 == c2)
        {
            if (r1 < r2)
            {
                p1.North = true;
                p2.South = true;
            } else
            {
                p2.North = true;
                p1.South = true;
            }
        }
    }

    private void CreateBoard()
    {
        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col < boardSize; col++)
            {
                GameObject go = Instantiate(pegPreFab, new Vector3(col, row, 0.0f), Quaternion.identity);
                go.transform.parent = transform;

                Peg peg = go.GetComponent<Peg>();
                peg.MaxLinks = 4;

                if (row == 0 || row == (boardSize-1))
                {
                    peg.MaxLinks = ((col == 0) || (col == boardSize - 1)) ? 2 : 3;
                }

                if (col == 0 || col == (boardSize - 1))
                {
                    peg.MaxLinks = ((row == 0) || (row == boardSize - 1)) ? 2 : 3;
                }

                go.name = $"Peg: {col}, {row} {peg.MaxLinks}";

                listGameObject.Add(go);
            }
        }

        // Initialize the box count array, to count the number of walls
        //-------------------------------------------------------------
        for (int row = 0; row < boardSize - 1; row++)
        {
            for (int col = 0; col < boardSize - 1; col++)
            {
                wall[col, row] = new Wall();
            }
        }

        cameraPos.transform.position = new Vector3((boardSize-1.0f) / 2.0f, (boardSize-1.0f) / 2.0f, -10.0f);
    }
}

public enum GameState
{
    SELECT_ANCHOR,
    SELECT_PIN,
    SELECT_CANCEL
}

public enum Player
{
    NO_PLAY,
    START_GAME,
    STOP_GAME,
    PLAYER_1,
    PLAYER_2
}

public enum PlayerMode
{
    Player_One,
    Player_Two
}

public class Wall
{
    private int count;

    public Wall()
    {
        count = 0;
    }

    public bool Add()
    {
        return(++count == 4);
    }

    public int Debug()
    {
        return (count);
    }
}
