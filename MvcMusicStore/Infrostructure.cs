using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using NLog;

namespace MvcMusicStore
{
    public class Infrostructure
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void CreateCounterCategory(string name)
        {
            if (PerformanceCounterCategory.Exists(name))
            {
                try
                {
                    PerformanceCounterCategory.Delete(name);
                }
                catch (UnauthorizedAccessException ex)
                {
                    logger.Error($"Cannot delete existing performance counter {name}", ex);
                } 
            }

            var counters = new CounterCreationDataCollection();

            var usersAtWork = new CounterCreationData($"{name} counter",
                $"Count of {name} events",
                PerformanceCounterType.NumberOfItems32);

            counters.Add(usersAtWork);

            try
            {
                PerformanceCounterCategory.Create(name, "Info related to LogIn", counters);
            }
            catch (UnauthorizedAccessException ex)
            {
                logger.Error($"Cannot create performance counter with name: {name}", ex);
            }
            catch (InvalidOperationException ex)
            {
                logger.Error($"Cannot create Performance Category {name} because it already exists", ex);
            }
        }

        public static void StartUpdatingCounters(string name)
        {
            using (PerformanceCounter createdCounter = new PerformanceCounter(name,
                $"{name} counter", false))
            {
                createdCounter.Increment();
            }
        }
    }
}