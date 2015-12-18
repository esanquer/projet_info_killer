using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.common;

public class TeamMessenger : MonoBehaviour {



    private List<agent_behaviour> agents;

    private List<Message> messages;


    public void connectAgent(agent_behaviour agent)
    {
        if (agents == null)
        {
            agents = new List<agent_behaviour>();
        }
        agents.Add(agent);
       // Debug.Log("Agent ajoute au messenger ; "+ agents.Count);
    }
    public void disconnectAgent(agent_behaviour agent)
    {
        agents.Remove(agent);
    }

    public void addMessage(agent_behaviour source, agent_behaviour destinataire, MESSAGES_TYPE type, string infos)
    {
        Message m = new Message();
        m.Destinataire = destinataire;
        m.Source = source;
        m.MessageType = type;
        m.Infos = infos;
        this.messages.Add(m);
    }

    void sendMessages()
    {
        foreach (Message m in messages)
        {
            if (m.Destinataire != null)
            {
                m.Destinataire.communiquerMessage(m);
            }
            else
            {
                foreach (agent_behaviour agent in agents)
                {
                    if (m.Source.Pseudo != agent.Pseudo)
                    {
                        agent.communiquerMessage(m);
                    }
                }
            }
        }
        messages = new List<Message>();
    }
	// Use this for initialization
	void Start () {
        //agents = new List<agent_behaviour>();
        messages = new List<Message>();
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("Team messenger broadcast " + messages.Count + " messages à "+agents.Count+" agents");
        sendMessages();
	}
}
