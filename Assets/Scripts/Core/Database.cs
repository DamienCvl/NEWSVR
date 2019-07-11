using MySql.Data.MySqlClient;
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

        



        /*************************************************************************************************************************************************************************/
        /*************************************************************************************************************************************************************************/
        /*************************************************************************************************************************************************************************/
        /******************************************************************                                       ****************************************************************/
        /******************************************************************  ALL THE REQUEST WE NEED LISTED HERE  ****************************************************************/
        /******************************************************************                                       ****************************************************************/
        /*************************************************************************************************************************************************************************/
        /*************************************************************************************************************************************************************************/
        /*************************************************************************************************************************************************************************/
        /*  1 - MainMenu.cs  2 - Registration.cs   3 - Login.cs   4 - Profil.cs 5 - Comments 6-News*/

        /*******************/
        /*******************/
        /* 1 - MainMenu.cs */
        /*******************/
        /*******************/

        //Take all the news from the db
        public static void GenerateNewsList()
        {
            ConnectDB();
            MySqlCommand cmdSQL = new MySqlCommand("SELECT NEWS.idNews, NEWS.title, NEWS.text, NEWS.positionX, NEWS.positionZ, NEWS.nbView, NEWS.creationDate FROM NEWS;", con);
            MySqlDataReader reader = cmdSQL.ExecuteReader();

            List<string> tagsTemp = new List<string>();

            try
            {
                if (reader.HasRows)
                {
                    
                    while (reader.Read())
                    {                               
                        StaticClass.newsList.Add(new News(reader.GetUInt32(0), reader.GetString(1), reader.GetString(2), reader.GetFloat(3), reader.GetFloat(4), reader.GetUInt32(5), reader.GetDateTime(6), tagsTemp));
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


            uint idNewsTemp = 1;
            MySqlCommand cmdSQLtags = new MySqlCommand("SELECT * FROM TOPICS WHERE idNews = @dbIdNews ;", con);
            cmdSQLtags.Parameters.AddWithValue("@dbIdNews", idNewsTemp);
            MySqlDataReader readerTags = cmdSQLtags.ExecuteReader();

            try
            {
                foreach (News n in StaticClass.newsList)
                {
                    if (readerTags.HasRows)
                    {
                        while (readerTags.Read())
                        {
                            n.GetTags().Add(readerTags.GetString(1));
                        }
                    }

                    idNewsTemp++;
                }
            }
            catch (IOException ex)
            {
                Debug.Log(ex.ToString());
            }

            readerTags.Dispose();
            cmdSQL.Dispose();
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

            try
            {
                MySqlDataReader reader = cmdVerifName.ExecuteReader();
                if (reader.Read())
                {
                    reader.Close();
                    reader = null;
                    reader.Dispose();
                    cmdVerifName.Dispose();
                    return false;
                }
                else
                {
                    reader.Dispose();
                    cmdVerifName.Dispose();
                    return true;
                }

            }
            catch (IOException ex)
            {
                Debug.Log(ex.ToString());
                cmdVerifName.Dispose();
                return false;
            }
        }

        public static bool InsertNewPlayer(string name,string password)
        {
            ConnectDB();
            MySqlCommand cmdReg = new MySqlCommand("INSERT INTO PLAYERS VALUES (default,@dbUserName,@dbUserMDP,0,0,0,0,default);", con);
            cmdReg.Parameters.AddWithValue("@dbUserName", name);
            cmdReg.Parameters.AddWithValue("@dbUserMDP", password);

            try
            {
                cmdReg.ExecuteReader();
                cmdReg.Dispose();
                return true;
            }
            catch (IOException ex)
            {
                Debug.Log(ex.ToString());
                cmdReg.Dispose();
                return false;
            }
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

            try
            {
                if (reader.Read())
                {
                    StaticClass.CurrentPlayerId = reader.GetUInt32(0);
                    cmdSQL.Dispose();
                    reader.Dispose();
                    return true;

                }
                else
                {
                    cmdSQL.Dispose();
                    reader.Dispose();
                    return false;
                }
                
            }
            catch (IOException ex)
            {
                Debug.Log(ex.ToString());
                cmdSQL.Dispose();
                reader.Dispose();
                return false;
            }
        }

        public static void GetTagColors()
        {
            ConnectDB();
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
                            StaticClass.tagPrefColorList.Add(reader.GetString(0), Color.white);
                            Debug.Log("Impossible to read hexadecimal color on database. Set to default (white).");
                        }
                    }
                    reader.Dispose();
                }
            }
            catch (IOException ex)
            {
                Debug.Log(ex.ToString());
                cmdSQL.Dispose();
            }
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

            try
            {
                reader.Read();
                string var = "" + reader.GetValue(0);
                cmdSQL.Dispose();
                return var;
            }
            catch (IOException ex)
            {
                cmdSQL.Dispose();
                return "" + ex;
            }
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
            return tagsList;
        }



        public static bool PrefSucessfullySaved(int cmtNumbers, int cmtPosition)
        {
            ConnectDB();
            MySqlCommand cmdSQL = new MySqlCommand("UPDATE PLAYERS SET cmtNbShown = '" + cmtNumbers + "', cmtPositionPref = '" + cmtPosition + "' WHERE name = '" + StaticClass.CurrentPlayerName + "'; ", con);

            try
            {
                if (cmdSQL.ExecuteNonQuery() > 0)
                {
                    cmdSQL.Dispose();
                    return true;
                }
                else
                {
                    cmdSQL.Dispose();
                    return false;
                }
            }
            catch (IOException ex)
            {              
                cmdSQL.Dispose();
                Debug.Log(ex);
                return false;
            }
        }


        /******************/
        /******************/
        /* 5 - Comments **/
        /******************/
        /******************/
        internal static string ReadComntNum(uint id)
        {
            ConnectDB();
            MySqlCommand cmdSQL = new MySqlCommand("SELECT nbComment FROM NEWS WHERE idNews = @dbNewsId", con);
            cmdSQL.Parameters.AddWithValue("@dbNewsId", id);
            MySqlDataReader reader = cmdSQL.ExecuteReader();

            try
            {
                int res = reader.GetInt32(0);
                cmdSQL.Dispose();
                con.Dispose();
                return "" + res;

            }
            catch (IOException ex)
            {
                cmdSQL.Dispose();
                con.Dispose();
                Debug.Log(ex);
                return "/";
            }
        }

        public static void AddComment(int idNews, string text)
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
            }
            catch (IOException ex)
            {
                cmdSQL.Dispose();
                Debug.Log(ex);
            }      
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
        }


        static public List<Comment> QueryComments(uint idNews)
        {
            ConnectDB();
            List<Comment> cmntList = new List<Comment>();

            MySqlCommand cmdSQL = new MySqlCommand("SELECT idComment,date,text,idPlayer FROM `COMMENTS` WHERE IdNews = @dbNewsId; ", con);
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
            return cmntList;
        }

        public static void Add1ComntToPlayer()
        {
            ConnectDB();
            MySqlCommand cmdSQL = new MySqlCommand("UPDATE PLAYERS SET nbOfComment = nbOfComment + 1 WHERE  name = @dbUserId", con);
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
            Database.con.Close();


        }

        public static void Add1ViewToNews(uint idNews)
        {
            ConnectDB();
            MySqlCommand cmdSQL = new MySqlCommand("UPDATE NEWS SET nbViews = nbViews + 1 WHERE idNews = @dbNewsId", con);
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
        }

        public static void Add1ViewToPlayer()
        {
            ConnectDB();
            MySqlCommand cmdSQL = new MySqlCommand("UPDATE PLAYERS SET nbOfView = nbOfView + 1 WHERE  name = @dbUserId", con);
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
        }

        public static string NumOfReatcionToNews(string rea, uint idNews)
        {
            ConnectDB();
            MySqlCommand cmdSQL = new MySqlCommand("SELECT @reaToSelect FROM NEWS WHERE idNews = @dbNewsId", con);
            cmdSQL.Parameters.AddWithValue("@dbNewsId", idNews);
            cmdSQL.Parameters.AddWithValue("@reaToSelect", "nb" + rea);
            MySqlDataReader reader = cmdSQL.ExecuteReader();

            try
            {
                int res = reader.GetInt32(0);
                cmdSQL.Dispose();
                con.Dispose();
                return ""+res;
                
            }
            catch (IOException ex)
            {
                cmdSQL.Dispose();
                con.Dispose();
                Debug.Log(ex);
                return "/";
            }
        }

        internal static string ReadViewNum(uint id)
        {
            ConnectDB();
            MySqlCommand cmdSQL = new MySqlCommand("SELECT nbView FROM NEWS WHERE idNews = @dbNewsId", con);
            cmdSQL.Parameters.AddWithValue("@dbNewsId", id);
            MySqlDataReader reader = cmdSQL.ExecuteReader();

            try
            {
                int res = reader.GetInt32(0);
                cmdSQL.Dispose();
                con.Dispose();
                return "" + res;

            }
            catch (IOException ex)
            {
                cmdSQL.Dispose();
                con.Dispose();
                Debug.Log(ex);
                return "/";
            }
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
        }

        
    }


}
