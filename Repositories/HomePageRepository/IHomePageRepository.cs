namespace Student_Hall_Management.Repositories
{
    public interface IHomePageRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public void RemoveEntity<T>(T entityToRemove);
        public void UpdateEntity<T>(T entityToUpdate);
        public string GetSingleStudentImageData(string email);
        public Task<int?> GetHallId(string email);
        public Task<string?> GetSingleHallImageData(int hallId);
    }
}
