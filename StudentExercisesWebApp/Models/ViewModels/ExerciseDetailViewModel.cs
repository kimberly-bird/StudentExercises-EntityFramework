using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesWebApp.Models.ViewModels
{
    public class ExerciseDetailViewModel
    {
        public Exercise Exercise { get; set; }
        public List<Student> Students { get; set; }
        public List<StudentExercise> StudentExercises { get; set; }
    }
}
