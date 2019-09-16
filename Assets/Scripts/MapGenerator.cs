using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
  public MeshGenerator meshRenderer;
  public int xSize;
  public int zSize;
  public int loadDistance = 2;
  public int chunkSize = 64;
  public int yOffset = -14;

  public Transform player;

  private ArrayList loaded;
  private int lastPlayerX = 0;
  private int lastPlayerZ = 0;

  // Start is called before the first frame update
  void Start() {
    loaded = new ArrayList();
    for(int x = -xSize; x < xSize; x++) {
      for(int z = -zSize; z < zSize; z++) {
        loadChunk(x, z);
      }
    }
  }

  // Update is called once per frame
  void Update() {
    int playerX = ((int)player.position.x) / chunkSize;
    int playerZ = ((int)player.position.z) / chunkSize;

    if(playerX != lastPlayerX || playerZ != lastPlayerZ) {
      Debug.Log("Loading chunks");
      loadChunks(playerX, playerZ);
    }
  }

  private void loadChunks(int playerX, int playerZ) {
    for(int x = playerX - loadDistance; x < playerX + loadDistance; x++) {
      for(int z = playerZ - loadDistance; z < playerZ + loadDistance; z++) {
        if(!loaded.Contains(x + "," + z)) {
          loadChunk(x, z);
          //yield return null;
        }
      }
    }
    lastPlayerX = playerX;
    lastPlayerZ = playerZ;
  }

  private void loadChunk(int x, int z) {
    Debug.Log("Loading chunk: " + x + ", " + z);
    MeshGenerator map = Instantiate(meshRenderer, new Vector3(0, yOffset, 0), Quaternion.identity);
    map.xOffset = chunkSize * x;
    map.zOffset = chunkSize * z;
    loaded.Add(x + "," + z);
  }
}
