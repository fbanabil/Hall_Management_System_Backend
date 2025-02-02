namespace Student_Hall_Management.Repositories
{
    public interface IHallDetailsManagementRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public void RemoveEntity<T>(T entityToRemove);
        public void UpdateEntity<T>(T entityToUpdate);
    }
}
