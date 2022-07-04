using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DevsEvolutionBack.Models;

namespace DevsEvolutionBack.Controllers
{
    [Route("api/cards")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public CardsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                        select id,userId,name,description,pillar from 
                        cards
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("AppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult(table);
        }


        [HttpPost]
        public JsonResult Post(Cards car)
        {
            string query = @"
                        insert into cards (userId,name,description,pillar)
                        values
                        (@userId,@name,@description,@pillar);
                        
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("AppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@userId", car.userId);
                    myCommand.Parameters.AddWithValue("@name", car.name);
                    myCommand.Parameters.AddWithValue("@description", car.description);
                    myCommand.Parameters.AddWithValue("@pillar", car.pillar);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }


        [HttpPut]
        public JsonResult Put(Cards car)
        {
            string query = @"
                        update cards set 
                        name =@name,
                        description =@description,
                        pillar =@pillar
                        where id=@id;
                        
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("AppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@id", car.id);
                    myCommand.Parameters.AddWithValue("@userId", car.userId);
                    myCommand.Parameters.AddWithValue("@name", car.name);
                    myCommand.Parameters.AddWithValue("@description", car.description);
                    myCommand.Parameters.AddWithValue("@pillar", car.pillar);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Updated Successfully");
        }



        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                        delete from cards 
                        where id=@id;
                        
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("AppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Deleted Successfully");
        }

        [HttpGet("{id}")]
        public JsonResult GetById(int id)
        {
            string CardQuery = @"
                        select cards.description,cards.name,cards.id,pillar,users.id,users.name,position,direction from 
                        cards inner join users on userId = users.id
                        where cards.id=@id;
            ";
            string TaskQuery = @"
                        select id,text,done from 
                        tasks
                        where cardId=@id;
            ";
            List<Tasks> tasks = new List<Tasks>();
            Users user = new Users();
            Cards card = new Cards();
            string sqlDataSource = _configuration.GetConnectionString("AppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(CardQuery, mycon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);

                    myReader = myCommand.ExecuteReader();

                    if (myReader.Read())
                    {
                        card.id = (int)myReader.GetValue(2);
                        card.name = (string)myReader.GetValue(1);
                        card.description = (string)myReader.GetValue(0);
                        user.id = (int)myReader.GetValue(4);
                        card.userId = (int)myReader.GetValue(4);
                        user.name = (string)myReader.GetValue(5);
                        card.pillar = myReader.GetString("pillar");
                        user.direction = myReader.GetString("direction");
                        user.position = myReader.GetString("position");
                    }

                    myReader.Close();
                }
                using (MySqlCommand myCommand = new MySqlCommand(TaskQuery, mycon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);

                    myReader = myCommand.ExecuteReader();

                    while (myReader.Read())
                    {
                        Tasks task = new Tasks();
                        task.id = myReader.GetInt32("id");
                        task.text = myReader.GetString("text");
                        task.done = myReader.GetBoolean("done");
                        tasks.Add(task);
                    }
                    myReader.Close();
                }
                mycon.Close();
            }
            if (tasks == null || user == null || card == null)
            {
                throw new SystemException("Object not found");
            }
            card.Users = user;
            card.Task = tasks;

            return new JsonResult(card);
        }

    }
}