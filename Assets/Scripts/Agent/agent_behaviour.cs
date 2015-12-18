using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Assets.Scripts.common;

public class agent_behaviour : MonoBehaviour
{
    public Transform ballspawn;

    public TeamCreator teamCreator;
    float timeSinceLastShoot = 0.0f;

    float fireDelay = 0.8f;
    private string equipe = "";

    public string pseudo = "";

    public float healthPoint;

    public bool entraide;

    public int munitions;

    public bool estLeader;

    public bool showTarget;
    public Transform _munition_fired;
    public Transform crossTarget;
    public Transform _cross;
    private Transform _target_shown;
    private Transform currenttarget;

    private Formation formation;

    private List<Message> messageBox;

    private enum actions_de_fond { AIDER_COEQUIPIER, SE_DEPLACER_EN_FORMATION, ALLER_N_IMPORTE_OU };
    private enum actions { ATTAQUER_ENNEMI, FUIR_ENNEMI, RECUPERER_VIE, RECUPERER_MUNITION, EVITER_MUR };

    public Vector3 target;
    public Vector3 targetDeFond;
    private NavMeshAgent agent;
    private Dictionary<actions, GameObject> actions_a_faire = new Dictionary<actions, GameObject>();
    private Dictionary<actions_de_fond, GameObject> actions_de_fond_a_faire = new Dictionary<actions_de_fond, GameObject>();
    private actions etat_courrant;
    private actions etat_precedent;

    private Dictionary<GameObject, Vector3> pointsMursProches;
    private Dictionary<GameObject, float> distancesMurs;

    private Dictionary<GameObject, int> munitionsPrises =new Dictionary<GameObject, int>();

    // Use this for initialization

    //private float vitesse = 10;
    private Vector3 velocity = new Vector3(0, 0, 12);


    private Vector3 DirectionDeplacement = Vector3.forward;

    public Vector3 directionDeplacement1
    {
        get { return DirectionDeplacement; }
        set { DirectionDeplacement = value; }
    }

    public TeamMessenger TeamMessenger
    {
        get
        {
            return teamMessenger;
        }

        set
        {
            teamMessenger = value;
        }
    }

    public Formation Formation
    {
        get
        {
            return formation;
        }

        set
        {
            formation = value;
        }
    }

    public bool EstLeader
    {
        get
        {
            return estLeader;
        }

        set
        {
            estLeader = value;
        }
    }

    public bool Entraide
    {
        get
        {
            return entraide;
        }

        set
        {
            entraide = value;
        }
    }

    public int Munitions
    {
        get
        {
            return munitions;
        }

        set
        {
            munitions = value;
        }
    }

    public float HealthPoint
    {
        get
        {
            return healthPoint;
        }

        set
        {
            healthPoint = value;
        }
    }

    public string Pseudo
    {
        get
        {
            return pseudo;
        }

        set
        {
            pseudo = value;
        }
    }

    public string Equipe
    {
        get
        {
            return equipe;
        }

        set
        {
            equipe = value;
        }
    }

    private TeamMessenger teamMessenger;

