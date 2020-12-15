using System;
using System.Collections.Generic;
using System.Text;

namespace TestShopDb
{
    class Program
    {
        private static string usage = "usage:\nsave <shopId> <url> \nprint <shopId>";
        static void Main(string[] args)
        {
            if(args.Length > 0)
            {
                var Driver = new DbDriver();
                List<DbRecord> records;
                try
                {
                    //check args, select mode or close with usage info
                    switch (args[0])
                    {
                        case "save":
                            var p = new Parser();
                            records = p.GetList(args[2]);
                            if(records.Count > 0)
                            {
                                Driver.RefreshShopData(args[1], records);
                                Console.WriteLine("Update successful");
                            }
                            else
                            {
                                Console.WriteLine("New offer list is empty, old version to be used");
                            }
                            break;
                        case "print":
                            records = Driver.ExtractData(args[1]);
                            var printer = new Printer();
                            printer.Print(records);
                            break;
                        default:
                            Console.WriteLine(usage);
                            break;
                    }
                }
                catch(IndexOutOfRangeException)
                {
                    Console.WriteLine(usage);
                }
                catch (Exception)
                {
                    return;
                }
            }
            else
            {
                Console.WriteLine(usage);
            }
        }
    }
}
