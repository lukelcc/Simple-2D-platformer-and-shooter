using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DeathZoneTilemap : MonoBehaviour
{
    [Header("Hazard tilemap settings")]
    [SerializeField] public Tilemap hazardTilemap;
    [SerializeField] public TileBase hazardTile;
    [SerializeField] private int distanceToTravelByTiles = 20;
    [SerializeField] private int mapHeightInTiles = 20;
    [SerializeField] private int mapWidthInTiles = 38;
    [SerializeField] private Vector2 topLeftCorner;
    [SerializeField] private Vector2 bottomLeftCorner;
    [SerializeField] private Vector2 bottomRightCorner;

    [Header("Damage settings")]
    [SerializeField] private float moveInterval = 5f;
    [SerializeField] public int damage = 2;
    [SerializeField] public float damageInterval = 0.5f;

    [Header("Timer settings")]   
    [SerializeField] public int countDownTimer = 10;
    [SerializeField] public float countDownTimerInterval = 1f;

    //[SerializeField] public Transform spawnLocation;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<GameSession>().initializedSuddenDeathTimer();
        StartCoroutine(pauseThenStartSuddenDeathTimer(FindObjectOfType<GameSession>().countdownDuration));
    }

    // Update is called once per frame
    void Update()
    {
        //SpawnLeftColumn();
        //SpawnTopRow();
    }

    IEnumerator ActivateDeathZone()
    {
        for(int columnRowOffset=0;columnRowOffset<distanceToTravelByTiles;columnRowOffset++)//spawn left death zone column by column
        {
            SpawnLeftColumn(columnRowOffset);
            SpawnRightColumn(columnRowOffset);
            SpawnTopRow(columnRowOffset);
            yield return new WaitForSeconds(moveInterval);           
        }
    }

    //spawn the left column
    public void SpawnLeftColumn(int columnOffsetFromLeft)
    {
        for(int y = 0; y < mapHeightInTiles; y++)
        {
            SpawnTileAtWorldPosition(new Vector3(bottomLeftCorner.x + columnOffsetFromLeft, bottomLeftCorner.y + y,0));
        }
    }
    //spawn the right column
    public void SpawnRightColumn(int columnOffsetFromRight)
    {
        for (int y = 0; y <mapHeightInTiles; y++)
        {
            SpawnTileAtWorldPosition(new Vector3(bottomRightCorner.x - columnOffsetFromRight, bottomRightCorner.y + y, 0));
        }
    }

    //spawn the top row
    public void SpawnTopRow(int rowOffsetFromTop)
    {
        for (int x = 0; x < mapWidthInTiles; x++)
        {
            SpawnTileAtWorldPosition(new Vector3(topLeftCorner.x + x, topLeftCorner.y - rowOffsetFromTop, 0));
        }
    }

    public void SpawnTileAtWorldPosition(Vector3 worldPosition)
    {
        //Convert the Vector3 World Position to Vector3Int Cell Coordinates
        Vector3Int cellPosition = hazardTilemap.WorldToCell(worldPosition);

        //Place the tile on the tilemap grid
        hazardTilemap.SetTile(cellPosition, hazardTile);
    }



    IEnumerator pauseThenStartSuddenDeathTimer(float pausetime)
    {
        Debug.Log("Pause sudden death count down timer");
        yield return new WaitForSeconds(pausetime);
        Debug.Log("Start sudden death count down timer");
        StartCoroutine(startSuddenDeathTimer(countDownTimer));
    }

    IEnumerator startSuddenDeathTimer(int countDownTimer)//start sudden death countdown
    {
        for (int timeLeft = countDownTimer; timeLeft > 0; timeLeft--)
        {
            FindObjectOfType<GameSession>().updateSuddenDeathTimer(timeLeft);
            yield return new WaitForSeconds(countDownTimerInterval);
        }
        FindObjectOfType<GameSession>().displaySuddenDeathText();
        StartCoroutine(ActivateDeathZone());//times up, start sudden death
    }


   
}
