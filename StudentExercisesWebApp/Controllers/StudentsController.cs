using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentExercisesWebApp.Data;
using StudentExercisesWebApp.Models;
using StudentExercisesWebApp.Models.ViewModels;

namespace StudentExercisesWebApp.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Students.Include(s => s.Cohort);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // get single student with cohort, exercises, StudentExercises 
            var student = await _context.Students
                .Include(s => s.Cohort)
                .Include(s => s.StudentExercises)
                .FirstOrDefaultAsync(m => m.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            // get all single students' assigned exercises from StudentExercise join table
            IEnumerable<StudentExercise> studentExercises =
                 _context.StudentExercise
                    .Include(se => se.Exercise)
                    .Where(se => se.StudentId == student.StudentId);

            // get exercise details (name, language, etc.)
            IEnumerable<Exercise> exercises = studentExercises.Select(se => se.Exercise);

            StudentDetailViewModel viewmodel = new StudentDetailViewModel()
            {
                Student = student,
                // put exercise details into a list
                Exercises = exercises.ToList()
            };

            return View(viewmodel);
        }

        // GET: Students/Create
        [HttpGet]
        public IActionResult Create()
        {
            CreateStudentViewModel model = new CreateStudentViewModel(_context);
            return View(model);
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateStudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                // insert new student into db
                _context.Add(model.Student);

                // loop over each selected exercise
                foreach (int exerciseId in model.SelectedExercises)
                {
                    // create a new instance of StudentExercise for each selected exercise
                    StudentExercise newSE = new StudentExercise()
                    {
                        StudentId = model.Student.StudentId,
                        ExerciseId = exerciseId
                    };
                    // insert each selected exercise to the StudentExercise join table in db
                    _context.Add(newSE);
                }
                // once complete with db updates, save changes to the db
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CohortId"] = new SelectList(_context.Cohorts, "CohortId", "Name", model.Student.CohortId);
            return View(model);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["CohortId"] = new SelectList(_context.Cohorts, "CohortId", "Name", student.CohortId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,FirstName,LastName,CohortId")] Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CohortId"] = new SelectList(_context.Cohorts, "CohortId", "Name", student.CohortId);
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Cohort)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }
    }
}