    void Start()
    {
       // Debug.Log(pseudo + " start");
        this.messageBox = new List<Message>();
        this.agent = GetComponent<NavMeshAgent>();
        agent.speed = 20;
        //agent.SetDestination(this.target);
        //allerNImporteOu(300.0f);
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastShoot += Time.deltaTime;
        trierMessages(2);
        if (actions_a_faire.Count > 0)
        {
            if (Munitions > 0 && HealthPoint > 20) // je vais bien et j'ai des munitions
            {
                if (actions_a_faire.ContainsKey(actions.ATTAQUER_ENNEMI) && actions_a_faire[actions.ATTAQUER_ENNEMI] != null)
                {
                    attaquerEnnemi(actions_a_faire[actions.ATTAQUER_ENNEMI]);
                    changerEtat(actions.ATTAQUER_ENNEMI);
                    direSiJeSuisTranquilleMaintenant();
                }
                else if (actions_a_faire.ContainsKey(actions.RECUPERER_VIE) && actions_a_faire[actions.RECUPERER_VIE] != null)
                {
                    seDirigerVersObjet(actions_a_faire[actions.RECUPERER_VIE].transform.position);
                    changerEtat(actions.RECUPERER_VIE);
                    direSiJeSuisTranquilleMaintenant();
                }
                else if (actions_a_faire.ContainsKey(actions.RECUPERER_MUNITION) && actions_a_faire[actions.RECUPERER_MUNITION] != null)
                {
                    seDirigerVersObjet(actions_a_faire[actions.RECUPERER_MUNITION].transform.position);
                    changerEtat(actions.RECUPERER_MUNITION);
                    direSiJeSuisTranquilleMaintenant();
                }
                else if (actions_a_faire.ContainsKey(actions.FUIR_ENNEMI) && actions_a_faire[actions.FUIR_ENNEMI] != null)
                {
                    fuirEnnemi(actions_a_faire[actions.FUIR_ENNEMI]);
                    changerEtat(actions.FUIR_ENNEMI);
                    direSiJaiBesionDAide();
                }
            }
            else if (Munitions <= 0 && HealthPoint > 20) // je vais bien mais j'ai pas de munitions
            {
                if (actions_a_faire.ContainsKey(actions.RECUPERER_MUNITION) && actions_a_faire[actions.RECUPERER_MUNITION] != null)
                {
                    seDirigerVersObjet(actions_a_faire[actions.RECUPERER_MUNITION].transform.position);
                    changerEtat(actions.RECUPERER_MUNITION);
                    direSiJeSuisTranquilleMaintenant();
                }
                else if (actions_a_faire.ContainsKey(actions.FUIR_ENNEMI) && actions_a_faire[actions.FUIR_ENNEMI] != null)
                {
                    fuirEnnemi(actions_a_faire[actions.FUIR_ENNEMI]);
                    changerEtat(actions.FUIR_ENNEMI);
                    direSiJaiBesionDAide();
                }
                else if (actions_a_faire.ContainsKey(actions.RECUPERER_VIE) && actions_a_faire[actions.RECUPERER_VIE] != null)
                {
                    seDirigerVersObjet(actions_a_faire[actions.RECUPERER_VIE].transform.position);
                    changerEtat(actions.RECUPERER_VIE);
                    direSiJeSuisTranquilleMaintenant();
                }
                else if (actions_a_faire.ContainsKey(actions.ATTAQUER_ENNEMI) && actions_a_faire[actions.ATTAQUER_ENNEMI] != null)
                {
                    attaquerEnnemi(actions_a_faire[actions.ATTAQUER_ENNEMI]);
                    changerEtat(actions.ATTAQUER_ENNEMI);
                    direSiJeSuisTranquilleMaintenant();
                }

            }
            else if (Munitions > 0 && HealthPoint <= 20) // je vais mal mais j'ai des munitions
            {
               if (actions_a_faire.ContainsKey(actions.RECUPERER_VIE) && actions_a_faire[actions.RECUPERER_VIE] != null)
                {
                    seDirigerVersObjet(actions_a_faire[actions.RECUPERER_VIE].transform.position);
                    changerEtat(actions.RECUPERER_VIE);
                    direSiJeSuisTranquilleMaintenant();
                }
                else if (actions_a_faire.ContainsKey(actions.ATTAQUER_ENNEMI) && actions_a_faire[actions.ATTAQUER_ENNEMI] != null)
                {
                    attaquerEnnemi(actions_a_faire[actions.ATTAQUER_ENNEMI]);
                    changerEtat(actions.ATTAQUER_ENNEMI);
                    direSiJeSuisTranquilleMaintenant();
                }
                else if (actions_a_faire.ContainsKey(actions.FUIR_ENNEMI) && actions_a_faire[actions.FUIR_ENNEMI] != null)
                {
                    fuirEnnemi(actions_a_faire[actions.FUIR_ENNEMI]);
                    changerEtat(actions.FUIR_ENNEMI);
                    direSiJaiBesionDAide();
                }
                else if (actions_a_faire.ContainsKey(actions.RECUPERER_MUNITION) && actions_a_faire[actions.RECUPERER_MUNITION] != null)
                {
                    seDirigerVersObjet(actions_a_faire[actions.RECUPERER_MUNITION].transform.position);
                    changerEtat(actions.RECUPERER_MUNITION);
                    direSiJeSuisTranquilleMaintenant();
                }
            }
            else if (Munitions <= 0 && HealthPoint <= 20) // j'ai ni vie ni munitions
            {
                if (actions_a_faire.ContainsKey(actions.FUIR_ENNEMI) && actions_a_faire[actions.FUIR_ENNEMI] != null)
                {
                    fuirEnnemi(actions_a_faire[actions.FUIR_ENNEMI]);
                    changerEtat(actions.FUIR_ENNEMI);
                    direSiJaiBesionDAide();
                }
                else if (actions_a_faire.ContainsKey(actions.RECUPERER_VIE) && actions_a_faire[actions.RECUPERER_VIE] != null)
                {
                    seDirigerVersObjet(actions_a_faire[actions.RECUPERER_VIE].transform.position);
                    changerEtat(actions.RECUPERER_VIE);
                    direSiJeSuisTranquilleMaintenant();
                }
                else if (actions_a_faire.ContainsKey(actions.RECUPERER_MUNITION) && actions_a_faire[actions.RECUPERER_MUNITION] != null)
                {
                    seDirigerVersObjet(actions_a_faire[actions.RECUPERER_MUNITION].transform.position);
                    changerEtat(actions.RECUPERER_MUNITION);
                    direSiJeSuisTranquilleMaintenant();
                }
                else if (actions_a_faire.ContainsKey(actions.ATTAQUER_ENNEMI) && actions_a_faire[actions.ATTAQUER_ENNEMI] != null)
                {
                    attaquerEnnemi(actions_a_faire[actions.ATTAQUER_ENNEMI]);
                    changerEtat(actions.ATTAQUER_ENNEMI);
                    direSiJeSuisTranquilleMaintenant();
                }
            }
            actions_a_faire = new Dictionary<actions, GameObject>();
            
        }
        else if (actions_de_fond_a_faire.Count > 0)
        {
            if(actions_de_fond_a_faire.ContainsKey(actions_de_fond.AIDER_COEQUIPIER) && this.Munitions>0){
                if (actions_de_fond_a_faire[actions_de_fond.AIDER_COEQUIPIER] != null)
                {
                    targetDeFond = actions_de_fond_a_faire[actions_de_fond.AIDER_COEQUIPIER].transform.position;
                    seDirigerVersObjet(targetDeFond);
                }
                else
                {
                    actions_de_fond_a_faire.Remove(actions_de_fond.AIDER_COEQUIPIER);
                }
            }
            else if (actions_de_fond_a_faire.ContainsKey(actions_de_fond.SE_DEPLACER_EN_FORMATION) && Formation!=null && Formation.Leader.Pseudo!=Pseudo)
            {
                
                targetDeFond = Formation.getSlotPositionForAgent(this);
                if (showTarget)
                {
                    if (_target_shown != null)
                        Destroy(_target_shown.gameObject);
                    _target_shown = Instantiate(crossTarget, targetDeFond, Quaternion.identity) as Transform;
                }
                seDirigerVersObjet(targetDeFond);
            }
            else if (actions_de_fond_a_faire.ContainsKey(actions_de_fond.ALLER_N_IMPORTE_OU))
            {
                seDirigerVersObjet(targetDeFond);
                if (showTarget)
                {
                    if (_target_shown != null)
                        Destroy(_target_shown.gameObject);
                    _target_shown = Instantiate(crossTarget, targetDeFond, Quaternion.identity) as Transform;
                }
                if (agent.remainingDistance < 10)
                {
                    actions_de_fond_a_faire.Remove(actions_de_fond.ALLER_N_IMPORTE_OU);
                    allerNImporteOu(200.0f);
                }
            }
        }
        else // si on n'a rien à faire
        {
            allerNImporteOu(200.0f);
        }
        
    }

