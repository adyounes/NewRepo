using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaLibrairie
{
    public enum Erreurs { ErrMail, ErrTel, ErrHorsLimite, ErrAutres}
    public class MonException:Exception
    {
        public Erreurs CodeErreur { get; init; }
        public MonException(Erreurs codeErreur = Erreurs.ErrAutres)
        {
            CodeErreur = codeErreur;
        }

        public override string Message
        {
            get
            {
                string message = "Erreur - ";
                switch (CodeErreur)
                {
                    case Erreurs.ErrMail:
                        message += "Mail Invalide";
                        break;
                    case Erreurs.ErrTel:
                        message += "Tel Invalide";
                        break;
                    case Erreurs.ErrHorsLimite:
                        message += "En dehors des limites du conteneur ";
                        break;
                    default:
                        message += "Autres Cas";
                        break;
                }
                return message;


            }


        }
    }
}
