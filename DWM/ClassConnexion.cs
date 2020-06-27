using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DWM
{
    static  class ClassConnexion
    {
        public static string Conx = "SERVER =localhost;PORT =3308; DATABASE =dwm; UID =root; PASSWORD =2845;CHARSET=utf8";
        public static MySqlConnection Macon = new MySqlConnection(Conx);
        public static MySqlDataAdapter DA;
        public static MySqlDataReader DR;

    }
}
