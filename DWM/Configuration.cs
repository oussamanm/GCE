using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using System.Net;

namespace DWM
{
   static class Configuration
    {

        public static  string Func(int idcon) 
        {
            if (ClassConnexion.Macon.State == ConnectionState.Closed)
            {
                ClassConnexion.Macon.Open();
            }       

            MySqlCommand Requetuser = new MySqlCommand("select * from configuration where IdConf='"+ idcon + "'", ClassConnexion.Macon);
            ClassConnexion.DR = Requetuser.ExecuteReader();
            ClassConnexion.DR.Read();

            string resultat = ClassConnexion.DR["LibEntr"].ToString();
            ClassConnexion.DR.Close();
            ClassConnexion.Macon.Close();
            return resultat;
        }
        public static int LastID(string table,string idenr)
        {
            if (ClassConnexion.Macon.State == ConnectionState.Closed)
            {
                ClassConnexion.Macon.Open();
            }
            int existe = 0;
            int resultat = 0;
            MySqlCommand count = new MySqlCommand("select count(*) as con from "+ table + " ",ClassConnexion.Macon);
            ClassConnexion.DR = count.ExecuteReader();
            ClassConnexion.DR.Read();
            existe = int.Parse(ClassConnexion.DR["con"].ToString());
            ClassConnexion.DR.Close();

            if (existe != 0)
            {
                MySqlCommand Requetlast = new MySqlCommand("select MAX("+ idenr + ") as lastid from "+table+" ", ClassConnexion.Macon);
                ClassConnexion.DR = Requetlast.ExecuteReader();
                ClassConnexion.DR.Read();              
                resultat = int.Parse(ClassConnexion.DR["lastid"].ToString());
                ClassConnexion.DR.Close();              
            }
            
            ClassConnexion.Macon.Close();
            return resultat;
        }
        public static string ReturnValueMax(string Req)
        {
            try
            {
                if (ClassConnexion.Macon.State == ConnectionState.Closed)
                {
                    ClassConnexion.Macon.Open();
                }
                string Resu = "";
                MySqlCommand CmdReturnvalue = new MySqlCommand(Req, ClassConnexion.Macon);
                ClassConnexion.DR = CmdReturnvalue.ExecuteReader();
                ClassConnexion.DR.Read();
                Resu = ClassConnexion.DR[0].ToString();
                ClassConnexion.DR.Close();
                ClassConnexion.Macon.Close();
                return Resu;
            }
            catch (Exception ex)
            {
                return "";
                MessageBox.Show(ex.ToString());
            }
        }
        public static void Historique(int type, string Requete, string MessgeAncien, string MessageNouveau, string MessageEntete, string requeteSupp1, string requeteSupp2)
        {
            if (Func(15) == "Indirect")
            {
                String formatdt = "yyyy-MM-dd HH:mm:ss";
                int lastidhistorie = LastID("historiqueMod", "IdHis") + 1;
                ClassConnexion.Macon.Open();
                MySqlCommand Comajouter;
                Comajouter = new MySqlCommand("insert into historiqueMod value(" + lastidhistorie + "," + type + "," + UserConnecte.IdUser + ",0,'" + DateTime.Now.ToString(formatdt) + "',NULL,'" + Requete + "','" + requeteSupp1 + "','" + requeteSupp2 + "','" + MessgeAncien + "','" + MessageNouveau + "','" + MessageEntete + "')", ClassConnexion.Macon);
                Comajouter.ExecuteNonQuery();

                ClassConnexion.Macon.Close();

            }
            else if (Func(15) == "Direct")
            {
                Requete = Requete.Replace("\\\'", "'");

                ClassConnexion.Macon.Open();
                MySqlCommand Cmdajouter = new MySqlCommand(Requete, ClassConnexion.Macon);
                Console.WriteLine(Requete);
                Cmdajouter.ExecuteNonQuery();
                ClassConnexion.Macon.Close();

                if (requeteSupp1 != "")
                {
                    requeteSupp1 = requeteSupp1.Replace("\\\'", "'");

                    ClassConnexion.Macon.Open();
                    MySqlCommand CmdRequ1 = new MySqlCommand(requeteSupp1, ClassConnexion.Macon);
                    CmdRequ1.ExecuteNonQuery();
                    ClassConnexion.Macon.Close();
                }
                if (requeteSupp2 != "")
                {
                    requeteSupp2 = requeteSupp2.Replace("\\\'", "'");

                    ClassConnexion.Macon.Open();
                    MySqlCommand CmdRequ2 = new MySqlCommand(requeteSupp2, ClassConnexion.Macon);
                    CmdRequ2.ExecuteNonQuery();
                    ClassConnexion.Macon.Close();
                }
            }

        }
        public static int ExisteEnre(string table, string champ,string condition)
        {
            if (ClassConnexion.Macon.State == ConnectionState.Closed)
                ClassConnexion.Macon.Open();

            MySqlCommand Requetext = new MySqlCommand("select count(*) as nbr from "+ table + " where  " + champ + " = "+ condition + "  ", ClassConnexion.Macon);
            ClassConnexion.DR = Requetext.ExecuteReader();

            ClassConnexion.DR.Read();

            int resultat = int.Parse(ClassConnexion.DR["nbr"].ToString());
            ClassConnexion.DR.Close();
            ClassConnexion.Macon.Close();
            return resultat;
        }
        public static int ExisteEnreSansCondition(string table)
        {
            if (ClassConnexion.Macon.State == ConnectionState.Closed)
                ClassConnexion.Macon.Open();

            MySqlCommand Requetext = new MySqlCommand("select count(*) as nbr from " + table +" ", ClassConnexion.Macon);
            ClassConnexion.DR = Requetext.ExecuteReader();

            ClassConnexion.DR.Read();

            int resultat = int.Parse(ClassConnexion.DR["nbr"].ToString());
            ClassConnexion.DR.Close();
            ClassConnexion.Macon.Close();
            return resultat;
        }
        public static float CalculMtConsommtion(int quantite)
        {


            int nbrtr = Configuration.ExisteEnre("tranches", "IdTran", "0 or IdTran>=0 ");
            int isuiv = 0;
            string conffu = Configuration.Func(14);
            int mttr, smax, lastcon = 0, nvcon = 0;
            float MtTotal = 0;
            float quarest = 0;
            quarest = quantite;
            ClassConnexion.Macon.Open();
            MySqlCommand Requetext = new MySqlCommand("select SeuilMinTran,PrixUTran from tranches order by SeuilMinTran asc ", ClassConnexion.Macon);
            MySqlDataReader DRcal = Requetext.ExecuteReader();

            if (conffu == "Facil")
            {
                // Methode simple

                while (DRcal.Read())
                {
                    if (quarest == 0)
                        break;

                    isuiv = isuiv + 1;
                    
                    if (isuiv == nbrtr)
                    {
                        MtTotal = MtTotal + (quarest * float.Parse(DRcal["PrixUTran"].ToString()));
                        break;
                        //if (quarest > 0)
                        //{
                        //    MtTotal = MtTotal + ((quarest - (int.Parse(DRcal["SeuilMinTran"].ToString()) - lastcon)) * float.Parse(DRcal["PrixUTran"].ToString()));
                        //}
                    }

                    if (quarest >= int.Parse(DRcal["SeuilMinTran"].ToString()) - lastcon)
                    {
                        MtTotal = MtTotal + ((float.Parse(DRcal["SeuilMinTran"].ToString()) - lastcon) * float.Parse(DRcal["PrixUTran"].ToString()));
                        quarest = quarest - (float.Parse(DRcal["SeuilMinTran"].ToString()) - lastcon);
                        lastcon = int.Parse(DRcal["SeuilMinTran"].ToString());
                    }
                    else
                    {
                        MtTotal = MtTotal + (quarest * float.Parse(DRcal["PrixUTran"].ToString()));
                        quarest = 0;
                    }
                }
                // Fin methode simple 
            }
            else
            {
                // Fort

                while (DRcal.Read())
                {
                    isuiv = isuiv + 1;

                    if (quantite<= float.Parse(DRcal["SeuilMinTran"].ToString()) || isuiv== nbrtr)
                    {
                        MtTotal = quantite * float.Parse(DRcal["PrixUTran"].ToString());
                        break;
                    }
                }
                    // Fin Fort
             }

            DRcal.Close();
            ClassConnexion.Macon.Close();
            return MtTotal;
        }
        public static void RempCombo(ComboBox Cb, string Requ, string Table, String Value, String Display)
        {
            DataSet dsRemp = new DataSet();
            MySqlDataAdapter daRemp = new MySqlDataAdapter(Requ, ClassConnexion.Macon);
            daRemp.Fill(dsRemp, Table);
            Cb.DataSource = dsRemp.Tables[Table];
            Cb.ValueMember = Value;
            Cb.DisplayMember = Display;
        }
        public static void RempComboSimple(System.Windows.Forms.ComboBox Cb, string strReq, string V, string D)
        {
            DataTable Tab = new DataTable();
            if (ClassConnexion.Macon.State == ConnectionState.Closed)
                ClassConnexion.Macon.Open();

            using (MySqlCommand CmdRempCombo = new MySqlCommand(strReq, ClassConnexion.Macon))
            {
                ClassConnexion.DR = CmdRempCombo.ExecuteReader(CommandBehavior.CloseConnection);
                Tab.Load(ClassConnexion.DR);
                Cb.DataSource = Tab;
                Cb.ValueMember = V;
                Cb.DisplayMember = D;
            }
        }
        public static String ConvertToMony(float Nbr)
        {
            return Nbr.ToString("N2", CultureInfo.CreateSpecificCulture("ar-MA"));
        }
        public static String ConvertToMonyC(float Nbr)
        {
            return Nbr.ToString("c2", CultureInfo.CreateSpecificCulture("fr-MA"));
        }
        public static void  TestIn()
        {
            string formatt = "yyyy-MM-dd";
            int NbrChjs = 0;
            Boolean Bl, BlH ;
            Bl = BlH = true;
            MySqlDataReader drr;
            string StrIdHd = HardwareInfo.GetBIOSserNo();

            try
            {
                if (ClassConnexion.Macon.State==ConnectionState.Closed)
                    ClassConnexion.Macon.Open();

                MySqlCommand CmdNbrChh = new MySqlCommand("select LibEntr from configuration where IdConf=19 ", ClassConnexion.Macon);
                drr = CmdNbrChh.ExecuteReader();
                while (drr.Read())
                {
                    NbrChjs = int.Parse(drr["LibEntr"].ToString());
                }
                drr.Close();

                //// test Date(NbrCh)
                MySqlCommand Cmd = new MySqlCommand("select LibEntr from configuration where IdConf=17 ", ClassConnexion.Macon);
                drr = Cmd.ExecuteReader();
                drr.Read();
                if (drr.HasRows)
                {
                    if (drr["LibEntr"].ToString() == "")
                        Bl = true;
                    else
                    {
                        DateTime date;
                        date = Convert.ToDateTime(drr["LibEntr"].ToString());
                        date = date.AddDays(NbrChjs);

                        DateTime dt1 = DateTime.Parse(date.ToString(formatt));
                        DateTime dt2 = DateTime.Now;

                        if (dt2.Date >= dt1.Date)
                            Bl = true;
                        else
                            Bl = false;
                    }
                }
                drr.Close();

                //// test IdDD

                MySqlCommand CmdTID = new MySqlCommand("select LibEntr from configuration  where IdConf=18", ClassConnexion.Macon);
                drr = CmdTID.ExecuteReader();
                drr.Read();
                if (drr.HasRows)
                {
                    if (drr["LibEntr"].ToString() != StrIdHd)
                        BlH = false;
                }
                drr.Close();

                if (Bl == true || BlH == false)
                {
                    MySqlCommand Cmdupd = new MySqlCommand("update configuration set LibEntr=0 where IdConf=16", ClassConnexion.Macon);
                    Cmdupd.ExecuteNonQuery();
                }
                ClassConnexion.Macon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public static string CalculeRequte(string Req)
        {
            if (ClassConnexion.Macon.State == ConnectionState.Closed)
                ClassConnexion.Macon.Open();
            string Resu = "";
            using (MySqlCommand Cmd = new MySqlCommand(Req, ClassConnexion.Macon))
            {
                MySqlDataReader dr = Cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dr.Read())
                {
                    if (dr[0].ToString() != "")
                    {
                        Resu = dr[0].ToString();
                        dr.Close();
                        return Resu;
                    }
                }
                dr.Close();
                return "";
            }
        }
        public static DataTable GetDataTable(string Req)
        {
            DataTable dataTable = new DataTable();
            using (MySqlConnection Con = new MySqlConnection(ClassConnexion.Conx))
            {
                MySqlCommand Cmd = new MySqlCommand(Req, Con);

                ClassConnexion.DR = Cmd.ExecuteReader();
                if (ClassConnexion.DR.Read())
                    dataTable.Load(ClassConnexion.DR);

                ClassConnexion.DR.Close();
            }
            return dataTable;
        }
    }
}
