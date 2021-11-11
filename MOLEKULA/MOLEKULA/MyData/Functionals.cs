using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Diagnostics;
using System.IO;

namespace MOLEKULA.MyData
{
    class Functionals
    {
        private Functionals() { }
        private static Functionals _Functionals;
        public static Functionals DB()
        {
            Properties.Settings.Default["Connection"] = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + Directory.GetCurrentDirectory() + @"\Molekules.mdf;Integrated Security=True;Connect Timeout=30";
            if (_Functionals == null)
            {
                _Functionals = new Functionals();
            }
            return _Functionals;
        }

        public string nameDB = Properties.Settings.Default.Connection;
        Random rand = new Random();
        public void test()
        {
            SqlConnection conn = new SqlConnection(nameDB);
            conn.Open();

            string sql = "INSERT INTO Types (types,color) " +
                "values (@types,@color)";

            using (SqlCommand myCommand = new SqlCommand(sql, conn))
            {
                myCommand.Parameters.AddWithValue("@types", "type");
                myCommand.Parameters.AddWithValue("@color", 1);

                myCommand.ExecuteNonQuery();
            }

            conn.Close();
        }


        public int GetCount(string sqlQuery)
        {
            SqlConnection conn = new SqlConnection(nameDB);
            conn.Open();

            int x = 0;

            using (SqlCommand myCommand = new SqlCommand(sqlQuery, conn))
            {
                SqlDataReader reader = myCommand.ExecuteReader();
                while (reader.Read()) x++;
                reader.Close();
            }
            conn.Close();
            return x;
        }
        public int GetMax(string sql, string selectCol)
        {
            SqlConnection conn = new SqlConnection(nameDB);
            conn.Open();

            int max = 0, x;

            using (SqlCommand myCommand = new SqlCommand(sql, conn))
            {
                SqlDataReader reader = myCommand.ExecuteReader();

                while (reader.Read())
                {
                    x = reader.GetInt32(reader.GetOrdinal(selectCol));
                    if (x > max) max = x;
                };
                reader.Close();
            }
            conn.Close();
            return max;
        }
        public int GetMin(string sql, string selectCol)
        {
            SqlConnection conn = new SqlConnection(nameDB);
            conn.Open();

            int min = int.MaxValue, x;

            using (SqlCommand myCommand = new SqlCommand(sql, conn))
            {
                SqlDataReader reader = myCommand.ExecuteReader();

                while (reader.Read())
                {
                    x = reader.GetInt32(reader.GetOrdinal(selectCol));
                    if (x < min) min = x;
                };
                reader.Close();
            }
            conn.Close();
            return min;
        }
        public int GetId(string Table = "Atoms", string selectCols = "id_atom")
        {
            SqlConnection conn = new SqlConnection(nameDB);
            conn.Open(); int max = 0, x;

            using (SqlCommand myCommand = new SqlCommand($"select {selectCols} from {Table}", conn))
            {
                SqlDataReader reader = myCommand.ExecuteReader();

                while (reader.Read())
                {
                    x = reader.GetInt32(reader.GetOrdinal(selectCols));
                    if (x > max) max = x;
                };
                reader.Close();
            }
            conn.Close();
            return max;
        }

