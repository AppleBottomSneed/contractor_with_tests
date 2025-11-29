namespace TestContractor;

using System.Diagnostics.Contracts;
using Contractor; 

// ensure both properties of project and unit tests are the same
[TestClass]
public sealed class TestContractor
{
    // tests GetContractors as well
    [TestMethod]
    public void TestAddContractor()
    {
        // arrange 
        RecruitmentSystem system = new RecruitmentSystem();
        Contractors contractor = new Contractors("Billy", "Bob", 10);

        //act
        system.AddContractors(contractor);

        //assert - checks if count for GetContractors() increases to 1
        Assert.AreEqual(system.GetContractors().Count, 1);
    }

    // tests GetJobs as well
    [TestMethod]
    public void TestAddJob()
    {
        //arrange
        RecruitmentSystem system = new RecruitmentSystem();
        Job job = new Job("Cook", 100);

        //act
        system.AddJobs(job);

        //assert
        Assert.AreEqual(system.GetJobs().Count, 1);
      
    }

    [TestMethod]
    public void TestRemoveContractor()
    {
        //arrange (incl method adding contractor)
        RecruitmentSystem system = new RecruitmentSystem();
        Contractors contractor = new Contractors("Billy", "Bob", 10);
        system.AddContractors(contractor);

        //act
        system.RemoveContractors(contractor);

        //assert - if contractor is found, fail test
        foreach (Contractors c in system.GetContractors())
        {
            if (c == contractor)
            {
                Assert.Fail();
            }
        }
    }

    [TestMethod]
    public void TestRemoveJob()
    { 
        RecruitmentSystem system = new RecruitmentSystem();
        Job job = new Job("Cook", 100);
        system.AddJobs(job);

        system.RemoveJobs(job);

        foreach (Job j in system.GetJobs())
        {
            if (j == job)
            {
                Assert.Fail();
            }
        }
    }

    [TestMethod]
    public void TestAssignJob()
    {
        RecruitmentSystem system = new RecruitmentSystem();
        Contractors contractor = new Contractors("Billy", "Bob", 10);
        Job job = new Job("Cook", 100);
        system.AddContractors(contractor);
        system.AddJobs(job);

        system.AssignJob(contractor, job);

        //ensure contractor AssignedJob = title and job's AssignedContractor = correct contractor
        Assert.AreEqual(contractor.AssignedJob, job.Title);
        Assert.AreEqual(job.AssignedContractor, contractor);

    }

    [TestMethod]
    public void TestCompleteJob()
    {
        RecruitmentSystem system = new RecruitmentSystem();
        Contractors contractor = new Contractors("Billy", "Bob", 10);
        Job job = new Job("Cook", 100);
        system.AddContractors(contractor);
        system.AddJobs(job);
        system.AssignJob(contractor, job);

        system.CompleteJob(job);

        Assert.IsTrue(job.Completed);
        Assert.IsNull(contractor.AssignedJob);
        Assert.IsNull(job.AssignedContractor);
    }

    [TestMethod]
    // assign neddy, unassign billy
    public void TestGetAvailableContractors()
    {
        RecruitmentSystem system = new RecruitmentSystem();
        Contractors contractorAvailable = new Contractors("Billy", "Bob", 10);
        Contractors contractorUnavailable = new Contractors("Ned", "Flanders", 30);
        system.AddContractors(contractorAvailable);
        system.AddContractors(contractorUnavailable);
        Job job = new Job("Cook", 100);
        system.AddJobs(job);
        system.AssignJob(contractorUnavailable, job);

        var available = system.GetAvailableContractors();

        //via GetAvailableContractors checks if billy and neddy is returned unassigned
        Assert.IsTrue(available.Contains(contractorAvailable));
        Assert.IsFalse(available.Contains(contractorUnavailable));
    }

    [TestMethod]
    public void TestGetUnassignedJobs()
    {
        RecruitmentSystem system = new RecruitmentSystem();
        Contractors contractor = new Contractors("Billy", "Bob", 10);
        system.AddContractors(contractor);
        Job jobAssigned = new Job("Cook", 100);
        Job jobUnassigned = new Job("Clean", 70);
        system.AddJobs(jobAssigned);
        system.AddJobs(jobUnassigned);
        system.AssignJob(contractor, jobAssigned);

        var available = system.GetUnassignedJobs();

        Assert.IsTrue(available.Contains(jobUnassigned));
        Assert.IsFalse(available.Contains(jobAssigned));
    }

    [TestMethod]
    // maximum cost explicitly declared in xaml (Maximum ="1000")
    //tests under pip, over pip, at pip and over max cost of 100 visibility
    public void TestGetJobByCost()
    {
        RecruitmentSystem system = new RecruitmentSystem();
        Job jobUnderPip = new Job("Undercook", 50);
        Job jobOverPip = new Job("Overcook", 150);
        Job jobAtPip = new Job("Just right", 100);
        Job jobOverMaxCost = new Job("Well done", 1001);
        system.AddJobs(jobUnderPip);
        system.AddJobs(jobOverPip);
        system.AddJobs(jobAtPip);
        system.AddJobs(jobOverMaxCost);

        float maxCost = 100;
        List<Job> filterList = system.GetJobByCost(maxCost);

        Assert.IsTrue(filterList.Contains(jobUnderPip));
        Assert.IsTrue(filterList.Contains(jobAtPip));
        Assert.IsFalse(filterList.Contains(jobOverPip));
        Assert.IsFalse(filterList.Contains(jobOverMaxCost));
    }

    //dont forget to add TestContractor solution to github!!!!
}
