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
            string query = @"
                        select id,userId,name,description,pillar from 
                        cards 
                        where id=@id;
                        
            ";
            Cards cards = null;
            string sqlDataSource = _configuration.GetConnectionString("AppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);

                    myReader = myCommand.ExecuteReader();

                    if (myReader.Read())
                    {
                        cards = new Cards();
                        cards.id = myReader.GetInt32("id");
                        cards.userId = myReader.GetInt32("userId");
                        cards.name = myReader.GetString("name");
                        cards.description = myReader.GetString("description");
                        cards.pillar = myReader.GetString("pillar");
                    }

                    myReader.Close();
                    mycon.Close();
                }
            }
            if (cards == null)
            {
                throw new SystemException("Object not found");
            }

            return new JsonResult(cards);
        }

    }
}