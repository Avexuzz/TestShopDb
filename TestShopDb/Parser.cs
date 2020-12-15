using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Xml;
using System.IO;

namespace TestShopDb
{
    class Parser
    {
        public List<DbRecord> GetList(string Url)
        {
            //add win1251 coding page to read xml
            var ret = new List<DbRecord>();
            EncodingProvider win1251 = CodePagesEncodingProvider.Instance;
            win1251.GetEncoding(1251);
            Encoding.RegisterProvider(win1251);
            //parse offers from xml
            XmlDocument xDoc = new XmlDocument();
            try
            {
                xDoc.Load(Url);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
            XmlElement xRoot = xDoc.DocumentElement;
            try
            {
                foreach (XmlElement xnode in xRoot)
                {
                    if (xnode.Name == "shop")
                    {
                        foreach (XmlElement childnode in xnode)
                        {
                            if (childnode.Name == "offers")
                            {
                                foreach (XmlElement offernode in childnode)
                                {
                                    if (offernode.Name == "offer")
                                    {
                                        foreach (XmlElement attnode in offernode)
                                        {
                                            if (attnode.Name == "name")
                                            {
                                                ret.Add(new DbRecord { IdInShop = Int32.Parse(offernode.GetAttribute("id")), Name = attnode.InnerText });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch(NullReferenceException e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
            
            return ret;
        }
    }
}
