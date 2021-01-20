using System;
using System.Collections.Generic;
using System.Reflection;
using DeepEqual.Syntax;
using gamitude_backend.Data;
using gamitude_backend.Models;
using gamitude_backend.Repositories;
using gamitude_backend.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace gamitude_backend.Tests
{
    public class StatisticsTests
    {
        public static IEnumerable<object[]> statsData()
        {
            var allStats = new STATS[] { STATS.STRENGTH, STATS.INTELLIGENCE, STATS.CREATIVITY, STATS.FLUENCY };
            var opString = "add";
            foreach (var stat in allStats)
            {
                var initStats = new Stats();
                var expectedStats = new Stats();
                expectedStats.GetType().InvokeMember(stat.ToString().ToLower(),
                                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                                        Type.DefaultBinder, expectedStats, new object[] { 90 });
                var project = new Project
                {
                    projectType = PROJECT_TYPE.STAT,
                    name = "test1",
                    stats= new STATS[] { stat },
                    dominantStat = stat
                };
                var projectLog = new ProjectLog
                {
                    log = "test",
                    project = project,
                    timeSpend = 90,
                    type = PROJECT_TYPE.STAT,
                };
                yield return new object[] { expectedStats, initStats, projectLog, opString };
            }
        }

        [Theory]
        [MemberData(nameof(statsData))]
        public void statsTest(Stats expectedStats, Stats initStats, ProjectLog projectLog, string opString)
        {
            var mockDatabaseCollections = Mock.Of<IDatabaseCollections>();
            var mockLogger = Mock.Of<ILogger<ProjectLogService>>();
            var mockStatsRepo = Mock.Of<IStatsRepository>();
            var mockProjectRepo = Mock.Of<IProjectRepository>();
            var mockDailyEnergyRepo = Mock.Of<IDailyEnergyRepository>();

            var projectLogService = new ProjectLogService(mockDatabaseCollections, mockLogger, mockStatsRepo, mockProjectRepo, mockDailyEnergyRepo);
            MethodInfo dynMethod = projectLogService.GetType().GetMethod("calculateAndUpdateStats", BindingFlags.NonPublic | BindingFlags.Instance);
            var dynOp = projectLogService.GetType().GetMethod(opString, BindingFlags.Public | BindingFlags.Instance);
            var delType = projectLogService.GetType().GetNestedType("Del");
            var op = Delegate.CreateDelegate(delType, projectLogService, dynOp);

            var wages = projectLog.getWages();
            var newStats = (Stats)dynMethod.Invoke(projectLogService, new object[] { op, initStats, wages, projectLog });

            expectedStats.ShouldDeepEqual(newStats);
        }
    }
}
