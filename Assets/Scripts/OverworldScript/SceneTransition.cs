using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pokemon
{
    public class SceneTransition : MonoBehaviour
    {
        public string sceneToLoad;
        public Vector2 playerPosition;
        public VectorValue playerStorage;
        public string location;

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !other.isTrigger)
            {
                playerStorage.initialValue = playerPosition;
                GameController.location = location;
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}
