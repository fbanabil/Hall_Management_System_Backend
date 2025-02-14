namespace Student_Hall_Management.Dtos
{
    public class ChatToShowDto
    {
        public int chatId { get; set; }
        public string message { get; set; }
        public int sender { get; set; }
        public string senderName { get; set; }
        public string SentTime { get; set; }

        public int myself { get; set; }

        public ChatToShowDto()
        {
            if (chatId == 0)
            {
                chatId = 0;
            }
            if (message == null)
            {
                message = " ";
            }
       
            if (SentTime == null)
            {
                SentTime = " ";
            }
        }
    }
}
