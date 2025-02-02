using Student_Hall_Management.Models;

namespace Student_Hall_Management.Repositories;

public interface IRegistrationRepository
{
    public bool SaveChanges();
    public void AddEntity<T>(T entityToAdd);
    public void RemoveEntity<T>(T entityToRemove);
    public void UpdateEntity<T>(T entityToUpdate);
    public Student GetSingleStudent(string email);
    public StudentPendingRequest PendingRequest(string email);
    public void AddStudentAuthentication(StudentAuthentication studentAuthentication);
    public string GetHallAdmin(string email);

}

