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
    public class ExercisesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExercisesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Exercises
        public async Task<IActionResult> Index()
        {
            return View(await _context.Exercises.ToListAsync());
        }

        // GET: Exercises/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // get single exercise with list of assigned students
            var exercise = await _context.Exercises
                .Include(s => s.StudentExercises)
                .FirstOrDefaultAsync(m => m.ExerciseId == id);

            if (exercise == null)
            {
                return NotFound();
            }

            // get all assigned exercises from StudentExercise join table
            IEnumerable<StudentExercise> studentExercises =
                 _context.StudentExercise
                    .Include(se => se.Student)
                    .Where(se => se.ExerciseId == exercise.ExerciseId);

            // get exercise details (name, language, etc.)
            IEnumerable<Student> students = studentExercises.Select(s => s.Student);

            ExerciseDetailViewModel viewmodel = new ExerciseDetailViewModel()
            {
                Exercise = exercise,
                Students = students.ToList()
            };

            return View(viewmodel);
        }

        // GET: Exercises/Create
        [HttpGet]
        public IActionResult Create()
        {
            CreateExerciseViewModel model = new CreateExerciseViewModel(_context);
            return View(model);
        }

        // POST: Exercises/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateExerciseViewModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model.Exercise);

                foreach (int studentId in model.SelectedStudents)
                {
                    // create a new instance of StudentExercise for each selected student
                    StudentExercise newSE = new StudentExercise()
                    {
                        ExerciseId = model.Exercise.ExerciseId,
                        StudentId = studentId
                    };
                    // insert each selected exercise to the StudentExercise join table in db
                    _context.Add(newSE);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Exercises/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null)
            {
                return NotFound();
            }
            return View(exercise);
        }

        // POST: Exercises/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExerciseId,Name,Language")] Exercise exercise)
        {
            if (id != exercise.ExerciseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exercise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExerciseExists(exercise.ExerciseId))
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
            return View(exercise);
        }

        // GET: Exercises/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises
                .FirstOrDefaultAsync(m => m.ExerciseId == id);
            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }

        // POST: Exercises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExerciseExists(int id)
        {
            return _context.Exercises.Any(e => e.ExerciseId == id);
        }
    }
}