    private void allerNImporteOu(float rayon)
    {
        //Debug.Log("je vais n'importe ou");
        Vector3 randomDirection = Random.insideUnitSphere * rayon;
        randomDirection += new Vector3(250,0,250);
        NavMeshHit hit;
        bool foundPoint = NavMesh.SamplePosition(randomDirection, out hit, rayon, 1);
        if (foundPoint)
        {
            Vector3 finalPosition = hit.position;
            finalPosition.y = transform.position.y;
            targetDeFond = finalPosition;
            actions_de_fond_a_faire.Add(actions_de_fond.ALLER_N_IMPORTE_OU, null);
        }
    }

    private void changerEtat(actions etat)
    {
        if (etat != this.etat_courrant)
        {
            this.etat_precedent = this.etat_courrant;
            this.etat_courrant = etat;
           // Debug.Log(this.pseudo+" : "+this.etat_courrant);
        }
    }

    private void direSiJeSuisTranquilleMaintenant()
    {
        if(this.etat_courrant != actions.FUIR_ENNEMI && this.etat_precedent == actions.FUIR_ENNEMI)
        {
            TeamMessenger.addMessage(this, null, MESSAGES_TYPE.JE_SUIS_EN_SECURITE,"");
            //Debug.Log(this.pseudo + " : JE SUIS TRANQUILLE MAINTENANT");
        }
    }
    private void direSiJaiBesionDAide()
    {
        if (this.etat_courrant == actions.FUIR_ENNEMI && etat_precedent != actions.FUIR_ENNEMI)
        {
            TeamMessenger.addMessage(this, null, MESSAGES_TYPE.JE_ME_FAIS_ATTAQUER, "");
          //  Debug.Log(this.pseudo + " : J'APPELLE A L'AIDE");
        }
    }

   
    Vector2 LineIntersectionPoint(Vector2 ps1, Vector2 pe1, Vector2 ps2, Vector2 pe2)
    {
        // Get A,B,C of first line - points : ps1 to pe1
        float A1 = pe1.y - ps1.y;
        float B1 = ps1.x - pe1.x;
        float C1 = A1 * ps1.x + B1 * ps1.y;

        // Get A,B,C of second line - points : ps2 to pe2
        float A2 = pe2.y - ps2.y;
        float B2 = ps2.x - pe2.x;
        float C2 = A2 * ps2.x + B2 * ps2.y;

        // Get delta and check if the lines are parallel
        float delta = A1 * B2 - A2 * B1;
        if (delta == 0)
            throw new System.Exception("Lines are parallel");

        // now return the Vector2 intersection point
        return new Vector2(
            (B2 * C1 - B1 * C2) / delta,
            (A1 * C2 - A2 * C1) / delta
        );
    }

