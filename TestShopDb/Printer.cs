using System;
using System.Collections.Generic;
using System.Text;

namespace TestShopDb
{
    class Printer
    {

        public void Print(List<DbRecord> dbRecords)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Id;Name");
            foreach(DbRecord record in dbRecords)
            {
                Console.WriteLine(record.IdInShop + ";" + record.Name);
            }
        }

    }
}
