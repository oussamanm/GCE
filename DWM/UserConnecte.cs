using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DWM
{
    static class UserConnecte
    {
        public static int IdUser;
        public static string NomUser ;
        public static string PrenomUser;
        public static int IdType;
        public static string LibelleType;
        public static string PseudoUser;
        public static string PasseUser;
        public static string EmailUser;
        public static string TeleUser;

        public static DateTime CompHeur= DateTime.Now;

        public static void login(string Pseudo, string pass)
        {
            ClassConnexion.Macon.Open();
            MySqlCommand Requetuser = new MySqlCommand("select * from utilisateurs U,typeutilisateur TU where U.IdType=TU.IdType and U.PseudoUser='" + Pseudo + "' and PasseUser='" + pass + "'", ClassConnexion.Macon);
            ClassConnexion.DR = Requetuser.ExecuteReader();
            ClassConnexion.DR.Read();
            if (ClassConnexion.DR.HasRows)
            {
                IdUser = int.Parse(ClassConnexion.DR["IdUser"].ToString());
                NomUser = ClassConnexion.DR["NomUser"].ToString();
                PrenomUser = ClassConnexion.DR["PrenomUser"].ToString();
                PseudoUser = ClassConnexion.DR["PseudoUser"].ToString();

                EmailUser = ClassConnexion.DR["EmailUser"].ToString();
                PasseUser = ClassConnexion.DR["PasseUser"].ToString();
                TeleUser = ClassConnexion.DR["TeleUser"].ToString();

                IdType = int.Parse(ClassConnexion.DR["IdType"].ToString().ToString());
                LibelleType= ClassConnexion.DR["LibelleType"].ToString();

                CompHeur = DateTime.Now;
            }
      
            ClassConnexion.DR.Close();
            ClassConnexion.Macon.Close();
        }
    }
}
