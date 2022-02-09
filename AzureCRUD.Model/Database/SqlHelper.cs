using AzureCRUD.Model.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AzureCRUD.Model.Database
{
    public class SqlHelper
    {
        private readonly SqlConnection _sqlConnection;
        public SqlHelper( string connectionString )
        {
            _sqlConnection = new SqlConnection( connectionString );
            _sqlConnection.Open();
        }

        ~SqlHelper()
        {
            _sqlConnection.Close();
        }

        public async Task<List<ToDoModel>> Get()
        {
            try
            {

                var list = new List<ToDoModel>();

                SqlCommand command = new("SELECT * FROM Todo", _sqlConnection);

                SqlDataReader reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    ToDoModel model = new()
                    {
                        Id = (int)reader["Id"],
                        Task = reader["Task"].ToString(),
                        Done = (bool)reader["Done"],
                        CreatedAt = reader["CreatedAt"].ToString()
                    };
                    list.Add(model);
                }
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ToDoModel> GetById (int id)
        {
            try
            {

                SqlCommand command = new($"SELECT * FROM Todo WHERE Id = {id}", _sqlConnection);

                SqlDataReader reader = await command.ExecuteReaderAsync();

                var result = new ToDoModel();

                while (reader.Read())
                {
                    result = new ToDoModel
                    {
                        Id = (int)reader["Id"],
                        Task = reader["Task"].ToString(),
                        Done = (bool)reader["Done"],
                        CreatedAt = reader["CreatedAt"].ToString()
                    };
                }
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> Create( Stream body )
        {
            try
            {
                string requestBody = await new StreamReader(body).ReadToEndAsync();

                var todoModel = JsonConvert.DeserializeObject<ToDoModel>(requestBody);

                var insertQuery = $"INSERT INTO Todo (Task, Done, CreatedAt)" +
                                  $"values ('{todoModel.Task}', '{false}', '{DateTime.Now.ToShortDateString()}');";

                SqlCommand command = new(insertQuery, _sqlConnection);
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<ToDoModel> Update(int id, Stream body )
        {
            try
            {
                string requestBody = await new StreamReader(body).ReadToEndAsync();

                var todoModel = JsonConvert.DeserializeObject<ToDoModel>(requestBody);

                string updateQuery = $"UPDATE Todo SET " +
                                     $"Task = '{todoModel.Task}', " +
                                     $"Done = '{todoModel.Done} '" +
                                     $"WHERE Id = {id};";

                SqlCommand command = new(updateQuery, _sqlConnection);
                await command.ExecuteNonQueryAsync();
                return await GetById(id);
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }

        public async Task<bool> Delete( int id )
        {
            try
            {
                string deleteQuery = $"DELETE FROM Todo WHERE Id = {id}";

                SqlCommand command = new(deleteQuery, _sqlConnection);

                await command.ExecuteNonQueryAsync();

                return true;

            }catch(Exception ex)
            {
                return false;
            }
        }
    }
}
