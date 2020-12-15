using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Configuration;

namespace TestShopDb
{
    class DbDriver
    {
        SqlConnection conn;

        public DbDriver()
        {
            //open connection to db
            try
            {
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);//rework?
                conn.Open();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        ~DbDriver()
        {
            conn.Close();
        }

        public void AddShopData(string ShopId, List<DbRecord> dbRecords)
        {
            //insert new rows into table
            foreach (DbRecord record in dbRecords)
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = conn;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = @"
INSERT 
    INTO Goods (Name, IdInShop, ShopId)
    VALUES(@dbRecordName, @dbRecordIdInShop, @ShopId)";
                    command.Parameters.AddWithValue("@dbRecordName", record.Name);
                    command.Parameters.AddWithValue("@dbRecordIdInShop", record.IdInShop);
                    command.Parameters.AddWithValue("@shopId", ShopId);
                    try
                    {
                        int rows = command.ExecuteNonQuery();
                    }
                    catch(SqlException e)
                    {
                        Console.WriteLine(e.Message);
                        throw e;
                    }
                }

            }
        }

        public void DeleteShopData(string ShopId)
        {
            //delete old rows
            using (var command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = @"
DELETE 
    FROM Goods
    WHERE Goods.ShopId = @ShopId;";
                command.Parameters.AddWithValue("@shopId", ShopId);
                try
                {
                    int rows = command.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                    throw e;
                }
            }
        }

        public void RefreshShopData(string ShopId, List<DbRecord> dbRecords)
        {
            //simpliest update: delete old offers, insert new
            DeleteShopData(ShopId);
            AddShopData(ShopId, dbRecords);
        }

        public List<DbRecord> ExtractData(string ShopId)
        {
            //select shop's offers from db
            var ret = new List<DbRecord>();
            using (var command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = @"
SELECT Goods.IdInShop, Goods.Name
FROM Goods
WHERE ShopId = @ShopId";
                command.Parameters.AddWithValue("@shopId", ShopId);
                try
                {
                    using (DbDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ret.Add(new DbRecord { IdInShop = dr.GetInt32(0), Name = dr.GetString(1) });
                        }
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                    throw e;
                }

            }
            return ret;
        }
    }
    
}
