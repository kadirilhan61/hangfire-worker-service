namespace WorkerService1
{
    public class ProcessJobs
    {
        public string JobName { get; set; } = nameof(JobName);
        public string JobTitle { get; set; } = nameof(JobName);
        public string ProcedureName { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public bool IsNewItem { get; set; } = false;


        public static List<ProcessJobs> GetProcessJobs()
        {
            return new List<ProcessJobs>()
            {
                new ProcessJobs()
                {
                    JobName = "Job1",
                    JobTitle = "Job - 1",
                    ProcedureName = "test",
                    IsNewItem =     true,
                    IsDeleted = true,
                },
                new ProcessJobs()
                {
                    JobName = "Job2",
                    JobTitle = "Job - 2",
                    IsNewItem =     true
                },
                //new ProcessJobs()
                //{
                //    JobName = "Job2",
                //    JobTitle = "Job - 2",
                //}
            };
        }

        public void Test()
        {

        }
        public void Test2(string test)
        {
            Console.WriteLine(test);
        }
    }
}
