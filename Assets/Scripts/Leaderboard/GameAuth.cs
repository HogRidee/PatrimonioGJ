using UnityEngine;
using Dan.Main;

public class LeaderboardBootstrapper : MonoBehaviour
{
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("LC_USER_GUID"))
        {
            // Usamos el ID único del dispositivo como GUID simulado
            string guid = SystemInfo.deviceUniqueIdentifier;
            LeaderboardCreator.SetUserGuid(guid);
            PlayerPrefs.SetString("LC_USER_GUID", guid);
            Debug.Log("GUID generado localmente: " + guid);
        }
        else
        {
            string savedGuid = PlayerPrefs.GetString("LC_USER_GUID");
            LeaderboardCreator.SetUserGuid(savedGuid);
            Debug.Log("GUID cargado: " + savedGuid);
        }
    }
}
