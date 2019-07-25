﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Assets.Scripts.Core
{


    static class Database 
    {
       
        /***********************************************************************************************************************************************************/
        /******************************************************************                         ****************************************************************/
        /******************************************************************  CONNECTION PARAMETERS  ****************************************************************/
        /******************************************************************                         ****************************************************************/
        /***********************************************************************************************************************************************************/

        public static MySqlConnection con;

        public static void ConnectDB()
        {
            string constr = "Server='mysql-levelup.alwaysdata.net';DATABASE='levelup_newsvr';User ID='levelup';Password='LevelUp20!)';Pooling=true;Charset=utf8;";
            try
            {
                con = new MySqlConnection(constr);
                con.Open();

                
            }
            catch (IOException ex)
            {
                Debug.Log(ex.ToString());
            }
        }

        public static void DisconnectDB()
        {
            try
            {
                con.Close();
            }
            catch (IOException ex)
            {
                Debug.Log(ex.ToString());
            }
        }



        /*************************************************************************************************************************************************************************/
        /*************************************************************************************************************************************************************************/
        /*************************************************************************************************************************************************************************/
        /******************************************************************                                       ****************************************************************/
        /******************************************************************  ALL THE REQUEST WE NEED LISTED HERE  ****************************************************************/
        /******************************************************************                                       ****************************************************************/
        /*************************************************************************************************************************************************************************/
        /*************************************************************************************************************************************************************************/
        /*************************************************************************************************************************************************************************/
        /*  1-MainMenu.cs   2-Registration.cs   3-Login.cs   4-Profil.cs  5-Comments  6-News*/

        /*******************/
        /*******************/
        /* 1 - MainMenu.cs */
        /*******************/
        /*******************/

        //Take all the news from the db
        public static void GenerateNewsList()
        {
            ConnectDB();
            MySqlCommand cmdSQL = new MySqlCommand("SELECT DISTINCT NEWS.idNews, NEWS.title, NEWS.text, NEWS.positionX, NEWS.positionZ, NEWS.nbView, NEWS.nbComment, NEWS.creationDate FROM NEWS LEFT JOIN TOPICS ON NEWS.idNews = TOPICS.idNews ORDER BY TOPICS.tagName DESC;", con);
            MySqlDataReader reader = cmdSQL.ExecuteReader();

            List<string> tagsTemp;

            try
            {
                if (reader.HasRows)
                {
                    
                    while (reader.Read())
                    {
                        tagsTemp = new List<string>();
                        StaticClass.newsList.Add(new News(reader.GetUInt32(0), reader.GetString(1), reader.GetString(2), reader.GetFloat(3), reader.GetFloat(4), reader.GetUInt32(5), reader.GetUInt32(6), reader.GetDateTime(7), tagsTemp));
                    }
                    reader.Dispose();
                }
            
            }
            catch (IOException ex)
            {
                Debug.Log(ex.ToString());
            }
            reader.Dispose();
            cmdSQL.Dispose();

            try
            {
                foreach (News n in StaticClass.newsList)
                {
                    MySqlCommand cmdSQLtags = new MySqlCommand("SELECT * FROM TOPICS WHERE idNews = @dbIdNews ;", con);
                    cmdSQLtags.Parameters.AddWithValue("@dbIdNews", n.GetId());
                    MySqlDataReader readerTags = cmdSQLtags.ExecuteReader();

                    if (readerTags.HasRows)
                    {
                        while (readerTags.Read())
                        {
                            n.GetTags().Add(readerTags.GetString(1));
                        }
                    }
                    Debug.Log(n.GetTags().Count);
                    readerTags.Dispose();
                    cmdSQL.Dispose();
                }
            }
            catch (IOException ex)
            {
                Debug.Log(ex.ToString());
            }


           
            DisconnectDB();
        }


        public static int ReadNbrCommentDisplayed()
        {
            ConnectDB();
            string sqlCmdName = "SELECT cmtNbShown FROM PLAYERS WHERE name = @dbUserName;";
            MySqlCommand cmdSQL = new MySqlCommand(sqlCmdName, con);
            cmdSQL.Parameters.AddWithValue("@dbUserName", StaticClass.CurrentPlayerName);
            MySqlDataReader reader = cmdSQL.ExecuteReader();
            int response;

            try
            {
                if (reader.Read())
                {
                    int res = reader.GetInt32(0);
                    cmdSQL.Dispose();
                    con.Dispose();
                    response = res;
                }
                else
                {
                    response = 6;
                }
            }
            catch (IOException ex)
            {
                cmdSQL.Dispose();
                con.Dispose();
                Debug.Log(ex);
                response = 6;  ///default number shown
            }
            DisconnectDB();
            return response;
        }

        public static int ReadCommentPosition()
        {
            ConnectDB();
            string sqlCmdName = "SELECT cmtPositionPref FROM PLAYERS WHERE name = @dbUserName;";
            MySqlCommand cmdSQL = new MySqlCommand(sqlCmdName, con);
            cmdSQL.Parameters.AddWithValue("@dbUserName", StaticClass.CurrentPlayerName);
            MySqlDataReader reader = cmdSQL.ExecuteReader();
            int response;

            try
            {
                if (reader.Read())
                {
                    int res = reader.GetInt32(0);
                    cmdSQL.Dispose();
                    con.Dispose();
                    response = res;
                }
                else
                {
                    response = 0;
                }
            }
            catch (IOException ex)
            {
                cmdSQL.Dispose();
                con.Dispose();
                Debug.Log(ex);
                response = 0;  ///default position
            }
            DisconnectDB();
            return response;
        }

        /*********************/
        /*********************/
        /* 2-Registration.cs */
        /*********************/
        /*********************/
        public static bool VerifNameAvailable(string name)
        {
            ConnectDB();
            string sqlCmdName = "SELECT name FROM PLAYERS WHERE name = @dbUserName;";
            MySqlCommand cmdVerifName = new MySqlCommand(sqlCmdName, con);
            cmdVerifName.Parameters.AddWithValue("@dbUserName", name);
            bool response;

            try
            {
                MySqlDataReader reader = cmdVerifName.ExecuteReader();
                if (reader.Read())
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                    cmdVerifName.Dispose();
                    response = false;
                }
                else
                {
                    reader.Dispose();
                    cmdVerifName.Dispose();
                    response = true;
                }

            }
            catch (IOException ex)
            {
                Debug.Log(ex.ToString());
                cmdVerifName.Dispose();
                response = false;
            }
            DisconnectDB();
            return response;
        }

        public static bool InsertNewPlayer(string name,string password)
        {
            ConnectDB();
            MySqlCommand cmdReg = new MySqlCommand("INSERT INTO PLAYERS VALUES (default,@dbUserName,@dbUserMDP,0,0,0,0,default);", con);
            cmdReg.Parameters.AddWithValue("@dbUserName", name);
            cmdReg.Parameters.AddWithValue("@dbUserMDP", password);
            bool response;

            try
            {
                cmdReg.ExecuteReader();
                cmdReg.Dispose();
                response = true;
            }
            catch (IOException ex)
            {
                Debug.Log(ex.ToString());
                cmdReg.Dispose();
                response = false;
            }
            DisconnectDB();
            return response;
        }


        /******************/
        /******************/
        /** 3 - Login.cs **/
        /******************/
        /******************/
        public static bool IsThisUserAnAuthenticPlayer(string name, string password)
        {
            ConnectDB();
            MySqlCommand cmdSQL = new MySqlCommand("SELECT idPlayer FROM PLAYERS WHERE name = @dbUserName AND password = @dbUserMDP;", con);
            cmdSQL.Parameters.AddWithValue("@dbUserName", name);
            cmdSQL.Parameters.AddWithValue("@dbUserMDP", password);
            MySqlDataReader reader = cmdSQL.ExecuteReader();
            bool response;

            try
            {
                if (reader.Read())
                {
                    StaticClass.CurrentPlayerId = reader.GetUInt32(0);
                    cmdSQL.Dispose();
                    reader.Dispose();
                    response = true;

                }
                else
                {
                    cmdSQL.Dispose();
                    reader.Dispose();
                    response = false;
                }
                
            }
            catch (IOException ex)
            {
                Debug.Log(ex.ToString());
                cmdSQL.Dispose();
                reader.Dispose();
                response = false;
            }
            DisconnectDB();
            return response;
        }

        public static void GetTagColors()
        {
            ConnectDB();
            StaticClass.tagPrefColorList.Clear();
            MySqlCommand cmdSQL = new MySqlCommand("SELECT tagName, color FROM `NOTIFICATIONS` WHERE idPlayer= @dbUserId;", con);
            cmdSQL.Parameters.AddWithValue("@dbUserId", StaticClass.CurrentPlayerId);
            MySqlDataReader reader = cmdSQL.ExecuteReader();

            try
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (ColorUtility.TryParseHtmlString(reader.GetString(1), out Color color))
                        {
                            StaticClass.tagPrefColorList.Add(reader.GetString(0), color);
                        }
                        else
                        {
                            Debug.Log("Impossible to read hexadecimal color on database. Set to default (white).");
                        }
                    }
                    reader.Dispose();
                }
                else  // if there is nothing from this id player in the db yet, initialize every tag to white
                {
                    
                    foreach (string s in Database.GetTags())
                    {
                        StaticClass.tagPrefColorList.Add(s, StaticClass.tagDefaultColor);
                    }
                }
            }
            catch (IOException ex)
            {
                Debug.Log(ex.ToString());
                cmdSQL.Dispose();
            }
            DisconnectDB();
        }


        /******************/
        /******************/
        /* 4 - Profil.cs **/
        /******************/
        /******************/

        public static string SqlCmd(string selectName)
        {
            ConnectDB();
            MySqlCommand cmdSQL = new MySqlCommand("SELECT " + selectName + " FROM PLAYERS WHERE name = '" + StaticClass.CurrentPlayerName + "'", con);
            MySqlDataReader reader = cmdSQL.ExecuteReader();
            string response;
            try
            {
                reader.Read();
                string var = "" + reader.GetValue(0);
                cmdSQL.Dispose();
                response = var;
            }
            catch (IOException ex)
            {
                cmdSQL.Dispose();
                response = "" + ex;
            }
            DisconnectDB();
            return response;
        }


        public static bool InsertTagColorChoice(string tag, string player)
        {
            // Add comment to database
            bool res;
            ConnectDB();
            MySqlCommand cmdSQL = new MySqlCommand("INSERT INTO NOTIFICATIONS (tagName, idPlayer, color) VALUES(@dbTextTag,(SELECT idPlayer FROM PLAYERS WHERE name = @dbUserId), @dbHexColor);", con);
            cmdSQL.Parameters.AddWithValue("@dbTextTag", tag);
            cmdSQL.Parameters.AddWithValue("@dbHexColor", "#FFFFAF");
            cmdSQL.Parameters.AddWithValue("@dbUserId", player);        
            MySqlDataReader reader = cmdSQL.ExecuteReader();

            try
            {
                reader.Read();
                cmdSQL.Dispose();
                res = true;
            }
            catch (IOException ex)
            {
                cmdSQL.Dispose();
                Debug.Log(ex);
                res = false;
            }
            DisconnectDB();
            return res;
        }

        public static bool ChangeTagColorChoice(string tag, string hexColor)
        {
            bool res;
            ConnectDB();
            MySqlCommand cmdSQL = new MySqlCommand("UPDATE `NOTIFICATIONS` SET `color`= @dbHexColor WHERE `tagName`= @dbTextTag AND `idPlayer`= @dbPlayerId", con);
            cmdSQL.Parameters.AddWithValue("@dbTextTag", tag);
            cmdSQL.Parameters.AddWithValue("@dbHexColor", hexColor);
            cmdSQL.Parameters.AddWithValue("@dbPlayerId", StaticClass.CurrentPlayerId);
           
            try
            {
                if (cmdSQL.ExecuteNonQuery() > 0)
                {
                    cmdSQL.Dispose();
                    res = true;
                }
                else
                {
                    cmdSQL.Dispose();
                    res = false;
                }
            }
            catch (IOException ex)
            {
                cmdSQL.Dispose();
                Debug.Log(ex);
                res = false;
            }
            DisconnectDB();
            return res;
        }

        public static List<string> GetTags()
        {
            ConnectDB();
            MySqlCommand cmdSQL = new MySqlCommand("SELECT `tagLabel` FROM `TAGS`;", con);
            MySqlDataReader reader = cmdSQL.ExecuteReader();
            List<string> tagsList = new List<string>();

            try
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tagsList.Add(reader.GetString(0));
                    }
                    reader.Dispose();
                }
                
            }
            catch (IOException ex)
            {
                Debug.Log(ex.ToString());
                cmdSQL.Dispose();
            }
            DisconnectDB();
            return tagsList;
        }



        public static bool PrefSucessfullySaved(int cmtNumbers, int cmtPosition)
        {
            ConnectDB();
            MySqlCommand cmdSQL = new MySqlCommand("UPDATE PLAYERS SET cmtNbShown = '" + cmtNumbers + "', cmtPositionPref = '" + cmtPosition + "' WHERE name = '" + StaticClass.CurrentPlayerName + "'; ", con);
            bool response;

            try
            {
                if (cmdSQL.ExecuteNonQuery() > 0)
                {
                    cmdSQL.Dispose();
                    response = true;
                }
                else
                {
                    cmdSQL.Dispose();
                    response = false;
                }
            }
            catch (IOException ex)
            {              
                cmdSQL.Dispose();
                Debug.Log(ex);
                response = false;
            }
            DisconnectDB();
            return response;
        }


        /******************/
        /******************/
        /* 5 - Comments **/
        /******************/
        /******************/
        internal static string ReadComntNum(uint idNews)
        {
            ConnectDB();
            MySqlCommand cmdSQL = new MySqlCommand("SELECT nbComment FROM NEWS WHERE idNews = @dbNewsId", con);
            cmdSQL.Parameters.AddWithValue("@dbNewsId", idNews);
            MySqlDataReader reader = cmdSQL.ExecuteReader();
            string response;

            try
            {
                if (reader.Read())
                {
                    int res = reader.GetInt32(0);
                    cmdSQL.Dispose();
                    con.Dispose();
                    response = "" + res;
                }
                else
                {
                    response = "/";
                }
            }
            catch (IOException ex)
            {
                cmdSQL.Dispose();
                con.Dispose();
                Debug.Log(ex);
                response = "/";
            }
            DisconnectDB();
            return response;
        }

        public static void AddComment(uint idNews, string text)
        {
            // Add comment to database
            ConnectDB();
            MySqlCommand cmdSQL = new MySqlCommand("INSERT INTO COMMENTS (idNews, idPlayer, text, date) VALUES(@dbNewsId,@dbUserId,@dbComtText,@dbDate); ", con);
            cmdSQL.Parameters.AddWithValue("@dbComtText", text);
            cmdSQL.Parameters.AddWithValue("@dbNewsId", idNews);
            cmdSQL.Parameters.AddWithValue("@dbUserId", StaticClass.CurrentPlayerId);
            cmdSQL.Parameters.AddWithValue("@dbDate", DateTime.Now);
            MySqlDataReader reader = cmdSQL.ExecuteReader();

            try
            {
                reader.Read();               
                cmdSQL.Dispose();
                Add1CommentToPlayer();
                Add1CommentToNews();
            }
            catch (IOException ex)
            {
                cmdSQL.Dispose();
                Debug.Log(ex);
            }      
            DisconnectDB();
        }

        public static void Add1CommentToPlayer()
        {
            ConnectDB();
            MySqlCommand cmdSQL = new MySqlCommand("UPDATE PLAYER SET nbOfComment = nbOfComment + 1 WHERE name = @dbUserId", con);
            cmdSQL.Parameters.AddWithValue("@dbUserId", StaticClass.CurrentPlayerId);

            try
            {
                cmdSQL.ExecuteNonQuery();
                cmdSQL.Dispose();
            }
            catch (IOException ex)
            {
                cmdSQL.Dispose();
                Debug.Log(ex);
            }            
            DisconnectDB();
        }


        public static void Add1CommentToNews()
        {
            ConnectDB();
            MySqlCommand cmdSQL = new MySqlCommand("UPDATE NEWS SET nbComment = nbComment + 1 WHERE  idNews = @dbUserId", con);
            cmdSQL.Parameters.AddWithValue("@dbNEwsId", StaticClass.CurrentNewsId);

            try
            {
                cmdSQL.ExecuteNonQuery();
                cmdSQL.Dispose();
            }
            catch (IOException ex)
            {
                cmdSQL.Dispose();
                Debug.Log(ex);
            }
            DisconnectDB();
        }


        static public void DeleteComment(uint id)
        {
            ConnectDB();
            MySqlCommand cmdDeleteAction = new MySqlCommand("DELETE FROM COMMENTS WHERE ID = @dbIdComment;", Database.con);
            cmdDeleteAction.Parameters.AddWithValue("@dbIdComment", id);

            try
            {
                cmdDeleteAction.ExecuteReader();
                Remove1CommentToPlayer();
                Remove1CommentToNews();
            }
            catch (IOException ex)
            {
                Debug.Log(ex.ToString());
            }

            cmdDeleteAction.Dispose();
            con.Dispose();
            DisconnectDB();
        }

        public static void Remove1CommentToPlayer()
        {
            ConnectDB();
            MySqlCommand cmdSQL = new MySqlCommand("UPDATE PLAYER SET nbOfComment = nbOfComment - 1 WHERE name = @dbUserId", con);
            cmdSQL.Parameters.AddWithValue("@dbUserId", StaticClass.CurrentPlayerId);

            try
            {
                cmdSQL.ExecuteNonQuery();
                cmdSQL.Dispose();
            }
            catch (IOException ex)
            {
                cmdSQL.Dispose();
                Debug.Log(ex);
            }
            DisconnectDB();
        }


        public static void Remove1CommentToNews()
        {
            ConnectDB();
            MySqlCommand cmdSQL = new MySqlCommand("UPDATE NEWS SET nbComment = nbComment - 1 WHERE  idNews = @dbUserId", con);
            cmdSQL.Parameters.AddWithValue("@dbNEwsId", StaticClass.CurrentNewsId);

            try
            {
                cmdSQL.ExecuteNonQuery();
                cmdSQL.Dispose();
            }
            catch (IOException ex)
            {
                cmdSQL.Dispose();
                Debug.Log(ex);
            }
            DisconnectDB();
        }


        static public List<Comment> QueryComments(uint idNews)
        {
            ConnectDB();
            List<Comment> cmntList = new List<Comment>();

            MySqlCommand cmdSQL = new MySqlCommand("SELECT idComment,date,text,PLAYERS.name FROM `COMMENTS` INNER JOIN PLAYERS ON COMMENTS.idPlayer = PLAYERS.idPlayer WHERE IdNews = " + idNews + " ORDER BY date DESC;", con);
            cmdSQL.Parameters.AddWithValue("@dbNewsId", idNews);
            MySqlDataReader reader = cmdSQL.ExecuteReader();


            try
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        cmntList.Add(new Comment(reader.GetUInt32(0), reader.GetDateTime(1), reader.GetString(2), reader.GetString(3)));
                    }
                    reader.Dispose();
                }

            }
            catch (IOException ex)
            {
                Debug.Log(ex.ToString());
                cmdSQL.Dispose();
                reader.Dispose();
            }
            DisconnectDB();
            return cmntList;
        }


        static public Comment GetLastComment()
        {
            ConnectDB();
            Comment cmt = new Comment();
            MySqlCommand cmdLastComment = new MySqlCommand("SELECT idComment,date,text FROM COMMENTS WHERE idPlayer = @dbIdPlayer AND idNews = @dbIdNews ORDER BY idComment DESC LIMIT 1;", Database.con);
            cmdLastComment.Parameters.AddWithValue("@dbIdPlayer", StaticClass.CurrentPlayerId);
            cmdLastComment.Parameters.AddWithValue("@dbIdNews", StaticClass.CurrentNewsId);
            MySqlDataReader reader = cmdLastComment.ExecuteReader();

            try
            {
                if (reader.Read())
                {               
                    cmt = new Comment(reader.GetUInt32(0), reader.GetDateTime(1), reader.GetString(2), StaticClass.CurrentPlayerName);                                       
                }

            }
            catch (IOException ex)
            {
                Debug.Log(ex.ToString());
            }
            reader.Dispose();
            cmdLastComment.Dispose();
            con.Dispose();
            DisconnectDB();
            return cmt;
        }


        /******************/
        /******************/
        /**** 6 - NEWS ****/
        /******************/
        /******************/

        public static void CreateANews(string title, string text, float posX, float posZ)
        {
            Database.ConnectDB();
            MySqlCommand cmdCreateNews = new MySqlCommand("INSERT INTO NEWS(title, text, author, creationDate, nbView, nbComment, nbHappy, nbSad, nbAngry, nbSurprised, positionX, positionZ, laserTarget) VALUES(@dbNewsCreaTitle,@dbNewsCreaText,@dbNewsCreaAuthor, @dbNewsCreaDate,0,0,0,0,0,0,@dbNewsCreaPosiX, @dbNewsCreaPosiZ,'');", Database.con);
            cmdCreateNews.Parameters.AddWithValue("@dbNewsCreaTitle", title);
            cmdCreateNews.Parameters.AddWithValue("@dbNewsCreaText", text);
            cmdCreateNews.Parameters.AddWithValue("@dbNewsCreaAuthor", StaticClass.CurrentPlayerName);
            cmdCreateNews.Parameters.AddWithValue("@dbNewsCreaDate", DateTime.Now);
            cmdCreateNews.Parameters.AddWithValue("@dbNewsCreaPosiX", posX);
            cmdCreateNews.Parameters.AddWithValue("@dbNewsCreaPosiZ", posZ);

            try
            {
                cmdCreateNews.ExecuteReader();
            }
            catch (IOException ex)
            {
                Debug.Log(ex.ToString());
            }

            cmdCreateNews.Dispose();
            cmdCreateNews = null;
            DisconnectDB();


        }

        public static void Add1ViewToNews(uint idNews)
        {
            ConnectDB();
            MySqlCommand cmdSQL = new MySqlCommand("UPDATE NEWS SET nbView = nbView + 1 WHERE idNews = @dbNewsId", con);
            cmdSQL.Parameters.AddWithValue("@dbNewsId", idNews);

            try
            {
                cmdSQL.ExecuteNonQuery();
                cmdSQL.Dispose();
            }
            catch (IOException ex)
            {
                cmdSQL.Dispose();
                Debug.Log(ex);
            }
            DisconnectDB();
        }

        public static void Add1ViewToPlayer()
        {
            ConnectDB();
            MySqlCommand cmdSQL = new MySqlCommand("UPDATE PLAYERS SET nbOfView = nbOfView + 1 WHERE idPlayer = @dbUserId", con);
            cmdSQL.Parameters.AddWithValue("@dbUserId", StaticClass.CurrentPlayerId);

            try
            {
                cmdSQL.ExecuteNonQuery();
                cmdSQL.Dispose();
            }
            catch (IOException ex)
            {
                cmdSQL.Dispose();
                Debug.Log(ex);
            }
            DisconnectDB();
        }

        public static string NumOfReatcionToNews(string rea, uint idNews)
        {
            ConnectDB();
            MySqlCommand cmdSQL = new MySqlCommand("SELECT @reaToSelect FROM NEWS WHERE idNews = @dbNewsId", con);
            cmdSQL.Parameters.AddWithValue("@dbNewsId", idNews);
            cmdSQL.Parameters.AddWithValue("@reaToSelect", "nb" + rea);
            MySqlDataReader reader = cmdSQL.ExecuteReader();
            string response;

            try
            {
                if (reader.Read())
                {
                    int res = reader.GetInt32(0);
                    cmdSQL.Dispose();
                    con.Dispose();
                    response = "" + res;
                }
                else
                {
                    response = "/";
                }

            }
            catch (IOException ex)
            {
                cmdSQL.Dispose();
                con.Dispose();
                Debug.Log(ex);
                response = "/";
            }
            DisconnectDB();
            return response;
        }

        internal static string ReadViewNum(uint id)
        {
            ConnectDB();
            MySqlCommand cmdSQL = new MySqlCommand("SELECT nbView FROM NEWS WHERE idNews = @dbNewsId", con);
            cmdSQL.Parameters.AddWithValue("@dbNewsId", id);
            MySqlDataReader reader = cmdSQL.ExecuteReader();
            string response;

            try
            {
                if (reader.Read())
                {
                    int res = reader.GetInt32(0);
                    cmdSQL.Dispose();
                    con.Dispose();
                    response = "" + res;
                }
                else
                {
                    response = "/";
                }
            }
            catch (IOException ex)
            {
                cmdSQL.Dispose();
                con.Dispose();
                Debug.Log(ex);
                response = "/";
            }
            DisconnectDB();
            return response;
        }

        /***********************/
        /***********************/
        /******  Reaction ******/
        /***********************/
        /***********************/

        public static void AddReactionToDatabaseNews(string reactionType, uint idNews)
        {
            ConnectDB();
            MySqlCommand cmdSQL = new MySqlCommand("UPDATE NEWS SET @reaToSelect = @reaToSelect + 1 WHERE idNews = @dbNewsId", con);
            cmdSQL.Parameters.AddWithValue("@dbNewsId", idNews);
            cmdSQL.Parameters.AddWithValue("@reaToSelect", "nb" + reactionType);
           

            try
            {
                cmdSQL.ExecuteNonQuery();
              
                
            }
            catch (IOException ex)
            {
              
                Debug.Log(ex);
            }
            cmdSQL.Dispose();
            con.Dispose();
            DisconnectDB();
        }

        
    }


}
