using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.common;
public class TeamCreator : MonoBehaviour {
    public Transform equipier;
    public Transform cibleDebug;
    public string nomEquipe;
    public List<Transform> temmatesList;

    public Text nombre, pourcentLeaders, tauxEntraide, nbrMunitions, quantiteVie;
    public bool showTarget;
    [Range(0, 100)]
    public int tailleEquipe;

    [Range(0, 100)]
    public int organisation;

    [Range(0, 100)]
    public int entraide;

    [Range(0, 100)]
    public int munitionsDebut;
    [Range(0, 100)]
    public int vieDebut;
    // Use this for initialization
    public void Demarrer () {
        temmatesList = new List<Transform>();
        for (int i = 0; i < tailleEquipe; i++){
            Quaternion rndRotation = Random.rotation;
            Vector3 randomDirection = Random.insideUnitSphere * 50;
            randomDirection += transform.position;
            NavMeshHit hit;
            while (!NavMesh.SamplePosition(randomDirection, out hit, 300, 1)) { }
            Vector3 finalPosition = hit.position;
            finalPosition.y = transform.position.y ;
            //Create a random rotation.

            //Instantiate a new object at a random position with a random rotation.
            Transform newTeammate = Instantiate(equipier, finalPosition, rndRotation) as Transform;

            temmatesList.Add(newTeammate);
            agent_behaviour teammateScript = newTeammate.GetComponent<agent_behaviour>();
            if (teammateScript != null)
            {
                
                newTeammate.transform.parent = transform;
                int rand = Random.Range(0, 100);
                Debug.Log(rand);
                teammateScript.EstLeader = (rand < organisation) ? true : false;
                teammateScript.Formation = (teammateScript.EstLeader) ? new Formation(teammateScript) : null;
                teammateScript.Entraide = (Random.Range(0, 100) < entraide) ? true : false;
                TeamMessenger messenger = GetComponent<TeamMessenger>();
                messenger.connectAgent(teammateScript);
                teammateScript.TeamMessenger = messenger;
                teammateScript.healthPoint = vieDebut;
                teammateScript.munitions = munitionsDebut;
                teammateScript.Equipe = nomEquipe;
                teammateScript.Pseudo = nomEquipe + "_" + i.ToString();
                teammateScript.crossTarget = cibleDebug;
                teammateScript.teamCreator = this;
            }

        }
	}

    public void arreter()
    {
        foreach(Transform t in temmatesList)
        {
            Destroy(t.gameObject);
        }
        temmatesList = new List<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}


    public void onNombreChange(Slider slider)
    {
        tailleEquipe = (int)slider.value;
        nombre.text = slider.value.ToString();
    }
    public void onPourcentLeaderChange(Slider slider)
    {
        organisation = (int)slider.value;
        pourcentLeaders.text = slider.value.ToString();
    }
    public void onTauxEntraideChange(Slider slider)
    {
        entraide = (int)slider.value;
        tauxEntraide.text = slider.value.ToString();
    }

    public void onNombreMunitionChange(Slider slider)
    {
        munitionsDebut = (int)slider.value;
        nbrMunitions.text = slider.value.ToString();
    }
    public void onQuantiteVieChange(Slider slider)
    {
        vieDebut = (int)slider.value;
        quantiteVie.text = slider.value.ToString();
    }

}
