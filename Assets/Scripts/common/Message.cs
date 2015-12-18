using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.common
{
    public enum MESSAGES_TYPE { JE_ME_FAIS_ATTAQUER, JE_SUIS_EN_SECURITE };
    public class Message
    {
        private agent_behaviour destinataire;
        private agent_behaviour source;

        public agent_behaviour Source
        {
            get { return source; }
            set { source = value; }
        }

        public agent_behaviour Destinataire
        {
            get { return destinataire; }
            set { destinataire = value; }
        }
        private MESSAGES_TYPE messageType;

        public MESSAGES_TYPE MessageType
        {
            get { return messageType; }
            set { messageType = value; }
        }
        private string infos;

        public string Infos
        {
            get { return infos; }
            set { infos = value; }
        }


    }
}
