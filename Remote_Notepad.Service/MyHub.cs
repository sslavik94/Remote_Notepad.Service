using Microsoft.AspNet.SignalR;
using Remote_Notepad.Service.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remote_Notepad.Service
{
    public class MyHub : Hub
    {
        public static DataTable messagesDataTable;
        public static DataTable userDataTable;

        string currentUserLogin;
        string currentUserPassword;
        string currentUserId;

        public static OleDbConnection dbConn;
        UserInfo member = new UserInfo();

        public void UserConnected(string username)
        {
            Console.WriteLine("User <" + username + "> connected");
        }


        public void UserDisconnected(string username)
        {
            //Console.WriteLine("User <" + username + "> disconnected");
            dbConn.Close();
        }

        public void MessageAdd(string messageName, string messageContent)
        {
            OleDbCommand dbCommand = dbConn.CreateCommand();
            dbCommand.CommandText = "insert into message_info (message_name, message_content, message_owner_id) VALUES (" + "'" + messageName + "'" + ", " + "'" + messageContent + "'" + ", " + currentUserId + ")";
            dbCommand.ExecuteNonQuery();
            Console.WriteLine("Message <" + messageName + "> added");
        }

        public void MessageDelete(string messageName, string messageContent)
        {
            OleDbCommand dbCommand = dbConn.CreateCommand();
            dbCommand.CommandText = "delete from message_info where message_name = '" + messageName + "' and message_content = '" + messageContent + "'";
            dbCommand.ExecuteNonQuery();
            Console.WriteLine("Message <" + messageName + "> deleted");
        }

        public void MessageUpdate(string oldMessageName, string oldMessageContent, string newMessageName, string newMessageContent)
        {
            OleDbCommand dbCommand = dbConn.CreateCommand();
            dbCommand.CommandText = "update message_info set message_name = '" + newMessageName + "', message_content = '" + newMessageContent + "' where message_name = '" + oldMessageName + "'";
            dbCommand.ExecuteNonQuery();
            Console.WriteLine("Message <" + oldMessageName + "> updated");
        }

        public void Send(string login)
        {
            string connString = "Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=Remote_Notepad.DataBase;Data Source=DEV-PC";
            dbConn = new OleDbConnection(connString);
            dbConn.Open();

            GetUserFromDB(login);

            member.Login = currentUserLogin;
            member.Password = currentUserPassword;

            member.MessageCollection = new ObservableCollection<MessageInfo>();
            GetMessagesFromDB();
            member.Profile = member.MessageCollection.Count;

            Clients.Caller.GetMember(member);
            //Console.WriteLine(currentUserId);
            //Console.WriteLine(currentUserLogin);
            //Console.WriteLine(currentUserPassword);
        }

        public void GetMessagesFromDB()

        {
            messagesDataTable = new DataTable();
            OleDbDataAdapter dbAdapter = new OleDbDataAdapter("select message_name, message_content from message_info where message_owner_id = '" + currentUserId + "'", dbConn);
            dbAdapter.Fill(messagesDataTable);

            foreach (DataRow row in messagesDataTable.Rows)
            {
                member.MessageCollection.Add(new MessageInfo(Convert.ToString(row[0]), Convert.ToString(row[1])));
            }
        }

        public void GetUserFromDB(string login)

        {
            userDataTable = new DataTable();
            OleDbDataAdapter dbAdapter = new OleDbDataAdapter("select user_id, user_login, user_password from user_info where user_login = '" + login + "'", dbConn);
            dbAdapter.Fill(userDataTable);

            currentUserId = userDataTable.Rows[0][0].ToString();
            currentUserLogin = userDataTable.Rows[0][1].ToString();
            currentUserPassword = userDataTable.Rows[0][2].ToString();
        }

        public override Task OnConnected()
        {
            Console.WriteLine("Client connected: " + Context.ConnectionId);
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool isDisconnected)
        {
            Console.WriteLine("Client disconnected: " + Context.ConnectionId);
            return base.OnDisconnected(true);
        }
    }
}