    void seDirigerVersObjet(Vector3 coordonnéesObjet)
    {
        target = coordonnéesObjet;
        agent.SetDestination(target);
    }

    void traiterVisonAgent(GameObject agent)
    {
        if (agent.GetComponent<agent_behaviour>().Equipe != this.Equipe)
        {
            //Debug.Log(this.pseudo + " : je vais l'exploser ce batard");
            traiterCasVisionAgentEnnemi(agent);
        }
        else
        {
            traiterCasVisionAgentCoequipier(agent);
        }
    }

    void traiterCasVisionAgentEnnemi(GameObject ennemi)
    {
        if (this.Munitions > 0) // on attaque si on est armé
        {
            ajouterActionAFaire(actions.ATTAQUER_ENNEMI, ennemi);
        }
        else // on fui
        {
            ajouterActionAFaire(actions.FUIR_ENNEMI, ennemi);
            TeamMessenger.addMessage(this, null, MESSAGES_TYPE.JE_ME_FAIS_ATTAQUER, "");
            //Debug.Log(this.pseudo + " : AU SECOUR ");
        }
    }

    void attaquerEnnemi(GameObject ennemi)
    {
        seDirigerVersObjet(ennemi.transform.position);
        tirerSurAgentEnnemi(ennemi);
    }

