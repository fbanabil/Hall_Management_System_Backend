namespace Student_Hall_Management.Dtos
{
    public class ChatBoxDto
    {
        public List<ChatToShowDto> Chats { get; set; }

        public ChatBoxDto()
        {
            Chats = new List<ChatToShowDto>();
        }
    }
}
