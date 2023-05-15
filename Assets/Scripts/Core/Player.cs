
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float pushForce;
    [SerializeField] private float cubeMaxPosX;
    [Space]
    [SerializeField]
    private CubeSpawner spawner;
    [SerializeField]
    private TouchSlider slider;
    private Cube mainCube;

    private bool isPointerDown;
    private bool canMove;
    private Vector3 cubePos;
    private bool inputEnabled = true;

    [SerializeField] private IntValue _multyCubes;
    [SerializeField] private IntValue _bombCubes;

    private void Start()
    {
        SpawnCube();
        canMove = true;

        // listen to slider events
        slider.OnPointerDownEvent += OnPointerDown;
        slider.OnPointerDragEvent += OnPointerDrag;
        slider.OnPointerUpEvent += OnPointerUp;
    }

    private void Update()
    {
        if (isPointerDown)
            mainCube.transform.position = Vector3.Lerp(mainCube.transform.position, cubePos, moveSpeed * Time.deltaTime);
    }

    private void OnPointerDown()
    {
        if(inputEnabled)
        isPointerDown = true;
    }
    private void OnPointerDrag(float xMovement)
    {
        if (inputEnabled)
        {
            if (isPointerDown)
            {
                cubePos = mainCube.transform.position;
                cubePos.x = xMovement * cubeMaxPosX;
            }
        }
    }
    private void OnPointerUp()
    {
        if (inputEnabled)
        {
            if (isPointerDown && canMove)
            {
                inputEnabled = false;
                isPointerDown = false;
                canMove = false;

                mainCube.CubeRgidbody.AddForce(Vector3.forward * pushForce, ForceMode.Impulse);

                Invoke("SpawnNewCube", 0.3f);
                Invoke("EnableInput", 0.3f);
            }
        }
    }
    private void EnableInput()
    {
        inputEnabled = true;
    }
    private void SpawnNewCube()
    {
        mainCube.IsMainCube = false;
        canMove = true;
        SpawnCube();
    }
    private void SpawnCube()
    {
        mainCube = spawner.SpawnRandom();
        mainCube.IsMainCube = true;


        cubePos = mainCube.transform.position;
    }
    private void OnDestroy()
    {

        slider.OnPointerDownEvent -= OnPointerDown;
        slider.OnPointerDragEvent -= OnPointerDrag;
        slider.OnPointerUpEvent -= OnPointerUp;
    }
    public void ActivateMultyCube()
    {
        if(_multyCubes.Value > 0)
        {
            _multyCubes.Value--;
            CubeSpawner.Instance.DestroyCube(mainCube);
            mainCube = CubeSpawner.Instance.SpawnMultyCube().gameObject.GetComponent<Cube>();
        }
        
    }

    public void ActivateBombCube()
    {
        if (_bombCubes.Value > 0)
        {
            _bombCubes.Value--;
            CubeSpawner.Instance.DestroyCube(mainCube);
            mainCube = CubeSpawner.Instance.SpawnBombCube().gameObject.GetComponent<Cube>();
        }
    }
}
