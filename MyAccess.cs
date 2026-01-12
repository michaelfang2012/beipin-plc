namespace beipin
{
    using System;
    using System.Data;
    using System.Data.OleDb;
    using System.IO;
    using System.Windows.Forms;

    internal class MyAccess : MyDatabase
    {
        public bool m_bConnectSuccess;
        public OleDbConnection odcConnection;

        public bool CloseDatabase()
        {
            try
            {
                this.odcConnection.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ConnectToDatabase(string mdbPath)
        {
            try
            {
                if (!File.Exists(mdbPath))
                {
                    this.m_bConnectSuccess = false;
                    return false;
                }
                string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mdbPath;
                this.odcConnection = new OleDbConnection(connectionString);
                if (this.odcConnection.State == ConnectionState.Open)
                {
                    this.m_bConnectSuccess = true;
                    return false;
                }
                this.odcConnection.Open();
                this.m_bConnectSuccess = true;
                return true;
            }
            catch (Exception)
            {
                this.m_bConnectSuccess = false;
                return false;
            }
        }

        public bool DeleteData(string strDelete)
        {
            if (!this.m_bConnectSuccess)
            {
                return false;
            }
            try
            {
                new OleDbCommand(strDelete, this.odcConnection).ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool InsertData(string strInsert)
        {
            if (!this.m_bConnectSuccess)
            {
                return false;
            }
            try
            {
                new OleDbCommand(strInsert, this.odcConnection).ExecuteNonQuery();
                return true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
                return false;
            }
        }

        public bool ReadData(string strSql, ref DataTable dt)
        {
            if (!this.m_bConnectSuccess)
            {
                return false;
            }
            if (strSql == null)
            {
                return false;
            }
            try
            {
                OleDbCommand command = this.odcConnection.CreateCommand();
                command.CommandText = strSql;
                OleDbDataReader reader = command.ExecuteReader();
                int fieldCount = reader.FieldCount;
                for (int i = 0; i < fieldCount; i++)
                {
                    DataColumn column = new DataColumn(reader.GetName(i));
                    dt.Columns.Add(column);
                }
                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    for (int j = 0; j < fieldCount; j++)
                    {
                        row[reader.GetName(j)] = reader[reader.GetName(j)].ToString();
                    }
                    dt.Rows.Add(row);
                }
                reader.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateData(string strUpdate)
        {
            if (!this.m_bConnectSuccess)
            {
                return false;
            }
            try
            {
                new OleDbCommand(strUpdate, this.odcConnection).ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

