﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using RepositoryParser.Core.Interfaces;

namespace RepositoryParser.Core.Models
{
    public class SqLiteService : ISqLiteService
    {
        public SQLiteConnection Connection { get; set; }

        private string DBName
        {
            get; set;

        }

        private bool isOpened = false;

        public SqLiteService()
        {
            Connection = null;
            DBName = "";
            isOpened = false;
        }

        public SqLiteService(string path)
        {
            Connection = null;
            DBName = path;
            isOpened = false;
        }

        public void OpenConnection(string query = "")
        {
            try
            {
                if (!Directory.Exists("Databases"))
                    Directory.CreateDirectory("Databases");

                string path = "./Databases/" + DBName + ".sqlite";
                if (!File.Exists(path))
                    SQLiteConnection.CreateFile(path);
                Connection = new SQLiteConnection("Data Source=" + path + ";Version=3;PRAGMA cache_size=20000; PRAGMA page_size=32768; PRAGMA synchronous=off");
                Connection.Open();
                isOpened = true;
                if (!string.IsNullOrEmpty(query))
                {
                    SQLiteCommand command = new SQLiteCommand(query, Connection);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void OpenConnection(List<string> transactions)
        {
            try
            {
                if (!Directory.Exists("Databases"))
                    Directory.CreateDirectory("Databases");

                string path = "./Databases/" + DBName + ".sqlite";
                if (!File.Exists(path))
                    SQLiteConnection.CreateFile(path);
                Connection = new SQLiteConnection("Data Source=" + path + ";Version=3;PRAGMA cache_size=20000; PRAGMA page_size=32768; PRAGMA synchronous=off");
                Connection.Open();
                isOpened = true;

                if (transactions != null && transactions.Count > 0)
                {
                    ExecuteTransaction(transactions);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }





        public void ExecuteQuery(string query)
        {
            if (!isOpened) return;
            try
            {
                SQLiteCommand command = new SQLiteCommand(query, Connection);
                command.ExecuteNonQuery();

            }
            catch
            {
                MessageBox.Show(query);
            }
        }

        public void ExecuteTransaction(List<string> transactions)
        {
            SQLiteTransaction dbTransaction = Connection.BeginTransaction();
            SQLiteCommand cmd = Connection.CreateCommand();
            cmd.Transaction = dbTransaction;
            transactions.ForEach(x =>
            {
                cmd.CommandText = x;
                cmd.ExecuteNonQuery();
            });
            dbTransaction.Commit();
        }

        public void CloseConnection()
        {
            if (!isOpened)
            {
                Connection.Close();
                isOpened = false;
            }
        }

        public static string getDateTimeFormat(string dateString)
        {
            //example data format :     29.10.2015 18:50:21
            string day = "";
            string month = "";
            string year = "";
            string hour = "";
            string pattern = "(..).(..).(....) (.*)";
            Regex regex = new Regex(pattern);
            Match m = regex.Match(dateString);
            if (m.Success)
            {
                if (m.Groups.Count >= 5)
                {
                    day = m.Groups[1].Value;
                    month = m.Groups[2].Value;
                    year = m.Groups[3].Value;
                    hour = m.Groups[4].Value;
                }
            }
            return year + "-" + month + "-" + day + " " + hour;

        }

        public static string StripSlashes(string InputTxt)
        {
            // List of characters handled:
            // \000 null
            // \010 backspace
            // \011 horizontal tab
            // \012 new line
            // \015 carriage return
            // \032 substitute
            // \042 double quote
            // \047 single quote
            // \134 backslash
            // \140 grave accent

            string Result = InputTxt;

            try
            {
                Result = System.Text.RegularExpressions.Regex.Replace(InputTxt, @"([\000\010\011\015\032\042\047\134\140])", " ");
            }
            catch (Exception Ex)
            {
                // handle any exception here
                Console.WriteLine(Ex.Message);
            }

            return Result;
        }
    }
}