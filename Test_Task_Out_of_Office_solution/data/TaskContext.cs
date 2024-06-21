using Microsoft.EntityFrameworkCore;
using Test_Task_Out_of_Office_solution.models;

namespace Test_Task_Out_of_Office_solution.data;

public class TaskContext : DbContext
{
    public TaskContext(DbContextOptions<TaskContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<LeaveRequest> LeaveRequests { get; set; }
    public DbSet<ApprovalRequest> ApprovalRequests { get; set; }
    public DbSet<Project> Projects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.PeoplePartner)
            .WithMany()
            .HasForeignKey(e => e.PeoplePartnerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LeaveRequest>()
            .HasOne(lr => lr.Employee)
            .WithMany(e => e.LeaveRequests)
            .HasForeignKey(lr => lr.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ApprovalRequest>()
            .HasOne(ar => ar.Approver)
            .WithMany()
            .HasForeignKey(ar => ar.ApproverId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ApprovalRequest>()
            .HasOne(lr => lr.LeaveRequest)
            .WithOne(lr => lr.ApprovalRequest)
            .HasForeignKey<ApprovalRequest>(ar => ar.LeaveRequestId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Project)
            .WithMany(p => p.Employees)
            .HasForeignKey(e => e.ProjectId);

        modelBuilder.Entity<Project>()
            .HasOne(p => p.ProjectManager)
            .WithMany()
            .HasForeignKey(p => p.ProjectManagerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}