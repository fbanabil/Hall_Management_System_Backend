using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student_Hall_Management.Dtos;
using Student_Hall_Management.Helpers;
using Student_Hall_Management.Models;
using Student_Hall_Management.Repositories;

namespace Student_Hall_Management.Controllers
{
    [Authorize("StudentPolicy")]
    [ApiController]
    [Route("/[controller]")]
    public class ChatController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly IChatRepository _chatRepository;
        private readonly IPresentDateTime _presentDateTime;

        public ChatController(IChatRepository chatRepository,IPresentDateTime presentDateTime)
        {
            _chatRepository = chatRepository;
            _presentDateTime = presentDateTime;
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<StudentsMessage, ChatToShowDto>();
                cfg.CreateMap<ChatToAddDto, StudentsMessage>();
            }).CreateMapper();
        }


        [HttpGet("GetChats")]
        public async Task<IActionResult> GetChats()
        {
            ChatBoxDto chatBoxDto = new ChatBoxDto();
            List<ChatToShowDto> chatToShowDtos = new List<ChatToShowDto>();
            string email = User.FindFirst("userEmail")?.Value;
            Student student = await _chatRepository.GetStudentByEmail(email);
            List<StudentsMessage> chats = await _chatRepository.GetChats(student.HallId);

            foreach (var chat in chats)
            {
                ChatToShowDto chatToShowDto = new ChatToShowDto();
                chatToShowDto.chatId = chat.Id;
                chatToShowDto.message = chat.Message;
                chatToShowDto.sender = chat.Sender;
                chatToShowDto.senderName =await _chatRepository.GetStudentName(chat.Sender);
                chatToShowDto.SentTime = chat.Date.ToString("o"); // Convert DateTime to string
                chatToShowDto.myself =student.Id; 
                chatToShowDtos.Add(chatToShowDto);
            }

            chatBoxDto.Chats = chatToShowDtos;
            chatBoxDto.Chats.Reverse();
            return Ok(chatBoxDto);
        }



        [HttpPost("AddChat")]
        public async Task<IActionResult> AddChat(ChatToAddDto chatDto)
        {
            StudentsMessage chat = _mapper.Map<StudentsMessage>(chatDto);
            chat.Date=_presentDateTime.GetPresentDateTime();
            string email= User.FindFirst("userEmail")?.Value;
            Student student = await _chatRepository.GetStudentByEmail(email);
            Console.WriteLine(student.Id);
            chat.Sender = student.Id;
            chat.HallId = student.HallId.Value;
            _chatRepository.AddEntity(chat);

            if(_chatRepository.SaveChanges())
            {
                ChatBoxDto chatBoxDto = new ChatBoxDto();
                List<ChatToShowDto> chatToShowDtos = new List<ChatToShowDto>();

                List<StudentsMessage> chats = await _chatRepository.GetChats(student.HallId.Value);

                foreach (var chat1 in chats)
                {
                    ChatToShowDto chatToShowDto = new ChatToShowDto();
                    chatToShowDto.chatId = chat1.Id;
                    chatToShowDto.message = chat1.Message;
                    chatToShowDto.sender = chat1.Sender;
                    chatToShowDto.SentTime = chat1.Date.ToString("o"); // Convert DateTime to string
                    chatToShowDtos.Add(chatToShowDto);
                }

                chatBoxDto.Chats = chatToShowDtos;
                chatBoxDto.Chats.Reverse();

                return Ok(chatBoxDto);
            }
            return BadRequest("Failed to add chat");

        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateChat()
        {
            ChatBoxDto chatBoxDto = new ChatBoxDto();
            List<ChatToShowDto> chatToShowDtos = new List<ChatToShowDto>();
            string email = User.FindFirst("userEmail")?.Value;
            Student student = await _chatRepository.GetStudentByEmail(email);
            List<StudentsMessage> chats = await _chatRepository.GetChats(student.HallId);

            foreach (var chat1 in chats)
            {
                ChatToShowDto chatToShowDto = new ChatToShowDto();
                chatToShowDto.chatId = chat1.Id;
                chatToShowDto.message = chat1.Message;
                chatToShowDto.sender = chat1.Sender;
                chatToShowDto.SentTime = chat1.Date.ToString("o"); // Convert DateTime to string
                chatToShowDtos.Add(chatToShowDto);
            }

            chatBoxDto.Chats = chatToShowDtos;
            chatBoxDto.Chats.Reverse();

            return Ok(chatBoxDto);

            //return BadRequest("Failed to update chat");
        }
    }
}
