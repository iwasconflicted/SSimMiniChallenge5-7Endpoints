using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCAPI.Data;
using MVCAPI.Models;

namespace MVCAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context){
            _context = context; 
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(Student newStudent){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            await _context.AddAsync(newStudent);

            var result = await _context.SaveChangesAsync();
            if(result > 0 ){
                return Ok("Student Created Successfully");
            }
            else{
                return BadRequest("Could not create student");
            }
        }

        [HttpGet("/getAllStudents")]
        public async Task<IEnumerable<Student>> getStudents(){
            var ourStudents = await _context.Students.AsNoTracking().ToListAsync();
            return ourStudents; 
        }

        [HttpGet("studentById")]
        public async Task<ActionResult<Student>> getStudent(int id){
            var ourStudent = await _context.Students.FindAsync(id);

            if(ourStudent == null){
                return NotFound("Sorry bro, wrong guy pal");
            }
            else{
                return Ok(ourStudent);
            }
        }

        [HttpPut]

        public async Task<IActionResult> EditStudent(int id, Student student){
            var ourStudent = await _context.Students.FindAsync(id);

            if(ourStudent == null){
                return BadRequest();
            }

            ourStudent.Name = student.Name;
            ourStudent.Address = student.Address;
            ourStudent.Email = student.Email;
            ourStudent.PhoneNumber = student.PhoneNumber;

            var result = await _context.SaveChangesAsync();

            if(result > 0){
                return Ok("Student successfully updated");
            }
            else{
                return BadRequest();
            }
        }

        [HttpDelete]

        public async Task<ActionResult<Student>> deleteStudent(int id){
            var ourStudent = await _context.Students.FindAsync(id);

            if(ourStudent == null){
                return NotFound("Error homie");
            }
            _context.Remove(ourStudent);
            var result = await _context.SaveChangesAsync();
            if(result > 0){
                return Ok("student deleted homie");
            }
            else {
                return BadRequest();
            }
        }

        [HttpGet("MadLib")]
        public string MadLib(string Name, string Place, string Verb1, string Verb2, string Adj1, string Adj2, string Noun)
        {
            string madLib = $"{Name} hails from {Place} and he was on his way to {Noun} and he was feeling {Adj1} so he decided to {Verb1} and {Verb2} while also {Adj2} and it was a great day.";
            return madLib;
        }

        [HttpGet("OddEven")]
        public IActionResult OddEven(int number)
        {
            string result = (number % 2 == 0) ? "even" : "odd";

            return Ok($"The number {number} is {result}.");
        }

        [HttpGet("ReverseIt")]
            public ActionResult<string> ReverseIt(string word)
            {
                if (string.IsNullOrEmpty(word))
                    return BadRequest("It can't be blank");

                char[] charArray = word.ToCharArray();
                Array.Reverse(charArray);
                return $"The original is {word} \nThe reverse of this is {new string(charArray)}";
            }

        [HttpGet("ReverseItNumbersOnly")]
        public ActionResult<string> NumbersOnly(string number)
        {
            if (string.IsNullOrEmpty(number))
                return BadRequest("It can't be blank");

            char[] charArray = number.ToCharArray();
            Array.Reverse(charArray);
            return $"The original is {number} \nThe reverse of this is {new string(charArray)}";
        }

    }
}