using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesWebApp.Models.ViewModels
{
    public class CohortDetailViewModel
    {
        public Cohort Cohort { get; set; }
        public List<Student> Students { get; set; }
    }
}
