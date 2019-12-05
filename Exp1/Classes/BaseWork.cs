// Здесь работаем с базой данных

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity;

namespace Exp1
{
    class BaseWork
    {
        // Строка подключения.
        private string connect = @"Data Source=localhost;Initial Catalog=BxgBase;User ID=sa;Password=P@ssw0rd;";
        private SqlConnection connection = new SqlConnection();        

        // Вывод таблицы.
        public void PrintSqlData(string tableName)
        {
            string connectionString = this.connect;
            
            // Чтение из базы и вывод в консоль.
            string sqlExpression1 = String.Format("SELECT * FROM {0}",tableName);
            using (connection)
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression1, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows) // если есть данные
                {
                    // Выводим количество столбцов.                  
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write(reader.GetName(i) + "\t");
                    }
                    Console.WriteLine();

                    // Построчно считываем данные.
                    while (reader.Read()) 
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write(reader.GetValue(i) + "\t");
                        }
                        Console.WriteLine();
                    }                
                }
            }
        }
        // Добавление объектов в таблицу. 
        public void AddSqlData(string tableName, string column, string values)
        {
            string connectionString = this.connect;
            string sqlExpression = String.Format("INSERT dbo.{0} ({1}) VALUES({2})", tableName, column, values);
            using (connection)
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                //Console.WriteLine("State: {0}", connection.State);
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandText = sqlExpression;
                var number = command.ExecuteNonQuery();
                //Console.WriteLine("Добавлено объектов: {0}", number);
                connection.Close();
                connection.Dispose();
            }

        }
        // Обновление ранее добавленного объекта.
        public void UpdSqlData(string tableName, string set, string where)
        {
            string connectionString = this.connect;
            string sqlExpression = String.Format("UPDATE dbo.{0} SET {1} WHERE {2}", tableName, set, where);
            using (connection)
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandText = sqlExpression;
                int number = command.ExecuteNonQuery();
                Console.WriteLine("Обновлено объектов: {0}", number);
                connection.Close();
                connection.Dispose();
            }
        }
        // Удаление данных из таблицы.
        public void DeleteSqlData(string tableName)
        {
            string connectionString = this.connect;
            string sqlExpression = String.Format("DELETE FROM {0};", tableName);
            using (connection)
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandText = sqlExpression;
                var number = command.ExecuteNonQuery();
                Console.WriteLine("Удалено объектов: {0}", number);
            }
        }
        

        public void GetSqlData(string tableName, string rowname, out ArrayList arr)
        {
            ArrayList array = new ArrayList();
            string connectionString = this.connect;

            // Чтение из базы и вывод в консоль.
            string sqlExpression1 = String.Format("SELECT {1} FROM {0}", tableName, rowname);
            using (connection)
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression1, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows) // если есть данные
                {
                    // Выводим количество столбцов.                  
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write(reader.GetName(i) + "\t");
                    }
                    Console.WriteLine();

                    // Построчно считываем данные.
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            array.Add(reader.GetValue(i));
                            //Console.Write(reader.GetValue(i) + "\t");

                        }
                        //Console.WriteLine();
                    }
                }
            }

            arr = array;

        }


    }
}


// ЗАДАЧИ 
// TODO: 1. Написать метод создать таблицу 
// TODO: 2. Написать метод Удалить таблицу
// TODO: 3. Метод очистки индекса








/*
//-------------------- Разные варианты. --------------------
public void DeleteSqlIndex(string tableName)
{
string connectionString = this.connect;
//string sqlExpression = String.Format("DROP INDEX {0};", tableName);
string sqlExpression = String.Format("ALTER TABLE {0}.Id DROP CONSTRAINT Id;", tableName);

using (connection)
{
    connection.ConnectionString = connectionString;
    connection.Open();
    SqlCommand command = new SqlCommand(sqlExpression, connection);
    command.CommandText = sqlExpression;
    var number = command.ExecuteNonQuery();
    Console.WriteLine("Удалено объектов: {0}", number);
}

}
public void PrintSqlKey(string tableName)
{
string connectionString = this.connect;
// Чтение из базы и вывод в консоль
string sqlExpression1 = String.Format("SELECT name FROM sys.key_constraints WHERE type = 'PK' AND OBJECT_NAME(parent_object_id) = N'{0}';", tableName);
using (connection)
{
    connection.ConnectionString = connectionString;
    connection.Open();
    SqlCommand command = new SqlCommand(sqlExpression1, connection);
    SqlDataReader reader = command.ExecuteReader();
    if (reader.HasRows) // если есть данные
    {
        // выводим названия столбцов
        Console.WriteLine("{0}\t{1}\t{2}\t{3}", reader.GetName(0), reader.GetName(1), reader.GetName(2), reader.GetName(3));
        while (reader.Read()) // построчно считываем данные
        {
            object id = reader.GetValue(0);
            object X = reader.GetValue(1);
            object Y = reader.GetValue(2);
            object Z = reader.GetValue(3);
            Console.WriteLine("{0}\t{1}\t{2}\t{3}", id, X, Y, Z);
        }
    }
}
}


*/


//private string connect = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True;";
//private string connect = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=F:\YANDEXDISK\YANDEXDISK\ВОЛГГТУ\BOXGURU\EXP10\EXP1\BIN\DEBUG\DATABASE1.MDF;Integrated Security=True;";



/*
// Открыть подключение.
public void OpenSqlConnection()
{
    string connectionString = this.connect;
    using (connection)
    {
        connection.ConnectionString = connectionString;
        connection.Open();
        Console.WriteLine("State: {0}", connection.State);
        Console.WriteLine("ConnectionString: {0}", connection.ConnectionString);
    }
}

// Закрыть подключение.
public void CloseSqlConnection()
{
    string connectionString = this.connect;
    using (connection)
    {
        connection.ConnectionString = connectionString;
        Console.WriteLine("ConnectionString: {0}", connection.ConnectionString);              
        if (connection.State != 0)
        {                    
            connection.Close();
            connection.Dispose();
        }
        Console.WriteLine("State: {0}", connection.State);
    }
}

 
     
                         //Console.WriteLine("{0}\t{1}\t{2}\t{3}", reader.GetName(0), reader.GetName(1), reader.GetName(2), reader.GetName(3));
                    //while (reader.Read()) // построчно считываем данные
                    //{
                    //    object id = reader.GetValue(0);
                    //    object X = reader.GetValue(1);
                    //    object Y = reader.GetValue(2);
                    //    object Z = reader.GetValue(3);
                    //    Console.WriteLine("{0}\t{1}\t{2}\t{3}", id, X, Y, Z);
                    //}
     
     
     
     
     */
