using UnityEngine;

//[RequireComponent(typeof(MeshFilter))]
//[ExecuteInEditMode]
public class MeshGeneratorOld : MonoBehaviour {
  Mesh mesh;

  Vector3[] vertices;
  int[] triangles;
  Vector2[] uvs;
  Color[] colors;

  public Gradient gradient;
  public float textureScale = 100;
  public int totalScale = 1;
  public int triangleScale = 4;
  public int xSize = 100;
  public int zSize = 100;

  public int xOffset = 0;
  public int zOffset = 0;
  public int baseOffset = 25000;
  public int baseOffset2 = 50000;

  public float largeNoiseDensity = 5f;
  public float noiseDensity = 0.02f;
  public float mapHeightScale = 35f;

  private float minTerrainHeight = 11;
  private float maxTerrainHeight = 61;

  private float _textureScale;
  private int _totalScale;
  private int _triangleScale;
  private int _xSize;
  private int _zSize;
  private int _xOffset;
  private int _zOffset;
  private int _baseOffset;
  private int _baseOffset2;
  private float _largeNoiseDensity;
  private float _noiseDensity;
  private float _mapHeightScale;

  // Start is called before the first frame update
  void Start() {
    GenerateMesh();
    _textureScale = textureScale;
    _totalScale = totalScale;
    _triangleScale = triangleScale;
    _xSize = xSize;
    _zSize = zSize;
    _xOffset = xOffset;
    _zOffset = zOffset;
    _baseOffset = baseOffset;
    _baseOffset2 = baseOffset2;
    _largeNoiseDensity = largeNoiseDensity;
    _noiseDensity = noiseDensity;
    _mapHeightScale = mapHeightScale;
  }

  private void Awake() {
    //MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
    //meshCollider.sharedMesh = mesh;
  }

  // Update is called once per frame
  void Update() {
    if(_textureScale != textureScale ||
       _totalScale != totalScale ||
       _triangleScale != triangleScale ||
       _xSize != xSize ||
       _zSize != zSize ||
       _xOffset != xOffset ||
       _zOffset != zOffset ||
       _baseOffset != baseOffset ||
       _baseOffset2 != baseOffset2 ||
       _largeNoiseDensity != largeNoiseDensity ||
       _noiseDensity != noiseDensity ||
       _mapHeightScale != mapHeightScale) {

      GenerateMesh();
      _textureScale = textureScale;
      _totalScale = totalScale;
      _triangleScale = triangleScale;
      _xSize = xSize;
      _zSize = zSize;
      _xOffset = xOffset;
      _zOffset = zOffset;
      _baseOffset = baseOffset;
      _baseOffset2 = baseOffset2;
      _largeNoiseDensity = largeNoiseDensity;
      _noiseDensity = noiseDensity;
      _mapHeightScale = mapHeightScale;
    }
  }

  public void GenerateMesh() {
    mesh = new Mesh();
    GetComponent<MeshFilter>().mesh = mesh;

    CreateShape();
    UpdateMesh();
    //yield return new WaitForSeconds(2f);
  }

  void CreateShape() {
    vertices = new Vector3[((xSize / triangleScale) + 1) * ((zSize / triangleScale) + 1)];

    for(int i = 0, z = 0; z <= zSize; z += triangleScale) {
      for(int x = 0; x <= xSize; x += triangleScale) {
        float perlinMultiplier = Mathf.PerlinNoise((x + xOffset + baseOffset2) * (largeNoiseDensity / 1000) / totalScale, (z + zOffset + baseOffset2) * (largeNoiseDensity / 1000) / totalScale);
        float y = Mathf.PerlinNoise((x + xOffset + baseOffset) * noiseDensity / totalScale, (z + zOffset + baseOffset) * noiseDensity / totalScale) * mapHeightScale * perlinMultiplier;
        vertices[i] = new Vector3((x + xOffset), y, (z + zOffset));
        i++;
        /*if(i == 0) {
          maxTerrainHeight = y;
          minTerrainHeight = y;
        }
        else if(y > maxTerrainHeight) {
          maxTerrainHeight = y;
        }
        else if(y < minTerrainHeight) {
          minTerrainHeight = y;
        }*/
      }
    }

    triangles = new int[(xSize / triangleScale) * (zSize / triangleScale) * 6];

    int vert = 0;
    int tris = 0;

    for(int z = 0; z < zSize; z += triangleScale) {
      for(int x = 0; x < xSize; x += triangleScale) {
        triangles[tris + 0] = vert + 0;
        triangles[tris + 1] = vert + (xSize / triangleScale) + 1;
        triangles[tris + 2] = vert + 1;
        triangles[tris + 3] = vert + 1;
        triangles[tris + 4] = vert + (xSize / triangleScale) + 1;
        triangles[tris + 5] = vert + (xSize / triangleScale) + 2;

        vert++;
        tris += 6;
      }
      vert++;
    }

    uvs = new Vector2[vertices.Length];
    for(int i = 0, z = 0; z <= zSize; z += triangleScale) {
      for(int x = 0; x <= xSize; x += triangleScale) {
        uvs[i] = new Vector2(textureScale * ((float)x / xSize), textureScale * ((float)z / zSize));
        i++;
      }
    }

    colors = new Color[vertices.Length];
    for(int i = 0, z = 0; z <= zSize; z += triangleScale) {
      for(int x = 0; x <= xSize; x += triangleScale) {
        float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);
        //float height = Mathf.InverseLerp(1, 10, vertices[i].y);
        colors[i] = gradient.Evaluate(height);
        i++;
      }
    }
  }

  void UpdateMesh() {
    mesh.Clear();

    mesh.vertices = vertices;
    mesh.triangles = triangles;
    //mesh.uv = uvs;
    mesh.colors = colors;
    mesh.RecalculateNormals();

    GetComponent<MeshCollider>().sharedMesh = mesh;
    //GetComponent<MeshCollider>().sharedMaterial = PhysicMaterial.

  }

  /*
  private void OnDrawGizmos()
  {
      if (vertices == null)
          return;

      for(int i = 0; i < vertices.Length; i++)
      {
          Gizmos.DrawSphere(vertices[i], 0.1f);
      }
  }
  */
}
