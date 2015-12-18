using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.common
{
    class GameParam
    {
        private static GameParam instance;

        private float capsuleHealthPoint;

        public float CapsuleHealthPoint
        {
            get { return capsuleHealthPoint; }
        }
        private float agentInitialHealthPoint;

        public float AgentInitialHealthPoint
        {
            get { return agentInitialHealthPoint; }
        }
        private float bulletDamage;

        public float BulletDamage
        {
            get { return bulletDamage; }
        }



        internal static GameParam Instance
        {
            get {
                if (instance == null)
                {
                    instance = new GameParam();
                }
                return instance;
            }
        }

        private GameParam() {
            agentInitialHealthPoint = 100;
            bulletDamage = 20;
            capsuleHealthPoint = 15;
        }

    }
}