        public string GetString<T>(string Table, string colName,string idCol, T id)
        {
            SqlConnection conn = new SqlConnection(nameDB);
            conn.Open();
            string myValue = "";
            string sql = string.Format($"select {colName} from {Table} where {idCol} = '{id}'");
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                    myValue = reader.GetString(reader.GetOrdinal(colName));

                reader.Close();
            }
            conn.Close();
            return myValue;
        }
        public int GetInt<T>(string Table, string colName, string idCol, T id)
        {
            SqlConnection conn = new SqlConnection(nameDB);
            conn.Open();
            int myValue = 0;
            string sql = string.Format($"select {colName} from {Table} where {idCol} = '{id}'");
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                    myValue = reader.GetInt32(reader.GetOrdinal(colName));

                reader.Close();
            }
            conn.Close();
            return myValue;
        }
        public double GetFloat<T>(string Table, string colName, string idCol, T id)
        {
            SqlConnection conn = new SqlConnection(nameDB);
            conn.Open();
            double myValue = 0;
            string sql = string.Format($"select {colName} from {Table} where {idCol} = '{id}'");
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                    myValue = reader.GetDouble(reader.GetOrdinal(colName));

                reader.Close();
            }
            conn.Close();
            return myValue;
        }
        public int GetInt(string sql, string colName)
        {
            SqlConnection conn = new SqlConnection(nameDB);
            conn.Open();
            int myValue = 0;
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                    myValue = reader.GetInt32(reader.GetOrdinal(colName));

                reader.Close();
            }
            conn.Close();
            return myValue;
        }
        public int GetColor(string type)
        {
            SqlConnection conn = new SqlConnection(nameDB);
            conn.Open();
            int myValue = 0;
            string sql = $"select color from Types where types='{type}'";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                    myValue = reader.GetInt32(reader.GetOrdinal("color"));

                reader.Close();
            }
            conn.Close();
            return myValue;
        }

        public DataTable GetQuery(string myQuery)
        {
            SqlConnection conDB = new SqlConnection(nameDB);//all
            conDB.Open();

            DataTable tabord = new DataTable();
            SqlDataAdapter dazak = new SqlDataAdapter(myQuery, conDB);
            dazak.Fill(tabord);

            return tabord;
        }
        public DataTable GetTable(string tableName)
        {
            SqlConnection conDB = new SqlConnection(nameDB);
            conDB.Open();
            DataTable tabord = new DataTable();
            SqlDataAdapter dazak = new SqlDataAdapter("SELECT * FROM " + tableName, conDB);
            dazak.Fill(tabord);
            return tabord;
        }

        public bool DeleteFrom(int delID, string Table, string colName)
        {

            SqlConnection conn = new SqlConnection(nameDB);
            conn.Open();
            string sql = string.Format($"Delete from {Table} where {colName} = '{delID}'");
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                { Exception error = new Exception("К сожалению!", ex); throw error; }
            }
            conn.Close();
            return true;
        }
        public bool DeleteFrom<T>(T delID, string Table, string colName)
        {

            SqlConnection conn = new SqlConnection(nameDB);
            conn.Open();
            string sql = string.Format($"Delete from {Table} where {colName} = '{delID}'");
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                { Exception error = new Exception("К сожалению!", ex); throw error; }
            }
            conn.Close();
            return true;
        }
        public bool Delete(string sql)
        {
            SqlConnection conn = new SqlConnection(nameDB);
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                { 
                    
                }
            }
            conn.Close();
            return true;
        }

        public bool Add(int id_type, double pos_x, double pos_y, double pos_z, int anim_i, int con)
        {
            SqlConnection conn = new SqlConnection(nameDB);
            conn.Open();

            string sql = "INSERT INTO Atoms (id_type,pos_x,pos_y,pos_z,anim_i) " +
                "values (@id_type,@pos_x,@pos_y,@pos_z,@anim_i)";

            using (SqlCommand myCommand = new SqlCommand(sql, conn))
            {
                myCommand.Parameters.AddWithValue("@id_type", id_type);
                myCommand.Parameters.AddWithValue("@pos_x", pos_x);
                myCommand.Parameters.AddWithValue("@pos_y", pos_y);
                myCommand.Parameters.AddWithValue("@pos_z", pos_z);
                myCommand.Parameters.AddWithValue("@anim_i", anim_i);
                myCommand.ExecuteNonQuery();
            }

            sql = "INSERT INTO Connects (id_atom,id_conn) " +
                "values (@id_atom,@id_conn)";

            using (SqlCommand myCommand = new SqlCommand(sql, conn))
            {
                myCommand.Parameters.AddWithValue("@id_atom", GetId());
                myCommand.Parameters.AddWithValue("@id_conn", con);
                myCommand.ExecuteNonQuery();
            }
            conn.Close();
            return true;
        }
        public bool AddMolekula(int id_mol, string name_mol)
        {
            string sql; int count = GetCount($"select * from Molekuls where id_mol={id_mol}");
            if (count == 0)
                sql = $"INSERT INTO Molekuls (name_mol) values ('{name_mol}')";
            else sql = $"UPDATE Molekuls SET name_mol='{name_mol}' where id_mol={id_mol}";



            SqlConnection conn = new SqlConnection(nameDB);
            conn.Open();

            using (SqlCommand myCommand = new SqlCommand(sql, conn))
            {
                if(count!=0)myCommand.Parameters.AddWithValue("@id_mol", id_mol);
                myCommand.Parameters.AddWithValue("@name_mol", name_mol);
                myCommand.ExecuteNonQuery();
            }
            conn.Close();
            return true;
        }
        public bool AddMolAtom(int id_mol, int id_atom)
        {
            string sql = $"INSERT INTO Mol_atom (id_mol,id_atom) values (@id_mol,@id_atom)";
            SqlConnection conn = new SqlConnection(nameDB);
            conn.Open();
            using (SqlCommand myCommand = new SqlCommand(sql, conn))
            {
                myCommand.Parameters.AddWithValue("@id_mol", id_mol);
                myCommand.Parameters.AddWithValue("@id_atom", id_atom);
                myCommand.ExecuteNonQuery();
                try { }
                catch (Exception a) { return false; }
            }
            conn.Close();
            return true;
        }
        public bool Update(int id_atom,int id_type, double pos_x, double pos_y, double pos_z, int anim_i, int con)
        {
            SqlConnection conn = new SqlConnection(nameDB);
            conn.Open();

            string sql = $"UPDATE Atoms SET id_type=@id_type,pos_x=@pos_x,pos_y=@pos_y,pos_z=@pos_z,anim_i=@anim_i " +
                " Where id_atom=@id_atom";

            using (SqlCommand myCommand = new SqlCommand(sql, conn))
            {
                myCommand.Parameters.AddWithValue("@id_atom", id_atom);
                myCommand.Parameters.AddWithValue("@id_type", id_type);
                myCommand.Parameters.AddWithValue("@pos_x", pos_x);
                myCommand.Parameters.AddWithValue("@pos_y", pos_y);
                myCommand.Parameters.AddWithValue("@pos_z", pos_z);
                myCommand.Parameters.AddWithValue("@anim_i", anim_i);
                myCommand.ExecuteNonQuery();
            }

            sql = $"UPDATE Connects SET id_conn=@id_conn Where id_atom=@id_atom";
            using (SqlCommand myCommand = new SqlCommand(sql, conn))
            {
                myCommand.Parameters.AddWithValue("@id_atom", id_atom);
                myCommand.Parameters.AddWithValue("@id_conn", con);
                myCommand.ExecuteNonQuery();
            }
            conn.Close();
            return true;
        }
        public DataView Obnova(string tableName)
        {
            SqlConnection cnn = new SqlConnection(nameDB);
            cnn.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter dacust = new SqlDataAdapter("select * from " + tableName, cnn);
            dacust.Fill(ds, tableName);

            dacust.Update(ds.Tables[tableName]);
            ds.AcceptChanges();

            DataTable tabcust = ds.Tables[tableName];
            DataView myDataView = new DataView(tabcust);

            return myDataView;
        }
        public void GetSmaile(bool sms = false)
        {
            string img = Properties.Settings.Default.Logo;int i = rand.Next(24, 87);
            if (i < 54) img += "1_"; img += i.ToString()+".jpg";
            string data = DateTime.Now.ToString().Split(' ')[1];
            int hour = Convert.ToInt32(data.Split(':')[0]);
            int hour1 = Properties.Settings.Default.DateSmile;
            int min = Convert.ToInt32(data.Split(':')[1]);
            int min1 = Properties.Settings.Default.TimeSmile;

            if (hour == hour1 && (min>=min1-5 && min<=min1+5) && DateTime.Now.ToString().IndexOf(Properties.Settings.Default.maxDate)>=0)
            {
                if(sms)MessageBox.Show(smska);
                else Process.Start(img);
            }
        }
        public DataView SqlQuery(string sql_query)
        {
            SqlConnection cnn = new SqlConnection(nameDB);
            cnn.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter dacust = new SqlDataAdapter(sql_query, cnn);
            dacust.Fill(ds, "Table");

            dacust.Update(ds.Tables["Table"]);
            ds.AcceptChanges();

            DataTable tabcust = ds.Tables["Table"];
            DataView myDataView = new DataView(tabcust);

            return myDataView;
        }
        string smska = "Улыбнись) Я тебя очень сильно люблю";
        public DataView SqlTable(string tableName, string colSort)
        {
            SqlConnection cnn = new SqlConnection(nameDB);
            cnn.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter dacust = new SqlDataAdapter("select * from " + tableName, cnn);
            dacust.Fill(ds, tableName);

            dacust.Update(ds.Tables[tableName]);
            ds.AcceptChanges();

            DataTable tabcust = ds.Tables[tableName];
            DataView myDataView = new DataView(tabcust){ Sort = colSort + " asc"};

            return myDataView;
        }
        
    }
}