    void tirerSurAgentEnnemi(GameObject ennemi)
    {
        /* RaycastHit hit;
         if (Physics.Raycast(transform.position, transform.forward, out hit, 25.0f))
         {
             // Debug.Log(this.pseudo + " : je tente le tir !");
             agent_behaviour ennemyscript = hit.collider.gameObject.GetComponent<agent_behaviour>();
             if (ennemyscript!=null && ennemyscript.pseudo==ennemi.GetComponent<agent_behaviour>().pseudo)
             {
                 emetreTir(hit.collider.gameObject);
             }
         }*/
        Vector3 init_pos = transform.position ;
        Vector3 direction = ennemi.transform.position - init_pos;
        if (direction.magnitude < 15 && timeSinceLastShoot>= fireDelay)
        {
            timeSinceLastShoot = 0;
            transform.LookAt(ennemi.transform.position);
            Transform bullet = Instantiate(_munition_fired, ballspawn.transform.position, transform.rotation) as Transform;
            bullet.tag = "munition_tiree";
            munitions--;
        }
        

        
    }
   
    void fuirEnnemi(GameObject ennemi)
    {
        Vector3 directionFuite = this.transform.position - ennemi.transform.position;
        directionFuite = this.transform.position + directionFuite;
        seDirigerVersObjet(directionFuite);
    }

    void traiterCasVisionAgentCoequipier(GameObject coequipier)
    {
        
        if (this.Formation == null)
        {
            agent_behaviour equipier = coequipier.GetComponent<agent_behaviour>();
            Formation f = equipier.demanderRejoindreFormation(this);
            if (f != null)
            {
                this.Formation = f;
                actions_de_fond_a_faire.Add(actions_de_fond.SE_DEPLACER_EN_FORMATION, null);
               // Debug.Log(this.pseudo + " : J'ai rejoin la formation de " + Formation.Leader.pseudo);
                
            }
        }
    }


    public Formation demanderRejoindreFormation(agent_behaviour agent)
    {
        if (this.Formation != null)
        {
            bool placelibre = this.Formation.placeLibre();
            if (placelibre)
            {
                this.Formation.addAgent(agent);
            }
            else
            {
                return null;
            }
        }
        return this.Formation;
    }
    public void forcerSortieDeFormation()
    {
        if(this.Formation != null && actions_de_fond_a_faire.ContainsKey(actions_de_fond.SE_DEPLACER_EN_FORMATION))
        {
            actions_de_fond_a_faire.Remove(actions_de_fond.SE_DEPLACER_EN_FORMATION);
            this.Formation = null;
        }
        
    }
    void recupererCapsuleVie(GameObject capsule)
    {
        //Debug.Log(this.pseudo + " : recuperation capsule vie");
        this.HealthPoint += GameParam.Instance.CapsuleHealthPoint;
        Destroy(capsule);
    }

    void recupererMunition(GameObject munition)
    {
      //  Debug.Log(this.pseudo + " : recuperation munition");
        this.Munitions++;
        Destroy(munition);
    }

    void OnCollisionEnter(Collision other)
    {

        switch (other.gameObject.tag)
        {
          
            case "capsule_vie":
                recupererCapsuleVie(other.gameObject);
                break;
            case "munition":
                recupererMunition(other.gameObject);
                break;      
        }
    }

