namespace Student_Hall_Management.Dtos
{
    public class SuggestionsDto
    {
        public List<StudentSuggestionDto> Suggestions { get; set; }

        public SuggestionsDto()
        {
            Suggestions = new List<StudentSuggestionDto>();
        }
    }
}
