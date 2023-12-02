using System;
using System.Collections.Generic;
using System.IO;


namespace NewspaperSellerModels
{ public class task2
    {
        public SimulationSystem simulationSystem = new SimulationSystem();
        public SimulationSystem get_data1()
        {
            return simulationSystem;
        }
        // get the demand by cumulative range 
        public int calc_demand(int random_demand, Enums.DayType day_type)
        {
            int res = 0;
            Int32 index = (int)day_type;
            for (int i = 0; i < simulationSystem.DemandDistributions.Count; i++)
            {
                if (random_demand <= simulationSystem.DemandDistributions[i].DayTypeDistributions[index].MaxRange
                    && random_demand >= simulationSystem.DemandDistributions[i].DayTypeDistributions[index].MinRange)
                {
                    res = simulationSystem.DemandDistributions[i].Demand;
                    break;
                }
            }
            return res;
        }

        // get the day type by cumulative range 
        public Enums.DayType calc_day(int random_day_type)
        {
            Enums.DayType day_type = Enums.DayType.Fair;
            for (int i = 0; i < simulationSystem.DayTypeDistributions.Count; i++)
            {
                if (random_day_type <= simulationSystem.DayTypeDistributions[i].MaxRange && random_day_type >= simulationSystem.DayTypeDistributions[i].MinRange)
                {
                    day_type = simulationSystem.DayTypeDistributions[i].DayType;
                    break;
                }

            }

            return day_type;
        }
        //reading the test file 
        public void read_test_file(string file)
        {
            DayTypeDistribution day_good = new DayTypeDistribution();
            DayTypeDistribution day_fair = new DayTypeDistribution();
            DayTypeDistribution day_poor = new DayTypeDistribution();
            decimal cumulative = 0, cumu1 = 0, cumu2 = 0, cumu3 = 0;
            List<String> testing_lines = new List<string>();

            using (FileStream test_file = new FileStream(file, FileMode.Open))
            using (StreamReader sr = new StreamReader(test_file))
            {
                while (!sr.EndOfStream)
                {
                    string linee = sr.ReadLine();
                    if (!string.IsNullOrEmpty(linee))
                    {
                        testing_lines.Add(linee);
                    }
                }
            }
            //reading the single values
            simulationSystem.NumOfNewspapers = int.Parse(testing_lines[1]);
            simulationSystem.NumOfRecords = int.Parse(testing_lines[3]);
            simulationSystem.PurchasePrice = decimal.Parse(testing_lines[5]);
            simulationSystem.ScrapPrice = decimal.Parse(testing_lines[7]);
            simulationSystem.SellingPrice = decimal.Parse(testing_lines[9]);
            simulationSystem.UnitProfit = simulationSystem.SellingPrice - simulationSystem.PurchasePrice;
            char[] sep = { ' ', ',' };


            //reading DayTypeDistributions (cumu ranges) from the file
            String[] DayType_dis_test = testing_lines[11].Split(sep, StringSplitOptions.RemoveEmptyEntries);
            day_good.day_prob(Enums.DayType.Good, decimal.Parse(DayType_dis_test[0]), cumulative);
            simulationSystem.DayTypeDistributions.Add(day_good);
            cumulative += day_good.Probability;
            day_fair.day_prob(Enums.DayType.Fair, decimal.Parse(DayType_dis_test[1]), cumulative);
            simulationSystem.DayTypeDistributions.Add(day_fair);
            cumulative += day_fair.Probability;
            day_poor.day_prob(Enums.DayType.Poor, decimal.Parse(DayType_dis_test[2]), cumulative);
            simulationSystem.DayTypeDistributions.Add(day_poor);



            int line = 13;
            //reading DemandDistributions from the file
            int row_demand = 0;
            while (row_demand < 7)
            {
                DayTypeDistribution good = new DayTypeDistribution();
                DayTypeDistribution fair = new DayTypeDistribution();
                DayTypeDistribution poor = new DayTypeDistribution();
                DemandDistribution demand_dist = new DemandDistribution();
                String[] test_DemandDistributions = testing_lines[line].Split(sep, StringSplitOptions.RemoveEmptyEntries);
                demand_dist.Demand = int.Parse(test_DemandDistributions[0]);
                good.day_prob(Enums.DayType.Good, decimal.Parse(test_DemandDistributions[1]), cumu1);
                cumu1 += good.Probability;
                demand_dist.DayTypeDistributions.Add(good);
                fair.day_prob(Enums.DayType.Fair, decimal.Parse(test_DemandDistributions[2]), cumu2);
                cumu2 += fair.Probability;
                demand_dist.DayTypeDistributions.Add(fair);
                poor.day_prob(Enums.DayType.Poor, decimal.Parse(test_DemandDistributions[3]), cumu3);
                cumu3 += poor.Probability;
                demand_dist.DayTypeDistributions.Add(poor);
                simulationSystem.DemandDistributions.Add(demand_dist);
                line++;
                row_demand++;
            }
        }



        //calculate the values and fill the table
        public void Simulation_table()
        {

            Random rand = new Random();
            for (int i = 0; i < simulationSystem.NumOfRecords; i++)
            {
                SimulationCase simulation_case = new SimulationCase();
                int random_day_type = rand.Next(1, 100);
                int random_demand = rand.Next(1, 100);
                simulation_case.DayNo = i + 1;
                simulation_case.RandomNewsDayType = random_day_type;
                simulation_case.NewsDayType = calc_day(random_day_type);
                simulation_case.RandomDemand = random_demand;
                simulation_case.Demand = calc_demand(random_demand, simulation_case.NewsDayType);
                simulation_case.DailyCost = simulationSystem.NumOfNewspapers * simulationSystem.PurchasePrice; // ex: 20 *0.33 cent


                // we don't have loss profit (no Excess demand)
                if (simulation_case.Demand <= simulationSystem.NumOfNewspapers)
                {// in case we have Scrap
                    if (simulation_case.Demand != simulationSystem.NumOfNewspapers)
                        simulationSystem.PerformanceMeasures.DaysWithUnsoldPapers++;
                    simulation_case.SalesProfit = (simulationSystem.SellingPrice * simulation_case.Demand);
                    simulation_case.LostProfit = 0;
                    simulation_case.ScrapProfit = ((simulationSystem.NumOfNewspapers - simulation_case.Demand) * simulationSystem.ScrapPrice);
                    simulation_case.DailyNetProfit = simulation_case.SalesProfit - simulation_case.DailyCost + simulation_case.ScrapProfit;
                }


                else
                {// we don't have Scrap and have Excess demand----LostProfit have a value 
                    simulation_case.SalesProfit = (simulationSystem.NumOfNewspapers * simulationSystem.SellingPrice);
                    simulation_case.LostProfit = ((simulation_case.Demand - simulationSystem.NumOfNewspapers) * simulationSystem.UnitProfit);
                    simulation_case.ScrapProfit = 0;
                    simulation_case.DailyNetProfit = simulation_case.SalesProfit - simulation_case.DailyCost - simulation_case.LostProfit;
                    simulationSystem.PerformanceMeasures.DaysWithMoreDemand++;
                }


                simulationSystem.SimulationTable.Add(simulation_case);
                simulationSystem.PerformanceMeasures.TotalLostProfit += simulation_case.LostProfit;
                simulationSystem.PerformanceMeasures.TotalNetProfit += simulation_case.DailyNetProfit;
                simulationSystem.PerformanceMeasures.TotalCost += simulation_case.DailyCost;
                simulationSystem.PerformanceMeasures.TotalSalesProfit += simulation_case.SalesProfit;
                simulationSystem.PerformanceMeasures.TotalScrapProfit += simulation_case.ScrapProfit;

            }
        }

        public class get_data
        {
        }
    }






}