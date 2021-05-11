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
                if (name == "SceneTransitionPokecenter")
                    GetChordsPokecenter();

                playerStorage.initialValue = playerPosition;
                GameController.prevLocation = GameController.location;
                GameController.location = location;
                GameController.music = location;
                SceneManager.LoadScene(sceneToLoad);
            }
        }

        public void GetChordsPokecenter()
        {
            switch(GameController.prevLocation)
            {
                case "Viridian City":
                    playerPosition = new Vector2(17.5f, 110.8f);
                    break;
                case "Pewter City":
                    playerPosition = new Vector2(-5.5f, 340.8f);
                    break;
                default:
                    break;
            }
            location = GameController.prevLocation;
        }
    }
}
