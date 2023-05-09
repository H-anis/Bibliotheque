using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.Common;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using MySql.Data;
using MySqlX.XDevAPI;

namespace Mysql
{
    public class ConnexionSql

    {

        // Variable locale pour stocker une référence vers l'instance
        private static ConnexionSql connection = null;

        private MySqlConnection mySqlCn;

        private static readonly object mylock = new object();




        private ConnexionSql(string unProvider, string uneDataBase, string unUid, string unMdp)
        {


            try
            {
                string connString;

                connString = "SERVER=" + unProvider + ";" + "DATABASE=" +
                uneDataBase + ";" + "UID=" + unUid + ";" + "PASSWORD=" + unMdp + ";";

                try
                {
                    mySqlCn = new MySqlConnection(connString);
                }
                catch (Exception emp)
                {
                    throw (emp);
                }
            }
            catch (Exception emp)
            {
                throw (emp);

            }



        }



        /**
         * méthode de création d'une instance de connexion si elle n'existe pas (singleton)
         */
        public static ConnexionSql getInstance(string unProvider, string uneDataBase, string unUid, string unMdp)
        {

            lock ((mylock))
            {

                try
                {


                    if (null == connection)
                    { // Premier appel
                        connection = new ConnexionSql(unProvider, uneDataBase, unUid, unMdp);
                    }
                }
                catch (Exception emp)
                {

                    throw (emp);

                }
                return connection;

            }
        }

        /**
         * Ouverture de la connexion
         */
        public void openConnection()
        {
            try
            {
                if (mySqlCn.State == System.Data.ConnectionState.Closed)
                    mySqlCn.Open();
            }
            catch (Exception emp)
            {

                throw (emp);
            }
        }

        /**
         * Fermeture de la connexion
         */
        public void closeConnection()
        {
            if (mySqlCn.State == System.Data.ConnectionState.Open)
                mySqlCn.Close();
        }

        /**
         * Exécution d'une requête
         */
        public void reqExec(string req)
        {
            MySqlCommand mysqlCom = new MySqlCommand(req, this.mySqlCn);
            mysqlCom.ExecuteNonQuery();
        }
        public MySqlDataReader reqRead(string request)
        {
            MySqlDataReader read;
           // MySqlCommand mysqlCom = new MySqlCommand("select "+selectedValue+" from "+selectedTable, mySqlCn);
            MySqlCommand mysqlCom = new MySqlCommand(request, mySqlCn);
            read = mysqlCom.ExecuteReader();
            return(read);
        }

        public DataTable getData(string selectedTable)
        {
            DataTable dt = new DataTable();
            MySqlDataReader reader = this.reqRead(selectedTable);
            for (int i = 0; i < reader.FieldCount; i++)
            {
                dt.Columns.Add(reader.GetName(i));
            }

            while (reader.Read())
            {

                DataRow dr = dt.NewRow();

                for (int i = 0; i < reader.FieldCount; i++)
                {

                    dr[i] = reader.GetValue(i);

                }

                dt.Rows.Add(dr);

            }

            reader.Close();

            return (dt);
        }
    }
}