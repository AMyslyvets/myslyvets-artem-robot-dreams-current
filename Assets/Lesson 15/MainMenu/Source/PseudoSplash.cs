using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lesson16
{
    public class PseudoSplash : MonoBehaviour
    {
        [SerializeField] private float _delay;
        [SerializeField] private string _sceneName;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(_delay);
            SceneManager.LoadScene(_sceneName);
        }
    }
}