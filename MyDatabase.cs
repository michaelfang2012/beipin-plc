namespace beipin
{
    using System;
    using System.Data;

    internal interface MyDatabase
    {
        bool CloseDatabase();
        bool ConnectToDatabase(string mdbPath);
        bool DeleteData(string strDelete);
        bool InsertData(string strInsert);
        bool ReadData(string strSql, ref DataTable dt);
        bool UpdateData(string strUpdate);
    }
}

