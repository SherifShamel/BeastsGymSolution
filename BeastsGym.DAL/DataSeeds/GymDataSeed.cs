using BeastsGym.DAL.Contexts;
using BeastsGym.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BeastsGym.DAL.DataSeeds
{
    public static class GymDataSeed
    {
     public static async Task SeedAsync(BeastsGymDbContext dbContext, string filePath, ILogger logger, CancellationToken ct = default)
        {
			try
			{
				if(!await dbContext.Plans.AnyAsync())
				{
					var Data = LoadDataFromJsonFile<Plan>("plans.json", filePath);
					if(Data.Count > 0)
					{
						dbContext.Plans.AddRange(Data);
						logger.LogInformation($"Plans Are Seesed Successfully {Data.Count}");
					}
				}

				if(dbContext.ChangeTracker.HasChanges())
                    await dbContext.SaveChangesAsync(ct);
                
            }
			catch (Exception ex)
			{
				logger.LogError("DataSeeding Failed");
				throw;
			}
        }

		private static List<T> LoadDataFromJsonFile<T>(string fileName, string folderPath)
		{
			var FilePath = Path.Combine(folderPath, fileName);
			if (!File.Exists(FilePath))
				throw new FileNotFoundException($"File {fileName} not found in folder {folderPath}");

			var Data = File.ReadAllText(FilePath);

			var Options = new JsonSerializerOptions()
			{
				PropertyNameCaseInsensitive = true
			};
			Options.Converters.Add(new JsonStringEnumConverter());

			return JsonSerializer.Deserialize<List<T>>(Data, Options) ?? [];
		}
    }
}
