using Microsoft.AspNetCore.Mvc.Rendering;
using StudentExercisesWebApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesWebApp.Models.ViewModels
{
    public class CreateExerciseViewModel
    {
        public Exercise Exercise { get; set; }

        // list all students who are available to be assigned the exercise
        public List<SelectListItem> AvailableStudents { get; private set; }

        // holds the students that were selected when exercise is posted
        public List<int> SelectedStudents { get; set; }

        public CreateExerciseViewModel() { }

        public CreateExerciseViewModel(ApplicationDbContext ctx)
        {
            // construct the select list items with a list of students
            AvailableStudents = ctx.Students.Select(li => new SelectListItem()
            {
                Text = li.FirstName,
                Value = li.StudentId.ToString()
            }).ToList();
        }

    }
}