    void OnTriggerStay(Collider other)
    {

        switch (other.gameObject.tag)
        {
            case "agent":
                traiterVisonAgent(other.gameObject);
                break;
            case "capsule_vie":
                ajouterActionAFaire(actions.RECUPERER_VIE, other.gameObject);
                break;
            case "munition":
                ajouterActionAFaire(actions.RECUPERER_MUNITION, other.gameObject);
                break;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
           
            case "agent":
                traiterVisonAgent(other.gameObject);
                break;
            case "capsule_vie":
                ajouterActionAFaire(actions.RECUPERER_VIE, other.gameObject);
                //recupererCapsuleVie(other.gameObject);
                break;
            case "munition":
                ajouterActionAFaire(actions.RECUPERER_MUNITION, other.gameObject);
                // seDirigerVersObjet(other.gameObject.transform.position);
                break;
        }

    }

    void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.tag)
        {
           
            case "agent":
                break;
            case "capsule_vie":
                break;
            case "munition":
                break;
        }
    }

    void ajouterActionAFaire(actions a, GameObject g)
    {
        if (!actions_a_faire.ContainsKey(a))
        {
            actions_a_faire.Add(a, g);
        }
    }

    void indiquerCible(Vector3 target)
    {
        
    }
    

    public void prendreTir()
    {
        this.HealthPoint -= 50;
       // Debug.Log(this.Pseudo + " : AIE");
        if (this.HealthPoint <= 0)
        {
            mourrir();
        }
    }

    private void mourrir()
    {
        TeamMessenger.disconnectAgent(this);
        if (this.Formation != null)
        {
            if (this.EstLeader)
                this.Formation.dissoudre();
            else
                this.Formation.removeAgent(this);
        }
        gameObject.GetComponent<ParticleSystem>().
        gameObject.GetComponent<ParticleSystem>().enableEmission = true;
        gameObject.GetComponent<ParticleSystem>().Play();
        teamCreator.temmatesList.Remove(transform);
        Destroy(this.gameObject);
    }
    public void communiquerMessage(Message m)
    {
        //  Debug.Log(this.pseudo + " : message recu");
        //   this.messageBox.Add(m);
        traiterMessage(m);
    }

    private void trierMessages(int numbers)
    {
        for (int i = 0; i < numbers; i++)
        {
            if (i < messageBox.Count) { 
                traiterMessage(messageBox[i]);
                messageBox.RemoveAt(i);
            }
        }
    }

    private void traiterMessage(Message m)
    {
        if (m.Source != null)
        {
            switch (m.MessageType)
            {
                case MESSAGES_TYPE.JE_ME_FAIS_ATTAQUER:
                    if (!actions_de_fond_a_faire.ContainsKey(actions_de_fond.AIDER_COEQUIPIER) && entraide)
                    {
                        actions_de_fond_a_faire.Add(actions_de_fond.AIDER_COEQUIPIER, m.Source.gameObject);
                       // Debug.Log(this.Pseudo + " : @"+m.Source.Pseudo+" : JE VAIS T'AIDER");
                    }
                    break;
                  case MESSAGES_TYPE.JE_SUIS_EN_SECURITE:
                    if (actions_de_fond_a_faire.ContainsKey(actions_de_fond.AIDER_COEQUIPIER) && actions_de_fond_a_faire[actions_de_fond.AIDER_COEQUIPIER]!=null &&  m.Source.Pseudo == actions_de_fond_a_faire[actions_de_fond.AIDER_COEQUIPIER].GetComponent<agent_behaviour>().Pseudo)
                    {
                        actions_de_fond_a_faire.Remove(actions_de_fond.AIDER_COEQUIPIER);
                      //  Debug.Log(this.Pseudo + " : @" + m.Source.Pseudo + " : J'ARRETE DE T'AIDER MAINTENANT");
                    }
                    break;
            }
        }
    }

    private Vector2 lireCoordonnees(string coord)
    {
        string[] split = coord.Split(';');
        return new Vector2(float.Parse(split[0]), float.Parse(split[1]));
    }
    /* void OnCollisionStay(Collision other)
        {
            Debug.Log(other.collider.gameObject.name + " détécté !");
        }
        */

    void OnDestroy()
    {
        mourrir();
    }
}
