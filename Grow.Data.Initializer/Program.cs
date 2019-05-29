using Microsoft.EntityFrameworkCore;
using System;

namespace Grow.Data.Initializer
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting database preparation");

            // Extract connection string from arguments
            string connectionString;
            if (args.Length == 0 || string.IsNullOrEmpty(args[0]))
                throw new ArgumentException("A connection string has to be provided as the first argument");
            connectionString = args[0];

            try
            {
                // Create context
                var optionsBuilder = new DbContextOptionsBuilder<GrowDbContext>();
                optionsBuilder.EnableSensitiveDataLogging(true);
                optionsBuilder.UseSqlServer(connectionString);
                var context = new GrowDbContext(optionsBuilder.Options);

                // Reset data
                Console.WriteLine("Resetting database");
                context.ResetDatabase();

                // Seed data
                Console.WriteLine("Adding seed data");
                context.SeedDataFrom2018();
                context.SeedDataFrom2017();

                Console.WriteLine("COMPLETED");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error has occurred: " + e.Message);
                Console.ReadKey();
            }
        }
    }
}
