using Student_Hall_Management.Models;

namespace Student_Hall_Management.Repositories
{
    public interface ILoginRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public void RemoveEntity<T>(T entityToRemove);
        public void UpdateEntity<T>(T entityToUpdate);
        public StudentAuthentication GetSingleStudentAuthentication(string email);
        public HallAdminAuthentication GetSingleHallAdminAuthentication(string email);
        public  Task UpdateActivity(bool isActive, string email);


    }
}
