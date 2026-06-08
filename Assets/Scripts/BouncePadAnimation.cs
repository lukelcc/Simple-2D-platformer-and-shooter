using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BouncePadAnimation : MonoBehaviour
{
    public Tilemap bouncePadTerrain;
    public TileBase BouncedSprite;
    public TileBase OriginalSprite;

    [SerializeField] private float bounceDelay = 0.05f;
    [SerializeField] private float springExtendDuration = 0.1f;


    public void AnimateBouncePad(Vector3 playerBouncePos)
    {
        //Vector3Int bounceTilePos = new Vector3Int(bouncePadTerrain.WorldToCell(playerBouncePos).x,
        //    bouncePadTerrain.WorldToCell(playerBouncePos).y - 1,
        //    bouncePadTerrain.WorldToCell(playerBouncePos).z);

        Vector3Int bounceTilePos = bouncePadTerrain.WorldToCell(playerBouncePos);
        //Debug.Log("bounce point:" + bounceTilePos);
        if(bouncePadTerrain.GetTile(bounceTilePos)!=null)
        {
            //Debug.Log("bounce tile pos:" + bounceTilePos);
            StartCoroutine(SpringExtendAndContract(bounceTilePos));
            //bouncePadTerrain.SetTile(bounceTilePos, BouncedSprite);
        }
    }
    IEnumerator SpringExtendAndContract(Vector3Int bounceTilePos)
    {
        yield return new WaitForSeconds(bounceDelay);
        bouncePadTerrain.SetTile(bounceTilePos, BouncedSprite);
        yield return new WaitForSeconds(springExtendDuration);
        bouncePadTerrain.SetTile(bounceTilePos, OriginalSprite);
    }

}
