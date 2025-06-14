using UnityEngine;

namespace Dan.Main
{
    public class LeaderboardCreatorManager : MonoBehaviour
    {
        private static LeaderboardCreatorManager _instance;

        void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
