﻿using UnityEngine;

namespace TutorialPlatformer.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/PlayerInfo", fileName = "PlayerInfo")]
    public class PlayerInfo : ScriptableObject
    {
        public float Speed;
        public float JumpForce;
    }
}