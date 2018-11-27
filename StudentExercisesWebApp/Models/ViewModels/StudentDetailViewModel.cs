using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesWebApp.Models.ViewModels
{
    public class StudentDetailViewModel
    {
        public Student Student { get; set; }
        public List<Exercise> Exercises { get; set; }
        public List<StudentExercise> StudentExercises { get; set; }
    }
}
