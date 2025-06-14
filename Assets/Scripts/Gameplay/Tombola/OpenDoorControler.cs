using UnityEngine;

public class OpenDoorControler : MonoBehaviour 
{
    [SerializeField ] private float interval = 10f; 
    private HouseControler[] _houseControllers;
    private float _timer;
    private void Start()
    {
        _houseControllers = FindObjectsByType<HouseControler>(FindObjectsSortMode.None);
        _timer = interval;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            OpenRandomDoor();
            _timer = interval;
        }
    }
    void OpenRandomDoor()
    {
        if (_houseControllers == null || _houseControllers.Length == 0)
        {
            Debug.LogWarning("No se encontraron HouseControllers en la escena");
            return;
        }

        // Filtrar solo los HouseController que tienen puerta cerrada (si es necesario)
        //var availableHouses = _houseControllers.Where(h => h != null && !h.doorIsOpen).ToArray();

        //if (_availableHouses.Length == 0)
        //{
            //Debug.Log("Todas las puertas están abiertas o no hay HouseControllers válidos");
            //return;
        //}

        // Seleccionar uno aleatorio
        int randomIndex = Random.Range(0, _houseControllers.Length);
        HouseControler selectedHouse = _houseControllers[randomIndex];

        // Llamar a su función OpenDoor()
        if(VerifyDoorsClosed())
            selectedHouse.OpenDoor();
        //Debug.Log($"Puerta abierta en: {selectedHouse.name}");
    }

    private bool VerifyDoorsClosed() {
        foreach (HouseControler house in _houseControllers) {
            if (house.IsDoorOpen()) {
                return false;
            }
        }
        return true;
    }

}
