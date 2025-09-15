using AutoMapper;
using MassTransit;
using Outbox.Events.V1;
using OutboxDesignPattern.Data.Entity;
using OutboxDesignPattern.Services.User;

namespace OutboxDesignPattern.Consumers
{
    public class UserCreatedConsumer : IConsumer<UserCreated>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserCreatedConsumer(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<UserCreated> context)
        {
            var user = _mapper.Map<UserEntity>(context.Message);
            await _userService.SaveUserAsync(user);
        }
    }
}
