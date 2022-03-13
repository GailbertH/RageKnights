using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RageKnight.Environment
{
    public class EnvironmentHandler : MonoBehaviour
    {
        [SerializeField] private BackgroundController foregroundController;
        [SerializeField] private BackgroundController maingroundController;
        [SerializeField] private BackgroundController backgroundController;
        [SerializeField] private BackgroundController deepgroundController;
        [SerializeField] private List<Sprite> backgroundSprites;
        [SerializeField] private List<Sprite> planegroundSprites;

        public void MoveEnvironment(float speed)
        {
            foregroundController?.MoveBackground(speed - 0.02f);
            maingroundController?.MoveBackground(speed);
            backgroundController?.MoveBackground(speed + 0.02f);
            deepgroundController?.MoveBackground(speed + 0.03f);
        }
    }
}
