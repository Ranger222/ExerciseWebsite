﻿using ExerciseWebsite.Entities;
using ExerciseWebsite.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExerciseWebsite.Services
{
    public interface IWorkoutService
    {
        Task<Workout> Create(Workout workout);
        Task<Workout> GetById(int id);
        Task<IEnumerable<Workout>> GetAllAsync();
        Task Update(Workout workoutParam);
        Task UpdateRating(int id, int newRating);
        Task UpdateAvgDifficulty(int id, double avgDifficulty);
        Task Delete(int id);

    }
    public class WorkoutService : IWorkoutService
    {
        private DataContext _context;

        public WorkoutService(DataContext context)
        {
            _context = context;
        }

        public async Task<Workout> Create(Workout workout)
        {
            // Check if already added?

            _context.Workouts.Add(workout);
            await _context.SaveChangesAsync();

            return workout;
        }

        public async Task<IEnumerable<Workout>> GetAllAsync()
        {
            IEnumerable<Workout> workouts;
            return await Task.Run(() => workouts = GetAll());
        }

        public IEnumerable<Workout> GetAll()
        {
            return _context.Workouts;
        }

        public async Task<Workout> GetById(int id)
        {
            var workout = await _context.Workouts.FindAsync(id);

            if (workout != null)
                return workout;
            else
                throw new AppException($"No workout with id {id} found.");
        }

        public async Task Update(Workout workoutParam)
        {
            var workout = await _context.Workouts.FindAsync(workoutParam.Id);

            if (workout == null)
                throw new AppException($"No workout with id {workoutParam.Id} found.");


            workout.Title = workoutParam.Title;
            workout.Description = workoutParam.Description;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateRating(int id, int newRating)
        {
            var workout = await _context.Workouts.FindAsync(id);

            if (workout == null)
                throw new AppException($"No workout with id {id} found.");

            workout.RatingCount++;
            workout.Rating = (workout.Rating * workout.RatingCount + newRating) / workout.RatingCount;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAvgDifficulty(int id, double avgDifficulty)
        {
            var workout = await _context.Workouts.FindAsync(id);
            
            if (workout == null)
                throw new AppException($"No workout with id {id} found.");

            workout.AvgDifficulty = avgDifficulty;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var workout = await _context.Workouts.FindAsync(id);

            if (workout != null)
            {
                _context.Workouts.Remove(workout);
                await _context.SaveChangesAsync();
            }
            else
                throw new AppException($"Workout with id {id} does not exist");
        }
    }
}
