using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data.SqlClient;
//using MySql.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Text text;
    private int value = 0;

    void Start()
    {
        Debug.Log("**start");
        
    }

    void Update()
    {

    }

    public void SetText()
    {
        try
        {
            value = System.Convert.ToInt32(text.text);
        }
        catch (System.Exception ex) { value = 0; }

        value += 1;
        text.text = value.ToString();

        return;
    }

    public void SetTextSQL()
    {
        string queryString = "SELECT * FROM sys_config";
        string connectionString = @"Server=localhost,3306;Database=sys;UID=root;Pwd=password;";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(queryString, connection);
            //command.Parameters.AddWithValue("@val", "OFF");
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    System.Console.WriteLine(System.String.Format("{0}, {1}",
                    reader["variable"], reader["value"]));// etc

                    text.text = reader["variable"].ToString();
                }
            }
            finally
            {
                // Always call Close when done reading.
                reader.Close();
            }
        }
    }



}
