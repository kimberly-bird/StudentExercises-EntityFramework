using Microsoft.AspNetCore.Mvc.Rendering;
using StudentExercisesWebApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesWebApp.Models.ViewModels
{
    public class CreateStudentViewModel
    {
        public Student Student { get; set; }

        // show all of the exercises in the select list. private because property is only set inside this class (in the constructor)
        public List<SelectListItem> AvailableExercises { get; private set; }

        // hold the exercises that are selected from the multiselect form
        public List<int> SelectedExercises { get; set; }

        // list of cohorts 
        public List<SelectListItem> Cohorts { get; set; }

        public CreateStudentViewModel() { }

        public CreateStudentViewModel(ApplicationDbContext ctx)
        {
            // construct the select list items with a list of exercises
            AvailableExercises = ctx.Exercises.Select(li => new SelectListItem()
            {
                Text = li.Name,
                Value = li.ExerciseId.ToString()
            }).ToList();

            // construct the select list items with a list of cohorts
            Cohorts = ctx.Cohorts.Select(li => new SelectListItem()
            {
                Text = li.Name,
                Value = li.CohortId.ToString()
            }).ToList();
        }
    }
}
