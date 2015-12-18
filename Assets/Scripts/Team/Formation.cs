using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.common
{
    
    public class Formation
    {
        private float scale = 3.0f;
        private agent_behaviour leader;
        private Dictionary<int,Vector3> slots;
        private Dictionary<agent_behaviour, int> slotByAgent;
        private Dictionary<int, agent_behaviour> agentBySlot;
        private int nombreMax = 8;

        public float Scale
        {
            get
            {
                return scale;
            }

            set
            {
                scale = value;
            }
        }

        public agent_behaviour Leader
        {
            get
            {
                return leader;
            }

            set
            {
                leader = value;
            }
        }

        public int NombreMax
        {
            get
            {
                return nombreMax;
            }

            set
            {
                nombreMax = value;
            }
        }

        public Formation(agent_behaviour leader)
        {
            this.Leader = leader;
            this.slots = new Dictionary<int, Vector3>();
            slots.Add(1, new Vector3(-1, 0, 1));
            slots.Add(2, new Vector3(1, 0, 1));
            slots.Add(3, new Vector3(-2, 0, 2));
            slots.Add(4, new Vector3(0, 0, 2));
            slots.Add(5, new Vector3(2, 0, 2));
            slots.Add(6, new Vector3(-1, 0, 3));
            slots.Add(7, new Vector3(1, 0, 3));
            slots.Add(8, new Vector3(0, 0, 4));
            this.slotByAgent = new Dictionary<agent_behaviour, int>();
            this.agentBySlot = new Dictionary<int, agent_behaviour>();
            agentBySlot.Add(1, null);
            agentBySlot.Add(2, null);
            agentBySlot.Add(3, null);
            agentBySlot.Add(4, null);
            agentBySlot.Add(5, null);
            agentBySlot.Add(6, null);
            agentBySlot.Add(7, null);
            agentBySlot.Add(8, null);
        }

        private int premierSpotLibre()
        {
            int num = -1;
            foreach(KeyValuePair<int, agent_behaviour> kvp in agentBySlot)
            {
                if(kvp.Value == null)
                {
                    num = kvp.Key;
                    break;
                }
            }
            return num;
        }

        public bool addAgent(agent_behaviour agent)
        {
            int numSpot = premierSpotLibre();
            if (!this.slotByAgent.ContainsKey(agent))
            {
                if (numSpot != -1)
                {
                    slotByAgent.Add(agent, numSpot);
                    agentBySlot[numSpot] = agent;
                    return true;
                }
            }
            return false;
        }

        public bool placeLibre()
        {
            return this.slotByAgent.Count < this.nombreMax;
        }
        public Vector3 getSlotPositionForAgent(agent_behaviour agent)
        {
            Vector3 positionSlot = slots[slotByAgent[agent]];

            Matrix4x4 transformLeaderToWorld = Matrix4x4.identity;
            if (Leader != null)
            {
                transformLeaderToWorld = Leader.transform.localToWorldMatrix;
            }
            else
            {
                dissoudre();
                return agent.transform.position;
            }
            

            return transformLeaderToWorld.MultiplyPoint((-positionSlot*scale));
        }

        public void removeAgent(agent_behaviour agent)
        {
            if (this.slotByAgent.ContainsKey(agent)) { 
                int slot = this.slotByAgent[agent];
                slotByAgent.Remove(agent);
                agentBySlot[slot] = null;
            }
        }

        public void dissoudre()
        {
            bool leaderDesigne = false;
            foreach(KeyValuePair<agent_behaviour,int> kvp in slotByAgent)
            {
                if (!leaderDesigne)
                {
                    kvp.Key.EstLeader = true;
                    leaderDesigne = true;
                }
                kvp.Key.forcerSortieDeFormation();
            }
        }
    }

}
