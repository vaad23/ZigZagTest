using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public float spead; //скорость шара
    bool pause; //пауза игры
    public GameObject spherePrefab; //префаб шара
    GameObject sphere; //шар
    public GameObject gameCamera; //камера
    Vector3 distanceCamera; //дистанция от камеры до шара при старте
    Rigidbody rbSphere;
    bool rightMoveSphere; //направление движения
    Vector3 movementSphere;

    public GameObject PrefabBlock; //префаб блока поля
    List<GameObject> ListBlock = new List<GameObject>();
    public GameObject PrefabCrystal; // префаб кристалла
    public Vector2 fieldSizeByX; //размеры поля при старте 
    public Vector2 fieldSizeByZ; //размеры поля при старте 
    Vector2 lastFieldBlock;
    Vector2 fieldDirection; // x-направление: 0 - прямо, 1 - направо; y - количество
    public int fieldWidth; //дальность отступа от центра
    public int maximumNumberOfBlocks; //максимальное количество блоков поля

    int score;
    public Text scoreText;

    void Start()
    {
        StartGame();
    }

    private void FixedUpdate()
    {
        if (!pause)
        {
            rbSphere.AddForce(movementSphere * spead);
            // sphere.transform.position += movementSphere * spead;
            float position = (sphere.transform.position.x + sphere.transform.position.z) / 2;
            gameCamera.transform.position = new Vector3(position, 0.25f, position) + distanceCamera;
            if (sphere.transform.position.y < 0) pause = true;
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            rightMoveSphere = !rightMoveSphere;
            if (rightMoveSphere) movementSphere = new Vector3(1, 0, 0);
            else movementSphere = new Vector3(0, 0, 1);
            if (pause)
            {
                if (sphere.transform.position.y < 0)
                {
                    DestroyAllField();
                    Destroy(sphere);
                    StartGame();
                }
                else pause = false;
            }
        }
    }
    void StartGame()
    {
        score = 0;
        scoreText.text = score.ToString();
        pause = true;
        fieldDirection = new Vector2(0, 0);
        gameCamera.transform.position = new Vector3(-7, 7f, -7);
        CreateStartField();
        CreateSphere();
    }

    void CreateSphere()
    {
        GameObject prefabObject = Instantiate(spherePrefab, transform);
        sphere = prefabObject;
        distanceCamera = gameCamera.transform.position - sphere.transform.position;
        rbSphere = prefabObject.GetComponent<Rigidbody>();
        movementSphere = new Vector3(0, 0, 1);
    }
    void CreateStartField()
    {
        for (int i = (int)fieldSizeByX.x; i <= fieldSizeByX.y; i++)
            for (int j = (int)fieldSizeByZ.x; j <= fieldSizeByZ.y; j++)
            {
                GameObject prefabObject = Instantiate(PrefabBlock, transform);
                prefabObject.transform.position = new Vector3(i, -1, j);
                ListBlock.Add(prefabObject);
            }
        lastFieldBlock = new Vector2(fieldSizeByX.y, fieldSizeByZ.y);
        fieldDirection = new Vector2(Random.Range(0, 2), 0);
        for (int i = 0; i < maximumNumberOfBlocks; i++) CreateBlock(true);
    }
    void CreateBlock(bool startGame)
    {
        float probability = 50;
        Vector2 newDirection;
        if ((fieldWidth - Mathf.Abs(lastFieldBlock.x - lastFieldBlock.y)) == 0) probability += Mathf.Sign(lastFieldBlock.x - lastFieldBlock.y) * 100;
        if (probability > Random.Range(0f, 100f)) newDirection = new Vector2(0, 1); else newDirection = new Vector2(1, 0);
        lastFieldBlock += newDirection;
        if (newDirection.x == 0) { if (fieldDirection.x == 0) fieldDirection.y += 1; else fieldDirection = new Vector2(1, 0); }
        else { if (fieldDirection.x == 1) fieldDirection.y += 1; else fieldDirection = new Vector2(0, 0); }
        GameObject prefabObject = Instantiate(PrefabBlock, transform);
        if (startGame) prefabObject.transform.position = new Vector3(lastFieldBlock.x, -1, lastFieldBlock.y);
        else StartCoroutine(prefabObject.GetComponent<BlockLogic>().CreateBlock(new Vector3(lastFieldBlock.x, -1, lastFieldBlock.y)));
        ListBlock.Add(prefabObject);
        if (Random.Range(0f, 100f) < 20)
        {
            GameObject crystal = Instantiate(PrefabCrystal, prefabObject.transform);
            crystal.GetComponent<CrystalLogic>().gameLogic = this;
        }
    }
    public void IntersectionOfSphereAndCrystal()
    {
        score += 1;
        scoreText.text = score.ToString();
    }

    public void DestroyBlock(GameObject destroyObject)
    {
        ListBlock.Remove(destroyObject);
        StartCoroutine(destroyObject.GetComponent<BlockLogic>().DestroyBlock());
        while (ListBlock.Count < maximumNumberOfBlocks) CreateBlock(false);
    }
    void DestroyAllField()
    {
        while (ListBlock.Count > 0)
        {
            GameObject block = ListBlock[0];
            ListBlock.Remove(block);
            Destroy(block);
        }
    }
}
