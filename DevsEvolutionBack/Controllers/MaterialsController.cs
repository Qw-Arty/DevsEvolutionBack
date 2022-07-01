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
    [Route("api/materials")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public MaterialsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                        select id,name,type,description,DATE_FORMAT(date,'%Y-%m-%d') as date from 
                        materials
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
        public JsonResult Post(Materials mat)
        {
            string query = @"
                        insert into materials (name,type,description,date)
                        values
                        (@name,@type,@description,@date);
                        
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("AppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@name", mat.name);
                    myCommand.Parameters.AddWithValue("@type", mat.type);
                    myCommand.Parameters.AddWithValue("@description", mat.description);
                    myCommand.Parameters.AddWithValue("@date", mat.date);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }


        [HttpPut("{id}")]
        public JsonResult Put(Materials mat, int id)
        {
            string query = @"
                        update materials set 
                        name =@name,
                        type =@type,
                        description =@description,
                        date =@date
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
                    myCommand.Parameters.AddWithValue("@name", mat.name);
                    myCommand.Parameters.AddWithValue("@type", mat.type);
                    myCommand.Parameters.AddWithValue("@description", mat.description);
                    myCommand.Parameters.AddWithValue("@date", mat.date);

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
                        delete from materials 
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
                        select id,name,type,description,DATE_FORMAT(date,'%Y-%m-%d') as date from 
                        materials 
                        where id=@id;
                        
            ";
            Materials materials = null;
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
                        materials = new Materials();
                        materials.id = myReader.GetInt32("id");
                        materials.name = myReader.GetString("name");
                        materials.description = myReader.GetString("description");
                        materials.date = myReader.GetString("date");
                        materials.type = myReader.GetString("type");
                    }

                    myReader.Close();
                    mycon.Close();
                }
            }
            if (materials == null)
            {
                throw new SystemException("Object not found");
            }

            return new JsonResult(materials);
        }

    }
}
